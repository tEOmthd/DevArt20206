using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloneReplay : MonoBehaviour
{
    private List<FrameData> frames;
    private int currentFrame = 0;
    private Vector3 positionOffset;
    
    public void SetRecording(List<FrameData> recordedFrames, Vector3 posOffset, Vector3 rotOffsetEuler)
    {
        frames = new List<FrameData>(recordedFrames);
        positionOffset = posOffset;
        
        StartCoroutine(ReplayRoutine());
    }
    
    IEnumerator ReplayRoutine()
    {
        while (currentFrame < frames.Count)
        {
            FrameData frame = frames[currentFrame];
            
            // Position
            transform.position = frame.worldPosition + positionOffset;
            
            // Rotation : regarde vers le prochain point
            if (currentFrame < frames.Count - 1)
            {
                // UTILISE DIRECTEMENT transform.position au lieu de recalculer
                Vector3 nextPos = frames[currentFrame + 1].worldPosition + positionOffset;
                Vector3 direction = nextPos - transform.position;
                
                // Si le mouvement est assez grand pour calculer une direction
                if (direction.sqrMagnitude > 0.0001f)
                {
                    direction.y = 0; // Rotation horizontale seulement
                    
                    if (direction.sqrMagnitude > 0.0001f)
                    {
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                }
            }
            
            currentFrame++;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}