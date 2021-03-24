using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnSceneLoad : MonoBehaviour
{
    void OnLevelWasLoaded(int level)
    {
        gameObject.SetActive(false);
    }
}
