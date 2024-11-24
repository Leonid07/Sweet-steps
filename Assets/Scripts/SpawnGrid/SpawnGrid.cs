using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpawnGrid : MonoBehaviour
{
    public GameObject tilePrefab;  // Префаб плитки
    public float bpm = 113f;       // Количество ударов в минуту
    public float songDuration; // Длительность песни в секундах
    public float tileSpeed = 1f;   // Скорость движения плиток
    public Sprite firstTileSprite; // Спрайт для первой непрозрачной плитки
    public GameObject specialTilePrefab;

    private int totalTilesSpawned = 0;  // Общее количество заспавненных плиток
    private int totalPoints = 0;
    private int columns = 4;   // Количество колонок фиксировано на 4
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
            // Определяем размеры экрана в мировых координатах
            var topRight = new Vector3(Screen.width, Screen.height, 0);
            var topRightWorldPoint = cam.ScreenToWorldPoint(topRight);
            var bottomLeftWorldPoint = cam.ScreenToWorldPoint(Vector3.zero);
            var screenWidth = topRightWorldPoint.x - bottomLeftWorldPoint.x;
            var screenHeight = topRightWorldPoint.y - bottomLeftWorldPoint.y;

            // Определяем размеры плитки так, чтобы плитки заполняли весь экран без промежутков
            float tileWidth = screenWidth / columns;
            float tileHeight = tileWidth * (screenHeight / screenWidth);

            // Устанавливаем количество рядов равным значению BPM (или можно заменить на фиксированное значение)
            int totalRows = Mathf.RoundToInt(bpm / 60f * songDuration);

            // Определяем начальную позицию для спауна плиток
            var startPos = new Vector3(
                bottomLeftWorldPoint.x + tileWidth / 2,
                bottomLeftWorldPoint.y + tileHeight / 2,
                0);

            SpawnGridTiles(totalRows, tileWidth, tileHeight, startPos);
        }
    }

    private GameObject lastOpaqueTile; // Переменная для хранения последней непрозрачной плитки

    void SpawnGridTiles(int totalRows, float tileWidth, float tileHeight, Vector3 startPos)
    {
        bool isFirstTileSet = false;

        // Спавн первого ряда
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
                    totalPoints += 1;  // Первая плитка дает 1 поинт
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

            totalTilesSpawned++;  // Увеличиваем счетчик плиток
        }

        // Спавн остальных рядов
        for (int j = 1; j < totalRows; j++)
        {
            clickableColumn = Random.Range(0, columns);

            for (int i = 0; i < columns; i++)
            {
                Vector3 spawnPos = new Vector3(startPos.x + i * tileWidth, startPos.y + j * tileHeight, 0);
                GameObject tile;

                if (j % 6 == 0 && i == clickableColumn) // Спавн особенной плитки
                {
                    tile = Instantiate(specialTilePrefab, spawnPos, Quaternion.identity, transform);
                    ClickableTile clickableTile = tile.AddComponent<ClickableTile>();
                    clickableTile.isSpecialTile = true;
                    SetTileOpacity(tile, 1f);
                    totalPoints += 5;  // Особенная плитка дает 5 поинтов
                }
                else
                {
                    tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                    ClickableTile clickableTile = tile.AddComponent<ClickableTile>();

                    if (i == clickableColumn)
                    {
                        SetTileOpacity(tile, 1f);
                        clickableTile.isTransparent = false;
                        totalPoints += 1;  // Обычная плитка дает 1 поинт

                        // Сохраняем последнюю непрозрачную плитку
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

                totalTilesSpawned++;  // Увеличиваем счетчик плиток
            }
        }

        // Устанавливаем флаг для последней заспавненной непрозрачной плитки
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
            // Получаем исходные размеры спрайта плитки
            float spriteWidth = sr.sprite.bounds.size.x;
            float spriteHeight = sr.sprite.bounds.size.y;

            // Рассчитываем масштаб для плитки
            float scaleX = tileWidth / spriteWidth;
            float scaleY = tileHeight / spriteHeight;

            // Устанавливаем масштаб плитки так, чтобы она не была деформирована
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

            // Рассчитываем масштаб для плитки
            float scaleX = tileWidth / spriteWidth;
            float scaleY = tileHeight / spriteHeight;

            // Устанавливаем масштаб плитки так, чтобы она не была деформирована
            tile.transform.localScale = new Vector3(scaleX, scaleY, 1);
        }
    }
}

public class TileMover : MonoBehaviour
{
    public float speed = 1f; // Скорость движения плитки
    public bool isTransparent = false; // Прозрачна ли плитка

    void Update()
    {
        if (GameManager.InstanceGame.GameStart == true)
        {
            // Двигаем плитку вниз
            transform.Translate(Vector3.down * speed * Time.deltaTime);

            // Проверяем, выходит ли плитка за нижние границы камеры
            if (IsTileBelowCamera())
            {
                if (!isTransparent) // Проверяем, что плитка не прозрачная
                {
                    ClickableTile clickable = GetComponent<ClickableTile>();
                    clickable.GameOverAndViewPanel();
                    Debug.Log("Прозрачная плитка");
                }
                Destroy(gameObject); // Удаляем плитку, если она вышла за границы
            }
        }
    }

    bool IsTileBelowCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
            return false;

        // Получаем нижнюю границу камеры
        float camBottom = cam.transform.position.y - cam.orthographicSize;

        // Проверяем, находится ли плитка ниже нижней границы камеры
        return transform.position.y < camBottom;
    }
}

public class ClickableTile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isTransparent = false; // Прозрачна ли плитка
    public bool isFirstTile = false;   // Является ли плитка первой плиткой
    public bool isSpecialTile = false;  // Является ли плитка особенной
    private bool isHolding = false;      // Удерживается ли плитка

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
        // Проверка удерживания плитки
        if (isHolding && isSpecialTile)
        {
            isTransparent = true;

            // Обновляем состояние в TileMover
            TileMover tileMover = GetComponent<TileMover>();
            if (tileMover != null)
            {
                tileMover.isTransparent = true;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true; // Начинаем удерживание

        // Проверка на последнюю заспавненную непрозрачную плитку
        if (isLastTile == true)
        {
            Debug.Log("Last Tile");
            if (isFirstTile)
            {
                HandleFirstTileClick();
            }
            else if (isSpecialTile)
            {
                // Особенная плитка требует удерживания для изменения состояния
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
            // Особенная плитка требует удерживания для изменения состояния
            StartCoroutine(HoldSpecialTile());
        }
        else
        {
            HandleRegularTileClick();
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false; // Останавливаем удерживание
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
        StopAllCoroutines(); // Останавливаем корутины, если отпустили кнопку
    }

    private IEnumerator HoldSpecialTile()
    {
        float duration = 0.25f; // Время удержания
        float elapsed = 0f; // Прошедшее время
        parClamp.Play();
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime; // Увеличиваем прошедшее время
            slider.value = elapsed / duration; // Обновляем значение слайдера

            yield return null; // Ждем следующий кадр
        }

        // Если плитка все еще удерживается
        if (isHolding)
        {
            SetTileOpacity(0f); // Делаем плитку прозрачной
            isTransparent = true;

            // Обновляем состояние в TileMover
            TileMover tileMover = GetComponent<TileMover>();
            if (tileMover != null)
            {
                tileMover.isTransparent = true;
            }

            // Воспроизводим эффект
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
        // Дополнительное поведение для первой плитки
        SetTileOpacity(0f);
        isTransparent = true;

        TileMover tileMover = GetComponent<TileMover>();
        if (tileMover != null)
        {
            tileMover.isTransparent = true; // Меняем состояние в TileMover
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

                // Устанавливаем плитку как прозрачную
                SetTileOpacity(0f);
                isTransparent = true;

                // Обновляем состояние в TileMover
                TileMover tileMover = GetComponent<TileMover>();
                if (tileMover != null)
                {
                    tileMover.isTransparent = true; // Меняем состояние в TileMover
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