using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneReplay : MonoBehaviour
{
    private List<FrameData> frames;
    private int currentFrame = 0;
    private Vector3 recordStartHeadPos;
    private Vector3 replayStartHeadPos;
    
    public void SetRecording(List<FrameData> recordedFrames, Vector3 recordingStartHeadPosition, Vector3 currentHeadPosition)
    {
        frames = new List<FrameData>(recordedFrames);
        recordStartHeadPos = recordingStartHeadPosition;
        replayStartHeadPos = currentHeadPosition;
        StartCoroutine(ReplayRoutine());
    }
    
IEnumerator ReplayRoutine()
{
    while (currentFrame < frames.Count)
    {
        FrameData frame = frames[currentFrame];

        // Reconvertir les positions locales en world space
        // en utilisant la position/rotation du clone comme référence
        transform.position = replayStartHeadPos + frame.headPosition;
        transform.rotation = frame.headRotation;

        currentFrame++;
        yield return null;
    }

    yield return new WaitForSeconds(0.5f);
    Destroy(gameObject);
}
}