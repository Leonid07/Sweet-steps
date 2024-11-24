using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public Slider loadingSlider;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }
    IEnumerator LoadSceneAsync()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            loadingSlider.value++;
            if (loadingSlider.value == loadingSlider.maxValue)
            {
                gameObject.SetActive(false);
                yield break;
            }
        }
    }
}