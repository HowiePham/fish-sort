Shader "Custom/ColorRevealMultiple"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GrayscaleAmount ("Grayscale Amount", Range(0, 1)) = 1
        _EdgeSoftness ("Edge Softness", Range(0.1, 2.0)) = 0.5
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

            // Số lượng vòng tròn reveal tối đa
            #define MAX_REVEALS 20

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
            float _GrayscaleAmount;
            float _EdgeSoftness;
            
            // Arrays cho nhiều reveal circles
            uniform float4 _RevealPositions[MAX_REVEALS]; // x, y, z = position, w = radius
            uniform int _RevealCount; // Số lượng reveals hiện tại

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
                
                // Tạo grayscale
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
                fixed4 grayCol = fixed4(gray, gray, gray, col.a);
                
                // Tính toán reveal amount từ TẤT CẢ các vòng tròn
                float maxRevealAmount = 0.0;
                
                for (int j = 0; j < _RevealCount; j++)
                {
                    // Lấy vị trí và radius của vòng tròn thứ j
                    float3 revealPos = _RevealPositions[j].xyz;
                    float revealRadius = _RevealPositions[j].w;
                    
                    // Tính khoảng cách từ pixel hiện tại đến tâm vòng tròn
                    float dist = distance(i.worldPos.xy, revealPos.xy);
                    
                    // Tính reveal amount cho vòng tròn này
                    // Sử dụng smoothstep để tạo edge mềm mại
                    float revealAmount = 1.0 - smoothstep(
                        revealRadius - _EdgeSoftness, 
                        revealRadius + _EdgeSoftness, 
                        dist
                    );
                    
                    // Lấy giá trị lớn nhất (nếu pixel nằm trong nhiều vòng tròn)
                    maxRevealAmount = max(maxRevealAmount, revealAmount);
                }
                
                // Blend giữa grayscale và màu gốc
                fixed4 finalCol = lerp(grayCol, col, maxRevealAmount);
                
                return finalCol;
            }
            ENDCG
        }
    }
}