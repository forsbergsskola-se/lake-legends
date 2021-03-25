using UnityEngine;

namespace Fish
{
    public class Swimmer : MonoBehaviour
    {
        [SerializeField] float swimmingAreaSize = 6f;
        [SerializeField] float closeEnoughToTarget = 0.5f;
        [SerializeField] float speed = 1.5f;
        
        Vector3 targetPos;
        Vector3 startPos;
        void Start()
        {
            this.startPos = transform.position;
            targetPos = SetNewTarget();
        }
        bool IsAtTarget => Vector3.Distance(this.transform.position, targetPos) < closeEnoughToTarget;
        
        Vector3 SetNewTarget()
        {
            return new Vector3(
                Random.Range(startPos.x - swimmingAreaSize, startPos.x + swimmingAreaSize),
                this.transform.position.y, 
                Random.Range(startPos.z - swimmingAreaSize, startPos.z + swimmingAreaSize));
        }
        void Update()
        {
            if (!IsAtTarget)
            {
                this.transform.position =
                    Vector3.MoveTowards(this.transform.position, targetPos, speed * Time.deltaTime);
                this.transform.LookAt(targetPos);
            }
            else
            {
                this.targetPos = SetNewTarget();   
            }
        }
    }
}