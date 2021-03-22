using UnityEngine;

public class ChangeTargetFps : MonoBehaviour
{

    public void OnChangeTargetFps(float value)
    {
        Application.targetFrameRate = (int) Mathf.Lerp(30, 144, value);
    }
}
