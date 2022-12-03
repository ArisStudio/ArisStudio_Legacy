Shader "Custom/SmokeScroll"
{
    Properties{

_Color ("Color", Color) = (1,1,1,1) 
        _MainTex("Base (RGB)", 2D) = "white" {}
    _ScrollX("Scroll Speed", float) = 1.0
    }
        SubShader{
        Tags{ "RenderType" = "Opaque" "Queue" = "Background" }
        LOD 200


        Pass{
        Blend SrcAlpha OneMinusSrcAlpha
        Tags{ "LightMode" = "ForwardBase" }
 
        CGPROGRAM
        fixed4 _Color;
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
 
        sampler2D _MainTex;
    float4 _MainTex_ST;
    float _ScrollX;
 
    struct a2v {
        float4 vertex : POSITION;
        float4 texcoord : TEXCOORD0;
    };
 
    struct v2f {
        float4 pos : SV_POSITION;
        half2 uv : TEXCOORD0;
    };
 
    v2f vert(a2v v) {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = v.texcoord.xy*_MainTex_ST.xy + _MainTex_ST.zw;
        o.uv += frac(float2(_ScrollX, 0.0) * _Time.y);
        return o;
    }
 
    fixed4 frag(v2f i) : Color{
        fixed4 c = tex2D(_MainTex, i.uv);
        c.rgba *= _Color.rgba;
    return c;
    }
        ENDCG
    }
    }
        FallBack "VertexLit"
}