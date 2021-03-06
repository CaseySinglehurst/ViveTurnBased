﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour {

    // 1
    private SteamVR_TrackedObject trackedObj;
    // 2
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    [SerializeField]
    GameObject currentCollidingObject = null;
    [SerializeField]
    GameObject currentGrabbedObject = null;

    GameObject currentHoldingObject = null;

    public GameObject cameraRig;


    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    private void Update()
    {
        // 1
        if (Controller.GetAxis() != Vector2.zero)
        {
           // Debug.Log(gameObject.name + Controller.GetAxis());
        }

        // 2
        if (Controller.GetHairTriggerDown())
        {

            if (currentCollidingObject != null)
            {
                if (currentCollidingObject.tag == "Attachment")
                {
                    //Debug.Log(gameObject.name + " Trigger Press");
                    GrabAttachment(currentCollidingObject);
                }
            }
        }

        // 3
        if (Controller.GetHairTriggerUp())
        {
            Debug.Log(gameObject.name + " Trigger Release");
            if (currentHoldingObject != null)
            {
                if (currentHoldingObject.tag == "Attachment")
                {
                    ReleaseAttachment(currentHoldingObject);
                }
            }
        }

        // 4
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
           // Debug.Log(gameObject.name + " Grip Press");

            GrabOrReleaseObject();

        }
        // 5
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            //Debug.Log(gameObject.name + " Grip Release");
        }

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            StartLocomotion();
        }
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            EndLocomotion();
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentCollidingObject == null)
        {
            currentCollidingObject = other.gameObject;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (currentCollidingObject == null)
        {
            currentCollidingObject = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (currentCollidingObject == other.gameObject)
        {
            currentCollidingObject = null;
        }
    }

     void GrabOrReleaseObject()
    {
        if (currentGrabbedObject == null && currentCollidingObject != null)
        {
            currentGrabbedObject = currentCollidingObject;
            if (currentGrabbedObject.tag == "Grip")
            {
                Debug.Log("Grabbing grip");
                GrabGrip(currentGrabbedObject);
            }
            else if (currentGrabbedObject.tag == "MainHand")
            {
                GrabWeapon(currentGrabbedObject);
            }
        }
        else if (currentGrabbedObject != null)
        {
            if (currentGrabbedObject.tag == "Grip")
            {
                ReleaseGrip(currentGrabbedObject);
            }
            else if (currentGrabbedObject.tag == "MainHand")
            {
                ReleaseWeapon(currentGrabbedObject);
            }
        }


    }
    void GrabGrip(GameObject grip)
    {
        EquipController eQ = grip.GetComponentInParent<EquipController>();
        eQ.gripController = this.gameObject;
        eQ.isGripped = true;
    }

   public void ReleaseGrip(GameObject grip)
    {
        Debug.Log("Releasing Grip");
        EquipController eQ = grip.GetComponentInParent<EquipController>();
        eQ.gripController = null;
        eQ.isGripped = false;
        currentGrabbedObject = null;
    }
    void GrabWeapon(GameObject weapon)
    {
        EquipController eQ = weapon.GetComponentInParent<EquipController>();
        eQ.mainHandController = this.gameObject;
        eQ.isEquipped = true;
        eQ.isGripped = false;
        eQ.gripController = null;
    }
    void ReleaseWeapon(GameObject weapon)
    {
        currentGrabbedObject = null;
    }

    void StartLocomotion()
    {
        cameraRig.GetComponent<Locomotion>().StartLocomotion(this.gameObject);
    }

    void EndLocomotion()
    {
        cameraRig.GetComponent<Locomotion>().EndLocomotion(this.gameObject);
    }

    void GrabAttachment(GameObject at)
    {
        Attachment attachment = at.GetComponent<Attachment>();
        currentHoldingObject = at;
        attachment.PickUp(this.gameObject);
    }

    void ReleaseAttachment(GameObject at)
    {
        Attachment attachment = at.GetComponent<Attachment>();
        currentHoldingObject = null;
        attachment.LetGo();
    }

}
