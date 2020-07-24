using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public GameObject Player;
    public GameObject Center;
    public GameObject Up , Down , left , Right;
    public float speed;
    public int step = 9;
    public bool isInputRestricted = true;
    public Rigidbody NewCubePrefab;

    [Header("----- Touch control ------")]
    private float SWIPE_THRESHOLD = 50f;
    private Vector2 endTouch;
    private Vector2 startTouch;
    public bool isTouch;
    Vector3 position;
    public float amountOfY;
    

    void Start()
    {
        Player = this.gameObject;

        Center = GameObject.Find("Center ");
        Up = GameObject.Find("Up");
        Down = GameObject.Find("Down");
        left = GameObject.Find("Left");
        Right = GameObject.Find("Right");

        NewCubePrefab = Resources.Load<Rigidbody>("New Cube");

        if(PlayerPrefs.GetFloat("AmountOfYaxis") == 0)
        {
            PlayerPrefs.SetFloat("AmountOfYaxis", 13.5f);
        }

        amountOfY = PlayerPrefs.GetFloat("AmountOfYaxis");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            if(isTouch == true)
            {
                Vector3 colliderSize = new Vector3(100, collision.gameObject.GetComponent<BoxCollider>().size.y + gameObject.GetComponent<BoxCollider>().size.y, 100);

                Destroy(gameObject.GetComponent<Rigidbody>());
                Destroy(collision.gameObject.GetComponent<Rigidbody>());
                Destroy(gameObject.GetComponent<BoxCollider>());
                Destroy(collision.gameObject.GetComponent<BoxCollider>());

                if(gameObject.name == "New Cube(Clone)")
                {
                    position = new Vector3((collision.gameObject.transform.position.x + gameObject.transform.position.x) / 2, (collision.gameObject.transform.position.y + gameObject.transform.position.y) / 2 + amountOfY, (collision.gameObject.transform.position.z + gameObject.transform.position.z) / 2);
                    PlayerPrefs.SetFloat("AmountOfYaxis", amountOfY + 10.5f);
                }
                else
                {
                    position = new Vector3((collision.gameObject.transform.position.x + gameObject.transform.position.x) / 2, (collision.gameObject.transform.position.y + gameObject.transform.position.y) / 2, (collision.gameObject.transform.position.z + gameObject.transform.position.z) / 2);

                }


                Rigidbody clone = Instantiate(NewCubePrefab, position, Quaternion.identity);

                clone.GetComponent<BoxCollider>().size = colliderSize;

                collision.gameObject.transform.parent = clone.transform;
                gameObject.transform.parent = clone.transform;

                
            }
        }
    }

    void Update()
    {
       
        if (isInputRestricted == true)
        {
            if (isTouch == true)
            {
                Center.transform.position = Player.transform.position;

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    StartCoroutine("RotationUp");
                    isInputRestricted = false;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    StartCoroutine("RotationDown");
                    isInputRestricted = false;
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    StartCoroutine("RotationRight");
                    isInputRestricted = false;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    StartCoroutine("RotationLeft");
                    isInputRestricted = false;
                }
            }

        }

        if(isTouch == true)
        {
            if (isInputRestricted == true)
            {
                HandleInput();
            }
        }
    }

    private void OnMouseDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isTouch = true;

        }
    }
    private void OnMouseUp()
    {
        Invoke("TouchRemove", 0.3f);
    }

    public void TouchRemove()
    {
        isTouch = false;
    }


    public void HandleInput()
    {
        if (isInputRestricted == false)
            return;

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                this.startTouch = touch.position;
                this.endTouch = touch.position;

            }

            if (touch.phase == TouchPhase.Ended)
            {
                this.endTouch = touch.position;
                this.checkSwipe();
            }
        }
    }

    private void checkSwipe()
    {
        if ((double)this.verticalMove() > (double)this.SWIPE_THRESHOLD && (double)this.verticalMove() > (double)this.horizontalValMove())
        {
            if ((double)this.endTouch.y - (double)this.startTouch.y > 0.0)
            {
                this.StartCoroutine("RotationUp");
                isInputRestricted = false;
            }
            else if ((double)this.endTouch.y - (double)this.startTouch.y < 0.0)
            {
                this.StartCoroutine("RotationDown");
                isInputRestricted = false;
            }
            this.startTouch = this.endTouch;
        }
        else
        {
            if ((double)this.horizontalValMove() <= (double)this.SWIPE_THRESHOLD || (double)this.horizontalValMove() <= (double)this.verticalMove())
                return;
            if ((double)this.endTouch.x - (double)this.startTouch.x > 0.0)
            {
                this.StartCoroutine("RotationRight");
                isInputRestricted = false;
            }
            else if ((double)this.endTouch.x - (double)this.startTouch.x < 0.0)
            {
                this.StartCoroutine("RotationLeft");
                isInputRestricted = false;
            }
            this.startTouch = this.endTouch;
        }
    }

    private float verticalMove()
    {
        return Mathf.Abs(this.endTouch.y - this.startTouch.y);
    }

    private float horizontalValMove()
    {
        return Mathf.Abs(this.endTouch.x - this.startTouch.x);
    }


    IEnumerator RotationUp()
    {
        for ( int  i = 0; i < (180 / step); i++)
        {
            Player.transform.RotateAround(Up.transform.position , Vector3.right , step);
            yield return new WaitForSeconds(speed);
        }
        Center.transform.position = Player.transform.position;
        isInputRestricted = true;
      
    }

    IEnumerator RotationDown()
    {
        for (int i = 0; i < (180 / step); i++)
        {
            Player.transform.RotateAround(Down.transform.position, Vector3.left, step);
            yield return new WaitForSeconds(speed);
        }
        Center.transform.position = Player.transform.position;
        isInputRestricted = true;
        
    }

    IEnumerator RotationLeft()
    {
        for (int i = 0; i < (180 / step); i++)
        {
            Player.transform.RotateAround(left.transform.position, Vector3.forward, step);
            yield return new WaitForSeconds(speed);
        }
        Center.transform.position = Player.transform.position;
        isInputRestricted = true;
        
    }

    IEnumerator RotationRight()
    {
        for (int i = 0; i < (180 / step); i++)
        {
            Player.transform.RotateAround(Right.transform.position, Vector3.back, step);
            yield return new WaitForSeconds(speed);
        }
        Center.transform.position = Player.transform.position;
        isInputRestricted = true;
       
    }


    IEnumerator dependentRotationUp()
    {
        for (int i = 0; i < (180 / step); i++)
        {
            Player.transform.RotateAround(Up.transform.position, Vector3.right, step);
            yield return new WaitForSeconds(speed);
        }
        Center.transform.position = Player.transform.position;

    }

    IEnumerator dependentRotationDown()
    {
        for (int i = 0; i < (180 / step); i++)
        {
            Player.transform.RotateAround(Down.transform.position, Vector3.left, step);
            yield return new WaitForSeconds(speed);
        }
        Center.transform.position = Player.transform.position;

    }

    IEnumerator dependentRotationLeft()
    {
        for (int i = 0; i < (180 / step); i++)
        {
            Player.transform.RotateAround(left.transform.position, Vector3.forward, step);
            yield return new WaitForSeconds(speed);
        }
        Center.transform.position = Player.transform.position;

    }

    IEnumerator dependentRotationRight()
    {
        for (int i = 0; i < (180 / step); i++)
        {
            Player.transform.RotateAround(Right.transform.position, Vector3.back, step);
            yield return new WaitForSeconds(speed);
        }
        Center.transform.position = Player.transform.position;
    }
}
