Shader "Custom/White" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader 
	{
		Pass
		{
			Cull Off
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform float4 _Color;

			float4 vert(float4 vertexPos : POSITION) : SV_POSITION
			{
				return mul(UNITY_MATRIX_MVP,  vertexPos);
			}

			float4 frag(void) : COLOR
			{
				return _Color;
			}

			ENDCG
		}
	}
}
