
float powerGreen;
float powerRed;
float powerBlue;
float2 blendTexturePosition2 = (0,0);
float4 rectangleA;
texture2D textureA;

// Global variables
// This will use the texture bound to the object( like from the sprite batch ).
sampler ColorMapSampler : register(s0);
sampler crysTex = sampler_state
	{
	texture = <textureA>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear; 
	AddressU  = Clamp;
	AddressV  = Clamp;
	};

// Grayscale
float4 PixelShader(float2 Tex: TEXCOORD0) : COLOR
{
	float4 Color = tex2D(ColorMapSampler, Tex);	
	float4 color2 = tex2D(ColorMapSampler, Tex);
	float4 colorCrys = tex2D(crysTex ,Tex*4-rectangleA); 
    //float4 color3 = tex2D(ColorMapSampler, Tex + blendTexturePosition2); 
    
    
	//Color.rgb = (Color.r + Color.g + Color.b)/3;
	Color.rgb = dot(Color.rgb, float3(0.3, .59, .11));
	if (powerRed == 1)
	{
			if (color2.r*.66 > color2.b && color2.r*.66 > color2.g)
			{
				Color = color2;
			}
			if(powerGreen == 1)
			{
				if(color2.g*.66 > color2.b)
					Color = color2;
					if(powerBlue == 1)
						Color = color2;
			}
			if(powerBlue == 1)
			{
				if(color2.b*.66 > color2.g)
					Color = color2;
			}
	}
	if (powerGreen == 1)
	{
			if (color2.g*.66 > color2.b && color2.g*.66 > color2.r)
			{
				Color = color2;
			}
			if(powerBlue == 1)
			{
				if(color2.b*.66 > color2.r)
					Color = color2;
			}
			
	}
	if (powerBlue == 1)
	{
			if (color2.b*.66 > color2.r && color2.b*.66 > color2.g)
			{
				Color = color2;
			}
	}
	//Color = colorCrys;
	//if (Tex.x > rectangleA.x && Tex.x <= rectangleA.x+rectangleA.z && Tex.y > rectangleA.y && Tex.y <= rectangleA.y+rectangleA.z)
        //if (colorCrys.g > color2.g)//  && Tex.r > rectangleA.r)
           //Color = color2;

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