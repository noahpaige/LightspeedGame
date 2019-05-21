using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleFlicker : MonoBehaviour
{
    private TextMeshProUGUI text;

    [Range(1, 40)] public  int underlayFlickerDelta = 10;
    [Range(1, 40)] public  int glowFlickerDelta     = 10;

    [Range(0, 255)] public int glowMinAlpha = 140;
    [Range(0, 255)] public int glowMaxAlpha = 200;

    [Range(0, 255)] public int underlayMinAlpha = 100;
    [Range(0, 255)] public int underlayMaxAlpha = 180;

    [Range(0.0001f, 0.1f)] public float jumpFrequency = 0.04f;


    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //text.fontMaterial = text.material;
        Material textMat = text.fontMaterial;
        Color32 oldUnderlayColor = textMat.GetColor(ShaderUtilities.ID_UnderlayColor);
        Color32 oldGlowColor     = textMat.GetColor(ShaderUtilities.ID_GlowColor);

        //Debug.Log("Old glow a " + oldGlowColor.a);

        bool jump = Random.Range(0f, 1f) < jumpFrequency;

        if(jump)
        {
            Color32 newUnderlayColor = new Color32(oldUnderlayColor.r, oldUnderlayColor.g, oldUnderlayColor.b, (byte)Random.Range(underlayMinAlpha, underlayMaxAlpha));
            Color32 newGlowColor     = new Color32(oldGlowColor.r, oldGlowColor.g, oldGlowColor.b, (byte)Random.Range(glowMinAlpha, glowMaxAlpha));

            textMat.SetColor(ShaderUtilities.ID_UnderlayColor, newUnderlayColor);
            textMat.SetColor(ShaderUtilities.ID_GlowColor, newGlowColor);
        }
        else
        {
            Color32 newUnderlayColor = new Color32(oldUnderlayColor.r, 
                                                   oldUnderlayColor.g, 
                                                   oldUnderlayColor.b,
                                                   (byte)Random.Range(Mathf.Max(oldUnderlayColor.a - underlayFlickerDelta, underlayMinAlpha), 
                                                                      Mathf.Min(oldUnderlayColor.a + underlayFlickerDelta, underlayMaxAlpha)));
            Color32 newGlowColor = new Color32(oldGlowColor.r,
                                               oldGlowColor.g,
                                               oldGlowColor.b,
                                               (byte)Random.Range(Mathf.Max(oldGlowColor.a - glowFlickerDelta, glowMinAlpha),
                                                                  Mathf.Min(oldGlowColor.a + glowFlickerDelta, glowMaxAlpha)));


            textMat.SetColor(ShaderUtilities.ID_UnderlayColor, newUnderlayColor);
            textMat.SetColor(ShaderUtilities.ID_GlowColor, newGlowColor);
        }
        text.material = textMat;
        //textMat.EnableKeyword("UNDERLAY_ON");
        //textMat.DisableKeyword("UNDERLAY_INNER");
        text.UpdateMeshPadding();
    }
}
