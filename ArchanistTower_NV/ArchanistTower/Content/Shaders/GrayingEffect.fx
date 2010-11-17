
float powerGained;
float2 blendTexturePosition2;

// Global variables
// This will use the texture bound to the object( like from the sprite batch ).
sampler ColorMapSampler : register(s0);

// Grayscale
float4 PixelShader(float2 Tex: TEXCOORD0) : COLOR
{
	float4 Color = tex2D(ColorMapSampler, Tex);	
	float4 color2 = tex2D(ColorMapSampler, Tex); 
    //float4 color3 = tex2D(ColorMapSampler, Tex + blendTexturePosition2); 
    
    
	//Color.rgb = (Color.r + Color.g + Color.b)/3;
	if (powerGained == 0)
		Color.rgb = dot(Color.rgb, float3(0.3, .59, .11));
	if (powerGained == 1)
	{
		Color.rgb = dot(Color.rgb, float3(0.3, .59, .11));
			if (color2.r*.66 > color2.b && color2.r*.66 > color2.g)
			{
				Color = color2;
			}
	}
	if (powerGained == 2)
	{
		Color.rgb = dot(Color.rgb, float3(0.3, .59, .11));
			if (color2.r*.66 > color2.b  || color2.g*.66 > color2.b)
			{
				Color = color2;
			}
	}

	//Color.b = color2.b;
	// Keep our alphachannel at 1.
	Color.a = 1.0f;
		
    return Color;
}
float4 PixelShader2(float2 Tex: TEXCOORD0, float1 progress: INT) : COLOR
{
	float4 Color = tex2D(ColorMapSampler, Tex);	
	float4 color2 = tex2D(ColorMapSampler, Tex); 
    //float4 color3 = tex2D(ColorMapSampler, Tex + blendTexturePosition2); 
    
    
	//Color.rgb = (Color.r + Color.g + Color.b)/3;
	Color.rgb = dot(Color.rgb, float3(0.3, .59, .11));
	//Color.g = color2.g;
	if (progress == 2)
		Color.r = color2.r;
	//Color.b = color2.b;
	// Keep our alphachannel at 1.
	Color.a = 1.0f;
		
    return Color;
}
technique PostProcess
{
	pass P0
	{
		// A post process shader only needs a pixel shader.
		PixelShader = compile ps_2_0 PixelShader();
	}
}