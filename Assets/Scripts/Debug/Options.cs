using UnityEngine;

public class Options : MonoBehaviour
{
    public void SetFps(bool mode)
    {
        Application.targetFrameRate = mode ? 60 : 30;
    }

    public void VSync(bool state)
    {
        QualitySettings.vSyncCount = state ? 1 : 0;
    }
}