using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipePanel : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int maxPage;
    public int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] float tweenTime;

    float dragThreshould;

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        dragThreshould = Screen.width / 15;
    }
    public Button buttonLeft;
    public Button buttonRight;
    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            StartCoroutine(MovePage());
            int count = currentPage;
            count--;
            if (DataManager.InstanceData.character[count].GetComponent<CharacterButton>().isBuy == 1)
            {
                DataManager.InstanceData.chraracterMainMenu.sprite
    = DataManager.InstanceData.character[count].GetComponent<CharacterButton>()
    .characte.GetComponent<Image>().sprite;
            }
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            StartCoroutine(MovePage());
            int count = currentPage;
            count--;
            if (DataManager.InstanceData.character[count].GetComponent<CharacterButton>().isBuy == 1)
            {
                DataManager.InstanceData.chraracterMainMenu.sprite
    = DataManager.InstanceData.character[count].GetComponent<CharacterButton>()
    .characte.GetComponent<Image>().sprite;
            }
        }
    }

    IEnumerator MovePage()
    {
        Vector3 startPos = levelPagesRect.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < tweenTime)
        {
            levelPagesRect.localPosition = Vector3.Lerp(startPos, targetPos, elapsedTime / tweenTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        levelPagesRect.localPosition = targetPos;
        CheckIsBuyRecord();
    }

    private void Start()
    {
        buttonLeft.onClick.AddListener(Previous);
        buttonRight.onClick.AddListener(Next);
        //if (Data.dataInstance.mainMenu.records[0].isBuy == 0)
        //{
        //    buttonStart.SetActive(false);
        //}
        //else
        //{
        //    buttonStart.SetActive(true);
        //}
    }

    public void CheckIsBuyRecord()
    {
        int count = currentPage;
        count--;

        //if (Data.dataInstance.mainMenu.records[count].isBuy == 0)
        //{
        //    buttonStart.SetActive(false);
        //}
        //else
        //{
        //    buttonStart.SetActive(true);
        //}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x) Previous();
            else Next();
        }
        else
        {
            StartCoroutine(MovePage());
        }
    }
}