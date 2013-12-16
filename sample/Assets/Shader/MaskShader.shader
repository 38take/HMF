Shader "Custom/MaskShader" {
    Properties {
        _MainTex ("Base (RGBA)", 2D) = "white" {}
        _BlendTex ("Alpha Blended (RGBA) ", 2D) = "white" {}
    }
    SubShader {
    	Blend SrcAlpha OneMinusSrcAlpha
		//ZTest Always
        Pass {
            // ベーステクスチャを適用します。
            SetTexture [_MainTex] {
                combine texture
            }
            // lerp演算子を使用してアルファテクスチャをブレンドします。
            SetTexture [_BlendTex] {
                combine texture * previous
            }
        }
    }
//	SubShader {
//		Tags { "RenderType"="Opaque" }
//		LOD 200
//		ZTest Always
//		
//		CGPROGRAM
//		#pragma surface surf Lambert
//
//		sampler2D _MainTex; 
//		sampler2D _BlendTex; 
//
//		struct Input {
//			float2 uv_MainTex;
//		};
//
//		void surf (Input IN, inout SurfaceOutput o) {
//			half4 c = tex2D (_MainTex, IN.uv_MainTex);  
//			half4 mask = tex2D (_BlendTex, IN.uv_MainTex); 
//			o.Albedo = c.rgb;
//			o.Alpha = c.a * mask.a;
//		}
//		ENDCG
//	} 
//	FallBack "Diffuse"
}
