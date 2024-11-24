using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] music;

    public Button[] buttonInShopPlay;

    [Space(25)]
    public Button[] buttonShopBuy;

    private void Start()
    {
        for (int i = 0; i < buttonInShopPlay.Length; i++)
        {
            int count = i;
            buttonInShopPlay[count].onClick.AddListener(()=> { PlayMusic(music[count]); });
        }

        for (int i = 0; i < buttonShopBuy.Length; i++)
        {
            int count = i;
            buttonShopBuy[count].onClick.AddListener(() => { PlayMusic(music[count]); });
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

}
