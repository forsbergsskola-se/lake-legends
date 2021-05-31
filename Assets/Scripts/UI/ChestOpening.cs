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
        [SerializeField] private LootBoxGoDictionary Dictionary;
        [SerializeField] private LootBoxGoDictionary videos;
        [SerializeField] private LootBoxStringDictionary soundEvents;
        [SerializeField] private LootBoxColorDictionary textColors;
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
            //currentGameObject.GetComponent<Animator>().SetBool("Open", true);
            StartCoroutine(CloseBox());
        }

        public IEnumerator CloseBox()
        {
            //var animator = currentGameObject.GetComponent<Animator>();
            var video = currentGameObject.GetComponent<VideoPlayer>();
            yield return new WaitForSeconds(2f);
            if (soundEvents.TryGetValue(currentLootBox, out var value))
            {
                FindObjectOfType<EventsBroker>().Publish(new PlaySoundEvent(SoundType.Sfx, value));
            }
            itemSlot.FadeIn();
            yield return new WaitForSeconds(5f);
            //animator.SetBool("Open", false);
            itemSlot.FadeOut();
            while (video.isPlaying)
            {
                yield return null;
            }
            //while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Default"))
            //{
                //yield return null;
            //}
            StopOpening();
        }

        public void StopOpening()
        {
            var video = currentGameObject.GetComponent<VideoPlayer>();
            video.Stop();
            //currentGameObject.SetActive(false);
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