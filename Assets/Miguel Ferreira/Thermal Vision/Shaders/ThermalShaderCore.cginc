
struct appdata
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};

struct v2f
{
	float4 vertex : SV_POSITION;
	float4 posWorld : TEXCOORD0;
	float3 normalDir : TEXCOORD1;
};

sampler2D _ThermalColorLUT;

v2f vert (appdata v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.posWorld = mul(unity_ObjectToWorld, v.vertex);
	o.normalDir = normalize( mul( float4( v.normal, 0.0 ), unity_WorldToObject ).xyz );;

	return o;
}

fixed4 thermal_frag (v2f i,float thermalPowExponent,float maxTemperature) : SV_Target
{
	float3 viewDirection = normalize( _WorldSpaceCameraPos.xyz - i.posWorld.xyz );
	float3 normalDirection = i.normalDir;

	float dotProduct = saturate(dot(viewDirection,normalDirection));
	float temperature = pow(dotProduct, thermalPowExponent);
	temperature = lerp(0, maxTemperature,temperature);
	half4 col = tex2D(_ThermalColorLUT,temperature);
	return col;
}