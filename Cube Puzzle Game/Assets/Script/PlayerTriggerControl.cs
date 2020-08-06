using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerControl : MonoBehaviour
{
    public bool PlayerMove = true;
    public GameObject isTrigger;
    public GameObject prefab;
    public bool MovementUpSide;

    private void Start()
    {
        if(MovementUpSide)
        {
            transform.position = new Vector3(transform.position.x, prefab.transform.position.y * 2.1f, transform.position.z);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Cube"))
        {
            PlayerMove = false;
            isTrigger = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            PlayerMove = true;
            isTrigger = null;
        }
    }

}
