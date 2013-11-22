//
// Author:
//   Based on the Unity3D built-in shaders
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012 Edelweiss Interactive (http://www.edelweissinteractive.com)
//

Shader "Decal/Colored/Transparent Diffuse Colored" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader {
		Tags {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		Offset -1, -1

		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
			float4 color: Color;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color  * IN.color;
			o.Albedo = fixed3(c.a);
			o.Alpha = c.a;
		}
		ENDCG
	}

	Fallback "Decal/Transparent VertexLit"
}