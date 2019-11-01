﻿Shader "Cg per-vertex diffuse lighting" {
	Properties{
	   _Color("Diffuse Material Color", Color) = (1,1,1,1)
	}
		SubShader{
		   Pass {
			  Tags { "LightMode" = "ForwardBase" }
			  // make sure that all uniforms are correctly set

			 CGPROGRAM

			 #pragma vertex vert  
			 #pragma fragment frag 

			 #include "UnityCG.cginc"

			 uniform float4 _LightColor0;
			// color of light source (from "Lighting.cginc")

			 uniform float4 _Color; // define shader property for shaders
			 uniform float4 _SpecColor;
			 uniform float _Shininess;

			 struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			 };

			 struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 col : COLOR;
			 };

			 vertexOutput vert(vertexInput input)
			 {
				vertexOutput output;

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				float3 normalDirection = normalize(
				   mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				// alternative: 
				// float3 normalDirection = UnityObjectToWorldNormal(input.normal);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

				float3 diffuseReflection = _LightColor0.rgb * _Color.rgb
				   * max(0.0, dot(normalDirection, lightDirection));

				float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;

				//float3 specularReflection = 

				output.col = float4(diffuseReflection, 1.0);
				output.pos = UnityObjectToClipPos(input.vertex);
				return output;
			 }

			 float4 frag(vertexOutput input) : COLOR
			 {
				return input.col;
			 }

			 ENDCG
			}
	}
		Fallback "Diffuse"
}