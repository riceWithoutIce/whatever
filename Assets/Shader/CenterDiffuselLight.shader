Shader "Custom/CenterDiffuselLight" 
{
	Properties 
	{
		_ColorCenter ("ColorCenter", Color) = (1, 1, 1, 1)
		_ColorOutside ("ColorOutSide", Color) = (1, 1, 1, 1)
		_Range("Range", Range(0, 20)) = 5
	}
	SubShader 
	{
		Pass
		{
			Tags { "LightMode" = "ForwardBase" } 
		
			CGPROGRAM

			#pragma vertex vert  
			#pragma fragment frag 

			#include "UnityCG.cginc"

			uniform float4 _LightColor0;

			uniform float4 _ColorCenter;
			uniform float4 _ColorOutside;
			uniform float _Range;

			struct vertexInput 
			{
			   float4 vertex : POSITION;
			   float3 normal : NORMAL;
			};
			struct vertexOutput 
			{
			   float4 pos : SV_POSITION;
			   float4 position_in_world_space : TEXCOORD0;
			   float4 col : COLOR;
			};

			vertexOutput vert(vertexInput input) 
			{
			   vertexOutput output;
			   
			   float4x4 modelMatrix = _Object2World;
			   float4x4 modelMatrixInverse = _World2Object;

			   float3 normalDirection = normalize(mul(float4(input.normal, 0.0f), modelMatrixInverse).xyz);
			   float3 lightDirection;
			   float attenuation;

			   if (0.0f == _WorldSpaceLightPos0.w)
			   {
					attenuation = 1.0f;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
			   }
			   else
			   {
					float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - mul(modelMatrix, input.vertex).xyz;
					float distance = length(vertexToLightSource);
					attenuation = 1.0f / distance;
					lightDirection = normalize(vertexToLightSource);
			   }
			   float3 diffuseReflection = attenuation * _LightColor0.rgb * _ColorOutside.rgb * max(0.0f, dot(normalDirection, lightDirection));

			   output.col = float4(diffuseReflection, 1.0f);
			   output.pos =  mul(UNITY_MATRIX_MVP, input.vertex);
			   output.position_in_world_space = mul(_Object2World, input.vertex);
			   return output;
			}
 
			float4 frag(vertexOutput input) : COLOR 
			{
				float dist = distance(input.position_in_world_space, float4(0.0f, 10.0f, 0.0f, 1.0f));
            
				if (dist < _Range)
				   return float4(_ColorCenter.x, _ColorCenter.y, _ColorCenter.z, 1.0); 
				else
				   return input.col; 
			}
			ENDCG  
      }
   }
   FallBack "Diffuse"
}
