using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMap : MonoBehaviour
{
    public int indexLevel = 0;

    Button thisButton;
    public Image imageBlockAndNumber;
    public Sprite imageNumber;
    public Sprite imageBlock;

    public int isLoad = 0;
    public string idLevel;

    public ButtonMap mapNextLevel;

    public ButtonMap thisLevel;

    public Image thisImage;
    public Sprite standartImage;
    public Sprite spriteBlockImage;

    private void Awake()
    {
        thisLevel = GetComponent<ButtonMap>();
        thisImage = GetComponent<Image>();
        idLevel = gameObject.name;
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnPointerClick);
        CheckLevel();
    }
    public void OnPointerClick()
    {
        DataManager.InstanceData.mapNextLevel = thisLevel;
        GameManager.InstanceGame.ResetScore();
        GameManager.InstanceGame.LoadLevel();
        GameManager.InstanceGame.spawnGrid.CalculateAndSpawnGrid();
        GameManager.InstanceGame.CanvasGame.SetActive(true);

        GameManager.InstanceGame.CanvasMain.SetActive(false);

        GameManager.InstanceGame.fadeUI.StartAnimGB();

        GameManager.InstanceGame.DisActiveTiles();

        GameManager.InstanceGame.ValueSlider();
    }

    public void RestartLevel()
    {
        DataManager.InstanceData.mapNextLevel = thisLevel;
        GameManager.InstanceGame.ResetScore();
        GameManager.InstanceGame.LoadLevel();
        foreach (Transform child in GameManager.InstanceGame.spawnGrid.transform)
        {
            Destroy(child.gameObject);
        }
        GameManager.InstanceGame.spawnGrid.CalculateAndSpawnGrid();
        GameManager.InstanceGame.CanvasGame.SetActive(true);

        GameManager.InstanceGame.CanvasMain.SetActive(false);

        GameManager.InstanceGame.fadeUI.StartAnimGB();

        GameManager.InstanceGame.DisActiveTiles();

        GameManager.InstanceGame.ValueSlider();

        PanelManager.InstancePanel.PanelWin.SetActive(false);
        PanelManager.InstancePanel.PanelGameOver_1.SetActive(false);
        PanelManager.InstancePanel.PanelGameOver_2.SetActive(false);
    }


    public void OpenLevel()
    {
        mapNextLevel.isLoad = 1;
        mapNextLevel.CheckLevel();
        DataManager.InstanceData.SaveLevel();
    }

    public void CheckLevel()
    {
        if (isLoad == 0)
        {
            imageBlockAndNumber.sprite = imageBlock;
            thisImage.sprite = spriteBlockImage;
        }
        if (isLoad == 1)
        {
            imageBlockAndNumber.sprite = imageNumber;
            thisImage.sprite = standartImage;
        }
    }
}
