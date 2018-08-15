using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttachment : MonoBehaviour {

    public Transform target;

    public TestRail testRailScript;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = testRailScript.gameObject.transform.position;

        Vector3 onNormal = transform.position -  (testRailScript.startRail.position);
        Debug.DrawLine( (testRailScript.startRail.position), transform.position, Color.red, 0, false);
        Vector3 targetPosition = target.transform.position - transform.position;
        Debug.DrawLine(transform.position, target.transform.position, Color.green, 0, false);

        Vector3 projection = Vector3.Project(targetPosition, onNormal);
        Debug.DrawLine(transform.position, transform.position + projection, Color.blue, 0, false);


        transform.Translate(projection);

        if (Vector3.Magnitude(transform.position - testRailScript.gameObject.transform.position) > Vector3.Magnitude(testRailScript.endRail.position - testRailScript.gameObject.transform.position))
        {

            if (Vector3.Distance(transform.position, testRailScript.startRail.position) > Vector3.Distance(transform.position, testRailScript.endRail.position))
            {
                transform.position = testRailScript.endRail.position;
            }
            else
            {
                transform.position = testRailScript.startRail.position;
            }
        }
        
    }
}
