Shader "Custom/WuWaWater"
{
    Properties
    {
        _DeepColor ("Deep Color", Color) = (0.02, 0.1, 0.15, 1)
        _ShallowColor ("Shallow Color", Color) = (0.1, 0.4, 0.4, 0.8)
        _RimColor ("Fresnel Color", Color) = (0.5, 0.8, 0.9, 1)
        _CausticsColor ("Caustics Color", Color) = (1, 1, 1, 1)
        
        _CausticsScale ("Caustics Scale", Range(0.1, 10)) = 2.0
        _CausticsSpeed ("Caustics Speed", Range(0, 2)) = 0.5
        _CausticsSplit ("Caustics Sharpness", Range(0.01, 0.5)) = 0.1
        _FresnelPower ("Fresnel Power", Range(0.1, 10)) = 3.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 normal : TEXCOORD3;
            };

            float4 _DeepColor, _ShallowColor, _RimColor, _CausticsColor;
            float _CausticsScale, _CausticsSpeed, _CausticsSplit, _FresnelPower;

            // Fungsi noise sederhana untuk menggantikan tekstur
            float2 hash22(float2 p) {
                p = float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)));
                return -1.0 + 2.0 * frac(sin(p) * 43758.5453123);
            }

            // Voronoi sederhana untuk pola air yang mulus
            float voronoi(float2 x) {
                float2 n = floor(x);
                float2 f = frac(x);
                float m = 8.0;
                for(int j=-1; j<=1; j++)
                for(int i=-1; i<=1; i++) {
                    float2 g = float2(float(i), float(j));
                    float2 o = hash22(n + g);
                    float2 r = g + o - f;
                    float d = dot(r, r);
                    if(d < m) m = d;
                }
                return sqrt(m);
            }

            v2f vert (appdata v) {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - o.worldPos);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // 1. Fresnel
                float fresnel = pow(1.0 - saturate(dot(i.normal, i.viewDir)), _FresnelPower);
                
                // 2. Caustics (Dibuat dua layer supaya tidak kaku)
                float2 uv = i.worldPos.xz * _CausticsScale;
                float t = _Time.y * _CausticsSpeed;
                
                float c1 = voronoi(uv + t);
                float c2 = voronoi(uv * 1.5 - t * 0.5);
                
                // Efek "Sharp Line" khas WuWa
                float pattern = min(c1, c2);
                float caustics = smoothstep(_CausticsSplit, 0.0, pattern);

                // 3. Warna
                fixed4 baseColor = lerp(_DeepColor, _ShallowColor, 0.5);
                fixed4 col = lerp(baseColor, _CausticsColor, caustics * 0.5);
                col = lerp(col, _RimColor, fresnel);

                col.a = lerp(_ShallowColor.a, 1.0, fresnel);
                return col;
            }
            ENDCG
        }
    }
}