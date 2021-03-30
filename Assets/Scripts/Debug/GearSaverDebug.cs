using System;
using Items.Gear;
using Newtonsoft.Json;
using PlayerData;
using UnityEngine;

public class GearSaverDebug : MonoBehaviour
{
    public Equipment Equipment;
    private GearInstance gearInstance;

    private void Start()
    {
        gearInstance = Load() ?? new GearInstance(new GearSaveData(Equipment));
    }

    private void OnDestroy()
    {
        Save();
    }

    private GearInstance Load()
    {
        var value = PlayerPrefs.GetString("Key", JsonConvert.SerializeObject(new GearSaveData(Equipment)));
        return new GearInstance(JsonConvert.DeserializeObject<GearSaveData>(value));
    }

    private void Save()
    {
        var json= JsonConvert.SerializeObject(gearInstance.GearSaveData);
        PlayerPrefs.SetString("Key", json);
    }
}