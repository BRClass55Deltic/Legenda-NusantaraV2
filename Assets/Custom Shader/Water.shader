Shader "Custom/Water"
{
    Properties
    {
        _MainTex ("Normal Map", 2D) = "bump" {}
        _WaterColor ("Water Color", Color) = (0.1,0.4,0.6,1)

        _NormalStrength ("Normal Strength", Float) = 1
        _WaveSpeed ("Wave Speed", Float) = 0.1

        _WaveHeight ("Wave Height", Float) = 0.1
        _WaveFrequency ("Wave Frequency", Float) = 1

        _FresnelPower ("Fresnel Power", Float) = 5
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)

        _RefractionStrength ("Refraction Strength", Float) = 0.02
        _Alpha ("Transparency", Range(0,1)) = 0.7
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        GrabPass { "_GrabTexture" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _GrabTexture;

            float4 _WaterColor;
            float _NormalStrength;
            float _WaveSpeed;

            float _WaveHeight;
            float _WaveFrequency;

            float _FresnelPower;
            float4 _FresnelColor;

            float _RefractionStrength;
            float _Alpha;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float3 worldPos : TEXCOORD4;
            };

            v2f vert (appdata v)
            {
                v2f o;

                // 🌊 GERAK GELOMBANG BESAR
                float wave =
                    sin(v.vertex.x * _WaveFrequency + _Time.y * _WaveSpeed) +
                    cos(v.vertex.z * _WaveFrequency + _Time.y * _WaveSpeed);

                v.vertex.y += wave * _WaveHeight;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldPos = worldPos;

                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);

                o.screenPos = o.pos;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 🌊 NORMAL MAP GERAK (DETAIL KECIL)
                float2 uv1 = i.uv + _Time.y * _WaveSpeed;
                float2 uv2 = i.uv - _Time.y * _WaveSpeed * 0.7;

                float3 n1 = UnpackNormal(tex2D(_MainTex, uv1));
                float3 n2 = UnpackNormal(tex2D(_MainTex, uv2));

                float3 normalTex = normalize(n1 + n2);
                normalTex.xy *= _NormalStrength;

                // SCREEN UV
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                screenUV = screenUV * 0.5 + 0.5;

                // 💎 REFRACTION
                screenUV += normalTex.xy * _RefractionStrength;

                float3 refracted = tex2D(_GrabTexture, screenUV).rgb;

                // ✨ FRESNEL
                float fresnel = pow(
                    1 - dot(normalize(i.viewDir), normalize(i.worldNormal)),
                    _FresnelPower
                );

                // 🎨 FINAL COLOR
                float3 water = refracted * _WaterColor.rgb;
                float3 finalColor = water + fresnel * _FresnelColor.rgb;

                return float4(finalColor, _Alpha);
            }

            ENDCG
        }
    }
}