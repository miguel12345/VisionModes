Shader "MiguelFerreira/NightVision"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
 
			#include "UnityCG.cginc"
 
			sampler2D _MainTex;
			sampler2D _CameraGBufferTexture0;
 			float _DeferredContribution;

			float4 frag(v2f_img i) : COLOR {
				half4 col = tex2D(_MainTex, i.uv);
				float luminance = Luminance (col.rgb);
				col.rgb = dot(col.rgb,half3(0.57,0.57,0.57));

				half4 deferredDiffuse = tex2D(_CameraGBufferTexture0, i.uv);			
				col.rgb += deferredDiffuse.rgb * (luminance +_DeferredContribution);
				col.rb = max (col.r - 0.75, 0);

				return col;
			}
			ENDCG
		}
	}
}
