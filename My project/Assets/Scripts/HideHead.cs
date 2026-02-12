using UnityEngine;

public class HideHead : MonoBehaviour
{
    public Transform headBone;
    void LateUpdate()
    {
        headBone.localScale = Vector3.zero;
    }
}