Shader "Custom/CircularShadowShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Color Tint", Color) = (0,0,0,1)
        _Radius ("Shadow Radius", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Calculate distance from the center
                float dist = distance(i.uv, float2(0.5, 0.5));

                // Create a smooth circular mask
                float alpha = smoothstep(_Radius, _Radius - 0.05, dist);

                // Apply color tint and mask for circular shadow
                return fixed4(_Color.rgb, alpha * _Color.a) * texColor;
            }
            ENDCG
        }
    }
}