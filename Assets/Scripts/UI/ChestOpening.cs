using System;
using System.Collections;
using LootBoxes;
using UnityEngine;

namespace UI
{
    public class ChestOpening : MonoBehaviour
    {
        [SerializeField] private LootBoxGoDictionary Dictionary;
        private GameObject currentLootBox;
        public void StartOpening(LootBox lootBox)
        {
            gameObject.SetActive(true);
            currentLootBox = Dictionary[lootBox];
            currentLootBox.SetActive(true);
            currentLootBox.GetComponent<Animator>().SetBool("Open", true);
            StartCoroutine(CloseClam());
        }

        public IEnumerator CloseClam()
        {
            yield return new WaitForSeconds(7f);
            var animator = currentLootBox.GetComponent<Animator>();
            animator.SetBool("Open", false);
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Default"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(2f);
            StopOpening();
        }

        public void StopOpening()
        {
            currentLootBox.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    [Serializable]
    public class LootBoxGoDictionary : SerializableDictionary<LootBox, GameObject>
    {
        
    }
}