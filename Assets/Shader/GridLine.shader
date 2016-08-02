Shader "Custom/GridLine" 
{
	SubShader 
	{
		Pass
		{
			//Tags { "Queue" = "Background - 20" }
			Blend SrcAlpha OneMinusSrcAlpha

			BindChannels
			{
				Bind "vertex", vertex Bind "color", color
			}
		}
	}
}
