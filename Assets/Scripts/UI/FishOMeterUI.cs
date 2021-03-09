using UnityEngine;

public class FishOMeterUI : MonoBehaviour
{
    //[SerializeField] private Image successBar;
    //[SerializeField] private RectTransform captureZone;
    //[SerializeField] private RectTransform fishPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /*
     *    Good to have for when working on the UI;
     *
     *     Clamps the Capture Zone UI panel/sprite position between 0 + half of its width
     *     and "full" - half of its width
     *      // captureZonePosition = Mathf.Clamp(captureZone.anchoredPosition.x,
            //     0 + (captureZone.rect.width / 2),
            //     GetComponentInParent<RectTransform>().rect.width - (captureZone.rect.width / 2));
     *
     *       For positioning the fishSprite itself to the CaptureZone area at START
     *       
     *      // var fishPositionAnchoredPosition = fishPosition.anchoredPosition;
            // fishPositionAnchoredPosition.x = captureZone.anchoredPosition.x;
     *
     *
     * 
     */
}
