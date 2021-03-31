using System;
using System.Collections;
using EventManagement;
using Events;
using Items;
using LootBoxes;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChestOpening : MonoBehaviour
    {
        [SerializeField] private LootBoxItemSlot itemSlot;
        [SerializeField] private LootBoxGoDictionary Dictionary;
        [SerializeField] private LootBoxStringDictionary soundEvents;
        private GameObject currentGameObject;
        private LootBox currentLootBox;
        public void StartOpening(LootBox lootBox, IItem item)
        {
            currentLootBox = lootBox;
            gameObject.SetActive(true);
            itemSlot.Setup(item);
            currentGameObject = Dictionary[currentLootBox];
            currentGameObject.SetActive(true);
            currentGameObject.GetComponent<Animator>().SetBool("Open", true);
            StartCoroutine(CloseBox());
        }

        public IEnumerator CloseBox()
        {
            var animator = currentGameObject.GetComponent<Animator>();
            yield return new WaitForSeconds(2f);
            if (soundEvents.TryGetValue(currentLootBox, out var value))
            {
                FindObjectOfType<EventsBroker>().Publish(new PlaySoundEvent(SoundType.Sfx, value));
            }
            itemSlot.FadeIn();
            yield return new WaitForSeconds(5f);
            animator.SetBool("Open", false);
            itemSlot.FadeOut();
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Default"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            StopOpening();
        }

        public void StopOpening()
        {
            currentGameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    [Serializable]
    public class LootBoxGoDictionary : SerializableDictionary<LootBox, GameObject>
    {
        
    }
    
    [Serializable]
    public class LootBoxStringDictionary : SerializableDictionary<LootBox, string>
    {
        
    }
}