float BloomSaturation;
float BloomTreshold;
float BloomInstensity;

float offset[31];
float weight[31];
int sampleCount;

float HDRBoost = 0.2f;

float2 viewport;

struct VertexToPixel
{
	float4 Position : SV_Position0;
	float2 TexCoord : TEXCOORD0;
	float4 Color : COLOR0;	
};

struct PixelToFrame
{
	float4 Color : COLOR0;
};

sampler DiffuseSampler : register(s0)
{
	Texture = (Diffuse);  
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
    AddressU = border;
    AddressV = border;
	BorderColor = 0x00000000;  
};

sampler Bloom1Sampler : register(s1)
{
	Texture = (bloomTex1);  
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = clamp;
	AddressV = clamp;
};

sampler Bloom2Sampler : register(s2)
{
	Texture = (bloomTex2);  
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = clamp;
	AddressV = clamp;
};

sampler Bloom4Sampler : register(s3)
{
	Texture = (bloomTex3);  
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = clamp;
	AddressV = clamp;
};

//Jim Hejl and Richard Burgess-Dawson equation, refer to http://filmicworlds.com/blog/filmic-tonemapping-with-piecewise-power-curves/
//float3 HdrMapping(float3 input)
//{
//   input *= 4;  // Hardcoded Exposure Adjustment
//   float3 x = max(0,input-0.004);
//   float3 retColor = (x*(6.2*x+.5))/(x*(6.2*x+1.7)+0.06);
//   return float4(retColor,1);
//}

//float A = 0.15;
//float B = 0.50;
//float C = 0.10;
//float D = 0.20;
//float E = 0.02;
//float F = 0.30;
//float W = 11.2;
//
////Uncharted 2 Tone mapping approach
//float3 Uncharted2Tonemap(float3 x)
//{
//   return ((x*(A*x+C*B)+D*E)/(x*(A*x+B)+D*F))-E/F;
//}
//
//float3 HdrMapping( float3 input ) : COLOR
//{
//   input *= 1;  // Hardcoded Exposure Adjustment
//
//   float ExposureBias = 1.2f;
//   float3 curr = Uncharted2Tonemap(ExposureBias*input);
//
//   float3 whiteScale = 1.0f/Uncharted2Tonemap(W);
//   float3 color = curr*whiteScale;
//      
//   float3 retColor = pow(color,1/2.2);
//   return retColor;
//}

float3 HdrMapping(float3 input)
{
	return saturate(input/ (input + 0.90) * 1.5);
}

float4 AdjustSaturation(float4 color, float saturation)
{
    // The constants 0.3, 0.59, and 0.11 are chosen because the
    // human eye is more sensitive to green light, and less to blue.
    float grey = dot(color, float3(0.3, 0.59, 0.11));
    return lerp(grey, color, saturation);
}


VertexToPixel FullVertexShader(float4 position : SV_Position0, float2 texCoord : TEXCOORD0)
{
	   VertexToPixel Output = (VertexToPixel)0;

	   Output.Position = position;
	   Output.TexCoord = texCoord;

	   return Output;
}

//Horizontal pass, lineary sampled
PixelToFrame HorizontalBlur(VertexToPixel PSIn) 
{
		PixelToFrame Output = (PixelToFrame)0;
		

		float4 diffuseMap = tex2D( DiffuseSampler , float2(PSIn.TexCoord.x, PSIn.TexCoord.y)) * weight[0]; //fixme, look for specific point
			
		float4 final;
		
		for (int i=1; i<sampleCount; i++) {
			diffuseMap +=
				tex2D( DiffuseSampler , ( PSIn.TexCoord+float2(offset[i], 0.0) /viewport.x ) )
					* weight[i];
			diffuseMap +=
				tex2D( DiffuseSampler , ( PSIn.TexCoord-float2(offset[i], 0.0) /viewport.x ) )
					* weight[i];
		}

		Output.Color = diffuseMap;
		Output.Color.a = 1;
		return Output;	

}

//Vertical pass, lineary sampled
PixelToFrame VerticalBlur(VertexToPixel PSIn) 
{
		PixelToFrame Output = (PixelToFrame)0;
		

		float4 diffuseMap = tex2D( DiffuseSampler , float2(PSIn.TexCoord.x, PSIn.TexCoord.y)) * weight[0]; //fixme, look for specific point
			
		float4 final;
		
		for (int i=1; i<sampleCount; i++) {
			diffuseMap +=
				tex2D( DiffuseSampler , ( PSIn.TexCoord+float2(0.0, offset[i]) /viewport.y ) )
					* weight[i];
			diffuseMap +=
				tex2D( DiffuseSampler , ( PSIn.TexCoord-float2(0.0, offset[i]) /viewport.y ) )
					* weight[i];
		}

		Output.Color = diffuseMap ;
		Output.Color.a = 1;

		return Output;	
}

//Extracts highlights for bloom
PixelToFrame ExtractBloom(VertexToPixel PSIn)
{
	PixelToFrame Output = (PixelToFrame)0;
    // Look up the original image color.
	float4 diffuseMap = tex2D( DiffuseSampler , float2(PSIn.TexCoord.x, PSIn.TexCoord.y));

    // Adjust it to keep only values brighter than the specified threshold.
	float maxHDR = max(max(diffuseMap.r, diffuseMap.g), diffuseMap.b);
	maxHDR -= BloomTreshold;
    Output.Color.rgb = diffuseMap.rgb * max(maxHDR,0); 
	
	Output.Color.a = 1;
	return Output;	
}

float4 CombineBloom(VertexToPixel PSIn) : COLOR0
{
	float4 diffuse = tex2D(DiffuseSampler, PSIn.TexCoord);
    float4 bloom1= tex2D(Bloom1Sampler, PSIn.TexCoord);
    float4 bloom2 = tex2D(Bloom2Sampler, PSIn.TexCoord);
    float4 bloom4 = tex2D(Bloom4Sampler, PSIn.TexCoord);
	float4 bloom = AdjustSaturation(bloom1 * 1.4 + bloom2 * 1.2 + bloom4, BloomSaturation) ;
	
	return float4(HdrMapping(diffuse.rgb + bloom.rgb * BloomInstensity), 1);
}

technique ExtractBloom
{
    pass Pass1
    {
		VertexShader = compile vs_4_0 FullVertexShader();
        PixelShader = compile ps_4_0 ExtractBloom();
    }
}


technique VerticalBlur
{
    pass Pass1
    {
		VertexShader = compile vs_4_0 FullVertexShader();
        PixelShader = compile ps_4_0 VerticalBlur();
    }
}

technique HorizontalBlur
{
    pass Pass1
    {
		VertexShader = compile vs_4_0 FullVertexShader();
        PixelShader = compile ps_4_0 HorizontalBlur();
    }
}

technique CombineBloom
{
    pass Pass1
    {
		VertexShader = compile vs_4_0 FullVertexShader();
        PixelShader = compile ps_4_0 CombineBloom();
    }
}

