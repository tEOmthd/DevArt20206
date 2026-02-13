using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloneReplay : MonoBehaviour
{
    private List<FrameData> frames;
    private int currentFrame = 0;
    private Vector3 positionOffset;
    private Quaternion rotationOffset;
    private Animator animator;
    
    [Header("Walking Detection")]
    public float movementThreshold = 0.01f;
    
    void Awake()
    {
        // Cherche l'Animator sur cet objet ET dans les enfants (au cas où le modèle est un enfant)
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Aucun Animator trouvé sur le clone !");
        }
        else
        {
            Debug.Log("Animator trouvé et prêt !");
        }
    }
    
    public void SetRecording(List<FrameData> recordedFrames, Vector3 posOffset, Vector3 rotOffsetEuler)
    {
        frames = new List<FrameData>(recordedFrames);
        positionOffset = posOffset;
        rotationOffset = Quaternion.Euler(rotOffsetEuler);
        StartCoroutine(ReplayRoutine());
    }
    
    IEnumerator ReplayRoutine()
    {
        Vector3 previousPosition = transform.position;
        
        while (currentFrame < frames.Count)
        {
            FrameData frame = frames[currentFrame];
            Vector3 newPosition = frame.worldPosition + positionOffset;
            
            transform.position = newPosition;
            transform.rotation = frame.worldRotation * rotationOffset;
            
            // Détection du mouvement basée sur le déplacement réel
            if (animator != null)
            {
                // Calcule la distance parcourue depuis la dernière frame
                float distance = Vector3.Distance(previousPosition, newPosition);
                bool isWalking = distance > movementThreshold;
                
                animator.SetBool("isWalking", isWalking);
                
                // Debug optionnel
                // Debug.Log($"Frame {currentFrame}: Distance = {distance}, isWalking = {isWalking}");
            }
            
            previousPosition = newPosition;
            currentFrame++;
            yield return null;
        }
        
        // Arrête l'animation avant de détruire
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
        }
        
        Destroy(gameObject);
    }
}