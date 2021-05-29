using System.Linq;
using EventManagement;
using Events;
using Items;
using Items.CurrencyItems;
using Items.Gear;
using LootBoxes;
using PlayerData;
using UnityEngine;
using UnityEngine.UI;
public class CheatMenu : MonoBehaviour
{
    public Transform content;
    public Button prefab;
    IMessageHandler messageHandler;
    private void Start()
    {
        messageHandler = FindObjectOfType<EventsBroker>();
        DontDestroyOnLoad(gameObject);
        ShowItemMenu();
    }

    public void ShowItemMenu()
    {
        foreach (var item in AllItems.ItemIndexer.indexer.Values)
        {
            if (item is Equipment equipment)
            {
                CreateButton(equipment);
            }

            if (item is CurrencySo currencySo)
            {
                CreateButton(currencySo);
            }

            if (item is LootBox lootBox)
            {
                CreateButton(lootBox);
            }
        }
    }

    private void CreateButton(LootBox lootBox)
    {
        if (lootBox.isLootStealer)
            return;
        var instance = Instantiate(prefab, content);
        var label = $"Give {lootBox}";
        instance.GetComponentInChildren<Text>().text = label;
        instance.onClick.AddListener(() => messageHandler.Publish(new AddItemToInventoryEvent(lootBox)));
    }

    private void CreateButton(CurrencySo currencySo)
    {
        var instance = Instantiate(prefab, content);
        var label = $"Give {currencySo.Name}";
        instance.GetComponentInChildren<Text>().text = label;
        instance.onClick.AddListener(currencySo.Use);
    }

    private void CreateButton(Equipment equipment)
    {
        var instance = Instantiate(prefab, content);
        var label = $"Give {equipment.RarityValue} Star {equipment.Name}";
        instance.GetComponentInChildren<Text>().text = label;
        instance.onClick.AddListener(() =>
        {
            var gearInstance = new GearInstance(new GearSaveData(equipment));
            messageHandler.Publish(new AddItemToInventoryEvent(gearInstance));
        });
    }
}