Shader "Hidden/Custom/PostDither"
{
    HLSLINCLUDE 

    #define OFF -1
    #define BLACK_AND_WHITE 0
    #define COLORS_8 1
    #define COLORS_27 2
    #define COLORS_64 3

    #define DITHER_MODE COLORS_27

    // StdLib.hlsl holds pre-configured vertex shaders (VertDefault), varying structs (VaryingsDefault), and most of the data you need to write common effects.
    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_Noise, sampler_Noise);
    float _CorrectGamma;

    float _TexWidth;
    float _TexHeight;

    float step4(float t, float4 vals) {
        return
            step(t, 0.16667) * vals.r +
            step(0.16667, t) * step(t, 0.5) * vals.g +
            step(0.5, t) * step(t, 0.8333) * vals.b +
            step(0.8333, t) * vals.a;
    }

    float step3(float t, float3 vals) {
        return
            step(t, 0.25) * vals.r +
            step(0.25, t) * step(t, 0.75) * vals.g +
            step(0.75, t) * vals.b;
    }

    // Lerp the pixel color with the luminance using the _Blend uniform.
    float4 Frag(VaryingsDefault i) : SV_Target
    {
        /*
        we could get aspect using differentials, yet we get them from Unity. Keeping this for reference
        float dx = ddx(i.texcoord.x);
        float dy = ddy(i.texcoord.y);
        float aspect = dx / dy;
        */

        // scaling for screen aspect ratio and relative proportion of viewport pixel size to texture pixel size at once
        // after transforming the uv with this, we sample a texel that exactly corresponds with fragment position
        // (the texture mode should pr "repeat" so texture is tiled
        float2 aspectAndScale = 
            // xy holds pixel width and height of viewport
            _ScreenParams.xy / 
            // TODO: hard-coded noise texture dimension. If noise size changes, so should this value
            float2(_TexWidth, _TexHeight);

        float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
      
        #if DITHER_MODE == OFF
            return color;
        #endif

        color = pow(color, _CorrectGamma);
        
        float4 blueNoise = SAMPLE_TEXTURE2D(_Noise, sampler_Noise, aspectAndScale * i.texcoord);

        #if DITHER_MODE == BLACK_AND_WHITE
            // Compute the luminance for the current pixel
            float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
            return float4( step(-luminance, -blueNoise.r).rrr, 1.0 );
        #elif DITHER_MODE == COLORS_8
            return float4( step(-color.rgb, -blueNoise.rgb), 1.0 );
        #elif DITHER_MODE == COLORS_27
            float4 unbiasedBlueNoise = (blueNoise - 0.5) / 3.0;
            float3 samples = float3(0, 0.5, 1.0);
            return float4( 
                step3( unbiasedBlueNoise.r + color.r, samples ), 
                step3( unbiasedBlueNoise.g + color.g, samples ), 
                step3( unbiasedBlueNoise.b + color.b, samples ), 
                1.0 
            );
        #elif DITHER_MODE == COLORS_64
            float4 unbiasedBlueNoise = (blueNoise - 0.5) / 4.0;
            float4 samples = float4(0, 0.3333, 0.6667, 1.0);
            return float4( 
                step4( unbiasedBlueNoise.r + color.r, samples ), 
                step4( unbiasedBlueNoise.g + color.g, samples ), 
                step4( unbiasedBlueNoise.b + color.b, samples ), 
                1.0 
            );
        #endif
    }
    ENDHLSL
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
        }
    }
}
