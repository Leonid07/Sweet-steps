using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpawnGrid : MonoBehaviour
{
    public GameObject tilePrefab;  // ������ ������
    public float bpm = 113f;       // ���������� ������ � ������
    public float songDuration; // ������������ ����� � ��������
    public float tileSpeed = 1f;   // �������� �������� ������
    public Sprite firstTileSprite; // ������ ��� ������ ������������ ������
    public GameObject specialTilePrefab;

    private int totalTilesSpawned = 0;  // ����� ���������� ������������ ������
    private int totalPoints = 0;
    private int columns = 4;   // ���������� ������� ����������� �� 4
    public int GetTotalPoint()
    {
        return totalPoints;
    }

    public void CalculateAndSpawnGrid()
    {

        songDuration = GameManager.InstanceGame.audioSourceGameMusicGame.clip.length;

        Camera cam = Camera.main;
        if (cam != null)
        {
            // ���������� ������� ������ � ������� �����������
            var topRight = new Vector3(Screen.width, Screen.height, 0);
            var topRightWorldPoint = cam.ScreenToWorldPoint(topRight);
            var bottomLeftWorldPoint = cam.ScreenToWorldPoint(Vector3.zero);
            var screenWidth = topRightWorldPoint.x - bottomLeftWorldPoint.x;
            var screenHeight = topRightWorldPoint.y - bottomLeftWorldPoint.y;

            // ���������� ������� ������ ���, ����� ������ ��������� ���� ����� ��� �����������
            float tileWidth = screenWidth / columns;
            float tileHeight = tileWidth * (screenHeight / screenWidth);

            // ������������� ���������� ����� ������ �������� BPM (��� ����� �������� �� ������������� ��������)
            int totalRows = Mathf.RoundToInt(bpm / 60f * songDuration);

            // ���������� ��������� ������� ��� ������ ������
            var startPos = new Vector3(
                bottomLeftWorldPoint.x + tileWidth / 2,
                bottomLeftWorldPoint.y + tileHeight / 2,
                0);

            SpawnGridTiles(totalRows, tileWidth, tileHeight, startPos);
        }
    }

    private GameObject lastOpaqueTile; // ���������� ��� �������� ��������� ������������ ������

    void SpawnGridTiles(int totalRows, float tileWidth, float tileHeight, Vector3 startPos)
    {
        bool isFirstTileSet = false;

        // ����� ������� ����
        int clickableColumn = Random.Range(0, columns);

        for (int i = 0; i < columns; i++)
        {
            Vector3 spawnPos = new Vector3(startPos.x + i * tileWidth, startPos.y, 0);
            GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
            ClickableTile clickableTile = tile.AddComponent<ClickableTile>();

            if (i == clickableColumn)
            {
                SetTileOpacity(tile, 1f);
                clickableTile.isTransparent = false;

                if (!isFirstTileSet && firstTileSprite != null)
                {
                    SetTileSprite(tile, firstTileSprite, tileWidth, tileHeight);
                    clickableTile.isFirstTile = true;
                    isFirstTileSet = true;
                    totalPoints += 1;  // ������ ������ ���� 1 �����
                }
            }
            else
            {
                SetTileOpacity(tile, 0f);
                clickableTile.isTransparent = true;
            }

            SetTileSize(tile, tileWidth, tileHeight);
            TileMover tileMover = tile.AddComponent<TileMover>();
            tileMover.speed = tileSpeed;
            tileMover.isTransparent = clickableTile.isTransparent;

            totalTilesSpawned++;  // ����������� ������� ������
        }

        // ����� ��������� �����
        for (int j = 1; j < totalRows; j++)
        {
            clickableColumn = Random.Range(0, columns);

            for (int i = 0; i < columns; i++)
            {
                Vector3 spawnPos = new Vector3(startPos.x + i * tileWidth, startPos.y + j * tileHeight, 0);
                GameObject tile;

                if (j % 6 == 0 && i == clickableColumn) // ����� ��������� ������
                {
                    tile = Instantiate(specialTilePrefab, spawnPos, Quaternion.identity, transform);
                    ClickableTile clickableTile = tile.AddComponent<ClickableTile>();
                    clickableTile.isSpecialTile = true;
                    SetTileOpacity(tile, 1f);
                    totalPoints += 5;  // ��������� ������ ���� 5 �������
                }
                else
                {
                    tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                    ClickableTile clickableTile = tile.AddComponent<ClickableTile>();

                    if (i == clickableColumn)
                    {
                        SetTileOpacity(tile, 1f);
                        clickableTile.isTransparent = false;
                        totalPoints += 1;  // ������� ������ ���� 1 �����

                        // ��������� ��������� ������������ ������
                        lastOpaqueTile = tile;
                    }
                    else
                    {
                        SetTileOpacity(tile, 0f);
                        clickableTile.isTransparent = true;
                    }
                }

                SetTileSize(tile, tileWidth, tileHeight);
                TileMover tileMover = tile.AddComponent<TileMover>();
                tileMover.speed = tileSpeed;
                tileMover.isTransparent = tile.GetComponent<ClickableTile>().isTransparent;

                totalTilesSpawned++;  // ����������� ������� ������
            }
        }

        // ������������� ���� ��� ��������� ������������ ������������ ������
        if (lastOpaqueTile != null)
        {
            lastOpaqueTile.GetComponent<ClickableTile>().isLastTile = true;
        }
    }


    void SetTileSize(GameObject tile, float tileWidth, float tileHeight)
    {
        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // �������� �������� ������� ������� ������
            float spriteWidth = sr.sprite.bounds.size.x;
            float spriteHeight = sr.sprite.bounds.size.y;

            // ������������ ������� ��� ������
            float scaleX = tileWidth / spriteWidth;
            float scaleY = tileHeight / spriteHeight;

            // ������������� ������� ������ ���, ����� ��� �� ���� �������������
            tile.transform.localScale = new Vector3(scaleX, scaleY, 1);
        }
    }

    void SetTileOpacity(GameObject tile, float opacity)
    {
        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color color = sr.color;
            color.a = opacity;
            sr.color = color;
        }
    }

    void SetTileSprite(GameObject tile, Sprite newSprite, float tileWidth, float tileHeight)
    {
        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = newSprite;

            float spriteWidth = sr.sprite.bounds.size.x;
            float spriteHeight = sr.sprite.bounds.size.y;

            // ������������ ������� ��� ������
            float scaleX = tileWidth / spriteWidth;
            float scaleY = tileHeight / spriteHeight;

            // ������������� ������� ������ ���, ����� ��� �� ���� �������������
            tile.transform.localScale = new Vector3(scaleX, scaleY, 1);
        }
    }
}

public class TileMover : MonoBehaviour
{
    public float speed = 1f; // �������� �������� ������
    public bool isTransparent = false; // ��������� �� ������

    void Update()
    {
        if (GameManager.InstanceGame.GameStart == true)
        {
            // ������� ������ ����
            transform.Translate(Vector3.down * speed * Time.deltaTime);

            // ���������, ������� �� ������ �� ������ ������� ������
            if (IsTileBelowCamera())
            {
                if (!isTransparent) // ���������, ��� ������ �� ����������
                {
                    ClickableTile clickable = GetComponent<ClickableTile>();
                    clickable.GameOverAndViewPanel();
                    Debug.Log("���������� ������");
                }
                Destroy(gameObject); // ������� ������, ���� ��� ����� �� �������
            }
        }
    }

    bool IsTileBelowCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
            return false;

        // �������� ������ ������� ������
        float camBottom = cam.transform.position.y - cam.orthographicSize;

        // ���������, ��������� �� ������ ���� ������ ������� ������
        return transform.position.y < camBottom;
    }
}

public class ClickableTile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isTransparent = false; // ��������� �� ������
    public bool isFirstTile = false;   // �������� �� ������ ������ �������
    public bool isSpecialTile = false;  // �������� �� ������ ���������
    private bool isHolding = false;      // ������������ �� ������

    public bool isLastTile = false;

    public Canvas canvas;
    public Slider slider;

    public ParticleSystem parClamp;

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        slider = GetComponentInChildren<Slider>();
        parClamp = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        // �������� ����������� ������
        if (isHolding && isSpecialTile)
        {
            isTransparent = true;

            // ��������� ��������� � TileMover
            TileMover tileMover = GetComponent<TileMover>();
            if (tileMover != null)
            {
                tileMover.isTransparent = true;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true; // �������� �����������

        // �������� �� ��������� ������������ ������������ ������
        if (isLastTile == true)
        {
            Debug.Log("Last Tile");
            if (isFirstTile)
            {
                HandleFirstTileClick();
            }
            else if (isSpecialTile)
            {
                // ��������� ������ ������� ����������� ��� ��������� ���������
                StartCoroutine(HoldSpecialTile());
            }
            else
            {
                HandleRegularTileClick();
            }
            return;
        }

        if (isFirstTile)
        {
            HandleFirstTileClick();
        }
        else if (isSpecialTile)
        {
            // ��������� ������ ������� ����������� ��� ��������� ���������
            StartCoroutine(HoldSpecialTile());
        }
        else
        {
            HandleRegularTileClick();
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false; // ������������� �����������
        if (isSpecialTile == true)
        {
            parClamp.Stop();
        }
        if (slider != null && gameObject.GetComponent<ClickableTile>().isTransparent == false)
        {
            slider.value = 0f;
            TileMover tileMover = GetComponent<TileMover>();
            tileMover.isTransparent = false;
        }
        StopAllCoroutines(); // ������������� ��������, ���� ��������� ������
    }

    private IEnumerator HoldSpecialTile()
    {
        float duration = 0.25f; // ����� ���������
        float elapsed = 0f; // ��������� �����
        parClamp.Play();
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime; // ����������� ��������� �����
            slider.value = elapsed / duration; // ��������� �������� ��������

            yield return null; // ���� ��������� ����
        }

        // ���� ������ ��� ��� ������������
        if (isHolding)
        {
            SetTileOpacity(0f); // ������ ������ ����������
            isTransparent = true;

            // ��������� ��������� � TileMover
            TileMover tileMover = GetComponent<TileMover>();
            if (tileMover != null)
            {
                tileMover.isTransparent = true;
            }

            // ������������� ������
            GameManager.InstanceGame.UpdateScore(true);
            ParticleSystem particle = Instantiate(GameManager.InstanceGame.SetRandomParticle(), transform.position, Quaternion.identity);
            particle.Play();
            canvas.gameObject.SetActive(false);

            PanelWin();
        }
    }

    void HandleFirstTileClick()
    {
        GameManager.InstanceGame.ActiveTiles();
        GameManager.InstanceGame.audioSourceGameMusicGame.Play();
        GameManager.InstanceGame.UpdateScore();
        GameManager.InstanceGame.GameStart = true;
        // �������������� ��������� ��� ������ ������
        SetTileOpacity(0f);
        isTransparent = true;

        TileMover tileMover = GetComponent<TileMover>();
        if (tileMover != null)
        {
            tileMover.isTransparent = true; // ������ ��������� � TileMover
            ParticleSystem particle = Instantiate(GameManager.InstanceGame.SetRandomParticle(), transform.position, Quaternion.identity);
            particle.Play();
        }
    }

    void HandleRegularTileClick()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (!isTransparent)
            {
                GameManager.InstanceGame.UpdateScore();

                // ������������� ������ ��� ����������
                SetTileOpacity(0f);
                isTransparent = true;

                // ��������� ��������� � TileMover
                TileMover tileMover = GetComponent<TileMover>();
                if (tileMover != null)
                {
                    tileMover.isTransparent = true; // ������ ��������� � TileMover
                    ParticleSystem particle = Instantiate(GameManager.InstanceGame.SetRandomParticle(), transform.position, Quaternion.identity);
                    particle.Play();
                }
            }
            else
            {
                GameOverAndViewPanel();
            }
        }
        PanelWin();
    }
    public void PanelWin()
    {
        if (isLastTile == true)
        {
            PanelManager.InstancePanel.PanelWin.SetActive(true);
            GameManager.InstanceGame.GameStart = false;
            GameManager.InstanceGame.SetValueToText();
            DataManager.InstanceData.SaveScore();
            DataManager.InstanceData.mapNextLevel.OpenLevel();
        }
    }

    public void GameOverAndViewPanel()
    {
        GameManager.InstanceGame.viewGameOver.SetActive(true);
        GameManager.InstanceGame.viewGameOver.GetComponent<FillAmountChangerGameOver>().StartCourutine();
        GameManager.InstanceGame.GameStart = false;
        GameManager.InstanceGame.DisActiveTiles();
        GameManager.InstanceGame.audioSourceGameMusicGame.Pause();
        DataManager.InstanceData.SaveScore();
    }

    void SetTileOpacity(float opacity)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color color = sr.color;
            color.a = opacity;
            sr.color = color;
        }
    }
}