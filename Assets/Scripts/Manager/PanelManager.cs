using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public static PanelManager InstancePanel { get; private set; }

    private void Awake()
    {
        if (InstancePanel != null && InstancePanel != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstancePanel = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [Header("кнопки нижней панели")]
    public Button ButtonhomeSprite;
    public Button ButtonshopeSprite;
    public Button ButtonbonusSprite;
    public Button ButtonsettingsSprite;

    [Header("Sprite")]
    public Image SpriteMainButton;
    public Sprite MainMenuSprite;
    public Sprite ShopMenuSprite;
    public Sprite BonusMenuSprite;
    public Sprite SettingMenuSprite;

    [Header("Игровые панели")]
    public GameObject MainMenu;
    public GameObject ShopMenu;
    public GameObject BonusMenu;
    public GameObject SettingMenu;
    public GameObject CharacterMenu;

    [Header("Кнопки закрытия")]
    public Button[] buttonClose;

    [Header("Панельки")]
    public GameObject[] panelMainMenu;

    [Space(15)]
    public Button character;

    public SwipePanel swipePanel;

    [Header("Мазыка в магазине")]
    public ShopButton[] shopButton;

    [Header("Панель выйгрыша")]
    public GameObject PanelWin;
    public TMP_Text textPanelWin;
    public TMP_Text textPanelWinCoin;

    public Button[] buttonHome;

    public GameObject PanelGameOver_1;
    public GameObject PanelGameOver_2;
    public TMP_Text textPanelLose;
    public TMP_Text textPanelLoseCoin;

    [Header("Start Over")]
    public Button buttonStartOver;

    public void CloseFoolPanel()
    {
        GameManager.InstanceGame.CanvasMain.SetActive(true);
        GameManager.InstanceGame.CanvasGame.SetActive(false);
        PanelWin.SetActive(false);
        PanelGameOver_1.SetActive(false);
        PanelGameOver_2.SetActive(false);
    }

    private void Start()
    {
        GameManager.InstanceGame.audioSourceGameMusicGame.Play();
        for (int i = 0; i < buttonHome.Length; i++)
        {
            int count = i;
            buttonHome[count].onClick.AddListener(() => { CloseFoolPanel(); CloseGamePanels(); });
        }

        for (int i = 0; i < buttonClose.Length; i++)
        {
            int count = i;
            buttonClose[count].onClick.AddListener(()=> { ClosePanel(panelMainMenu); });
        }

        ButtonhomeSprite.onClick.AddListener(()=> { ClosePanel(panelMainMenu); OpenPanelDownPanel(MainMenu); ResetSprite(MainMenuSprite); ClosePanelShop(ShopMenu); });
        ButtonshopeSprite.onClick.AddListener(()=> { ClosePanel(panelMainMenu); OpenShopPanel(ShopMenu); ResetSprite(ShopMenuSprite); });
        ButtonbonusSprite.onClick.AddListener(()=> { ClosePanel(panelMainMenu); OpenPanelDownPanel(BonusMenu); ResetSprite(BonusMenuSprite); ClosePanelShop(ShopMenu); });
        ButtonsettingsSprite.onClick.AddListener(()=> { ClosePanel(panelMainMenu); OpenPanelDownPanel(SettingMenu); ResetSprite(SettingMenuSprite); ClosePanelShop(ShopMenu); });

        character.onClick.AddListener(()=> 
        { 
            ClosePanel(panelMainMenu);
            OpenPanelDownPanel(CharacterMenu);
            DataManager.InstanceData.LoadBuyCharacter();
        });
    }

    public void CloseGamePanels()
    {
        GameManager.InstanceGame.CanvasMain.SetActive(true);
        GameManager.InstanceGame.CanvasGame.SetActive(false);

        foreach (Transform child in GameManager.InstanceGame.spawnGrid.transform)
        {
            Destroy(child.gameObject);
        }
        GameManager.InstanceGame.audioSourceGameMusicGame.Play();
    }

    public void OpenPanelDownPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void OpenShopPanel(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().alpha = 1;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void ClosePanelShop(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().alpha = 0;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void ResetSprite(Sprite sprite)
    {
        SpriteMainButton.sprite = sprite;
    }

    public void ClosePanel(GameObject[] panels)
    {
        foreach (GameObject gm in panels)
        {
            gm.SetActive(false);
        }
    }

    public void DisActiveMusic()
    {
        foreach (ShopButton au in shopButton)
        {
            au.imageButton.sprite = au.spritePlay;
        }
        GameManager.InstanceGame.audioSourceGameMusicGame.Stop();
    }
}
