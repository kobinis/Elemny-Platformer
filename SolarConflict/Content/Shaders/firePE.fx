#include "ParticleHelper.h"

int FrameCount = 14;
int PhaseCount = 13; //FrameCount - 1
float SubSpeed = 1.5;


float3 MapTemperature(float3 dif, float temperature)
{
  float3 Temp =  pow(dif,0.8)   * temperature;
  float u2 = ( 0.860117757f + 1.54118254e-4f * Temp + 1.28641212e-7f * Temp*Temp ) / ( 1.0f + 8.42420235e-4f * Temp + 7.08145163e-7f * Temp*Temp );
  float v2 = ( 0.317398726f + 4.22806245e-5f * Temp + 4.20481691e-8f * Temp*Temp ) / ( 1.0f - 2.89741816e-5f  * Temp + 1.61456053e-7f * Temp*Temp ); //( 1.0f - 2.89741816e-5f * Temp + 1.61456053e-7f * Temp*Temp );
  
  float x1 = 3*u2 / ( 2*u2 - 8*v2 + 4 );
  float y1 = 2*v2 / ( 2*u2 - 8*v2 + 4 );
  float z1 = 1 - x1 - y1;
  
  
  float Y = 1;
  float X = Y/y1 * x1;
  float Z = Y/y1 * z1;
  
  float3x3 XYZtoRGB =
        {
                 3.2404542, -1.5371385, -0.4985314,
                -0.9692660,  1.8760108,  0.0415560,
                 0.0556434, -0.2040259,  1.0572252,
        };
		
   Temp.xyz = float3(mul( XYZtoRGB, float3( X, Y, Z ) ) * pow( 0.0004 * Temp, 4 ));
	
   return Temp;
}



// Custom vertex shader animates particles entirely on the GPU.
VertexShaderOutput ParticleVertexShader(VertexShaderInput input)
{
    VertexShaderOutput output;
	
    // Compute the age of the particle.
    float age = CurrentTime - input.Time;
	
	// Normalize the age into the range zero to one.
    float normalizedAge = saturate(age / Duration) ;

	
    // Compute the particle position, size, color, and rotation.
    output.Position = ComputeParticlePosition(input.Position, input.Velocity,
                                              age, normalizedAge);
											  
    float size = ComputeParticleSize(input.Random.y, normalizedAge, input.Parameters.y);
    float2x2 rotation = ComputeParticleRotation(input.Random.w, age);
    output.Position.xy += mul(input.Corner, rotation) * size * ViewportScale; 
	float disOffset = lerp(0.68, 0.74, input.Random.w);
	output.Temperature = clamp( input.Parameters.x-(input.Parameters.x*(normalizedAge + disOffset)*(normalizedAge + disOffset)*(normalizedAge + disOffset) + 0),0,input.Parameters.x) ;   //+ lerp( input.Parameters.x * 2, 0, clamp(normalizedAge * 5,0,1))
    output.Color = ComputeParticleColorSpikeIn(output.Position, normalizedAge);
    output.TextureCoordinate = (input.Corner + 1) / 2;   
	
	//compute random offset
	float offset = lerp(0,1, input.Random.w);
	//randomize SubSpeed
	SubSpeed = lerp(0.6,1.0, input.Random.z) ;
    int index = floor((normalizedAge + offset) * PhaseCount * SubSpeed);
	output.Phase.y = (normalizedAge + offset) * (PhaseCount) * SubSpeed - floor((normalizedAge + offset) * SubSpeed* PhaseCount) ;
    output.Phase.x = index;

    
    return output;
}



// Pixel shader for drawing particles.
float4 ParticlePixelShader(VertexShaderOutput input) : COLOR0
{
  float invCount = 1 / float(FrameCount);
  float4 DiffuseA =  tex2D(Sampler, float2(input.TextureCoordinate.x * invCount + (invCount * input.Phase.x), input.TextureCoordinate.y)); 
  float4 DiffuseB =  tex2D(Sampler, float2(input.TextureCoordinate.x * invCount + invCount * (input.Phase.x + 1), input.TextureCoordinate.y)); 
  float4 Diffuse = lerp(DiffuseA, DiffuseB, input.Phase.y);
  Diffuse.a = input.Color.a * clamp(Diffuse.a * 6,0,1);
  Diffuse.rgb =  Diffuse.rgb * 0.2 + MapTemperature(Diffuse.rgb, input.Temperature);
  Diffuse.rgb *=  input.Color.a ;
  return Diffuse;
}



// Effect technique for drawing particles.
technique Particles
{
    pass P0
    {
        VertexShader = compile vs_4_0 ParticleVertexShader();
        PixelShader = compile ps_4_0 ParticlePixelShader();
    }
}
