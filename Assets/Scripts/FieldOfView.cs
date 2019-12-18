using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField]
    public float viewRadius;
    [SerializeField]
    [Range(0,359)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obsticleMask;
    public List<Transform> visibleTargets;
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while(true){
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        visibleTargets.Clear();
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            //check if its in view angle
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                //performa raycast to check for obsticles
                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obsticleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }


    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad) , 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


}
