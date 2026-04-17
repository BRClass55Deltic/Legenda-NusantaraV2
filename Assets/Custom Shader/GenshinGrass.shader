Shader "Custom/GenshinGrass"
{
    Properties
    {
        _MainTex ("Grass Texture", 2D) = "white" {}
        _TopColor ("Top Color", Color) = (0.5, 1, 0.5, 1)
        _BottomColor ("Bottom Color", Color) = (0, 0.5, 0, 1)
        _WindSpeed ("Wind Speed", Range(0, 5)) = 1
        _WindStrength ("Wind Strength", Range(0, 1)) = 0.2
    }

    SubShader
    {
        // Penting: RenderType harus Grass agar Terrain mengenalinya
        Tags { "RenderType"="TransparentCutout" "Queue"="Geometry" "IgnoreProjector"="True" }
        LOD 100
        Cull Off 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Baris keramat untuk GPU Instancing
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID // Tambahkan ID Instance
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID // Teruskan ID ke fragment
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TopColor;
            float4 _BottomColor;
            float _WindSpeed;
            float _WindStrength;

            v2f vert (appdata v)
            {
                v2f o;
                
                // Inisialisasi Instancing
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                // Ambil posisi dunia agar angin tidak gerak barengan semua
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                // Animasi Angin
                float wind = sin(_Time.y * _WindSpeed + worldPos.x + worldPos.z);
                // v.uv.y digunakan agar akar rumput (0) tidak gerak, ujung (1) gerak
                v.vertex.x += wind * _WindStrength * v.uv.y;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = lerp(_BottomColor, _TopColor, v.uv.y);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i); // Setup instancing di fragment
                
                fixed4 tex = tex2D(_MainTex, i.uv);
                
                // Gunakan clip jika tekstur kamu punya alpha
                clip(tex.a - 0.5);
                
                return tex * i.color;
            }
            ENDCG
        }
    }
}