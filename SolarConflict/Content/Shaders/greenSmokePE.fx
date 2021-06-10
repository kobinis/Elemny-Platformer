#include "ParticleHelper.h"

int FrameCount = 14;
int PhaseCount = 13; //FrameCount - 1
float SubSpeed = 1.5;


sampler LUTSampler = sampler_state
{
    Texture = (LUT);
    
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    
    AddressU = Clamp;
    AddressV = Clamp;
};


float3 MapTemperature(float3 dif, float temperature)
{
	float samplePoint =   dif.r * temperature * 2.5;
	float3 Temp =  tex2D(LUTSampler, float2(samplePoint / 10000, 0)).rgb; 
    return Temp * pow(1.0025,(temperature * 0.0075)) * 1.5;
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
    output.Color = ComputeParticleColor(output.Position, normalizedAge);
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
  //Diffuse.rbg = float3(1,1,1) + Diffuse * 0.00001;
  Diffuse.a = input.Color.a * clamp(Diffuse.a * 6,0,1);
  Diffuse.rgb =  Diffuse.rgb * 0.2 + MapTemperature(Diffuse.rgb, input.Temperature);
  Diffuse.rgb *=  input.Color.a ;
  Diffuse.rbg *= Diffuse.a;
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
