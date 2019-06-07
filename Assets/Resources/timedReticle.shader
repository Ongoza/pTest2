Shader "Custom/timedReticle" {
  Properties {
    _Color  ("Color", Color) = ( 1, 1, 0, 1 )
	_Angle  ("Angle", Range(0, 360)) = 360
	//_Temp1("Temp1", Float) = 0
    _InnerDiameter ("InnerDiameter", Range(0, 10.0)) = 1.5
    _OuterDiameter ("OuterDiameter", Range(0.00872665, 10.0)) = 2.0
    _DistanceInMeters ("DistanceInMeters", Range(0.0, 100.0)) = 2.0
  }

  SubShader {
    Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
    Pass {
      Blend SrcAlpha OneMinusSrcAlpha
      AlphaTest Off
      Cull Back
      Lighting Off
      ZWrite Off
      ZTest Always

      Fog { Mode Off }
      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag
	  #pragma enable_d3d11_debug_symbols

      #include "UnityCG.cginc"

      uniform float4 _Color;
      uniform float _InnerDiameter;
      uniform float _OuterDiameter;
      uniform float _DistanceInMeters;
	  uniform float _Angle;

      struct vertexInput {
        float4 vertex : POSITION;
      };

      struct fragmentInput{
          float4 position : SV_POSITION;	  
      };

      fragmentInput vert(vertexInput i) {        		
		fragmentInput o;				
		float x = lerp(_OuterDiameter, _InnerDiameter, i.vertex.z);		
		float a = -(atan2(i.vertex.x, i.vertex.y) - (_Angle - 180)*0.0178);
		// start timer show
		//float scale = 0;
		//if (a > 0) {scale = x;}		
		float temp = max(0, a) + 0.1;
		float scale = min(x, temp);
		// end timer show
		float3 vert_out = float3(i.vertex.x * scale, i.vertex.y * scale, _DistanceInMeters);
		o.position = UnityObjectToClipPos (vert_out);		
		return o;
      }

      fixed4 frag(fragmentInput i) : SV_Target {       
        return _Color;
      }

      ENDCG
    }
  }
}