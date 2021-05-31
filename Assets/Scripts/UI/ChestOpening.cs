using System;
using System.Collections;
using EventManagement;
using Events;
using Items;
using LootBoxes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

namespace UI
{
    public class ChestOpening : MonoBehaviour
    {
        [SerializeField] private LootBoxItemSlot itemSlot;
        [SerializeField] private LootBoxGoDictionary videos;
        [SerializeField] private LootBoxStringDictionary soundEvents;
        [SerializeField] private LootBoxColorDictionary textColors;
        [SerializeField] private GameObject skipButton;
        [SerializeField] private bool skipable;
        private GameObject currentGameObject;
        private LootBox currentLootBox;
        public void StartOpening(LootBox lootBox, IItem item)
        {
            currentLootBox = lootBox;
            gameObject.SetActive(true);
            itemSlot.Setup(item, textColors[currentLootBox]);
            currentGameObject = videos[currentLootBox].gameObject;
            currentGameObject.SetActive(true);
            var video = videos[currentLootBox];
            video.GetComponent<VideoPlayer>().Play();
            StartCoroutine(CloseBox());
        }

        private void Update()
        {
            if (!skipable)
                return;
            if (Input.GetMouseButton(0))
                Skip();
        }

        public void Skip()
        {
            itemSlot.Reset();
            StopOpening();
        }

        private IEnumerator CloseBox()
        {
            var video = currentGameObject.GetComponent<VideoPlayer>();
            yield return new WaitForSeconds(2f);
            if (soundEvents.TryGetValue(currentLootBox, out var value))
            {
                FindObjectOfType<EventsBroker>().Publish(new PlaySoundEvent(SoundType.Sfx, value));
            }
            itemSlot.FadeIn();
            skipable = true;
            skipButton.SetActive(skipable);
            yield return new WaitForSeconds(5f);
            itemSlot.FadeOut();
            while (video.isPlaying)
            {
                yield return null;
            }
            StopOpening();
        }

        public void StopOpening()
        {
            var video = currentGameObject.GetComponent<VideoPlayer>();
            video.Stop();
            skipable = false;
            skipButton.SetActive(skipable);
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

    [Serializable]
    public class LootBoxColorDictionary : SerializableDictionary<LootBox, Color>
    {
        
    }
}