using UnityEngine;

[CreateAssetMenu(fileName = "New Fishing Rod", menuName =  "ScriptableObjects/FishingRod")]
public class FishingRod : ScriptableObject, IEquipable, IGear
{
    public void Equip()
    {
        throw new System.NotImplementedException();
    }

    public void UnEquip()
    {
        throw new System.NotImplementedException();
    }

    public void Use()
    {
        throw new System.NotImplementedException();
    }
}