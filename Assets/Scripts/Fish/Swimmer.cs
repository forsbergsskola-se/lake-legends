using UnityEngine;

namespace Fish
{
    public class Swimmer : MonoBehaviour
    {
        [SerializeField] float swimmingAreaSize = 6f;
        [SerializeField] float closeEnoughToTarget = 0.5f;
        [SerializeField] float minMoveDistance = 1.5f;
        [SerializeField] float speed = 1.5f;
        [SerializeField] float rotationSpeed = 1f;

        [SerializeField] float deviancyEffect = 1f;
        [SerializeField] float deviancyTime = 1f;
        float lastDeviance;
        Vector3 devianceDirection;

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
            return new Vector3(Random.Range(startPos.x - swimmingAreaSize, startPos.x + swimmingAreaSize), Random.Range(startPos.y - 0.5f, startPos.y + 0.5f), Random.Range(startPos.z - swimmingAreaSize, startPos.z + swimmingAreaSize));
        }
        void Update()
        {
            if (!IsAtTarget)
            {
                //this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, speed * Time.deltaTime);
                transform.position += transform.forward * speed * Time.deltaTime;
                Rotate();
                AddDeviance();
            }
            else
            {
                this.targetPos = SetNewTarget(); 
                FixMinMaxRange();
            }
        }
        void Rotate()
        {
            //transform.LookAt(targetPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((targetPos - transform.position).normalized), rotationSpeed);
        }
        void FixMinMaxRange()
        {
            if ((targetPos - transform.position).magnitude < minMoveDistance)
            {
                targetPos = ((targetPos - transform.position).normalized * minMoveDistance) + transform.position;
            }
            if ((targetPos - startPos).magnitude > swimmingAreaSize)
            {
                targetPos = ((targetPos - startPos).normalized * swimmingAreaSize) + startPos;
            }
        }
        void AddDeviance()
        {
            if (Time.time - lastDeviance > deviancyTime)
            {
                targetPos -= devianceDirection;
                devianceDirection = new Vector3(Random.Range(-deviancyEffect, deviancyEffect), 0, Random.Range(-deviancyEffect, deviancyEffect));
                targetPos += devianceDirection;
                lastDeviance = Time.time;
            }

            //    if(Time.time - lastDeviance < deviancyTime)
            //    {
            //        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(devianceDirection), deviancyEffect + rotationSpeed);
            //    }
            //    else
            //    {
            //   
            //        devianceDirection = new Vector3(transform.rotation.x + Random.Range(-10f, 10.0f), transform.rotation.y + Random.Range(-10f, 10.0f), transform.rotation.z + Random.Range(-10f, 10.0f));
            //        lastDeviance = Time.time;
            //    }
        }

        private void OnDrawGizmos()
        {
            if(startPos == Vector3.zero)
                Gizmos.DrawWireSphere(transform.position, swimmingAreaSize);
            else
                Gizmos.DrawWireSphere(startPos, swimmingAreaSize);
        }
    }
}