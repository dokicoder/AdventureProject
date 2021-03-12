using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

//The Serializable attribute allows Unity to serialize this class and extend PostProcessEffectSettings.
[Serializable] 
// The [PostProcess()] attribute tells Unity that this class holds post-processing data. The first parameter links the settings to a renderer. The second parameter creates the injection point for the effect. The third parameter is the menu entry for the effect. You can use a forward slash (/) to create sub-menu categories. 
[PostProcess(typeof(DitherRenderer), PostProcessEvent.AfterStack, "Custom/Dither", allowInSceneView: false)]
public sealed class Dither : PostProcessEffectSettings
{
    [Range(0.1f, 10f), Tooltip("Gamma Correction")] 
    public FloatParameter gamma = new FloatParameter { value = 1.0f };

    [Range(0.0f, 10f), Tooltip("Scaling of the noise threshold. Larger value will lead to a more noisy appearance")] 
    public FloatParameter noiseScaling = new FloatParameter { value = 1.0f };

    public TextureParameter noise = new TextureParameter { };

    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        // TODO: why needed? Ideally, I would want this to be controlled from the outside
        return true;
        /*
        return enabled.value
            && blend.value > 0f;
        */
    }

}

public sealed class DitherRenderer : PostProcessEffectRenderer<Dither>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/PostDither"));
        sheet.properties.SetFloat("_CorrectGamma", settings.gamma);
        sheet.properties.SetFloat("_NoiseScaling", settings.noiseScaling);

        sheet.properties.SetTexture("_Noise", settings.noise);
        sheet.properties.SetFloat("_TexWidth", settings.noise.value.width);
        sheet.properties.SetFloat("_TexHeight", settings.noise.value.height);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}