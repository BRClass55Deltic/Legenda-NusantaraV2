Shader "Custom/GenshinGrass"
{
    Properties
    {
        _MainTex ("Grass Texture", 2D) = "white" {}
        _TopColor ("Top Color", Color) = (0.5, 1, 0.5, 1)
        _BottomColor ("Bottom Color", Color) = (0, 0.5, 0, 1)
        _WindSpeed ("Wind Speed", Range(0, 5)) = 1
        _WindStrength ("Wind Strength", Range(0, 1)) = 0.2
        _WindFrequency ("Wind Frequency", Range(0, 10)) = 2
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" "IgnoreProjector"="True" }
        LOD 100
        Cull Off // Agar rumput terlihat dari depan & belakang

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
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TopColor;
            float4 _BottomColor;
            float _WindSpeed;
            float _WindStrength;
            float _WindFrequency;

            v2f vert (appdata v)
            {
                v2f o;
                
                // Ambil posisi dunia agar gerakan angin bervariasi tiap posisi
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                // Logika Angin: Gunakan Sine Wave
                // Kita gunakan v.uv.y agar hanya bagian atas rumput yang bergoyang (bawah tetap di tanah)
                float wind = sin(_Time.y * _WindSpeed + worldPos.x * _WindFrequency + worldPos.z * _WindFrequency);
                v.vertex.x += wind * _WindStrength * v.uv.y;
                v.vertex.z += wind * _WindStrength * 0.5 * v.uv.y;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                // Interpolasi warna berdasarkan tinggi (UV Y)
                o.color = lerp(_BottomColor, _TopColor, v.uv.y);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                
                // Alpha clipping agar bagian transparan tekstur hilang
                clip(tex.a - 0.5);
                
                // Kalikan warna tekstur dengan gradient warna kita
                return tex * i.color;
            }
            ENDCG
        }
    }
}