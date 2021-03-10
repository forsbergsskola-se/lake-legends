using Fish;
using PlayerData;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{
    public Factory fishFactory;

    public void GenerateFish()
    {
        var fish = fishFactory.GenerateFish();
        FindObjectOfType<InventoryHandler>().AddItemToInventory(fish);
    }
}
