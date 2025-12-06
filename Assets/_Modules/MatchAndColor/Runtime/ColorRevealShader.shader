Shader "Custom/ColorReveal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RevealPos ("Reveal Position", Vector) = (0, 0, 0, 0)
        _RevealRadius ("Reveal Radius", Float) = 0
        _GrayscaleAmount ("Grayscale Amount", Range(0, 1)) = 1
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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _RevealPos;
            float _RevealRadius;
            float _GrayscaleAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Lấy màu gốc từ texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Tính khoảng cách từ pixel hiện tại đến điểm reveal
                float dist = distance(i.worldPos.xy, _RevealPos.xy);
                
                // Tạo grayscale
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
                fixed4 grayCol = fixed4(gray, gray, gray, col.a);
                
                // Tính toán giá trị blend dựa trên khoảng cách
                // Nếu dist < _RevealRadius thì hiện màu, nếu không thì xám
                float revealAmount = 1 - saturate((dist - _RevealRadius) / 0.5);
                
                // Blend giữa grayscale và màu gốc
                fixed4 finalCol = lerp(grayCol, col, revealAmount);
                
                return finalCol;
            }
            ENDCG
        }
    }
}