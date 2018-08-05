using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : MonoBehaviour {


    public float  currentStamina, maxStamina;

    LineRenderer lr;

    [Header("Reference Objects")]
    public GameObject head;
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
            Vector3 rayDirection = locomotionController.transform.TransformDirection(Vector3.forward);
            RaycastHit hit;
            Debug.DrawRay(locomotionController.transform.position, rayDirection);
            if (Physics.Raycast(locomotionController.transform.position, rayDirection, out hit, 100, 1 << LayerMask.NameToLayer("Surface")))
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

    public void StartLocomotion(GameObject startingController)
    {
        if (locomotionController == null)
        {
            locomotionController = startingController;
            
        }
    }

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
            currentStamina -= ( (movePoint + new Vector3(head.transform.position.x, 0, head.transform.position.z)) - transform.position).magnitude;
            Vector3 transformOffset = transform.position - head.transform.position;

            transform.position = movePoint + new Vector3(transformOffset.x, 0, transformOffset.z);
        }
    }

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
