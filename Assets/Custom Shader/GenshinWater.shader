Shader "Custom/GenshinWater"
{
    Properties
    {
        _ShallowColor ("Shallow Color", Color) = (0.3,0.8,1,1)
        _DeepColor ("Deep Color", Color) = (0,0.25,0.6,1)

        _WaveSpeed ("Wave Speed", Float) = 1
        _WaveStrength ("Wave Strength", Float) = 0.05

        _FresnelPower ("Fresnel Power", Float) = 3
        _FresnelColor ("Fresnel Color", Color) = (0.5,1,1,1)

        _FoamColor ("Foam Color", Color) = (1,1,1,1)
        _FoamDistance ("Foam Distance", Float) = 0.3

        _RefractionStrength ("Refraction Strength", Float) = 0.005

        _Alpha ("Transparency", Range(0,1)) = 0.75
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

            sampler2D _GrabTexture;
            sampler2D _CameraDepthTexture;

            float4 _ShallowColor;
            float4 _DeepColor;

            float _WaveSpeed;
            float _WaveStrength;

            float _FresnelPower;
            float4 _FresnelColor;

            float4 _FoamColor;
            float _FoamDistance;

            float _RefractionStrength;

            float _Alpha;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
            };

            v2f vert (appdata v)
            {
                v2f o;

                float wave = sin(v.vertex.x * 2 + _Time.y * _WaveSpeed) *
                             cos(v.vertex.z * 2 + _Time.y * _WaveSpeed);

                v.vertex.y += wave * _WaveStrength;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = o.pos;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldPos = worldPos;

                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // SCREEN UV
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                screenUV = screenUV * 0.5 + 0.5;

                // REFRACTION (HALUS)
                float distortion = sin(i.worldPos.x * 4 + _Time.y) *
                                   cos(i.worldPos.z * 4 + _Time.y);

                screenUV += distortion * _RefractionStrength;

                float3 refracted = tex2D(_GrabTexture, screenUV).rgb;

                // DEPTH (FIXED)
                float sceneDepth = LinearEyeDepth(
                    tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r
                );

                float waterDepth = LinearEyeDepth(i.screenPos.z / i.screenPos.w);

                float depthDiff = sceneDepth - waterDepth;

                // FOAM (PINGGIR DOANG)
                float foam = saturate(depthDiff / _FoamDistance);
                foam = 1 - foam;
                foam = smoothstep(0.6, 1.0, foam);

                // WATER COLOR
                float depthLerp = saturate(depthDiff * 0.2);
                float3 waterColor = lerp(_ShallowColor.rgb, _DeepColor.rgb, depthLerp);

                // FRESNEL
                float fresnel = pow(
                    1 - dot(normalize(i.viewDir), normalize(i.worldNormal)),
                    _FresnelPower
                );

                // FINAL COLOR (NO MORE MILK 😤)
                float3 finalColor = refracted * 0.3 + waterColor * 0.7;

                finalColor += fresnel * _FresnelColor.rgb * 0.5;
                finalColor += foam * _FoamColor.rgb;

                return float4(finalColor, _Alpha);
            }

            ENDCG
        }
    }
}