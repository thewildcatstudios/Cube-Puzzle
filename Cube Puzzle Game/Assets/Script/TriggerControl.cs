using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    public GameObject ParentObj;
    public PlayerMovements Player;
    public GameObject Up , Down , Right , Left;
    public bool TriggerWithCube;
    public bool UPMOVE, DOWNMOVE, RIGHTMOVE, LEFTMOVE;

  
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            Player.Up = other.gameObject.GetComponent<PlayerMovements>().Up;
            Player.Down = other.gameObject.GetComponent<PlayerMovements>().Down;
            Player.Right = other.gameObject.GetComponent<PlayerMovements>().Right;
            Player.left = other.gameObject.GetComponent<PlayerMovements>().left;

            TriggerWithCube = true;

            //UPMOVE = other.gameObject.GetComponent<PlayerMovements>().UpMove;
            //DOWNMOVE = other.gameObject.GetComponent<PlayerMovements>().DownMove;
            //LEFTMOVE = other.gameObject.GetComponent<PlayerMovements>().LeftMove;
            //RIGHTMOVE = other.gameObject.GetComponent<PlayerMovements>().RightMove;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player.Up = Up;
        Player.Down = Down;
        Player.Right = Right;
        Player.left = Left;

        TriggerWithCube = false;
    }
}
