using UnityEngine;
using TMPro; // IMPORTANT
using System.Collections.Generic;

[System.Serializable]
public class TutorialStep
{
    public string description; // texte à afficher
    public bool completed = false;
}

public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> steps = new List<TutorialStep>();
    public TMP_Text uiText; // Remplace Text par TMP_Text
    private int currentStepIndex = 0;

    void Start()
    {
        if (steps.Count > 0)
            uiText.text = steps[currentStepIndex].description;
    }

    void Update()
    {
        if (currentStepIndex >= steps.Count) return;

        var step = steps[currentStepIndex];

    }

    public void CompleteStep()
    {
        steps[currentStepIndex].completed = true;
        currentStepIndex++;

        if (currentStepIndex < steps.Count)
            uiText.text = steps[currentStepIndex].description;
        else
            uiText.text = "Tutoriel terminé !";
    }
}
