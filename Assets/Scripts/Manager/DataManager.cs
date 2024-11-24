using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public ButtonMap[] levels;
    public Sprite[] spriteNumber;

    public ButtonMap mapNextLevel;

    [Header("Сохранения для персонажей")]
    public List<GameObject> character;
    public List<string> indexBuy;
    public int[] isBuy;

    public string idScore = "scoreMain";
    public int scoreValue;
    public TMP_Text[] scoreText;

    public Image chraracterMainMenu;

    public Transform shopContentTransform;
    public List<string> idShopBuy = new List<string>();

    public static DataManager InstanceData { get; private set; }

    private void Awake()
    {
        if (InstanceData != null && InstanceData != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceData = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void SetSpriteNumber()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].imageNumber = spriteNumber[i];
            levels[i].CheckLevel();
        }
    }
    private void Start()
    {
        AddIndexShopContent();

        SetIndexBuyCharacter();

        SetIndexLevel();
        LoadLevel();
        LoadGold();
        LoadMusicBuy();
        SetSpriteNumber();
        LoadScore();
    }

    public void LoadScore()
    {
        if (PlayerPrefs.HasKey(idScore))
        {
            scoreValue = PlayerPrefs.GetInt(idScore);
        }
        for (int i = 0; i < scoreText.Length; i++)
        {
            scoreText[i].text = scoreValue.ToString();
        }
    }
    public void SaveScore()
    {
        PlayerPrefs.SetInt(idScore, scoreValue);
        PlayerPrefs.Save();
        for (int i = 0; i < scoreText.Length; i++)
        {
            scoreText[i].text = scoreValue.ToString();
        }
    }

    public void AddIndexShopContent()
    {
        foreach (Transform tra in shopContentTransform)
        {
            if (tra.GetComponent<ShopButton>() != null)
            {
                idShopBuy.Add(tra.name);
            }
        }
    }

    public void SetIndexLevel()
    {
        int count = 1;
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].indexLevel = count;
            count++;
        }
    }

    public void SaveBuyCharacter()
    {
        for (int i = 0; i < character.Count; i++)
        {
            PlayerPrefs.SetInt(indexBuy[i], character[i].GetComponent<CharacterButton>().isBuy);
            PlayerPrefs.Save();
        }
    }

    public void LoadBuyCharacter()
    {
        for (int i = 0; i < character.Count; i++)
        {
            if (PlayerPrefs.HasKey(indexBuy[i]))
            {
                character[i].GetComponent<CharacterButton>().isBuy = PlayerPrefs.GetInt(indexBuy[i]);
                character[i].GetComponent<CharacterButton>().LoadCat();
            }
        }
    }

    public void SetIndexBuyCharacter()
    {
        indexBuy = new List<string>(); // Инициализация нового списка для индексов

        // Проходим по списку character, который заполнен в инспекторе
        for (int i = 0; i < character.Count; i++)
        {
            indexBuy.Add(character[i].name); // Добавляем имя персонажа в список indexBuy
        }
    }

    public void SaveLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            PlayerPrefs.SetInt(levels[i].idLevel, levels[i].isLoad);
            PlayerPrefs.Save();
        }
    }
    public void LoadLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (PlayerPrefs.HasKey(levels[i].idLevel))
            {
                levels[i].isLoad = PlayerPrefs.GetInt(levels[i].idLevel);
                levels[i].CheckLevel();
            }
        }
    }
    public void SaveGold()
    {
        GameManager.InstanceGame.ApplyGold();
        PlayerPrefs.SetInt(GameManager.InstanceGame.idGold, GameManager.InstanceGame.gold);
        PlayerPrefs.Save();
    }
    public void LoadGold()
    {
        if (PlayerPrefs.HasKey(GameManager.InstanceGame.idGold))
        {
            GameManager.InstanceGame.gold = PlayerPrefs.GetInt(GameManager.InstanceGame.idGold);
            GameManager.InstanceGame.ApplyGold();
        }
    }

    public void SaveMusicBuy()
    {
        for (int i = 0; i < idShopBuy.Count;i++)
        {
            PlayerPrefs.SetInt(idShopBuy[i], PanelManager.InstancePanel.shopButton[i].isBuy);
            PlayerPrefs.Save();
        }
    }

    public void LoadMusicBuy()
    {
        for (int i = 0; i < idShopBuy.Count; i++)
        {
            if (PlayerPrefs.HasKey(idShopBuy[i]))
            {
                PanelManager.InstancePanel.shopButton[i].isBuy = PlayerPrefs.GetInt(idShopBuy[i]);
                PanelManager.InstancePanel.shopButton[i].CheckBuyLevel();
            }
        }
    }


    public int ConvertStringToInt(string input)
    {
        // Извлекаем только цифры из строки
        string digits = new string(input.Where(char.IsDigit).ToArray());

        // Пробуем преобразовать строку с цифрами в int
        if (int.TryParse(digits, out int number))
        {
            Debug.Log(number);
            return number; // Если получилось, возвращаем число
        }
        else
        {
            Debug.LogWarning("Не удалось преобразовать строку в целое число.");
            return 0; // Возвращаем 0 в случае ошибки
        }
    }
}
