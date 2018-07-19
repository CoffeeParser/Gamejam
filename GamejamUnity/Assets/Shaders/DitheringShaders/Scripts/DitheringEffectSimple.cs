using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Other/Dithering Simple")]
public class DitheringEffectSimple : MonoBehaviour
{
    public int ColorCount = 4;
    public int PaletteHeight = 64;
    public Texture PaletteTexture;
    public Material DitherMaterial;


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        DitherMaterial.SetFloat("_ColorCount", ColorCount);
        DitherMaterial.SetFloat("_PaletteHeight", PaletteHeight);
        DitherMaterial.SetTexture("_PaletteTex", PaletteTexture);
        Graphics.Blit(source, DitherMaterial);
    }
}