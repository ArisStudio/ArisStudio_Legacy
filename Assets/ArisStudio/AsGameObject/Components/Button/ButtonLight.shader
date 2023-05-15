Shader "Aris Studio/Sprites/Border Light Flow"
{
    Properties
    {
        [HideInInspector] [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        [HDR] _BorderColor("Border Color", Color) = (1,1,1,1)

        _Length ("Length", Range(0,0.5)) = 0.4
        _Speed("Speed", float) = 3
        _Size("Size", Range(0,0.5))=0.2
        _Alpha("Alpha", Range(0,1)) = 1
        _StartAngle("Start Angle", Range( 0, 360 )) = 0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

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
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _BorderColor;

            float _Length;
            float _Speed;
            float _Size;
            float _Alpha;
            float _StartAngle;

            float2 pointbyangle(float _Angle)
            {
                float2 pt;
                if (_Angle > 45 && _Angle <= 135)
                {
                    pt.x = _Length / tan(radians(_Angle));
                    pt.y = _Length;
                    return pt;
                }
                if (_Angle > 135 && _Angle <= 225)
                {
                    pt.x = -_Length;
                    pt.y = -_Length * tan(radians(_Angle));
                    return pt;
                }
                if (_Angle > 225 && _Angle <= 315)
                {
                    pt.x = -_Length / tan(radians(_Angle));
                    pt.y = -_Length;
                    return pt;
                }
                pt.x = _Length;
                pt.y = _Length * tan(radians(_Angle));
                return pt;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                float2 uv = i.uv - 0.5;
                float2 pt = pointbyangle(fmod(_StartAngle + _Time.y * _Speed, 360));
                float l = clamp(1 - length(uv - pt) / (_Size * 1.414213562373), 0, 1);
                float4 bor = lerp(_BorderColor, _BorderColor * col.a, _Alpha);
                return lerp(float4(0, 0, 0, 0), bor, l);
            }
            ENDCG
        }
    }
}
