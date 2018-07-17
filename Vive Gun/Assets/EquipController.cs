using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipController : MonoBehaviour {

    public GameObject controllerPosition;

    public bool isEquipped = true;

    float objectMoveSpeed = 20;

    public Vector3  holdRotationOffset;

    public GameObject grip;
    public GameObject gripController;

    public bool isGripped = false;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {


        if (isEquipped)
        {
            transform.position = Vector3.Lerp(transform.position, controllerPosition.transform.position, objectMoveSpeed * Time.deltaTime);
            Quaternion holdRotation = new Quaternion();
            if (isGripped)
            {
                Vector3 lookAtPosition = gripController.transform.position + (grip.transform.position - transform.position) ;

                transform.LookAt(lookAtPosition);
            }
            else
            {
                holdRotation = controllerPosition.transform.rotation * Quaternion.Euler(holdRotationOffset);
                transform.rotation = Quaternion.Lerp(transform.rotation, holdRotation, objectMoveSpeed * Time.deltaTime);
            }

        }
    }
    
    }
    
