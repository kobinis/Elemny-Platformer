float2 Viewport;
float4 SourceUV;

sampler TextureSampler : register(s0)
{
    AddressU = Wrap;
    AddressV = Wrap;
};


struct PS_INPUT
{   
    float4 screenPos : SV_Position0;
	float4 color: COLOR0;
	float2 textureCoordinate: TEXCOORD0;
	float2 uvPos: TEXCOORD01;
};

/// XNA's default vertex shader for sprites
PS_INPUT BackgroundVertexShader(float4 color : COLOR0, float2 texCoord : TEXCOORD0, float4 position :SV_Position0)
{
	   PS_INPUT output;

	   output.uvPos = position.xy;
	   position.xy = position.xy  / Viewport;

	   position.xy *= float2(2, -2.0);
	   position.xy -= float2(1, -1);
	
	   output.screenPos = position;
	   output.textureCoordinate = (SourceUV.xy + float2( SourceUV.zw * texCoord.xy)) / Viewport ;
	   output.color = color;

	   return output;
}

float4 BackgroundPixelShader(PS_INPUT input) : COLOR0
{            
	float4 tex = tex2D(TextureSampler, input.textureCoordinate);    
    return tex * input.color;//
}




technique BackgroundTechnique
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 BackgroundVertexShader();
        PixelShader = compile ps_4_0 BackgroundPixelShader();    
    }
}
