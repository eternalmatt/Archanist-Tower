//Holds all the information on which powers are activated.
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
	//Color is the color information that will be passed onto the screen, while color2 holds the default screen and is used for calculations.
	float4 Color = tex2D(ColorMapSampler, Tex);	
	float4 color2 = tex2D(ColorMapSampler, Tex);

	//The initial grayscale
	Color.rgb = dot(Color.rgb, float3(0.3, .59, .11));
	//if the red crystal is gained, it restores things that are primarily and strongly red.  Hence the .66 modifier.
	if (powerRed == 1)
	{
			if (color2.r*.66 > color2.b && color2.r*.66 > color2.g)
			{
				Color = color2;
			}
			//if Red and Green have been restored, this restores all strong mixtures of the two as well as the individual colors.  Same for blue later.
			if(powerGreen == 1)
			{
				if(color2.g*.66 > color2.b)
					Color = color2;
					//if all three are restored, all color is returned to normal.
					if(powerBlue == 1)
						Color = color2;
			}
			if(powerBlue == 1)
			{
				if(color2.b*.66 > color2.g)
					Color = color2;
			}
	}
	//As above, but only checks for green and blue, to save calculations.
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
	As above for blue.
	if (powerBlue == 1)
	{
			if (color2.b*.66 > color2.r && color2.b*.66 > color2.g)
			{
				Color = color2;
			}
	}
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