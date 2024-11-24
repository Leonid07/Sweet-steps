using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class FillAmountChangerGameOver : MonoBehaviour
{
    public Image fillImage; // Image � fillAmount (��������, UI ������� � ����� ���������� Fill)
    public TMP_Text timeText;   // Text ��� ����������� �������
    public float duration = 5f; // �����, �� ������� ����� ��������� ����������

    public GameObject nextView;

    private Coroutine fillCoroutine; // ������ �� ���������� ��������

    public void StartCourutine()
    {
        // ���� �������� ��� ��������, ������������� �
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }

        // ��������� �������� � ��������� ������ �� ��
        fillCoroutine = StartCoroutine(ChangeFillAmountOverTime());
    }

    public void StopCourutine()
    {
        // ���� �������� ��������, ������������� �
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;

            // �������������� �������� ��� ��������� (���� �����)
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

            // �������� fillAmount �� 0 �� 1
            fillImage.fillAmount = Mathf.Clamp01(progress);

            // ������� ���������� ����� � ����� ������
            timeText.text = Mathf.CeilToInt(duration - elapsedTime).ToString();

            yield return null;
        }

        // ����� ����� �������, ������������� ��������� ��������
        fillImage.fillAmount = 1f;
        timeText.text = "0";
        ViewNexPanel();
        // ���������� ������ �� ��������, ����� ��� ���������
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
