using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomObjManager : MonoBehaviour
{
    [Header("Collect Settings")]
    public int totalItems = 3;
    private int currentItems = 0;

    [Header("Win Settings")]
    public GameObject winTrigger;
    public Arrow_Pointer arrowPointer;

    [Header("Counter UI")]
    public TextMeshProUGUI counterText;
    public string itemPrefix = "Item: ";

    [Header("Quest UI")]
    public TextMeshProUGUI questText;

    [TextArea]
    public string[] questSteps;

    private int currentQuestStep = 0;

    [Header("UI Item Container")]
    public GameObject itemUIContainer;

    void Start()
    {
        if (winTrigger != null)
            winTrigger.SetActive(false);

        if (arrowPointer != null)
            arrowPointer.SetVisible(false);

        UpdateQuestUI();
        UpdateTextUI();
    }

    // =========================
    // QUEST SYSTEM
    // =========================

    void UpdateQuestUI()
    {
        if (questText != null &&
            currentQuestStep < questSteps.Length)
        {
            questText.text = questSteps[currentQuestStep];
        }
    }

    public void NextQuestStep()
    {
        currentQuestStep++;

        if (currentQuestStep < questSteps.Length)
        {
            UpdateQuestUI();
        }
    }

    // =========================
    // ITEM COLLECT SYSTEM
    // =========================

    public void CollectItem(GameObject specificUI)
    {
        currentItems++;

        // Level 3 icon item
        if (specificUI != null)
        {
            specificUI.SetActive(true);
        }

        UpdateTextUI();

        // Semua item terkumpul
        if (currentItems >= totalItems)
        {
            FinishCollectObjective();
        }
    }

    void UpdateTextUI()
    {
        if (counterText != null)
        {
            counterText.text =
                itemPrefix + currentItems + "/" + totalItems;
        }
    }

    void FinishCollectObjective()
    {
        // Next quest
        NextQuestStep();

        // Aktifkan win trigger
        if (winTrigger != null)
            winTrigger.SetActive(true);

        // Arrow
        if (arrowPointer != null)
        {
            arrowPointer.SetVisible(true);
            arrowPointer.SetTarget(winTrigger.transform);
        }

        // Hilangkan UI item
        if (itemUIContainer != null)
        {
            itemUIContainer.SetActive(false);
        }
    }
}