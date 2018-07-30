using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipController : MonoBehaviour {

    public GameObject mainHandController; 

    public bool isEquipped = true;

    float objectMoveSpeed = 20;

    public Vector3  holdRotationOffset; // rotation of guns hold position I.E you hold a wand straight but a sniper rifle at 45 degrees

    public GameObject grip; // position of the available secondary holding spot
    public GameObject gripController; // controller that is currently holding the grip

    public bool isGripped = false;

    bool gripForward = true;
    // Use this for initialization
    void Start()
    {
        if (transform.position.magnitude >= grip.transform.position.magnitude)
        {
            gripForward = true;
        }
        else
        {
            gripForward = false;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (isEquipped)
        {
            transform.position = Vector3.Lerp(transform.position, mainHandController.transform.position, objectMoveSpeed * Time.deltaTime); // move towards main hand position#
            
            if (isGripped) // we want to rotate towards the off hand grip
            {
                Vector3 lookAtPosition = new Vector3();
                if (gripForward)
                {
                    lookAtPosition = gripController.transform.position - transform.position; // get the vector from main hand to off hand
                }
                else
                {
                    lookAtPosition = transform.position - gripController.transform.position; // get the vector from main hand to off hand
                }
                
                Quaternion lookRotation = Quaternion.LookRotation(lookAtPosition); // get a rotation through that vector (only rotates x and y directions)
                lookRotation = Quaternion.Euler(lookRotation.eulerAngles.x, lookRotation.eulerAngles.y, mainHandController.transform.rotation.eulerAngles.z); // rotate z to be the same as main hand controller 
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, objectMoveSpeed * Time.deltaTime);
            }
            else // we want to follow the position and rotation of the main hand only
            {
                Quaternion holdRotation = mainHandController.transform.rotation * Quaternion.Euler(holdRotationOffset);
                transform.rotation = Quaternion.Lerp(transform.rotation, holdRotation, objectMoveSpeed * Time.deltaTime);
            }

        }

        if (gripController != null)
        {
            CheckIfStillHoldingGrip();
        }

    }
    
    void CheckIfStillHoldingGrip()
    {
        if (Mathf.Abs((grip.transform.position - gripController.transform.position).magnitude) >= .35f)
        {
            gripController.GetComponent<ControllerManager>().ReleaseGrip(grip);
        }
    }

    }
    
