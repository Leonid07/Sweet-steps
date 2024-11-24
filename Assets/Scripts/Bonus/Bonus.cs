using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    [Header("Цвета бонусов")]
    public Color32 DisActiveColor = new Color32(104,104,104,255);
    public Color32 ActiveColor = new Color32(255, 255, 255, 255);
    public Image imageDaily;
    public Image imageWeekly;
    public Image imageAds;

    public TMP_Text hourlyBonusText;
    public TMP_Text dailyBonusText;
    public TMP_Text weeklyBonusText;

    public TMP_Text hourlyText;
    public TMP_Text dailyText;
    public TMP_Text weeklyText;

    [Space(10)]
    public Button hourlyBonusButton;
    public Button dailyBonusButton;
    public Button weeklyBonusButton;

    public GameObject hourlyFG;
    public GameObject dailyFG;
    public GameObject weeklyFG;

    private const string DailyBonusTimeKey = "daily_bonus_time";
    private const string WeeklyBonusTimeKey = "weekly_bonus_time";
    private const string HourlyBonusTimeKey = "hourly_bonus_time";

    public int HourlyBonusCooldownInSeconds = 3600; // 1 
    public int DailyBonusCooldownInSeconds = 86400; // 24 
    public int WeeklyBonusCooldownInSeconds = 604800; // 7 

    public int countHourly = 1;
    public int countDaily = 5;
    public int countWeekly = 50;

    private void Start()
    {
        dailyBonusButton.onClick.AddListener(() => HandleButtonClick(ClaimDailyBonus));
        weeklyBonusButton.onClick.AddListener(() => HandleButtonClick(ClaimWeeklyBonus));
        hourlyBonusButton.onClick.AddListener(() => HandleButtonClick(ClaimHourlyBonus));

        StartCoroutine(UpdateBonusTextsRoutine());
    }

    private IEnumerator UpdateBonusTextsRoutine()
    {
        while (true)
        {
            UpdateBonusTexts();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void UpdateBonusTexts()
    {
        string dailyBonusTimeStr = PlayerPrefs.GetString(DailyBonusTimeKey, "0");
        string weeklyBonusTimeStr = PlayerPrefs.GetString(WeeklyBonusTimeKey, "0");
        string hourlyBonusTimeStr = PlayerPrefs.GetString(HourlyBonusTimeKey, "0");

        long dailyBonusTime = long.Parse(dailyBonusTimeStr);
        long weeklyBonusTime = long.Parse(weeklyBonusTimeStr);
        long hourlyBonusTime = long.Parse(hourlyBonusTimeStr);

        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

        long dailyCooldown = dailyBonusTime + DailyBonusCooldownInSeconds - currentTimestamp;
        long weeklyCooldown = weeklyBonusTime + WeeklyBonusCooldownInSeconds - currentTimestamp;
        long hourlyCooldown = hourlyBonusTime + HourlyBonusCooldownInSeconds - currentTimestamp;

        dailyText.text = FormatTimeDaily(dailyCooldown);
        weeklyText.text = FormatTimeWeekly(weeklyCooldown);
        hourlyText.text = FormatTimeHourly(hourlyCooldown);

        dailyBonusButton.interactable = dailyCooldown <= 0;
        weeklyBonusButton.interactable = weeklyCooldown <= 0;
        hourlyBonusButton.interactable = hourlyCooldown <= 0;
    }

    private string FormatTimeDaily(long seconds)
    {
        if (seconds <= 0)
        {
            dailyFG.SetActive(true);
            dailyText.gameObject.SetActive(false);
            dailyBonusText.gameObject.SetActive(true);
            imageDaily.color = ActiveColor;
            return "Ready";
        }
        dailyFG.SetActive(false);
        imageDaily.color = DisActiveColor;
        dailyBonusText.gameObject.SetActive(false);
        dailyText.gameObject.SetActive(true);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private string FormatTimeWeekly(long seconds)
    {
        if (seconds <= 0)
        {
            weeklyFG.SetActive(true);
            weeklyBonusText.gameObject.SetActive(true);
            weeklyText.gameObject.SetActive(false);
            imageWeekly.color = ActiveColor;
            return "Ready";
        }
        weeklyFG.SetActive(false);
        weeklyBonusText.gameObject.SetActive(false);
        weeklyText.gameObject.SetActive(true);
        imageWeekly.color = DisActiveColor;
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        int totalHours = (int)timeSpan.TotalHours;
        return string.Format("{0:D2}:{1:D2}:{2:D2}", totalHours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private string FormatTimeHourly(long seconds)
    {
        if (seconds <= 0)
        {
            hourlyFG.SetActive(true);
            hourlyBonusText.gameObject.SetActive(true);
            imageAds.color = ActiveColor;
            hourlyText.gameObject.SetActive(false);
            return "Ready";
        }
        hourlyFG.SetActive(false);
        hourlyText.gameObject.SetActive(true);
        hourlyBonusText.gameObject.SetActive(false);
        imageAds.color = DisActiveColor;
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private void ClaimDailyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameManager.InstanceGame.gold += countDaily;
        //DataManager.InstanceData.SaveGold();
        PlayerPrefs.SetString(DailyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Daily Bonus Claimed!");
        Debug.Log($"New Daily Bonus Time: {currentTimestamp}");
    }

    private void ClaimWeeklyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameManager.InstanceGame.gold += countWeekly;
        //DataManager.InstanceData.SaveGold();
        PlayerPrefs.SetString(WeeklyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Weekly Bonus Claimed!");
        Debug.Log($"New Weekly Bonus Time: {currentTimestamp}");
    }

    private void ClaimHourlyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameManager.InstanceGame.gold += countHourly;
        //DataManager.InstanceData.SaveGold();
        PlayerPrefs.SetString(HourlyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Hourly Bonus Claimed!");
        Debug.Log($"New Hourly Bonus Time: {currentTimestamp}");
    }

    private void HandleButtonClick(Action onAnimationComplete)
    {
        onAnimationComplete.Invoke();
    }
}
