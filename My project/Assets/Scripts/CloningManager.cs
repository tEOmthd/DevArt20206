using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CloningManager : MonoBehaviour
{
    [Header("References")]
    public BlueScreenFade blueScreenFade;
    public Transform xrOrigin; // XR Rig
    public Transform Camera; // XR Rig
    
    [Header("UI")]
    public TextMeshProUGUI cloningText;

    [Header("Clone")]
    public GameObject clonePrefab;
    
    [Header("Offsets réglables")]
    public Vector3 positionOffset;
    public Vector3 rotationOffsetEuler;
    
    private bool isCloning = false;
    private List<FrameData> recordedFrames = new List<FrameData>();
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    
    void Start()
    {
        cloningText.text = "";
        initialPosition = xrOrigin.position;
        initialRotation = xrOrigin.rotation;
    }

    void Update()
    {
        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        bool primaryButtonValue;

        if (leftDevice.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue) && primaryButtonValue)
        {
            if (!isCloning)
                StartCoroutine(CloningRoutine());
        }
    }

    IEnumerator CloningRoutine()
    {
        isCloning = true;
        recordedFrames.Clear();

        yield return StartCoroutine(blueScreenFade.Flash());
        blueScreenFade.StartFade();
        
        float recordTime = 5f;
        float timer = 0f;
        int lastDisplayedSecond = Mathf.CeilToInt(recordTime);
        cloningText.text = $"Clonage...\n{lastDisplayedSecond} sec restantes";
        
        while (timer < recordTime)
        {
            RecordFrame();
            timer += Time.deltaTime;

            int remainingSeconds = Mathf.CeilToInt(recordTime - timer);
            if (remainingSeconds != lastDisplayedSecond)
            {
                lastDisplayedSecond = remainingSeconds;
                cloningText.text = $"Clonage...\n{remainingSeconds} sec restantes";
            }

            yield return null;
        }

        yield return StartCoroutine(blueScreenFade.Flash());
        blueScreenFade.ResetFade();
        cloningText.text = "";
        Debug.Log("Enregistrement terminé.");
        
        Vector3 endPosition = xrOrigin.position;
        
        xrOrigin.position = initialPosition;
        xrOrigin.rotation = initialRotation;
        
        if (clonePrefab != null && recordedFrames.Count > 0)
        {
            // Instancie le clone SANS rotation (Quaternion.identity)
            GameObject clone = Instantiate(clonePrefab, endPosition, Quaternion.identity);
            CloneReplay replay = clone.AddComponent<CloneReplay>();
            replay.SetRecording(recordedFrames, positionOffset, rotationOffsetEuler);
        }

        isCloning = false;
    }

    void RecordFrame()
    {
        FrameData frame = new FrameData
        {
            worldPosition = xrOrigin.position,
            worldRotation = Camera.rotation
        };
        recordedFrames.Add(frame);
    }
}