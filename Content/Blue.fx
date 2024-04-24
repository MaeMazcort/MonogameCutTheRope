#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;
// Vertex Shader
float4x4 World;
float4x4 View;
float4x4 Projection;
float4 ColorTint: register(c0);

struct VertexShaderInput
{
    float4 Position : POSITION0;
   float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

VertexShaderOutput MainVS(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    output.Color = input.Color;
    output.Position = mul(input.Position, mul(World, mul(View, Projection)));
    
    return output;
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = input.Color;
    
    color = ColorTint * input.Color;
    
    return color;
}

// Technique
technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 MainVS();
        PixelShader = compile ps_3_0 MainPS();
    }
}