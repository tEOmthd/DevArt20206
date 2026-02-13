using UnityEngine;
using System.Collections.Generic;

public class BoxRecorder : MonoBehaviour
{
    [Header("References")]
    public CloningManager cloningManager;
    
    private List<BoxFrameData> recordedBoxFrames = new List<BoxFrameData>();
    private bool wasCloning = false;
    private Vector3 recordStartPosition;
    private Quaternion recordStartRotation;
    
    void Start()
    {
        if (cloningManager == null)
        {
            cloningManager = FindObjectOfType<CloningManager>();
            if (cloningManager == null)
            {
                Debug.LogError("CloningManager introuvable !");
            }
        }
    }
    
    void Update()
    {
        // Détecte le début du clonage
        if (cloningManager.isCloning && !wasCloning)
        {
            StartRecording();
        }
        
        // Enregistre pendant le clonage
        if (cloningManager.isCloning)
        {
            RecordBoxFrame();
        }
        
        // Détecte la fin du clonage
        if (!cloningManager.isCloning && wasCloning)
        {
            EndRecording();
        }
        
        wasCloning = cloningManager.isCloning;
    }
    
    void StartRecording()
    {
        recordedBoxFrames.Clear();
        recordStartPosition = transform.position;
        recordStartRotation = transform.rotation;
        Debug.Log("Début de l'enregistrement de la boîte");
    }
    
    void RecordBoxFrame()
    {
        BoxFrameData frame = new BoxFrameData
        {
            position = transform.position,
            rotation = transform.rotation
        };
        recordedBoxFrames.Add(frame);
    }
    
    void EndRecording()
    {
        Debug.Log($"Fin de l'enregistrement : {recordedBoxFrames.Count} frames enregistrées");
        
        // Remet la boîte à sa position initiale
        transform.position = recordStartPosition;
        transform.rotation = recordStartRotation;
        
        // Ajoute le composant de replay à la boîte
        if (recordedBoxFrames.Count > 0)
        {
            BoxReplay replay = gameObject.AddComponent<BoxReplay>();
            replay.SetRecording(recordedBoxFrames);
        }
    }
}

[System.Serializable]
public class BoxFrameData
{
    public Vector3 position;
    public Quaternion rotation;
}