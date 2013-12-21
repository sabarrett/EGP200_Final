uniform extern texture ScreenTexture;

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;
};

texture lightTexture;

sampler lightS = sampler_state
{
	Texture = <lightTexture>;
};

float4 PixelShaderFunction(float2 curCoord: TEXCOORD0) : COLOR
{
	float4 color = tex2D(ScreenS, curCoord);

	float4 lightColor = tex2D(lightS, curCoord);

	// add ambient lighting
	if (lightColor.r < 0.15)
		lightColor.r = 0.15;
	if (lightColor.g < 0.15)
		lightColor.g = 0.15;
	if (lightColor.b < 0.15)
		lightColor.b = 0.15;

	// add spot lighting
	color.r = color.r * lightColor.r;
	color.g = color.g * lightColor.g;
	color.b = color.b * lightColor.b;

    return color;
}

technique Technique1
{
    pass Pass0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
