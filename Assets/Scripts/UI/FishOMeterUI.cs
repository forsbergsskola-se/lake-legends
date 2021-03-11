using UnityEngine;
using UnityEngine.UI;

namespace UI
{
       public class FishOMeterUI : MonoBehaviour
       {
              [SerializeField] public Image successBar;
              [SerializeField] public RectTransform captureZone;
              [SerializeField] public RectTransform captureAreaParentZone;
              [SerializeField] public RectTransform fishPosition;
       }
}
