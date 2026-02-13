using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxReplay : MonoBehaviour
{
    private List<BoxFrameData> frames;
    private int currentFrame = 0;
    private bool isReplaying = false;
    
    public void SetRecording(List<BoxFrameData> recordedFrames)
    {
        frames = new List<BoxFrameData>(recordedFrames);
        
        // Attend un peu avant de commencer la relecture (pour synchroniser avec le clone)
        StartCoroutine(WaitAndReplay());
    }
    
    IEnumerator WaitAndReplay()
    {
        // Attend que le clonage se termine complètement
        yield return new WaitForSeconds(0.1f);
        isReplaying = true;
        StartCoroutine(ReplayRoutine());
    }
    
    IEnumerator ReplayRoutine()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        bool hadRigidbody = rb != null;
        
        // Désactive temporairement la physique pendant la relecture
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        
        while (currentFrame < frames.Count)
        {
            BoxFrameData frame = frames[currentFrame];
            
            transform.position = frame.position;
            transform.rotation = frame.rotation;
            
            currentFrame++;
            yield return null;
        }
        
        // Réactive la physique après la relecture
        if (hadRigidbody && rb != null)
        {
            rb.isKinematic = false;
        }
        
        isReplaying = false;
        
        // Retire le composant de replay une fois terminé
        Destroy(this);
    }
    
    void OnDestroy()
    {
        Debug.Log("Relecture de la boîte terminée");
    }
}