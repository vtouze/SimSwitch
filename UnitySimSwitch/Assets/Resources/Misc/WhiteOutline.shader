Shader "Unlit/WhiteOutline"
{
    SubShader
    {
        Tags { "RenderType"="Overlay" }
        Pass
        {
            Name "OUTLINE"
            ZWrite On
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            Offset 10, 10
            Color (1,1,1,1)
        }
    }
}