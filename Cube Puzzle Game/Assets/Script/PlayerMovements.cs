using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public GameObject Player;
    public GameObject Center , CenterInside;
    public GameObject Up , Down , left , Right;
    public float speed;
    public int step = 9;
    public bool isInputRestricted = true;
    public Rigidbody NewCubePrefab;
    public PlayerTriggerControl UpSide, DownSide, LeftSide , RightSide;
    private bool UpMove, DownMove , LeftMove , RightMove ;
    public Vector3 StartPostion;
    bool UpStop , DownStop ,LeftStop, RightStop;
   

    [Header("----- Touch control ------")]
    private float SWIPE_THRESHOLD = 50f;
    private Vector2 endTouch;
    private Vector2 startTouch;
    public bool isTouch;
    Vector3 position;
    public float amountOfY;
    private float[] force = { 10, -10 };
    private int i = 0;
    public PlayerMovements Player1, Player2;



    [Header("-----Level Win Control-------")]
    public PlayerMovements[] Players;
    public int FirstNumber, SecondNumber , CubesCountsNumber;
    

    void Start()
    {
        Player = this.gameObject;

        Center = GameObject.Find("Center ");
        Up = GameObject.Find("Up");
        Down = GameObject.Find("Down");
        left = GameObject.Find("Left");
        Right = GameObject.Find("Right");

        NewCubePrefab = Resources.Load<Rigidbody>("New Cube");

        Players = GameObject.FindObjectsOfType<PlayerMovements>();

        if(PlayerPrefs.GetFloat("AmountOfYaxis") == 0)
        {
            PlayerPrefs.SetFloat("AmountOfYaxis", 13.5f);
        }

        amountOfY = PlayerPrefs.GetFloat("AmountOfYaxis");

        for(int Number = 0; Number < Players.Length; Number++)
        {
            if (gameObject.name == "Cube(" + Number + ")")
            {
                //if(Number == 0)
                //{
                //    FirstNumber = Number + 1;
                //    SecondNumber = Number + 1;
                //}
                //else if(Number == Players.Length - 1)
                //{
                //    FirstNumber = Number - 1;
                //    SecondNumber = Number - 1;
                    
                //}else
                //{
                    FirstNumber = Number - 1;
                    SecondNumber = Number + 1;
                //}

                CubesCountsNumber = Number;
            }
        }

        StartPostion = gameObject.transform.position;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            //if(collision.gameObject.name == "New Cube(Clone)")
            //{
            //    if(gameObject.name == "New Cube(Clone)")
            //    {
            //        if (i == 0)
            //            i = 1;
            //        else
            //            i = 0;
            //    }
            //}


            if (isInputRestricted == false)
            {
                isInputRestricted = true;

                PlayerPrefs.SetInt("CollosonCounter", PlayerPrefs.GetInt("CollosonCounter") + 1);
                Vector3 colliderSize = new Vector3(100, collision.gameObject.GetComponent<BoxCollider>().size.y + gameObject.GetComponent<BoxCollider>().size.y, 100);


                Destroy(gameObject.GetComponent<Rigidbody>());
                Destroy(collision.gameObject.GetComponent<Rigidbody>());
                Destroy(gameObject.GetComponent<BoxCollider>());
                Destroy(collision.gameObject.GetComponent<BoxCollider>());

                position = new Vector3((collision.gameObject.transform.position.x + gameObject.transform.position.x) / 2,StartPostion.y + collision.gameObject.GetComponent<PlayerMovements>().StartPostion.y , (collision.gameObject.transform.position.z + gameObject.transform.position.z) / 2);

                //position = new Vector3((collision.gameObject.transform.position.x + gameObject.transform.position.x) / 2, (collision.gameObject.transform.position.y + gameObject.transform.position.y) / 2, (collision.gameObject.transform.position.z + gameObject.transform.position.z) / 2);


                //if (gameObject.name == "New Cube(Clone)")
                //{
                //    position = new Vector3((collision.gameObject.transform.position.x + gameObject.transform.position.x) / 2, (collision.gameObject.transform.position.y + gameObject.transform.position.y) / 2 + amountOfY, (collision.gameObject.transform.position.z + gameObject.transform.position.z) / 2);
                //    PlayerPrefs.SetFloat("AmountOfYaxis", amountOfY + 5.5f);
                //}
                //else
                //{
                //    position = new Vector3((collision.gameObject.transform.position.x + gameObject.transform.position.x) / 2, (collision.gameObject.transform.position.y + gameObject.transform.position.y) / 2, (collision.gameObject.transform.position.z + gameObject.transform.position.z) / 2);

                //}

                //if (collision.gameObject.name == "New Cube(Clone)")
                //{
                //    position = new Vector3((collision.gameObject.transform.position.x + gameObject.transform.position.x) / 2, (collision.gameObject.transform.position.y + gameObject.transform.position.y) / 2 - amountOfY, (collision.gameObject.transform.position.z + gameObject.transform.position.z) / 2);

                //}

                //if (gameObject.name == "New Cube(Clone)" && collision.gameObject.name == collision.gameObject.name)
                //{
                //    position = new Vector3((collision.gameObject.transform.position.x + gameObject.transform.position.x) / 2, (collision.gameObject.transform.position.y + gameObject.transform.position.y) / 2, (collision.gameObject.transform.position.z + gameObject.transform.position.z) / 2);

                //}

                Rigidbody clone = Instantiate(NewCubePrefab, position, Quaternion.identity);

                clone.GetComponent<BoxCollider>().size = colliderSize;

                collision.gameObject.transform.parent = clone.transform;
                gameObject.transform.parent = clone.transform;


                

                if (collision.gameObject.name == "Cube(" + FirstNumber + ")" || collision.gameObject.name == "Cube(" + SecondNumber + ")")
                {
                    PlayerPrefs.SetInt("Cube(" + collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber + ")", 1);
                    clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;

                    //for (int i = 0; i< PlayerPrefs.GetInt("NumberOfCubesInLevel"); i++)
                    //{
                    //    if(collision.gameObject.name == "Cube(" + i + ")")
                    //    {
                    //        if(i > CubesCountsNumber)
                    //        {
                    clone.GetComponent<PlayerMovements>().FirstNumber = FirstNumber;
                    clone.GetComponent<PlayerMovements>().SecondNumber = SecondNumber;

                    //        }
                    //        else
                    //        {
                    //            clone.GetComponent<PlayerMovements>().FirstNumber = collision.gameObject.GetComponent<PlayerMovements>().FirstNumber;
                    //            clone.GetComponent<PlayerMovements>().SecondNumber = SecondNumber;

                    //        }
                    //    }
                    //}

                    clone.GetComponent<PlayerMovements>().Player1 = this;
                    clone.GetComponent<PlayerMovements>().Player2 = collision.gameObject.GetComponent<PlayerMovements>();
                }
                else
                {
                    if (collision.gameObject.name != "New Cube(Clone)")
                    {
                        GameManager.Instans.GameOver = true;
                    }

                }

                if (collision.gameObject.name == "New Cube(Clone)")
                {
                    if(FirstNumber == collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber)
                    {
                        if(PlayerPrefs.GetInt("Cube(" + collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber + ")") == 0)
                        {
                            PlayerPrefs.SetInt("Cube(" + collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber + ")", 1);
                            clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;
                        }
                        else
                        {
                            PlayerPrefs.SetInt("Cube(" + CubesCountsNumber + ")", 1);
                            clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;
                        }

                   
                    }
                    else if (SecondNumber == collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber)
                    {
                        if (PlayerPrefs.GetInt("Cube(" + collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber + ")") == 0)
                        {
                            PlayerPrefs.SetInt("Cube(" + collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber + ")", 1);
                            clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;
                        }
                        else
                        {
                            PlayerPrefs.SetInt("Cube(" + CubesCountsNumber + ")", 1);
                            clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;
                        }
                    }
                    else
                    {
                        GameManager.Instans.GameOver = true;
                    }

                    clone.GetComponent<PlayerMovements>().FirstNumber = FirstNumber;
                    clone.GetComponent<PlayerMovements>().SecondNumber = SecondNumber;

                    if (collision.gameObject.name == "New Cube(Clone)")
                    {
                        if (i == 0)
                        {
                            clone.GetComponent<PlayerMovements>().Player2 = collision.gameObject.GetComponent<PlayerMovements>().Player2;
                        }
                        else
                        {
                            clone.GetComponent<PlayerMovements>().Player2 = collision.gameObject.GetComponent<PlayerMovements>().Player1;
                        }
                        clone.GetComponent<PlayerMovements>().Player1 = this;

                    }
                    else
                    {
                        if (i == 0)
                        {
                            clone.GetComponent<PlayerMovements>().Player2 = Player2;
                        }
                        else
                        {
                            clone.GetComponent<PlayerMovements>().Player2 = Player1;
                        }
                        clone.GetComponent<PlayerMovements>().Player1 = collision.gameObject.GetComponent<PlayerMovements>();
                    }


                    //if (i == 0)
                    //{
                    //    clone.GetComponent<PlayerMovements>().Player1 = Player1;
                    //}
                    //else
                    //{
                    //    clone.GetComponent<PlayerMovements>().Player1 = Player2;
                    //}
                    //clone.GetComponent<PlayerMovements>().Player2 = collision.gameObject.GetComponent<PlayerMovements>();

                    //for (int i = 0; i < PlayerPrefs.GetInt("NumberOfCubesInLevel"); i++)
                    //{
                    //    if (i == collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber)
                    //    {
                    //        if (i > CubesCountsNumber)
                    //        {
                    //            clone.GetComponent<PlayerMovements>().FirstNumber = FirstNumber;
                    //            clone.GetComponent<PlayerMovements>().SecondNumber = collision.gameObject.GetComponent<PlayerMovements>().SecondNumber;

                    //        }
                    //        else
                    //        {
                    //            clone.GetComponent<PlayerMovements>().FirstNumber = collision.gameObject.GetComponent<PlayerMovements>().FirstNumber;
                    //            clone.GetComponent<PlayerMovements>().SecondNumber = SecondNumber;

                    //        }
                    //    }
                    //}
                }

                //if (collision.gameObject.name == "Cube(" + SecondNumber + ")")
                //{
                //    PlayerPrefs.SetInt("Cube(" + CubesCountsNumber + ")", 1);
                //    clone.GetComponent<PlayerMovements>().FirstNumber = collision.gameObject.GetComponent<PlayerMovements>().FirstNumber;
                //    clone.GetComponent<PlayerMovements>().SecondNumber = SecondNumber;

                //}

             
            }

            if (i == 0)
                i = 1;
            else
                i = 0;
        }

        if(collision.gameObject.CompareTag("Platfrom"))
        {
            isInputRestricted = true;

            if (i == 0)
                i = 1;
            else
                i = 0;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("UpSider"))
        {
            UpStop = true;
        }
        if (other.gameObject.CompareTag("DownSider"))
        {
            DownStop = true;
        }
        if (other.gameObject.CompareTag("LeftSider"))
        {
            LeftStop = true;
        }
        if (other.gameObject.CompareTag("RightSider"))
        {
            RightStop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("UpSider"))
        {
            UpStop = false;
        }
        if (other.gameObject.CompareTag("DownSider"))
        {
            DownStop = false;
        }
        if (other.gameObject.CompareTag("LeftSider"))
        {
            LeftStop = false;
        }
        if (other.gameObject.CompareTag("RightSider"))
        {
            RightStop = false;
        }
    }

    void Update()
    {
        if (gameObject.name == "New Cube(Clone)")
        {
            if (GameManager.Instans.GameOver == true)
                return;

            //if (isInputRestricted == true)
            //{
                if (i == 1)
                {
                    FirstNumber = Player2.FirstNumber;
                    SecondNumber = Player2.SecondNumber;
                    CubesCountsNumber = Player2.CubesCountsNumber;
                }
                else
                {
                    FirstNumber = Player1.FirstNumber;
                    SecondNumber = Player1.SecondNumber;
                    CubesCountsNumber = Player1.CubesCountsNumber;
                }
            //}
            //else
            //{
            //    if (i == 1)
            //    {
            //        FirstNumber = Player2.FirstNumber;
            //        SecondNumber = Player2.SecondNumber;
            //        CubesCountsNumber = Player2.CubesCountsNumber;
            //    }
            //    else
            //    {
            //        FirstNumber = Player1.FirstNumber;
            //        SecondNumber = Player1.SecondNumber;
            //        CubesCountsNumber = Player1.CubesCountsNumber;
            //    }
            //}


        }
        if (isInputRestricted == true)
        {
            if (isTouch == true)
            {
                if(gameObject.name == "New Cube(Clone)")
                {
                    if(gameObject.transform.position.y < 30 && gameObject.transform.position.y > 25)
                    {
                        Center.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 27.1f, Player.transform.position.z);
                    }
                }
                else
                {
                    Center.transform.position = Player.transform.position;
                }

                

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (UpSide.PlayerMove == false)
                    {
                        UpMove = true;
                    }
                    else
                    {
                        if(UpStop  == false)
                        {
                            StartCoroutine("RotationUp");
                            isInputRestricted = false;
                        }
                    }
                   
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (DownSide.PlayerMove == false)
                    {
                        DownMove = true;
                    }
                    else
                    {
                        if(DownStop == false)
                        {
                            StartCoroutine("RotationDown");
                            isInputRestricted = false;
                        }
                    }
                 
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (RightSide.PlayerMove == false)
                    {
                        RightMove = true;
                    }
                    else
                    {
                        if(RightStop == false)
                        {
                            StartCoroutine("RotationRight");
                            isInputRestricted = false;
                        }
                    }

                    
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if(LeftSide.PlayerMove == false)
                    {
                        LeftMove = true;
                    }
                    else
                    {
                        if(LeftStop == false)
                        {
                            StartCoroutine("RotationLeft");
                            isInputRestricted = false;
                        }
                    }
                    
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

        if (UpMove == true)
        {
            if (UpSide.PlayerMove == false)
            {
                transform.Translate(0, force[i], 0);
            }
            else
            {
                StartCoroutine("RotationUp");
                isInputRestricted = false;
                UpMove = false;
            }
        }

        if (DownMove == true)
        {
            if (DownSide.PlayerMove == false)
            {
                transform.Translate(0, force[i], 0);
            }
            else
            {
                StartCoroutine("RotationDown");
                isInputRestricted = false;
                DownMove = false;
            }
        }

        if (LeftMove == true)
        {
            if(LeftSide.PlayerMove == false)
            {
                transform.Translate(0, force[i], 0);
            }
            else
            {
                StartCoroutine("RotationLeft");
                isInputRestricted = false;
                LeftMove = false;
            }
        }

        if (RightMove == true)
        {
            if (RightSide.PlayerMove == false)
            {
                transform.Translate(0, force[i], 0);
            }
            else
            {
                StartCoroutine("RotationRight");
                isInputRestricted = false;
                RightMove = false;
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
                if (UpSide.PlayerMove == false)
                {
                    UpMove = true;
                }
                else
                {
                    StartCoroutine("RotationUp");
                    isInputRestricted = false;
                }
            }
            else if ((double)this.endTouch.y - (double)this.startTouch.y < 0.0)
            {
                if (DownSide.PlayerMove == false)
                {
                    DownMove = true;
                }
                else
                {
                    StartCoroutine("RotationDown");
                    isInputRestricted = false;
                }
            }
            this.startTouch = this.endTouch;
        }
        else
        {
            if ((double)this.horizontalValMove() <= (double)this.SWIPE_THRESHOLD || (double)this.horizontalValMove() <= (double)this.verticalMove())
                return;
            if ((double)this.endTouch.x - (double)this.startTouch.x > 0.0)
            {
                if (RightSide.PlayerMove == false)
                {
                    RightMove = true;
                }
                else
                {
                    StartCoroutine("RotationRight");
                    isInputRestricted = false;
                }
            }
            else if ((double)this.endTouch.x - (double)this.startTouch.x < 0.0)
            {
                if (LeftSide.PlayerMove == false)
                {
                    LeftMove = true;
                }
                else
                {
                    StartCoroutine("RotationLeft");
                    isInputRestricted = false;
                }
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
        CenterInside.transform.rotation = new Quaternion(0, 0, 0, -180);
        //isInputRestricted = true;

        //if (i == 0)
        //    i = 1;
        //else
        //    i = 0;


      
    }

    IEnumerator RotationDown()
    {
        for (int i = 0; i < (180 / step); i++)
        {
            Player.transform.RotateAround(Down.transform.position, Vector3.left, step);
            yield return new WaitForSeconds(speed);
        }
        Center.transform.position = Player.transform.position;
        CenterInside.transform.rotation = new Quaternion(0, 0, 0 , 180);
        //isInputRestricted = true;
        //if (i == 0)
        //    i = 1;
        //else
        //    i = 0;
    }

    IEnumerator RotationLeft()
    {
        for (int i = 0; i < (180 / step); i++)
        {
            Player.transform.RotateAround(left.transform.position, Vector3.forward, step);
            yield return new WaitForSeconds(speed);
        }
        Center.transform.position = Player.transform.position;
        CenterInside.transform.rotation = new Quaternion(0, 0, 0, 90);
        //isInputRestricted = true;
        //if (i == 0)
        //    i = 1;
        //else
        //    i = 0;
    }

    IEnumerator RotationRight()
    {
        for (int i = 0; i < (180 / step); i++)
        {
            Player.transform.RotateAround(Right.transform.position, Vector3.back, step);
            yield return new WaitForSeconds(speed);
        }
        Center.transform.position = Player.transform.position;
        CenterInside.transform.rotation = new Quaternion(0, 0, 0, -90);
        //isInputRestricted = true;
        //if (i == 0)
        //    i = 1;
        //else
        //    i = 0;
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
