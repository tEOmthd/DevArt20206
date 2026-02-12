using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;

public class CloningManager : MonoBehaviour
{
    [Header("References")]
    public BlueScreenFade blueScreenFade;

    public Transform xrRig;          // XR Origin / Player root
    public Transform head;           // Camera (CenterEye)
    public Transform leftHand;       
    public Transform rightHand;

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

        while (timer < recordTime)
        {
            RecordFrame();
            timer += Time.deltaTime;
            yield return null;
        }

        yield return StartCoroutine(blueScreenFade.Flash());
        blueScreenFade.ResetFade();

        Debug.Log("Enregistrement terminé.");
        Debug.Log("Nombre total de frames : " + recordedFrames.Count);

        PrintRecordedData(); // 👈 on affiche les données

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

        // Première frame
        FrameData first = recordedFrames[0];
        Debug.Log("=== PREMIÈRE FRAME ===");
        Debug.Log("Rig Position: " + first.rigPosition + " | Rotation: " + first.rigRotation.eulerAngles);
        Debug.Log("Head Position: " + first.headPosition + " | Rotation: " + first.headRotation.eulerAngles);
        Debug.Log("Left Hand Position: " + first.leftHandPosition + " | Rotation: " + first.leftHandRotation.eulerAngles);
        Debug.Log("Right Hand Position: " + first.rightHandPosition + " | Rotation: " + first.rightHandRotation.eulerAngles);

        // Dernière frame
        FrameData last = recordedFrames[recordedFrames.Count - 1];
        Debug.Log("=== DERNIÈRE FRAME ===");
        Debug.Log("Rig Position: " + last.rigPosition + " | Rotation: " + last.rigRotation.eulerAngles);
        Debug.Log("Head Position: " + last.headPosition + " | Rotation: " + last.headRotation.eulerAngles);
        Debug.Log("Left Hand Position: " + last.leftHandPosition + " | Rotation: " + last.leftHandRotation.eulerAngles);
        Debug.Log("Right Hand Position: " + last.rightHandPosition + " | Rotation: " + last.rightHandRotation.eulerAngles);
    }



}
