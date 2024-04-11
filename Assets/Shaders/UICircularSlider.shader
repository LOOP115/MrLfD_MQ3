Shader "Custom/UICircularSlider"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _MinDist ("Min Distance", Float) = 0.25
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Fog { Mode Off }

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
            float4 _MainTex_ST;
            fixed4 _Color;
            float _MinDist;
            float _SliderValue;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate distance from the UV center (0.5, 0.5) to the current fragment
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                // Discard fragments closer than _MinDist to the center
                if(dist < _MinDist)
                {
                    discard;
                }

                // Sample the texture color and apply the alpha value from the _Color property
                fixed4 col = tex2D(_MainTex, i.uv);

                return col;
            }
            ENDCG
        }
    }
}
