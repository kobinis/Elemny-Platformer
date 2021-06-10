float2 Viewport;
float2 MaskStart;
float2 MaskSize;

sampler TextureSampler : register(s0)
{
    AddressU = Clamp;
    AddressV = Clamp;
};

sampler MaskSampler : register(s1)
{
	Texture = (MaskMap);
    AddressU =  Clamp;
	AddressV = Clamp;
};        

struct PS_INPUT
{   
    float4 screenPos : SV_Position0;
	float4 color: COLOR0;
	float2 textureCoordinate: TEXCOORD0;
	float2 uvPos: TEXCOORD01;
};

/// XNA's default vertex shader for sprites
PS_INPUT SpriteVertexShader(float4 color : COLOR0, float2 texCoord : TEXCOORD0, float4 position :SV_Position0)
{
	   PS_INPUT output;

	   output.uvPos = position.xy;
	   position.xy = position.xy  / Viewport;

	   position.xy *= float2(2, -2.0);
	   position.xy -= float2(1, -1);
	
	   output.screenPos = position;
	   output.textureCoordinate = texCoord;
	   output.color = color;

	   return output;
}

float4 MaskingPixelShader(PS_INPUT input) : COLOR0
{            
	float4 tex = tex2D(TextureSampler, input.textureCoordinate);    
	input.uvPos -= MaskStart; //we offset UV back
	input.uvPos /= MaskSize; //we get it into range
	float mask = tex2D(MaskSampler, input.uvPos).r;    
	input.color.a *= mask;
    return tex * input.color;//
}

float4 PassThroughPixelShader(PS_INPUT input) : COLOR0
{            
	float4 tex = tex2D(TextureSampler, input.textureCoordinate);    
    return tex * input.color;//
}

technique MinimapMasking
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 SpriteVertexShader();
        PixelShader = compile ps_4_0 MaskingPixelShader();    
    }
}

technique PassThrough
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 SpriteVertexShader();
        PixelShader = compile ps_4_0 PassThroughPixelShader();    
    }
}
