
using PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class BioArea : MonoBehaviour
    {
        public Text title;
        public Text bio;
        public Text amountText;
        public void Setup(FishItem fishItem)
        {
            title.text = fishItem.Name;
            bio.text = fishItem.Bio;
            amountText.text =
                $"Amount Caught: {FindObjectOfType<InventoryHandler>().FisherDexData.GetAllItems()[fishItem.ID]}";
        }
    }
}