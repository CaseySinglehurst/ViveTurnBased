using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : MonoBehaviour {
    
    [SerializeField]
    bool isOnRail = false;
    [SerializeField]
    GameObject attachedRail = null;
    [SerializeField]
    bool isAttached = false;
    [SerializeField]
    GameObject controller;

    public GameObject gripModel;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (!isOnRail && controller != null)
        {
            transform.position = Vector3.Lerp(transform.position, controller.transform.position, 20 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, controller.transform.rotation, 20 * Time.deltaTime);
        }
        if (isOnRail && !isAttached)
        {
            if (Vector3.Distance(transform.position, controller.transform.position) > .35f) {
                DetachFromRail();
                transform.position = controller.transform.position;
                transform.rotation = controller.transform.rotation;
            }
            else
            {
                // grip model is rails closest point
                transform.position = attachedRail.GetComponent<Rail>().GetClosestPointOnRail(controller.transform.position);
                transform.rotation = attachedRail.transform.rotation;
            }
        }
	}
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rail" && controller != null)
        {
            if (other.transform.GetComponent<Rail>().attachment == null)
            {
                isOnRail = true;
                attachedRail = other.gameObject;
                // grip model is rails closest point
                gripModel.transform.position = other.GetComponent<Rail>().GetClosestPointOnRail(controller.transform.position);
                gripModel.transform.rotation = other.transform.rotation;
            }
        }
    }
    

    public void AttatchToRail()
    {
        isAttached = true;
        transform.position = gripModel.transform.position;
        transform.rotation = gripModel.transform.rotation;
        gripModel.transform.position = transform.position;
        gripModel.transform.rotation = transform.rotation;
        transform.SetParent(attachedRail.transform);
    }

    public void DetachFromRail()
    {
        isAttached = false;
        isOnRail = false;
        transform.parent = null;
        attachedRail = null;
    }

    public void PickUp(GameObject c)
    {
        isAttached = false;
        attachedRail = null;
        controller = c;
    }

    public void LetGo()
    {
        if (isOnRail)
        {
            AttatchToRail();
        }
        else
        {
            isOnRail = false;
            isAttached = false;
            attachedRail = null;
        }
        controller = null;
    }

    

}
