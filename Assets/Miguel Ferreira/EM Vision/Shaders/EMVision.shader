Shader "Hidden/EMVision"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{

        Tags {"RenderType" = "Opaque"}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float depth : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.depth = -UnityObjectToViewPos(v.vertex).z * _ProjectionParams.w;
				
				return o;
			}
			
			sampler2D _MainTex;
			float _EMStrength;

			fixed4 frag (v2f i) : SV_Target
			{
			    fixed3 nonElectricColor = i.depth;
			    fixed3 fullElecticColor = half3(1,1,1);
			    fixed4 electricColor = fixed4(lerp(nonElectricColor,fullElecticColor,_EMStrength),1.0);
			    
				return electricColor;
			}
			ENDCG
		}
	}
}
