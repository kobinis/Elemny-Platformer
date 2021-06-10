/// Normal mapping shader for one or more point lights (plus ambient)
/// For each color, value will be textureValue * drawColorValue * (ambientValue + sumOfLights), where each light contributes
/// lightValue * (lightDirection.Dot(surfaceNormal)) to sumOfLights.

/// See LightingGlobals.Tilt
float Tilt;
float2 HalfResolution;
float Zoom;
float3 AmbientColor = 0;
float4 CityColor = float4(0.65, 0.85, 1.0, 1.0);
/// Angle of the texture's rotation (2D)
float Rotation;
float AtmosphereRatio = 0.875f;
float3 DominantLightPos = float3(-7500,3000, 80);
float3 DominantLightColor = float3(1,1,1);

float3 Color1 = float3(0.3, 0.1, 0.0);
float3 Color2 = float3(0.545, 0.4, 0.294);
float3 Color3 = float3(0.894, 0.954, 0.978);

float4x4 MatrixTransform;

float3 Phase;
int NumLights;

float BeamMaxLength;
float BeamLength;
float BeamLifetime;


float LightAttenuation[32];
float LightHotspots[32];
/// A light's position should be its world position, minus the world position of the center of the screen
float2 LightPositions[32];
float3 LightColors[32];

// we are in top down, thus this wont change
float3 ViewVector = float3(0,0,-1);

float3 AtmosphereColor = float3(1.0, 0.72, 0.55);
float AtmosphereIntensity = 0.7f;

float SpecularHardness = 15;

float FluidLevel;
float4 FluidColor;

float2 Viewport;
//float3 DominantDirection = float3(1,0,1);
//float3 DominantColor = float3(1,1,1);


sampler TextureSampler : register(s0)
{
    
};

sampler NormalSampler : register(s1)
{    
	Texture = (NormalMap);
    AddressU = Wrap;
    AddressV = Wrap;
	
	MinFilter = Linear;
    MagFilter = Linear;
};

sampler RoughnessSampler : register(s2)
{    
	Texture = (RoughnessMap);
    AddressU = Clamp;
    AddressV = Clamp;
	
	MinFilter = Linear;
    MagFilter = Linear;
};

sampler EmissionSampler : register(s3)
{    
	Texture = (EmissionMap);
    AddressU = Clamp;
    AddressV = Clamp;
	
	MinFilter = Linear;
    MagFilter = Linear;
};


struct PS_INPUT
{   
    float4 screenPos : SV_Position0;
	float4 color: COLOR0;
	float2 textureCoordinate: TEXCOORD0;
	float2 rotations: TEXCOORD1;
};


/// Client->raster space transform
//float4x4 ProjectionMatrix;
/// XNA's default vertex shader for sprites
PS_INPUT SpriteVertexShader(float4 vColor : COLOR0, float2 texCoord : TEXCOORD0, float4 position : SV_Position0)
{
	PS_INPUT output;

    output.screenPos = mul(position, MatrixTransform);
	output.color = vColor;
	output.textureCoordinate = texCoord;
	//We precalculate sin cos in vertex shader
	output.rotations = float2(sin(Rotation), cos(Rotation));
	return output;
}

PS_INPUT ScreenSpriteVertexShader(float4 color : COLOR0, float2 texCoord : TEXCOORD0, float4 position : SV_Position0)
{

	   PS_INPUT Output = (PS_INPUT)0;

	   // Viewport adjustment.
	   position.xy = position.xy  / Viewport;
	   position.xy *= float2(2, -2.0);
	   position.xy -= float2(1, -1);

	   //send data to output
	   Output.screenPos = position;
	   Output.textureCoordinate = texCoord;
	   Output.color = color;
	  
	   return Output;
}


float4 BasicSpritePS(PS_INPUT input) : COLOR0
{
	float4 diffuse = tex2D(TextureSampler, input.textureCoordinate);
	float4 result = diffuse * input.color;
	result.rgb *= (input.color.a * diffuse.a);
   	return result ;
}




float4 NormalMapPS(PS_INPUT input) : COLOR0
{            
	float4 tex = tex2D(TextureSampler, input.textureCoordinate);
	
	clip(tex.a  <= 0.0f ? -1:1 );
	
    float3 normal = tex2D(NormalSampler, input.textureCoordinate);
	float3 roughness = 1 - tex2D(RoughnessSampler, input.textureCoordinate).r;
    float3 emission = tex2D(EmissionSampler, input.textureCoordinate);
    
    // Convert the normal to world space
    normal = 2 * normal - 1;
    normal.y = -normal.y;
    
    // Rotate it to match the texture's rotation
	// input.rotations.x = precomputed sin, input.rotations.y = precomputed cos
    normal = float3(normal.x *input.rotations.y - normal.y * input.rotations.x, normal.x * input.rotations.x + normal.y * input.rotations.y, normal.z);

    // By "sorta world position", we mean worldPositionOfPixel - worldPositionOfScreenCenter. The LightPositions array uses the same
    // coordinate system
    float2 sortaWorldPosition = (input.screenPos - HalfResolution) / Zoom;        

    float3 sumOfLighting = 0; 
	float3 sumOfSpecularity = 0;
	


	
    for (int i = 0; i < NumLights; ++i) {
        float2 diff = LightPositions[i] - sortaWorldPosition;
		float hotSpotRemap =  saturate(LightHotspots[i] /  LightAttenuation[i]);
        // Attenuate light with distance
        //float attenuation = 1 / pow((LightDistanceCoefficients[i] * max(length(diff), LightMinDistances[i])), LightAttenuationExponents[i]);      
		
	    float  attenuation = saturate(1.0f - saturate((length(diff ) / LightAttenuation[i]) - hotSpotRemap) / (1 -  hotSpotRemap));		
        // Make light direction 3D (add tilt), dot it with normal        
        float2 direction = normalize(diff);     

        direction *= (1 - Tilt);
        float3 direction3D = float3(direction.x, direction.y, Tilt);
		
        sumOfLighting += max(dot(normal, direction3D), 0) * LightColors[i] * attenuation +  LightHotspots[i]; //TO DO add hotspot calculation        
		
		// Specularity
	    // Half Vector
		float3 halfVec = normalize(direction3D  + ViewVector) ;
		
		sumOfSpecularity +=  pow(saturate(dot(normal,  halfVec)), SpecularHardness) * LightColors[i] * attenuation * roughness; //we are using simple model before we decide if we want to use PBR
    }

    input.color.rgb *= AmbientColor + sumOfLighting  ;      
	float4 final = tex * input.color;
	final.rgb += sumOfSpecularity + emission * 1.4;
	final.rgb *= input.color.a;
    return final;
}

float4 BeamPS(PS_INPUT input) : COLOR0
{            
	float lengthNormalization = BeamLength / 100;

	float2 texUV = float2(input.textureCoordinate.x * lengthNormalization * (1.0f / 5.0f)  - BeamLifetime  , input.textureCoordinate.y  * 1);
	
	
	float4 tex = tex2D(TextureSampler, texUV);
	
	clip(tex.a  <= 0.0f ? -1:1 );
	
	
	float lFade = (input.textureCoordinate.x + 1)* 0.5;

	
	lFade = saturate((1 -lFade) * (BeamMaxLength / clamp(20 - (BeamMaxLength - BeamLength),0,20)));
	
	float EFade = (1 - input.textureCoordinate.x + 1)* 0.5;
	EFade = saturate((1 -EFade) * (BeamMaxLength / clamp(5 - (BeamMaxLength - BeamLength),0,5)));

	float4 final = tex * 1.8 * lFade *  EFade; // * 1.5 * 0.0001 + input.textureCoordinate.x;
	final.rgb *= input.color.rgb * input.color.a;
	final.a *= input.color.a;
    return final;
}


float4 PlanetSurfacePS(PS_INPUT input) : COLOR0
{
	
	    float radiusMulti = AtmosphereRatio;
		
		float pi = acos(-1);    

		//map texel coordinates to [-1, 1]
		float x = 2.0 * (input.textureCoordinate.x - 0.5);
		float y = 2.0 * (input.textureCoordinate.y - 0.5);
		x /= radiusMulti;
		y /= radiusMulti;
		float z = sqrt(1.0 - (x*x +  y*y));
		float r = sqrt(x * x + y * y);    
		
		
		float3 normal = float3(x,y,z);
		normal = normalize(normal);
		//normal.y *= -1;

		clip(r  > 1.0f ? -1:1 );
		
	    matrix <float, 3, 3> fMatrixX = { cos(Rotation), 0, sin(Rotation),
                                      0, 1,0,
								 -sin(Rotation),0, cos(Rotation)
                               };
		
		//3D half sphere position
		float3 p = float3(x, y, sqrt(1 - x*x - y*y));
		p=mul(p ,fMatrixX);
		
		//calculate UV mapping
		float u = 0.5 + atan2(p[2], p[0]) / (2.0*pi);
		float v = 0.5 - asin(p[1]) / pi;   
		
		float alpha = saturate((1-r) * 16);

		 float3 sortaWorldPosition = float3((input.screenPos - HalfResolution) / Zoom, -100);        
		//Light calculation
		 float3 direction = DominantLightPos - sortaWorldPosition;

        // Make light direction 3D (add tilt), dot it with normal        
        direction = normalize(direction);                
        
		
        float lightIntensity = saturate(dot(normal, direction)) + 0.15;        
		
	    float4 tex = tex2D(TextureSampler, float2(u,v));
	    float city = tex2D(RoughnessSampler, float2(u,v)).r;
		tex.rgb *=  lightIntensity * DominantLightColor;
		
		//cities
		float cityIntensity = (pow(1 - saturate(dot(direction, normal)),3)) *city ;
		tex.a = 1;
		float4 cityLights = CityColor * 4 * cityIntensity;	
		
		float4 final = tex +  cityLights;
		final.a = 1;
		return final;
}

float3 HeightLerp(float3 map1, float3 map2, float heightMap, float phase, float contrast)
{
   float  transition = heightMap / phase;
   transition = pow(transition, contrast);
   transition = clamp((transition / (phase  * 2)),0,1);
   return lerp(map1, map2, transition);
  
}


float4 HeightLerpF4(float4 map1, float4 map2, float heightMap, float phase, float contrast)
{
   float  transition = heightMap / phase;
   transition = pow(transition, contrast);
   transition = clamp((transition / (phase  * 2)),0,1);
   return lerp(map1, map2, transition);
  
}

float4 PlanetRandomSurfaceBarrenPS(PS_INPUT input) : COLOR0
{
	
	    float radiusMulti = AtmosphereRatio;
		
		float pi = acos(-1);    

		//map texel coordinates to [-1, 1]
		float x = 2.0 * (input.textureCoordinate.x - 0.5);
		float y = 2.0 * (input.textureCoordinate.y - 0.5);
		x /= radiusMulti;
		y /= radiusMulti;
		float z = sqrt(1.0 - (x*x +  y*y));
		float r = sqrt(x * x + y * y);    
		
		
		float3 normal = float3(x,y,z);
		normal = normalize(normal);
		//normal.y *= -1;

		clip(r  > 1.0f ? -1:1 );
		
	    matrix <float, 3, 3> fMatrixX = { cos(Rotation), 0, sin(Rotation),
                                      0, 1,0,
								 -sin(Rotation),0, cos(Rotation)
                               };
		
		//3D half sphere position
		float3 p = float3(x, y, sqrt(1 - x*x - y*y));
		p=mul(p ,fMatrixX);
		
		//calculate UV mapping
		float u = 0.5 + atan2(p[2], p[0]) / (2.0*pi);
		float v = 0.5 - asin(p[1]) / pi;   
		
		float alpha = saturate((1-r) * 16);

		 float3 sortaWorldPosition = float3((input.screenPos - HalfResolution) / Zoom, -100);        
		//Light calculation
		 float3 direction = DominantLightPos - sortaWorldPosition;

        // Make light direction 3D (add tilt), dot it with normal        
        direction = normalize(direction);                
        
		
        float lightIntensity = saturate(dot(normal, direction)) + 0.15;        
		
	    float height = tex2D(TextureSampler, float2(u,v)).r;
		
		float4 surfaceTextures = tex2D(NormalSampler, float2(u * 2,v) * 2   + 0.5 ); //Each channel for one texture
		
		float3 diffuse1 = surfaceTextures.r * Color1;
		float3 diffuse2 = surfaceTextures.g * Color2;
		float3 diffuse3 = surfaceTextures.b * Color3;
		
		
		float3 diffuse = HeightLerp(diffuse1, diffuse2, height, Phase.x + 0.15 , 8);
		diffuse = HeightLerp(diffuse, diffuse3, height, Phase.y, 2);


		
		float4 final;
		final.rgb = diffuse * lightIntensity;
		final.a = 1;
		return final;
}


float4 PlanetRandomSurfaceTerranPS(PS_INPUT input) : COLOR0
{
	
	    float radiusMulti = AtmosphereRatio;
		
		float pi = acos(-1);    

		//map texel coordinates to [-1, 1]
		float x = 2.0 * (input.textureCoordinate.x - 0.5);
		float y = 2.0 * (input.textureCoordinate.y - 0.5);
		x /= radiusMulti;
		y /= radiusMulti;
		float z = sqrt(1.0 - (x*x +  y*y));
		float r = sqrt(x * x + y * y);    
		
		
		float3 normal = float3(x,y,z);
		normal = normalize(normal);
		//normal.y *= -1;

		clip(r  > 1.0f ? -1:1 );
		
	    matrix <float, 3, 3> fMatrixX = { cos(Rotation), 0, sin(Rotation),
                                      0, 1,0,
								 -sin(Rotation),0, cos(Rotation)
                               };
		
		//3D half sphere position
		float3 p = float3(x, y, sqrt(1 - x*x - y*y));
		p=mul(p ,fMatrixX);
		
		//calculate UV mapping
		float u = 0.5 + atan2(p[2], p[0]) / (2.0*pi);
		float v = 0.5 - asin(p[1]) / pi;   
		
		float alpha = saturate((1-r) * 16);

		 float3 sortaWorldPosition = float3((input.screenPos - HalfResolution) / Zoom, -100);        
		//Light calculation
		 float3 direction = DominantLightPos - sortaWorldPosition;

        // Make light direction 3D (add tilt), dot it with normal        
        direction = normalize(direction);                
        
		
        float lightIntensity = saturate(dot(normal, direction)) + 0.15;        
		
	    float height = tex2D(TextureSampler, float2(u,v)).r;
		
		float4 surfaceTextures = tex2D(NormalSampler, float2(u * 2,v) * 2   + 0.5 ); //Each channel for one texture
		
		float3 diffuse1 = surfaceTextures.r * Color1;
		float3 diffuse2 = surfaceTextures.g * Color2;
		float3 diffuse3 = surfaceTextures.b * Color3;
		
		 float fluidDepth = saturate(FluidLevel - height + 0.06); //+ 0.06
		 float fluidInverse = 1 - fluidDepth;
		 fluidInverse = pow(fluidInverse,3);
		 

		
		float3 diffuse = HeightLerp(diffuse1, diffuse2, height, Phase.x + 0.15 , 8);
		diffuse = HeightLerp(diffuse, diffuse3, height, Phase.y, 2);

		float specularity = 0;
		
		 if(height < FluidLevel)
		{ 
			//Solve water absorbtion
			diffuse.r *= (fluidInverse * fluidInverse);
			diffuse.g *= (fluidInverse + 0.06);
			diffuse = saturate(pow(diffuse, 2)  * 4.0);
			
			float3 halfVec = normalize(direction  + ViewVector) ;
			specularity =  max(dot(normal,  halfVec),0);
			//float specularity =  pow(saturate(dot(normal,  halfVec)), 1.0f) * 20;
			

		 }

		
		float4 final;
		final.rgb = diffuse * lightIntensity + specularity * 0.6;
		final.a = 1;
		return final;
}

float4 PlanetRandomSurfaceMagmaPS(PS_INPUT input) : COLOR0
{
	
	    float radiusMulti = AtmosphereRatio;
		
		float pi = acos(-1);    

		//map texel coordinates to [-1, 1]
		float x = 2.0 * (input.textureCoordinate.x - 0.5);
		float y = 2.0 * (input.textureCoordinate.y - 0.5);
		x /= radiusMulti;
		y /= radiusMulti;
		float z = sqrt(1.0 - (x*x +  y*y));
		float r = sqrt(x * x + y * y);    
		
		
		float3 normal = float3(x,y,z);
		normal = normalize(normal);
		//normal.y *= -1;

		clip(r  > 1.0f ? -1:1 );
		
	    matrix <float, 3, 3> fMatrixX = { cos(Rotation), 0, sin(Rotation),
                                      0, 1,0,
								 -sin(Rotation),0, cos(Rotation)
                               };
		
		//3D half sphere position
		float3 p = float3(x, y, sqrt(1 - x*x - y*y));
		p=mul(p ,fMatrixX);
		
		//calculate UV mapping
		float u = 0.5 + atan2(p[2], p[0]) / (2.0*pi);
		float v = 0.5 - asin(p[1]) / pi;   
		
		float alpha = saturate((1-r) * 16);

		 float3 sortaWorldPosition = float3((input.screenPos - HalfResolution) / Zoom, -100);        
		//Light calculation
		 float3 direction = DominantLightPos - sortaWorldPosition;

        // Make light direction 3D (add tilt), dot it with normal        
        direction = normalize(direction);                
        
		
        float lightIntensity = saturate(dot(normal, direction)) + 0.15;        
		
	    float height = tex2D(TextureSampler, float2(u,v)).r;
		
		float4 surfaceTextures = tex2D(NormalSampler, float2(u * 2,v) * 2   + 0.5 ); //Each channel for one texture
		
		float3 diffuse1 = surfaceTextures.r * Color1;
		float3 diffuse2 = surfaceTextures.g * Color2;
		float3 diffuse3 = surfaceTextures.b * Color3;
		
		
		
		float3 diffuse = HeightLerp(diffuse1, diffuse2, height, Phase.x + 0.15 , 8);
		diffuse = HeightLerp(diffuse, diffuse3, height, Phase.y, 2);
		
		float fluid = HeightLerp(1,0,height,  FluidLevel , 3);

		float4 final;
		final.rgb = diffuse * lightIntensity + fluid * FluidColor * pow(surfaceTextures.a, 2) * (20 * 0.2);
		final.a = 1;
		return final;
}


float4 PlanetAtmospherePS(PS_INPUT input) : COLOR0
{
		float x = 2.0 * (input.textureCoordinate.x - 0.5);
		float y = 2.0 * (input.textureCoordinate.y - 0.5);
		float z = sqrt(1.0 - (x*x +  y*y));
		float r = sqrt(x * x + y * y);    
		
		float3 normal = float3(x,y,z);
		normal = normalize(normal);
		//normal.y *= -1;
		
		//Outer soft
		//remap 0.8f - 1.0f to 1f - 0f
		float outerLength = 1 - AtmosphereRatio;
		float outerAtmo = 1 - (r - (1 - outerLength)) / outerLength;
		outerAtmo = saturate(outerAtmo);
		
		//Substractive inner
		//remap 0.0 - 0.8 to 0.0 - 1.0
		float innerAtmo = 1 - saturate(r / (1.0f - outerLength));
		float spreadAtmo =  pow(outerAtmo - innerAtmo, 1.5);
		float highlightAtmo =  saturate(pow(outerAtmo - innerAtmo, 5));
		
		float3 sortaWorldPosition = float3((input.screenPos - HalfResolution) / Zoom, -100);        
		float3 direction = DominantLightPos - sortaWorldPosition;
		
		float directionalIntensity = saturate(dot(normal, normalize(direction)));
		
		return  (spreadAtmo * float4(AtmosphereColor,1) * AtmosphereIntensity + highlightAtmo * 0.15) * saturate(directionalIntensity   * 4 + 0.35f) * float4(DominantLightColor,1);
		
}


float4 FlatLightningPS(float4 color : COLOR0, float2 textureCoordinate : TEXCOORD0, PS_INPUT input) : COLOR0
{            
	float4 tex = tex2D(TextureSampler, textureCoordinate);    

    // By "sorta world position", we mean worldPositionOfPixel - worldPositionOfScreenCenter. The LightPositions array uses the same
    // coordinate system
    float2 sortaWorldPosition = (input.screenPos - HalfResolution) / Zoom;        

    float3 sumOfLighting = 0; 
    for (int i = 0; i < NumLights; ++i) {
        float2 diff = LightPositions[i] - sortaWorldPosition;
		float hotSpotRemap =  saturate(LightHotspots[i] /  LightAttenuation[i]);
        // Attenuate light with distance
        //float attenuation = 1 / pow((LightDistanceCoefficients[i] * max(length(diff), LightMinDistances[i])), LightAttenuationExponents[i]);      
		
	    float  attenuation = saturate(1.0f - saturate((length(diff ) / LightAttenuation[i]) - hotSpotRemap) / (1 -  hotSpotRemap));	

        sumOfLighting += LightColors[i] * attenuation;        
    }
    
    color.rgb *= AmbientColor + sumOfLighting;  
    
    return tex * color * color.a;
}



technique PlanetRandomSurfaceMagma
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 SpriteVertexShader();
        PixelShader = compile ps_4_0 PlanetRandomSurfaceMagmaPS();    
    }
}

technique PlanetRandomSurfaceBarren
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 SpriteVertexShader();
        PixelShader = compile ps_4_0 PlanetRandomSurfaceBarrenPS();    
    }
}

technique PlanetRandomSurfaceTerran
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 SpriteVertexShader();
        PixelShader = compile ps_4_0 PlanetRandomSurfaceTerranPS();    
    }
}


technique PlanetSurface
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 SpriteVertexShader();
        PixelShader = compile ps_4_0 PlanetSurfacePS();    
    }
}

technique PlanetAtmosphere
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 SpriteVertexShader();
        PixelShader = compile ps_4_0 PlanetAtmospherePS();    
    }
}

technique BasicSprite
{
	    pass Pass1
    {
        VertexShader = compile vs_4_0 ScreenSpriteVertexShader();
        PixelShader = compile ps_4_0 BasicSpritePS();    
    }
}

technique BeamEffect
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 SpriteVertexShader();
        PixelShader = compile ps_4_0 BeamPS();    
    }
}


technique Normalmap
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 SpriteVertexShader();
        PixelShader = compile ps_4_0 NormalMapPS();    
    }
}

technique Flatlighting
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 SpriteVertexShader();
        PixelShader = compile ps_4_0 FlatLightningPS();    
    }
}
