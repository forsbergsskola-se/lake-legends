using Fish;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{
    public Factory fishFactory;

    public void GenerateFish()
    {
        Debug.Log(fishFactory.GenerateFish().ToString());
    }
}
