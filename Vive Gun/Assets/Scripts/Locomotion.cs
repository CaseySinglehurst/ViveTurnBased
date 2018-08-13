using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : MonoBehaviour {


    public float  currentStamina, maxStamina;
    // line renderer that shows player where the controller is pointing when moving
    LineRenderer lr;

    [Header("Reference Objects")]
    //where the vive headset is
    public GameObject head;
    // current controller that is pressing the movement button
    GameObject locomotionController;

    float moveDistance;
    Vector3 movePoint;

    public GameObject moveIndicator;
    MeshRenderer moveIndicatorRenderer;
    
    bool canMove = false;

    [Header("Move Indicator Materials")]
    public Material canMoveMaterial;
    public Material cantMoveMaterial;


	// Use this for initialization
	void Start () {
        lr = GetComponent<LineRenderer>();
        moveIndicatorRenderer = moveIndicator.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        // regen stamina over time
        if (currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime;
            if (currentStamina > maxStamina){
                currentStamina = maxStamina;
            }
            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
        }

        if (locomotionController != null)
        {
            //create a raycast form the controller to where the controller is pointing
            Vector3 rayDirection = locomotionController.transform.TransformDirection(Vector3.forward);
            RaycastHit hit;
            Debug.DrawRay(locomotionController.transform.position, rayDirection);
            if (Physics.Raycast(locomotionController.transform.position, rayDirection, out hit, 100, 1 << LayerMask.NameToLayer("Surface"))) // surface applies to any object on the floor plane
            {
                moveIndicator.SetActive(true);
                lr.enabled = true;
                if (hit.transform.tag == "Floor")
                {
                    canMove = true;
                    movePoint = GetNearest(hit.point);
                    moveIndicatorRenderer.material = canMoveMaterial;
                }
                else
                {
                    canMove = false;
                    movePoint = hit.point;
                    moveIndicatorRenderer.material = cantMoveMaterial;

                }
                lr.positionCount = 2;
                lr.SetPosition(0, locomotionController.transform.position);
                lr.SetPosition(1, hit.point);
            }
            else
            {
                canMove = false;
                moveIndicator.SetActive(false);
                lr.enabled = false;
            }
            if (moveIndicator.activeInHierarchy)
            {
                moveIndicator.transform.position = movePoint;
            }
        }

}
    // when player presses movement button on a controller
    public void StartLocomotion(GameObject startingController)
    {
        //if player isnt already using a movement button
        if (locomotionController == null)
        {
            locomotionController = startingController;
            
        }
    }
    // happens when player releases movement button on a controller
    public void EndLocomotion(GameObject endingController)
    {
        if (locomotionController == endingController)
        {
            Teleport();
            locomotionController = null;
            canMove = false;
            moveIndicator.SetActive(false);
            lr.enabled = false;
            
        }
    }

    void Teleport()
    {
        if (canMove)
        {
            //subtract stamina relative to how far player has moved
            currentStamina -= ( (movePoint + new Vector3(head.transform.position.x, 0, head.transform.position.z)) - transform.position).magnitude;
            //get the offset between the rig and the headset
            Vector3 transformOffset = transform.position - head.transform.position;
            // move player so the headset is where the player pointed to, not the rig
            transform.position = movePoint + new Vector3(transformOffset.x, 0, transformOffset.z);
        }
    }


    // get the nearest point that the player can move to in regards to where the player is pointing to
    //(if player doesnt have enough stamina to reach a point, they can still move some distance towards it)
    Vector3 GetNearest(Vector3 target)
    {
        float targetDistance = (target - new Vector3(head.transform.position.x, 0, head.transform.position.z)).magnitude;

        if (targetDistance <= currentStamina)
        {
            return target;
        }
        else if (currentStamina > 0)
        {
            Vector3 vectorToTarget = (target - new Vector3(head.transform.position.x, 0, head.transform.position.z)).normalized;
            vectorToTarget = vectorToTarget * currentStamina;
            return (new Vector3(head.transform.position.x, 0, head.transform.position.z) + vectorToTarget);
        }
        else
        {
            return new Vector3(head.transform.position.x, 0, head.transform.position.z);
        }
        
    }
}
