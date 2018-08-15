using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{

    public Transform railStart, railEnd;
    
    public GameObject attachment = null;

    
    public Vector3 GetClosestPointOnRail(Vector3 target)
    {
        Vector3 closestPointOnRail = new Vector3(0, 0, 0);
        closestPointOnRail = transform.position;

        Vector3 onNormal = transform.position - (railStart.position);
        Vector3 targetPosition = target - transform.position;

        Vector3 projection = Vector3.Project(targetPosition, onNormal);

        closestPointOnRail += projection;

        if (Vector3.Magnitude(closestPointOnRail - transform.position) > Vector3.Magnitude(railEnd.position - transform.position))
        {

            if (Vector3.Distance(closestPointOnRail, railStart.position) > Vector3.Distance(closestPointOnRail, railEnd.position))
            {
                closestPointOnRail = railEnd.position;
            }
            else
            {
                closestPointOnRail = railStart.position;
            }
        }
        return closestPointOnRail;
    }
    
}