using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipController : MonoBehaviour {

    public Gun equippedGun;
    public GameObject currentGun;
    public GameObject mainHandController; 

    public bool isEquipped = true;

    float objectMoveSpeed = 20;
    
    // position of the available secondary holding spot
    GameObject grip;
    // controller that is currently holding the grip
    public GameObject gripController;

    public bool isGripped = false;

    bool gripForward = true;
    // Use this for initialization
    void Start()
    {

        currentGun = Instantiate(equippedGun.gunPrefab, this.transform);
        grip = GetChildWithTag(currentGun, "Grip");
        //if (transform.position.magnitude >= grip.transform.position.magnitude)

        if (grip.transform.localPosition.z >= 0)
        {
            Debug.Log("grip is forward");
            gripForward = true;
        }
        else
        {
            Debug.Log("Grip is backward");
            gripForward = false;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (isEquipped)
        {
            // move towards main hand position
            transform.position = Vector3.Lerp(transform.position, mainHandController.transform.position, objectMoveSpeed * Time.deltaTime);
            // we want to rotate towards the off hand grip
            if (isGripped) 
            {
                Vector3 lookAtPosition = new Vector3();
                if (gripForward)
                {
                    // get the vector from main hand to off hand
                    lookAtPosition = gripController.transform.position - transform.position; 
                }
                else
                {
                    // get the vector from main hand to off hand
                    lookAtPosition = transform.position - gripController.transform.position; 
                }
                // get a rotation through that vector (only rotates x and y directions)
                Quaternion lookRotation = Quaternion.LookRotation(lookAtPosition);
                // rotate z to be the same as main hand controller 
                lookRotation = Quaternion.Euler(lookRotation.eulerAngles.x, lookRotation.eulerAngles.y, mainHandController.transform.rotation.eulerAngles.z); 
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, objectMoveSpeed * Time.deltaTime);
            }
            else // we want to follow the position and rotation of the main hand only
            {
                Quaternion holdRotation = mainHandController.transform.rotation * Quaternion.Euler(equippedGun.holdRotationOffset);
                transform.rotation = Quaternion.Lerp(transform.rotation, holdRotation, objectMoveSpeed * Time.deltaTime);
            }

        }

        if (gripController != null)
        {
            CheckIfStillHoldingGrip();
        }

    }
    
    //if off hand is a certain distance away from the secondary frib, release that grip (for easy un-gripping)
    void CheckIfStillHoldingGrip()
    {
        
        if (Mathf.Abs((grip.transform.position - gripController.transform.position).magnitude) >= .35f)
        {
            gripController.GetComponent<ControllerManager>().ReleaseGrip(grip);
        }
    }


    GameObject GetChildWithTag(GameObject parent, string tag)
    {

        foreach(Transform t in parent.transform)
        {
            if (t.tag == tag)
            {
                return t.gameObject;
            }
        }

        return null;
    }

    }
    
