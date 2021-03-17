using Fish;
using PlayerData;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{
    public Factory fishFactory;

    public void GenerateFish()
    {
        var fish = fishFactory.GenerateFish(0);
        FindObjectOfType<InventoryHandler>().AddItemToInventory(fish);
    }
}
