using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class QuestStep
{
    [TextArea]
    public string questText;

    public int targetAmount;

    public string itemPrefix;

    public bool useTextCounter = true;

    public bool useItemIcons = false;

    public GameObject itemUIContainer;

    public bool isCollectionStep = true;
}

public class RandomObjManager : MonoBehaviour
{
    [Header("Quest Steps")]
    public QuestStep[] questSteps;

    private int currentStep = 0;
    private int currentAmount = 0;

    [Header("UI")]
    public TextMeshProUGUI questTMP;
    public TextMeshProUGUI counterTMP;

    [Header("Win Settings")]
    public GameObject winTrigger;
    public Arrow_Pointer arrowPointer;

    void Start()
    {
        if (winTrigger != null)
            winTrigger.SetActive(false);

        if (arrowPointer != null)
            arrowPointer.SetVisible(false);

        SetupCurrentStep();
    }

    // =========================
    // SETUP STEP
    // =========================

    void SetupCurrentStep()
    {
        currentAmount = 0;

        QuestStep step = questSteps[currentStep];

        // Update quest text
        if (questTMP != null)
        {
            questTMP.text = step.questText;
        }

        // Counter UI
        if (counterTMP != null)
        {
            counterTMP.gameObject.SetActive(step.useTextCounter);

            if (step.useTextCounter)
            {
                UpdateCounterUI();
            }
        }

        // Item UI Container
        if (step.itemUIContainer != null)
        {
            step.itemUIContainer.SetActive(step.useItemIcons);
        }
    }

    // =========================
    // UPDATE COUNTER
    // =========================

    void UpdateCounterUI()
    {
        QuestStep step = questSteps[currentStep];

        counterTMP.text =
            step.itemPrefix +
            currentAmount +
            "/" +
            step.targetAmount;
    }

    // =========================
    // COLLECT ITEM
    // =========================

    public void CollectItem(GameObject specificUI)
    {
        
        if (currentStep >= questSteps.Length)
        {
            return;
        }
        
        QuestStep step = questSteps[currentStep];

        currentAmount++;

        // Aktifkan icon item
        if (specificUI != null)
        {
            specificUI.SetActive(true);
        }

        // Update counter
        if (step.useTextCounter)
        {
            UpdateCounterUI();
        }

        // Objective selesai
        if (step.isCollectionStep && currentAmount >= step.targetAmount)
        {
            CompleteStep();
        }
    }

    // =========================
    // COMPLETE STEP
    // =========================

    void CompleteStep()
    {
        QuestStep step = questSteps[currentStep];

        // Matikan item UI
        if (step.itemUIContainer != null)
        {
            step.itemUIContainer.SetActive(false);
        }

        currentStep++;

        // Kalau masih ada step berikutnya
        if (currentStep < questSteps.Length)
        {
            // Kalau step berikutnya adalah step terakhir
            if (currentStep == questSteps.Length - 1)
            {
                ActivateWinCondition();
            }

            SetupCurrentStep();
        }
    }

    // =========================
    // WIN CONDITION
    // =========================

    void ActivateWinCondition()
    {
        if (winTrigger != null)
        {
            winTrigger.SetActive(true);
        }

        if (arrowPointer != null)
        {
            arrowPointer.SetVisible(true);
            arrowPointer.SetTarget(winTrigger.transform);
        }

        Debug.Log("WIN TRIGGER AKTIF");
    }
}