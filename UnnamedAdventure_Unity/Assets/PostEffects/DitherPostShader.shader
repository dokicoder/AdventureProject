Shader "Hidden/Custom/PostDither"
{
    HLSLINCLUDE 
    // StdLib.hlsl holds pre-configured vertex shaders (VertDefault), varying structs (VaryingsDefault), and most of the data you need to write common effects.
    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_Noise, sampler_Noise);
    float _CorrectGamma;
    // Lerp the pixel color with the luminance using the _Blend uniform.
    float4 Frag(VaryingsDefault i) : SV_Target
    {
        /*
        we could get aspect using differentials. Not needed though. Keep this for reference
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
            float2(256, 256);

        float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

        color = pow(color, _CorrectGamma);

        float4 blueNoise = SAMPLE_TEXTURE2D(_Noise, sampler_Noise, aspectAndScale * i.texcoord);

        //return float4( step(color.rgb, blueNoise.rgb), 1.0 );

        //return float4( step(color.r, blueNoise.r), step(color.g, blueNoise.g), step(color.b, blueNoise.b), 1.0 );

    // Compute the luminance for the current pixel
        float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
        //color.rgb = lerp(color.rgb, luminance.xxx, _Blend.xxx);
        //float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
        //return float4( step( blueNoise.g, luminance ).rrr, 1.0 );
        return float4( step( blueNoise.rgb, color.rgb ), 1.0 );


        //float timeMod = fmod(_Time, 0.05) * 20;
        //return float4(timeMod, timeMod, timeMod, 1.0);

/*
        float4 noiseColor = SAMPLE_TEXTURE2D(_Noise, sampler_Noise, i.texcoord);

        return  float4(noiseColor.r, 1) * step(2.0, timeMod) +
                float4(noiseColor.g, 1) * (2.0 - step(2.0, timeMod));
*/

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
