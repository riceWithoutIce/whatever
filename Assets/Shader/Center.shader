Shader "Custom/Center" 
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
         CGPROGRAM

         #pragma vertex vert  
         #pragma fragment frag 

		 uniform float4 _ColorCenter;
		 uniform float4 _ColorOutside;
		 uniform float _Range;

         struct vertexInput 
		 {
            float4 vertex : POSITION;
         };
         struct vertexOutput 
		 {
            float4 pos : SV_POSITION;
            float4 position_in_world_space : TEXCOORD0;
         };

         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output; 
 
            output.pos =  mul(UNITY_MATRIX_MVP, input.vertex);
            output.position_in_world_space = 
               mul(_Object2World, input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR 
         {
             float dist = distance(input.position_in_world_space, float4(0.0f, 10.0f, 0.0f, 1.0f));
            
            if (dist < _Range)
               return float4(_ColorCenter.x, _ColorCenter.y, _ColorCenter.z, 1.0); 
            else
               return float4(_ColorOutside.x, _ColorOutside.y, _ColorOutside.z, 1.0); 
         }
 
         ENDCG  
      }
   }
}
