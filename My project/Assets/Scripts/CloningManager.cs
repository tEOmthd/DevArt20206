using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
using TMPro; // 👈 ajouté

public class CloningManager : MonoBehaviour
{
    [Header("References")]
    public BlueScreenFade blueScreenFade;

    public Transform xrRig;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    [Header("UI")]
    public TextMeshProUGUI cloningText; // 👈 TextMeshPro ajouté

    private bool isCloning = false;
    private List<FrameData> recordedFrames = new List<FrameData>();

    void Update()
    {
        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        bool primaryButtonValue;
        if (leftDevice.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue) && primaryButtonValue)
        {
            if (!isCloning)
            {
                Debug.Log("Début du clonage");
                StartCoroutine(CloningRoutine());
            }
        }
    }

    IEnumerator CloningRoutine()
    {
        isCloning = true;
        recordedFrames.Clear();

        yield return StartCoroutine(blueScreenFade.Flash());
        blueScreenFade.StartFade();

        float recordTime = 10f;
        float timer = 0f;
        int lastDisplayedSecond = Mathf.CeilToInt(recordTime);

        // Affichage initial
        if (cloningText != null)
            cloningText.text = $"Clonage... {lastDisplayedSecond} sec restantes";

        while (timer < recordTime)
        {
            RecordFrame();

            timer += Time.deltaTime;

            int remainingSeconds = Mathf.CeilToInt(recordTime - timer);

            // Mise à jour uniquement si la seconde change
            if (remainingSeconds != lastDisplayedSecond)
            {
                lastDisplayedSecond = remainingSeconds;

                if (cloningText != null)
                    cloningText.text = $"Clonage... {remainingSeconds} sec restantes";
            }

            yield return null;
        }

        yield return StartCoroutine(blueScreenFade.Flash());
        blueScreenFade.ResetFade();

        // Nettoyage du texte à la fin
        if (cloningText != null)
            cloningText.text = "";

        Debug.Log("Enregistrement terminé.");
        Debug.Log("Nombre total de frames : " + recordedFrames.Count);

        PrintRecordedData();

        isCloning = false;
    }

    void RecordFrame()
    {
        FrameData frame = new FrameData
        {
            rigPosition = xrRig.position,
            rigRotation = xrRig.rotation,

            headPosition = head.position,
            headRotation = head.rotation,

            leftHandPosition = leftHand.position,
            leftHandRotation = leftHand.rotation,

            rightHandPosition = rightHand.position,
            rightHandRotation = rightHand.rotation
        };

        recordedFrames.Add(frame);
    }

    void PrintRecordedData()
    {
        if (recordedFrames.Count == 0)
        {
            Debug.Log("Aucune frame enregistrée.");
            return;
        }

        FrameData first = recordedFrames[0];
        Debug.Log("=== PREMIÈRE FRAME ===");
        Debug.Log("Rig Position: " + first.rigPosition + " | Rotation: " + first.rigRotation.eulerAngles);

        FrameData last = recordedFrames[recordedFrames.Count - 1];
        Debug.Log("=== DERNIÈRE FRAME ===");
        Debug.Log("Rig Position: " + last.rigPosition + " | Rotation: " + last.rigRotation.eulerAngles);
    }
}
