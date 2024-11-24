using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public AudioSource audioSource;
    public Button buttonPlay;
    public Image imageButton;

    public Sprite spriteStop;
    public Sprite spritePlay;

    public Button buttonBuy;
    public TMP_Text textPlayValue;

    public int isBuy = 0;
    public bool isFirstMusic = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CheckBuyLevel();

        buttonPlay.onClick.AddListener(()=> { PlayMusic(); });
        buttonBuy.onClick.AddListener(()=> { BuyMusic(); });
    }

    public void PlayMusic()
    {
        if (imageButton.sprite == spriteStop)
        {
            PanelManager.InstancePanel.DisActiveMusic();
            GameManager.InstanceGame.audioSourceGameMusicGame.Stop();

            imageButton.sprite = spritePlay;
            return;
        }
        if (imageButton.sprite == spritePlay)
        {
            PanelManager.InstancePanel.DisActiveMusic();
            GameManager.InstanceGame.audioSourceGameMusicGame.clip = audioSource.clip;
            GameManager.InstanceGame.audioSourceGameMusicGame.Play();
            imageButton.sprite = spriteStop;
        }
    }
    public void BuyMusic()
    {
        int count = DataManager.InstanceData.ConvertStringToInt(textPlayValue.text);
        if (GameManager.InstanceGame.gold >= count)
        {
            GameManager.InstanceGame.gold -= count;
            isBuy++;

            DataManager.InstanceData.SaveMusicBuy();
            DataManager.InstanceData.SaveGold();
            CheckBuyLevel();
        }
    }
    public void CheckBuyLevel()
    {
        if (isBuy == 1)
        {
            buttonBuy.gameObject.SetActive(false);
            buttonPlay.interactable = true;
        }
        else
        {
            buttonBuy.gameObject.SetActive(true);
            buttonPlay.interactable = false;
        }
        if (isFirstMusic == true)
        {
            buttonBuy.gameObject.SetActive(false);
            buttonPlay.interactable = true;
        }
    }
}
