using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerControl : MonoBehaviour
{
    public bool PlayerMove = true;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Cube"))
        {
            PlayerMove = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            PlayerMove = true;
        }
    }

}
