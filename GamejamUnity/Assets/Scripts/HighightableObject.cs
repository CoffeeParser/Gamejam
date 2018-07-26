using UnityEngine;

public class HighightableObject : MonoBehaviour
{
    public float OutLineMin = 0.0f;

    public float OutlineMax = 0.05f;

    public Material OutlineMaterial;
	// Use this for initialization
    public void EnableHighlight()
    {
        OutlineMaterial.SetFloat("_Outline", OutlineMax);
    }

    public void DisableHighlight()
    {
        OutlineMaterial.SetFloat("_Outline", OutLineMin);
    }
	

}
