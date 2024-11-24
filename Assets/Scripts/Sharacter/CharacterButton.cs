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

    // ������� ��� �������� ���������� alpha
    // BUTTON
    IEnumerator FadeOut()
    {
        float startAlpha = characteButton.alpha;  // ������� �������� alpha
        float elapsedTime = 0f;                // ����� � ������ ��������

        while (elapsedTime < duration)
        {
            // ������� ������������� ��������
            float t = elapsedTime / duration;
            // �������� ���������� alpha
            characteButton.alpha = Mathf.Lerp(startAlpha, 0f, t);
            elapsedTime += Time.deltaTime;

            // ��� �� ���������� �����
            yield return null;
        }

        // ������������� ��������� ��������
        characteButton.alpha = 0f;
    }

    // ������� ��� �������� ���������� alpha
    // character
    IEnumerator FadeIn()
    {
        float startAlpha = characte.alpha;  // ������� �������� alpha
        float elapsedTime = 0f;                // ����� � ������ ��������
        while (elapsedTime < duration)
        {
            // ������� ������������� ��������
            float t = elapsedTime / duration;
            // �������� ���������� alpha
            characte.alpha = Mathf.Lerp(startAlpha, 1f, t);
            elapsedTime += Time.deltaTime;

            // ��� �� ���������� �����
            yield return null;
        }

        // ������������� ��������� ��������
        characte.alpha = 1f;
    }
}
