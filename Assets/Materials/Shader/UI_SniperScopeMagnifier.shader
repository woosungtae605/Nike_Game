Shader "UI/Sniper Scope Magnifier"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _Center ("Scope Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Radius", Range(0.01, 1)) = 0.24
        _Softness ("Edge Softness", Range(0.001, 0.25)) = 0.025
        _Zoom ("Zoom", Range(1, 8)) = 2
        _OutsideDarkAlpha ("Outside Dark Alpha", Range(0, 1)) = 0
        _RingColor ("Ring Color", Color) = (0, 0, 0, 0.85)
        _RingWidth ("Ring Width", Range(0, 0.2)) = 0.015

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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
                float4 screenPosition : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _CameraOpaqueTexture;
            fixed4 _Color;
            fixed4 _RingColor;
            float4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _Center;
            float _Radius;
            float _Softness;
            float _Zoom;
            float _OutsideDarkAlpha;
            float _RingWidth;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.screenPosition = ComputeScreenPos(OUT.vertex);
                OUT.texcoord = v.texcoord;
                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 screenUV = IN.screenPosition.xy / IN.screenPosition.w;
                float2 center = _Center.xy;

                float2 fromCenter = screenUV - center;
                float distanceFromCenter = length(fromCenter);

                float innerMask = 1.0 - smoothstep(_Radius - _Softness, _Radius, distanceFromCenter);
                float ringMask = 1.0 - smoothstep(_Radius, _Radius + _Softness, distanceFromCenter);
                ringMask *= smoothstep(_Radius - _RingWidth - _Softness, _Radius - _RingWidth, distanceFromCenter);

                float2 zoomUV = center + fromCenter / max(_Zoom, 0.0001);
                fixed4 zoomColor = tex2D(_CameraOpaqueTexture, zoomUV) * IN.color;

                fixed4 outsideColor = fixed4(0, 0, 0, _OutsideDarkAlpha);
                fixed4 color = lerp(outsideColor, zoomColor, innerMask);
                color = lerp(color, _RingColor, ringMask * _RingColor.a);
                color.a = max(color.a, ringMask * _RingColor.a);

                color.a *= tex2D(_MainTex, IN.texcoord).a;

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
