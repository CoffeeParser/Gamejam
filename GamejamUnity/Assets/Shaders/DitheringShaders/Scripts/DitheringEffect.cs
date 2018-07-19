using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Other/Dithering")]
public class DitheringEffect : MonoBehaviour
{
    public int ColorCount = 4;
    public int PaletteHeight = 64;
    public Texture PaletteTexture;
    public int DitherSize = 8;
    public Texture DitherTexture;
    public Material DitherMaterial;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        DitherMaterial.SetFloat("_ColorCount", ColorCount);
        DitherMaterial.SetFloat("_PaletteHeight", PaletteHeight);
        DitherMaterial.SetTexture("_PaletteTex", PaletteTexture);
        DitherMaterial.SetFloat("_DitherSize", DitherSize);
        DitherMaterial.SetTexture("_DitherTex", DitherTexture);
        Graphics.Blit(source, DitherMaterial);
    }
}