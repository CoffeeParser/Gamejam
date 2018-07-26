using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_SpriteAnimator : MonoBehaviour {

    private Image image;
    public Sprite[] sprites;
    public float animationSpeed;

    public bool PlayOnce = false;
    public bool PlayOnEnable = true;

    public IEnumerator StartAnimate()
    {
        if (!PlayOnce)
        {
            while (true)
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    yield return new WaitForSeconds(animationSpeed);
                    image.sprite = sprites[i];
                }
            }
        }
        else
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                yield return new WaitForSeconds(animationSpeed);
                image.sprite = sprites[i];
            }
            yield return new WaitForSeconds(animationSpeed); // wait last frame again until its over to show current state
        }
    }

    private IEnumerator AnimateWithCallback(Action callbackAction)
    {
        yield return StartAnimate();
        callbackAction.Invoke();
    }

    private void OnEnable()
    {
        if (PlayOnEnable)
        {
            image = GetComponent<Image>();
            StartCoroutine(StartAnimate());
        }
    }

    /// <summary>
    /// Play Animation once and then Invoke calBackAction
    /// </summary>
    /// <param name="callbackAction"></param>
    public void PlayOneBurstWithCallback(Action callbackAction)
    {
        PlayOnce = true;
        image = GetComponent<Image>();
        StartCoroutine(AnimateWithCallback(callbackAction));
    }
}
