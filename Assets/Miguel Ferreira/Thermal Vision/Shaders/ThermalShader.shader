// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/ThermalShader"
{
	Properties
	{
		_ThermalColorLUT ("Thermal Color Look-up texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Thermal" = "Enabled" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "ThermalShaderCore.cginc"

			float _ThermalPowExponent;
			float _ThermalMax;
			float _ThermalMin;
			
			fixed4 frag (v2f i) : SV_Target
			{
				return thermal_frag(i,_ThermalPowExponent,_ThermalMax,_ThermalMin);
			}
			ENDCG
		}

	}

}
