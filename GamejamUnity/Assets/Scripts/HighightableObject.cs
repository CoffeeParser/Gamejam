using UnityEngine;

namespace DrEvil.Visuals
{
    /// <summary>
    /// Simple class to highlight aimed objects
    /// </summary>
    public class HighightableObject : MonoBehaviour
    {
        public float OutLineMin = 0.0f;

        public float OutlineMax = 0.05f;

        public Material OutlineMaterial;
        // Use this for initialization

            /// <summary>
            /// Highlight object
            /// </summary>
        public void EnableHighlight()
        {
            OutlineMaterial.SetFloat("_Outline", OutlineMax);
        }

        /// <summary>
        /// Dehighlight object
        /// </summary>
        public void DisableHighlight()
        {
            OutlineMaterial.SetFloat("_Outline", OutLineMin);
        }


    }
}