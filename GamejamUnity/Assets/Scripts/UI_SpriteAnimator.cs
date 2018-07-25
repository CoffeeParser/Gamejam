using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_SpriteAnimator : MonoBehaviour {

    private Image image;
    public Sprite[] sprites;
    public float animationSpeed;

    public IEnumerator StartAnimate()
    {
        while (true) {
            for (int i = 0; i < sprites.Length; i++)
            {
                yield return new WaitForSeconds(animationSpeed);
                image.sprite = sprites[i];
            }
        }
    }

    private void OnEnable()
    {
        image = GetComponent<Image>();
        StartCoroutine(StartAnimate());
    }
}
