using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class FillAmountChangerGameOver : MonoBehaviour
{
    public Image fillImage; // Image с fillAmount (например, UI элемент с типом заполнения Fill)
    public TMP_Text timeText;   // Text для отображения времени
    public float duration = 5f; // Время, за которое будет выполнено заполнение

    public GameObject nextView;

    private Coroutine fillCoroutine; // Ссылка на запущенную корутину

    public void StartCourutine()
    {
        // Если корутина уже запущена, останавливаем её
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }

        // Запускаем корутину и сохраняем ссылку на неё
        fillCoroutine = StartCoroutine(ChangeFillAmountOverTime());
    }

    public void StopCourutine()
    {
        // Если корутина запущена, останавливаем её
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;

            // Дополнительное действие при остановке (если нужно)
            Debug.Log("Coroutine stopped early.");
        }
    }

    private IEnumerator ChangeFillAmountOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            // Изменяем fillAmount от 0 до 1
            fillImage.fillAmount = Mathf.Clamp01(progress);

            // Выводим оставшееся время в целых числах
            timeText.text = Mathf.CeilToInt(duration - elapsedTime).ToString();

            yield return null;
        }

        // Когда время истекло, устанавливаем финальные значения
        fillImage.fillAmount = 1f;
        timeText.text = "0";
        ViewNexPanel();
        // Сбрасываем ссылку на корутину, когда она завершена
        fillCoroutine = null;
    }

    public void ViewNexPanel()
    {
        PanelManager.InstancePanel.textPanelLose.text = GameManager.InstanceGame.Score.text;
        int countCoin = Convert.ToInt32(GameManager.InstanceGame.Score.text);
        countCoin /= 2;
        PanelManager.InstancePanel.textPanelLoseCoin.text = countCoin.ToString();

        GameManager.InstanceGame.gold += countCoin;
        DataManager.InstanceData.SaveGold();
        nextView.SetActive(true);
        DataManager.InstanceData.SaveScore();
        PanelManager.InstancePanel.buttonStartOver.onClick.AddListener(DataManager.InstanceData.mapNextLevel.RestartLevel);
    }
}
