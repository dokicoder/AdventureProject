 Shader "SurfaceOutline" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _OutlineAmount ("Outline Amount", Range(0,1)) = 0.2
        _OutlineColor ("OutlineColor", Color) = (0.3,0.7,0,1)
        _Enabled ("Enabled", Int) = 0
    }
    SubShader {
        Tags { "Queue" = "Overlay" }
        ZWrite Off
        ZTest Always
        // TODO: this is just in here cause the first dead body mesh was crappy - enable culling again for performance
        Cull Off

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert
        struct Input {
            float2 uv_MainTex;
        };

        float _OutlineAmount;
        fixed4 _OutlineColor;
        int _Enabled;

        void vert (inout appdata_full v) {
            v.vertex.xyz += v.normal * _OutlineAmount;
        }
        sampler2D _MainTex;
        void surf (Input IN, inout SurfaceOutput o) {
            o.Emission = _OutlineColor;

            if(!_Enabled) {
                discard;
            }
        }
        ENDCG

        Tags { "RenderType" = "Opaque" }
        LOD 200
        ZWrite On
        ZTest LEqual
        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    } 
    Fallback "Diffuse"
  }