using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    public Image[] imageBG;
    public float changeInterval = 5f;
    public float fadeDuration = 1f;

    private Coroutine bgAnimationCoroutine;
    private int currentIndex = 0;

    public void StartAnimGB()
    {
        if (imageBG == null || imageBG.Length == 0)
        {
            return;
        }

        for (int i = 1; i < imageBG.Length; i++)
        {
            SetImageAlpha(imageBG[i], 0f);
        }

        bgAnimationCoroutine = StartCoroutine(SwitchImages());
    }

    void OnDestroy()
    {
        if (bgAnimationCoroutine != null)
            StopCoroutine(bgAnimationCoroutine);
    }

    IEnumerator SwitchImages()
    {
        while (true)
        {
            int nextIndex = (currentIndex + 1) % imageBG.Length;
            yield return StartCoroutine(FadeOutIn(imageBG[currentIndex], imageBG[nextIndex]));
            currentIndex = nextIndex;
            yield return new WaitForSeconds(changeInterval);
        }
    }

    IEnumerator FadeOutIn(Image currentImage, Image nextImage)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float newAlphaOut = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            float newAlphaIn = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            SetImageAlpha(currentImage, newAlphaOut);
            SetImageAlpha(nextImage, newAlphaIn);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetImageAlpha(currentImage, 0f);
        SetImageAlpha(nextImage, 1f);
    }

    void SetImageAlpha(Image image, float alpha)
    {
        Color imageColor = image.color;
        imageColor.a = alpha;
        image.color = imageColor;
    }

}
