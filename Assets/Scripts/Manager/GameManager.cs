using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text[] textGold;
    public int gold;
    public string idGold = "gold";

    //public AudioSource audioSourceGameMusic;
    public AudioSource audioSourceGameMusicGame;
    public AudioClip clip;

    [Header("Счёт")]
    public Slider sliper;
    public TMP_Text Score;
    public int scoreValue;

    public bool GameStart = false;

    [Header("Настройки для начала игры")]
    public SpawnGrid spawnGrid;
    public GameObject CanvasMain;
    public GameObject CanvasGame;
    public FadeUI fadeUI;

    public GameObject viewGameOver;

    [Header("Все плитки")]
    public GameObject tilesParent;

    [Header("Таймер")]
    public TMP_Text textTimer;

    public FillAmountChangerGameOver gameOver;

    [Header("Частицы фейверка разнного цвета")]
    public ParticleSystem[] particleSystemsFireWork;

    [Header("Slider счёта")]
    public Slider sliderCounter;

    public static GameManager InstanceGame { get; private set; }

    private void Awake()
    {
        if (InstanceGame != null && InstanceGame != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceGame = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ValueSlider()
    {
        sliderCounter.maxValue = spawnGrid.GetTotalPoint();
    }

    private void Start()
    {
        ApplyGold();
    }

    public void UpdateScore(bool special = false)
    {
        if (special == true)
        {
            scoreValue += 5;
            Score.text = scoreValue.ToString();
            sliper.value = scoreValue;

            DataManager.InstanceData.scoreValue += scoreValue;
            PanelManager.InstancePanel.textPanelLose.text = $"SCORE: {scoreValue}";
            PanelManager.InstancePanel.textPanelWin.text = $"SCORE: {scoreValue}";
        }
        else
        {
            scoreValue++;
            Score.text = scoreValue.ToString();
            sliper.value = scoreValue;


            DataManager.InstanceData.scoreValue += scoreValue;
            PanelManager.InstancePanel.textPanelLose.text = $"SCORE: {scoreValue}";
            PanelManager.InstancePanel.textPanelWin.text = $"SCORE: {scoreValue}";
        }
    }

    public void SetValueToText()
    {
        PanelManager.InstancePanel.textPanelWin.text = Score.text;
        int countCoin = Convert.ToInt32(Score.text);
        countCoin /= 2;
        PanelManager.InstancePanel.textPanelWinCoin.text = countCoin.ToString();

        gold += countCoin;
        DataManager.InstanceData.SaveGold();
    }

    public void ResetScore()
    {
        scoreValue = 0;
        Score.text = scoreValue.ToString();
        sliper.value = scoreValue;
    }

    public void ApplyGold()
    {
        foreach (TMP_Text text in textGold)
        {
            text.text = gold.ToString();
        }
    }

    public List<ShopButton> shops = new List<ShopButton>();

    public void LoadLevel()
    {
        foreach (ShopButton music in PanelManager.InstancePanel.shopButton)
        {
            if (music.isBuy == 1)
            {
                shops.Add(music);
            }
        }
        int randomMusic = UnityEngine.Random.Range(0, shops.Count);
        clip = shops[randomMusic].audioSource.clip;
        audioSourceGameMusicGame.clip = clip;
    }

    public void DisActiveTiles()
    {
        foreach (Transform child in tilesParent.transform)
        {
            if (spawnGrid.firstTileSprite != child.GetComponent<SpriteRenderer>().sprite)
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    public void ActiveTiles()
    {
        foreach (Transform child in tilesParent.transform)
        {
            child.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public ParticleSystem SetRandomParticle()
    {
        int rand = UnityEngine.Random.Range(0, particleSystemsFireWork.Length);
        return particleSystemsFireWork[rand];
    }

    public void StartTimer()
    {
        StartCoroutine(StartCountdown());
    }

    // таймер
    IEnumerator StartCountdown()
    {
        gameOver.StopCourutine();
        gameOver.gameObject.SetActive(false);
        textTimer.gameObject.SetActive(true);
        int countdownValue = 3;

        while (countdownValue > 0)
        {
            textTimer.text = $"GAME CONTINUES IN\r\n{countdownValue}"; // Обновление текста с текущим числом
            yield return new WaitForSeconds(1f); // Ожидание 1 секунду
            countdownValue--; // Уменьшение значения таймера
        }
        ActiveTiles();
        GameStart = true;
        textTimer.gameObject.SetActive(false);
        audioSourceGameMusicGame.Play();
    }
}
