Shader "Custom/CreateMaskShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" "LightMode"="Always" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Alpha = 1.0;
			if(c.a > 0.0)
			{
				o.Alpha = 1.0;
				o.Albedo = half3(1.0, 1.0, 1.0);
			}
			else
			{
				o.Alpha = 0.0;
				o.Albedo = half3(0.0, 0.0, 0.0);
			}
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
