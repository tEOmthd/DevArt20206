using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CloningManager : MonoBehaviour
{
    [Header("References")]
    public BlueScreenFade blueScreenFade;
    public Transform xrRig;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    [Header("UI")]
    public TextMeshProUGUI cloningText;

    [Header("Clone")]
    public GameObject clonePrefab;

    private bool isCloning = false;
    private List<FrameData> recordedFrames = new List<FrameData>();
    private Vector3 initialHeadPosition; // 👈 position de la TÊTE au début

    void Start()
    {
        cloningText.text = "";
        initialHeadPosition = head.position;
    }

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

        if (cloningText != null)
            cloningText.text = $"Clonage... {lastDisplayedSecond} sec restantes";

        // 👇 On enregistre la position de départ de la tête
        Vector3 recordingStartHeadPos = head.position;

        while (timer < recordTime)
        {
            RecordFrame();
            timer += Time.deltaTime;

            int remainingSeconds = Mathf.CeilToInt(recordTime - timer);
            if (remainingSeconds != lastDisplayedSecond)
            {
                lastDisplayedSecond = remainingSeconds;
                if (cloningText != null)
                    cloningText.text = $"Clonage...\n{remainingSeconds} sec restantes";
            }

            yield return null;
        }

        yield return StartCoroutine(blueScreenFade.Flash());
        blueScreenFade.ResetFade();
        cloningText.text = "";

        Debug.Log("Enregistrement terminé.");
        Debug.Log("Nombre total de frames : " + recordedFrames.Count);
        PrintRecordedData();

        // 👇 Sauvegarde la position actuelle de la tête
        Vector3 currentHeadPosition = head.position;

        // 👇 Téléportation du XR Rig pour remettre la tête à sa position initiale
        Vector3 headOffset = head.position - xrRig.position;
        xrRig.position = initialHeadPosition - headOffset;

        // 👇 Création du clone
        if (clonePrefab != null)
        {
            Vector3 spawnPosition = new Vector3(currentHeadPosition.x, 0f, currentHeadPosition.z);
            GameObject clone = Instantiate(clonePrefab, spawnPosition, Quaternion.identity);
            CloneReplay replay = clone.AddComponent<CloneReplay>();
            replay.SetRecording(recordedFrames, recordingStartHeadPos, currentHeadPosition);
        }

        isCloning = false;
    }

    void RecordFrame()
{
    FrameData frame = new FrameData
    {
        // Positions relatives au XR Rig
        headPosition = xrRig.InverseTransformPoint(head.position),
        headRotation = Quaternion.Inverse(xrRig.rotation) * head.rotation,
        leftHandPosition = xrRig.InverseTransformPoint(leftHand.position),
        leftHandRotation = Quaternion.Inverse(xrRig.rotation) * leftHand.rotation,
        rightHandPosition = xrRig.InverseTransformPoint(rightHand.position),
        rightHandRotation = Quaternion.Inverse(xrRig.rotation) * rightHand.rotation
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
        Debug.Log("Head Position: " + first.headPosition + " | Rotation: " + first.headRotation.eulerAngles);

        FrameData last = recordedFrames[recordedFrames.Count - 1];
        Debug.Log("=== DERNIÈRE FRAME ===");
        Debug.Log("Head Position: " + last.headPosition + " | Rotation: " + last.headRotation.eulerAngles);
    }
}