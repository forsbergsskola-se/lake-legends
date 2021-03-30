using System;
using System.Collections;
using Items;
using LootBoxes;
using UnityEngine;

namespace UI
{
    public class ChestOpening : MonoBehaviour
    {
        [SerializeField] private LootBoxItemSlot itemSlot;
        [SerializeField] private LootBoxGoDictionary Dictionary;
        private GameObject currentLootBox;
        public void StartOpening(LootBox lootBox, IItem item)
        {
            gameObject.SetActive(true);
            itemSlot.Setup(item);
            currentLootBox = Dictionary[lootBox];
            currentLootBox.SetActive(true);
            currentLootBox.GetComponent<Animator>().SetBool("Open", true);
            StartCoroutine(CloseBox());
        }

        public IEnumerator CloseBox()
        {
            var animator = currentLootBox.GetComponent<Animator>();
            var length = animator.GetAnimatorTransitionInfo(0).duration + animator.GetNextAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(length);
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Opened"))
            {
                yield return null;
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
            currentLootBox.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    [Serializable]
    public class LootBoxGoDictionary : SerializableDictionary<LootBox, GameObject>
    {
        
    }
}