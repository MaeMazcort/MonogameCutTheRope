#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4 ColorTint : register(c0);

// Pixel Shader entrypoint
float4 MainPS(float4 color : COLOR) : SV_TARGET
{
    // Multiplica el color del píxel por el ColorTint
    return color * ColorTint;
}

// Técnica predeterminada
technique Technique1
{
    pass Pass1
    {
        // Establece el shader de píxeles
        PixelShader = compile ps_3_0 MainPS();
    }
}