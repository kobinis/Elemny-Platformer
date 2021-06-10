/// Trimmed-down version of our normal map shader, for illumination without normals

float2 HalfResolution;
float Zoom;
float3 AmbientColor = 0;
/// For saturation prevention. If non-negative, acts as an upper bound on one way in which this shader can multiplicatively intensify the underlying texture's colors. 
/// For each color, a pixel's value will be (textureValue * drawColorValue * lightValue). MaxIntensity bounds lightValue.Note that textureValue and drawColorValue
/// components will be in the range (0, 1), only intensity can exceed 1.
float MaxIntensity = -1;

int NumLights;

float LightAttenuation[32];
float LightHotspots[32];
/// A light's position should be its world position, minus the world position of the center of the screen
float2 LightPositions[32];
float3 LightColors[32];


//float3 DominantDirection = float3(1,0,1);
//float3 DominantColor = float3(1,1,1);

sampler TextureSampler : register(s0)
{
    AddressU = Clamp;
    AddressV = Clamp;
};

/// Client->raster space transform
float4x4 ProjectionMatrix;
/// XNA's default vertex shader for sprites
void SpriteVertexShader(inout float4 vColor : COLOR0, inout float2 texCoord : TEXCOORD0, inout float4 position :SV_Position0)
{
    position = mul(position, ProjectionMatrix);
}

struct PS_INPUT
{   
    float4 screenPos : SV_Position0;
};

float4 main(float4 color : COLOR0, float2 textureCoordinate : TEXCOORD0, PS_INPUT input) : COLOR0
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
    
    if (MaxIntensity >= 0) {
        // Cap light intensity while avoiding white shift. Figure out how much the brightest channel exceeds our limit
        float brightest = 0;
        for (int i = 0; i < 3; ++i)
        {
            brightest = max(brightest, sumOfLighting[i] - MaxIntensity);
        }
        if (brightest > MaxIntensity)
        {            
            // Multiply each channel by the same coefficient, which'll reduce our brightest channel to MaxIntensity (and maintain the proportions between channels)
            float coefficient = MaxIntensity / brightest;
            for (int i = 0; i < 3; ++i)
            {
                sumOfLighting[i] *= coefficient;
            }
        }
    }

    color.rgb *= AmbientColor + sumOfLighting;  
    
    return tex * color;
}

technique Flatlighting
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 SpriteVertexShader();
        PixelShader = compile ps_4_0 main();    
    }
}
