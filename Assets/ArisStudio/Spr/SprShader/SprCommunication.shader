Shader "Spr/SprCommunication"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255

        _ColorMask("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0


        [Space(100)]
        [Toggle(_ENABLE_SIGNAL)] _ENABLE_SIGNAL("EnableSignal", Int) = 0
        [Space(20)]
        [NoScaleOffset]
        _DragTex("DragTex", 2D) = "white" {}
        _DragInterval("DragInterval", Range(1, 5)) = 2
        _DragStrength("DragStrength", Range(-0.08, 0.08)) = 0.004
        [Space(20)]
        _MainColor("MainColor", Color) = (0.5725, 0.7764, 1, 1)
        [Space(20)]
        _StripeColor("StripeColor", Color) = (0.8, 0.8, 0.8, 1)
        _StripeWidth("StripeWidth", Range(1, 10)) = 3
        [Space(20)]
        [NoScaleOffset]
        _FlowLightTex("FlowLightTex", 2D) = "white" {}
        _FlowLightSpeed("FlowLightSpeed", Range(0.001, 5)) = 1
        _SignalNoise("SignalNoise", Range(0, 1)) = 0.1

    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest[unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            #pragma multi_compile __ _ENABLE_SIGNAL

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            sampler2D _DragTex;
            half _DragInterval;
            half _DragStrength;
            half4 _MainColor;
            half4 _StripeColor;
            half _StripeWidth;
            sampler2D _FlowLightTex;
            float _FlowLightSpeed;
            float _SignalNoise;


            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            float random(float x, float y)
            {
                return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 totalColor;

                half dragOffset = (_Time.y % _DragInterval);
                if (dragOffset > 0 && dragOffset < 1)
                {
                    IN.texcoord.x = IN.texcoord.x + random(IN.texcoord.y, _Time.y) * _DragStrength * tex2D(_DragTex, IN.texcoord).r *
                        dragOffset;
                }
                totalColor = tex2D(_MainTex, IN.texcoord);
                _StripeWidth = _StripeWidth * 0.005;
                if (fmod((-IN.texcoord.y + 1 + _Time.y * 0.05) % 1, _StripeWidth) > _StripeWidth * 0.5)
                {
                    totalColor = totalColor * _StripeColor;
                }
                totalColor.rgb *= saturate(random(IN.texcoord.x, IN.texcoord.y % 0.01 * _Time.y) + (1 - _SignalNoise));

                totalColor = totalColor * _MainColor;

                half4 color = (totalColor + _TextureSampleAdd) * IN.color;

                #ifdef UNITY_UI_CLIP_RECT
					color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
					clip(color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
}
