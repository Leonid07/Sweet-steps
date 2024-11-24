using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public Button buttonBuy;

    public CanvasGroup characte;
    public CanvasGroup characteButton;
    public float duration = 1f;

    public int isBuy = 0;

    private void Start()
    {
        buttonBuy.onClick.AddListener(BuyCat);
    }
    public void BuyCat()
    {
        if (GameManager.InstanceGame.gold >= Convert.ToInt32(characteButton.transform.GetChild(0).GetComponent<TMP_Text>().text))
        {
            Buy();
            StartCoroutine(FadeOut());
            StartCoroutine(FadeIn());
            DataManager.InstanceData.SaveBuyCharacter();

            int count = PanelManager.InstancePanel.swipePanel.currentPage;
            count--;
            if (DataManager.InstanceData.character[count].GetComponent<CharacterButton>().isBuy == 1)
            {
                DataManager.InstanceData.chraracterMainMenu.sprite
    = DataManager.InstanceData.character[count].GetComponent<CharacterButton>()
    .characte.GetComponent<Image>().sprite;
            }
        }
        else
        {
            return;
        }
    }

    public void LoadCat()
    {
        if (isBuy == 1)
        {
            StartCoroutine(FadeOut());
            StartCoroutine(FadeIn());
        }
    }

    public void Buy()
    {
        GameManager.InstanceGame.gold -= Convert.ToInt32(characteButton.transform.GetChild(0).GetComponent<TMP_Text>().text);
        DataManager.InstanceData.SaveGold();
        isBuy = 1;
    }

    // Корутин для плавного уменьшения alpha
    // BUTTON
    IEnumerator FadeOut()
    {
        float startAlpha = characteButton.alpha;  // Текущее значение alpha
        float elapsedTime = 0f;                // Время с начала анимации

        while (elapsedTime < duration)
        {
            // Процент завершённости анимации
            float t = elapsedTime / duration;
            // Линейное уменьшение alpha
            characteButton.alpha = Mathf.Lerp(startAlpha, 0f, t);
            elapsedTime += Time.deltaTime;

            // Ждём до следующего кадра
            yield return null;
        }

        // Устанавливаем финальное значение
        characteButton.alpha = 0f;
    }

    // Корутин для плавного увеличения alpha
    // character
    IEnumerator FadeIn()
    {
        float startAlpha = characte.alpha;  // Текущее значение alpha
        float elapsedTime = 0f;                // Время с начала анимации
        while (elapsedTime < duration)
        {
            // Процент завершённости анимации
            float t = elapsedTime / duration;
            // Линейное увеличение alpha
            characte.alpha = Mathf.Lerp(startAlpha, 1f, t);
            elapsedTime += Time.deltaTime;

            // Ждём до следующего кадра
            yield return null;
        }

        // Устанавливаем финальное значение
        characte.alpha = 1f;
    }
}
