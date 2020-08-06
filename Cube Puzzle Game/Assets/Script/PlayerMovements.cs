using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public GameObject Player;
    public GameObject Center , CenterInside ;
    public GameObject Up , Down , left , Right;
    public GameObject CenterControl;
    public GameObject UpControl, DownControl, leftControl, RightControl;
    public float speed;
    public int step = 9;
    public bool isInputRestricted = true;
    public Rigidbody NewCubePrefab;
    public PlayerTriggerControl UpSide, DownSide, LeftSide , RightSide;
    private bool UpMove, DownMove , LeftMove , RightMove ;
    public Vector3 StartPostion;
    public bool UpStop , DownStop ,LeftStop, RightStop;
    public GameObject PrefabMoveingStop;
    public GameObject StopMoveing;

    [Header("----- Touch control ------")]
    private float SWIPE_THRESHOLD = 50f;
    private Vector2 endTouch;
    private Vector2 startTouch;
    public bool isTouch;
    Vector3 position;
    private float[] force = { 10, -10 };
    public int i = 0;
    public PlayerMovements Player1, Player2;
   


    [Header("-----Level Win Control-------")]
    public PlayerMovements[] Players;
    public int FirstNumber, SecondNumber , CubesCountsNumber;

    public bool MoveUpSideCube;
    bool gameover = false;

    void Start()
    {
        Player = this.gameObject;

        NewCubePrefab = Resources.Load<Rigidbody>("New Cube");
        PrefabMoveingStop = Resources.Load<GameObject>("Collider To stop Moveing");

        Players = GameObject.FindObjectsOfType<PlayerMovements>();


        for(int Number = 0; Number < Players.Length; Number++)
        {
            if (gameObject.name == "Cube(" + Number + ")")
            {
                    FirstNumber = Number - 1;
                    SecondNumber = Number + 1;

                CubesCountsNumber = Number;
            }
        }

        StartPostion = gameObject.transform.position;

        Center.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - StartPostion.y, Player.transform.position.z);

        CenterControl.transform.position = new Vector3(Player.transform.position.x, 2, Player.transform.position.z);

        UpControl.GetComponent<PlayerTriggerControl>().isTrigger = DownControl.GetComponent<PlayerTriggerControl>().isTrigger
            = leftControl.GetComponent<PlayerTriggerControl>().isTrigger = RightControl.GetComponent<PlayerTriggerControl>().isTrigger = null;
        UpControl.GetComponent<PlayerTriggerControl>().PlayerMove = DownControl.GetComponent<PlayerTriggerControl>().PlayerMove
            = leftControl.GetComponent<PlayerTriggerControl>().PlayerMove = RightControl.GetComponent<PlayerTriggerControl>().PlayerMove = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {

            if (isInputRestricted == false)
            {
                isInputRestricted = true;

                PlayerPrefs.SetInt("CollosonCounter", PlayerPrefs.GetInt("CollosonCounter") + 1);
                Vector3 colliderSize = new Vector3(100,collision.gameObject.GetComponent<BoxCollider>().size.y + gameObject.GetComponent<BoxCollider>().size.y, 100);


                Destroy(gameObject.GetComponent<Rigidbody>());
                Destroy(collision.gameObject.GetComponent<Rigidbody>());
                Destroy(gameObject.GetComponent<BoxCollider>());
                Destroy(collision.gameObject.GetComponent<BoxCollider>());

                position = new Vector3((collision.gameObject.transform.position.x + gameObject.transform.position.x) / 2,StartPostion.y + collision.gameObject.GetComponent<PlayerMovements>().StartPostion.y , (collision.gameObject.transform.position.z + gameObject.transform.position.z) / 2);


                Rigidbody clone = Instantiate(NewCubePrefab, position, Quaternion.identity);

                clone.GetComponent<BoxCollider>().size = colliderSize;

                collision.gameObject.transform.parent = clone.transform;
                gameObject.transform.parent = clone.transform;


                

                if (collision.gameObject.name == "Cube(" + FirstNumber + ")" || collision.gameObject.name == "Cube(" + SecondNumber + ")")
                {
                    PlayerPrefs.SetInt("Cube(" + collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber + ")", 1);
                    clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;

                   
                    clone.GetComponent<PlayerMovements>().FirstNumber = FirstNumber;
                    clone.GetComponent<PlayerMovements>().SecondNumber = SecondNumber;


                    clone.GetComponent<PlayerMovements>().Player1 = this;
                    clone.GetComponent<PlayerMovements>().Player2 = collision.gameObject.GetComponent<PlayerMovements>();

                    if(gameObject.name == "New Cube(Clone)")
                    {
                        if (collision.gameObject.name == "Cube(" + FirstNumber + ")" || collision.gameObject.name == "Cube(" + SecondNumber + ")")
                        {
                            PlayerPrefs.SetInt("Cube(" + collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber + ")", 1);
                            clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;

                            clone.GetComponent<PlayerMovements>().FirstNumber = FirstNumber;
                            clone.GetComponent<PlayerMovements>().SecondNumber = SecondNumber;

                            clone.GetComponent<PlayerMovements>().Player1 = gameObject.GetComponent<PlayerMovements>().Player2;
                           
                            clone.GetComponent<PlayerMovements>().Player2 = collision.gameObject.GetComponent<PlayerMovements>();
                        }
                    }
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
                        Debug.Log("1st");
                        if(PlayerPrefs.GetInt("Cube(" + collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber + ")") == 0)
                        {
                            PlayerPrefs.SetInt("Cube(" + collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber + ")", 1);
                            clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;
                        }
                        else if (PlayerPrefs.GetInt("Cube(" + CubesCountsNumber + ")" ) == 0)
                        {
                            PlayerPrefs.SetInt("Cube(" + CubesCountsNumber + ")", 1);
                            clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;
                        }
                        else
                        {
                            if(PlayerPrefs.GetInt("Cube(" + Player1.CubesCountsNumber + ")") == 0)
                            {
                                PlayerPrefs.SetInt("Cube(" + CubesCountsNumber + ")", 1);
                                clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;
                            }
                            else if(PlayerPrefs.GetInt("Cube(" + Player2.CubesCountsNumber + ")") == 0)
                            {
                                PlayerPrefs.SetInt("Cube(" + Player2.CubesCountsNumber + ")", 1);
                                clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;
                            }
                            else
                            {
                                bool Done = false;
                                for (int i = 0; i < 10; i++)
                                {
                                    if (PlayerPrefs.GetInt("Cube(" + i + ")") == 0)
                                    {
                                        if (Done == false)
                                        {
                                            PlayerPrefs.SetInt("Cube(" + i + ")", 1);
                                            Done = true;
                                        }
                                    }
                                }
                                Done = false;
                            }
                        }

                   
                    }
                    else if (SecondNumber == collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber)
                    {
                        Debug.Log("2nd");
                        if (PlayerPrefs.GetInt("Cube(" + collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber + ")") == 0)
                        {
                            PlayerPrefs.SetInt("Cube(" + collision.gameObject.GetComponent<PlayerMovements>().CubesCountsNumber + ")", 1);
                            clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;
                        }
                        else if (PlayerPrefs.GetInt("Cube(" + CubesCountsNumber + ")") == 0)
                        {
                            PlayerPrefs.SetInt("Cube(" + CubesCountsNumber + ")", 1);
                            clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;
                        }
                        else
                        {
                            if (PlayerPrefs.GetInt("Cube(" + Player1.CubesCountsNumber + ")") == 0)
                            {
                                PlayerPrefs.SetInt("Cube(" + Player1.CubesCountsNumber + ")", 1);
                                clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;
                            }
                            else if(PlayerPrefs.GetInt("Cube(" + Player2.CubesCountsNumber + ")") == 0)
                            {
                                PlayerPrefs.SetInt("Cube(" + Player2.CubesCountsNumber + ")", 1);
                                clone.GetComponent<PlayerMovements>().CubesCountsNumber = CubesCountsNumber;
                            }
                            else
                            {
                                bool Done = false;
                                for(int i = 0 ; i < 10 ; i++)
                                {
                                    if(PlayerPrefs.GetInt("Cube(" + i + ")") == 0)
                                    {
                                        if(Done == false)
                                        {
                                            PlayerPrefs.SetInt("Cube(" + i + ")", 1);
                                            Done = true;
                                        }
                                    }
                                }
                                Done = false;
                            }

                          
                        }
                    }
                    else
                    {
                        Debug.Log("Game over 2");
                        GameManager.Instans.GameOver = true;
                    }

                    clone.GetComponent<PlayerMovements>().FirstNumber = FirstNumber;
                    clone.GetComponent<PlayerMovements>().SecondNumber = SecondNumber;

                    if (collision.gameObject.name == "New Cube(Clone)")
                    {
                        if(gameObject.name == "New Cube(Clone)")
                        {
                            if (i == 0)
                            {
                                clone.GetComponent<PlayerMovements>().Player2 = collision.gameObject.GetComponent<PlayerMovements>().Player2;
                                clone.GetComponent<PlayerMovements>().Player1 = Player2;
                            }
                            else
                            {
                                clone.GetComponent<PlayerMovements>().Player2 = collision.gameObject.GetComponent<PlayerMovements>().Player1;
                                clone.GetComponent<PlayerMovements>().Player1 = Player1;
                            }
                        }
                        else
                        {
                            if (collision.gameObject.GetComponent<PlayerMovements>().i == 0)
                            {
                                clone.GetComponent<PlayerMovements>().Player2 = collision.gameObject.GetComponent<PlayerMovements>().Player2;
                            }
                            else
                            {
                                clone.GetComponent<PlayerMovements>().Player2 = collision.gameObject.GetComponent<PlayerMovements>().Player1;
                            }
                            clone.GetComponent<PlayerMovements>().Player1 = this;

                        }
                    }
                }
                if (i == 0)
                    i = 1;
                else
                    i = 0;

                Instantiate(PrefabMoveingStop, StartPostion, Quaternion.identity);

                clone.gameObject.GetComponent<PlayerMovements>().Center = Center;
                clone.gameObject.GetComponent<PlayerMovements>().Up = Up;
                clone.gameObject.GetComponent<PlayerMovements>().Down = Down;
                clone.gameObject.GetComponent<PlayerMovements>().Right = Right;
                clone.gameObject.GetComponent<PlayerMovements>().left = left;
                clone.gameObject.GetComponent<PlayerMovements>().CenterControl = CenterControl;
                clone.gameObject.GetComponent<PlayerMovements>().UpControl = UpControl;
                clone.gameObject.GetComponent<PlayerMovements>().DownControl = DownControl;
                clone.gameObject.GetComponent<PlayerMovements>().leftControl = leftControl;
                clone.gameObject.GetComponent<PlayerMovements>().RightControl = RightControl;

                clone.gameObject.GetComponent<PlayerMovements>().StopMoveing = StopMoveing;

                Destroy(collision.gameObject.GetComponent<PlayerMovements>().Center);
                Destroy(collision.gameObject.GetComponent<PlayerMovements>().CenterControl);
                Destroy(collision.gameObject.GetComponent<PlayerMovements>());
                Destroy(gameObject.GetComponent<PlayerMovements>());
                CenterInside.SetActive(false);
                collision.gameObject.GetComponent<PlayerMovements>().CenterInside.SetActive(false);
                Destroy(collision.gameObject.GetComponent<PlayerMovements>().Center);

                StopMoveing.SetActive(false);
            }

           
        }

        if (collision.gameObject.CompareTag("Platfrom"))
        {
            if(!isInputRestricted)
            {
                if (i == 0)
                    i = 1;
                else
                    i = 0;
            }
            isInputRestricted = true;

            StopMoveing.SetActive(false);
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

        if(UpStop == true)
        {
            if(DownStop == true)
            {
                if (LeftStop == true)
                {
                    if (RightStop == true)
                    {
                        if(!gameover)
                        {
                            GameManager.Instans.GameOver = true;
                            gameover = true;
                            Debug.Log("Gameover allside");
                        }
                    }
                }
            }
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


        }

     

        if (isInputRestricted == true)
        {
            if (isTouch == true)
            {
                Center.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - StartPostion.y, Player.transform.position.z);

                CenterControl.transform.position = new Vector3(Player.transform.position.x, 2, Player.transform.position.z);

                if(MoveUpSideCube == false)
                {
                    if (UpControl.GetComponent<PlayerTriggerControl>().PlayerMove == false)
                    {
                        if (UpControl.GetComponent<PlayerTriggerControl>().isTrigger.GetComponent<PlayerMovements>().StartPostion.y > StartPostion.y)
                        {
                            Up.transform.position = new Vector3(Up.transform.position.x, StartPostion.y * 2, Up.transform.position.z);

                        }
                        else
                        {
                            Up.transform.position = new Vector3(Up.transform.position.x, UpControl.GetComponent<PlayerTriggerControl>().isTrigger.transform.position.y + Player.transform.position.y, Up.transform.position.z);

                        }
                    }
                    else
                    {
                        Up.transform.position = new Vector3(Up.transform.position.x, Player.transform.position.y, Up.transform.position.z);
                    }

                    if (DownControl.GetComponent<PlayerTriggerControl>().PlayerMove == false)
                    {
                        if (DownControl.GetComponent<PlayerTriggerControl>().isTrigger.transform.position.y > StartPostion.y)
                        {
                            Down.transform.position = new Vector3(Down.transform.position.x, StartPostion.y * 2, Down.transform.position.z);

                        }
                        else
                        {
                            Down.transform.position = new Vector3(Down.transform.position.x, DownControl.GetComponent<PlayerTriggerControl>().isTrigger.transform.position.y + Player.transform.position.y, Down.transform.position.z);

                        }
                    }
                    else
                    {
                        Down.transform.position = new Vector3(Down.transform.position.x, Player.transform.position.y, Down.transform.position.z);
                    }

                    if (leftControl.GetComponent<PlayerTriggerControl>().PlayerMove == false)
                    {
                        if (leftControl.GetComponent<PlayerTriggerControl>().isTrigger.transform.position.y > StartPostion.y)
                        {
                            left.transform.position = new Vector3(left.transform.position.x, StartPostion.y * 2, left.transform.position.z);

                        }
                        else
                        {
                            left.transform.position = new Vector3(left.transform.position.x, leftControl.GetComponent<PlayerTriggerControl>().isTrigger.transform.position.y + Player.transform.position.y, left.transform.position.z);

                        }
                    }
                    else
                    {
                        left.transform.position = new Vector3(left.transform.position.x, Player.transform.position.y, left.transform.position.z);
                    }
                    if (RightControl.GetComponent<PlayerTriggerControl>().PlayerMove == false)
                    {
                        if (RightControl.GetComponent<PlayerTriggerControl>().isTrigger.transform.position.y > StartPostion.y)
                        {
                            Right.transform.position = new Vector3(Right.transform.position.x, StartPostion.y * 2, Right.transform.position.z);

                        }
                        else
                        {
                            Right.transform.position = new Vector3(Right.transform.position.x, RightControl.GetComponent<PlayerTriggerControl>().isTrigger.transform.position.y + Player.transform.position.y, Right.transform.position.z);

                        }
                    }
                    else
                    {
                        Right.transform.position = new Vector3(Right.transform.position.x, Player.transform.position.y, Right.transform.position.z);

                    }
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
                            StartCoroutine(RotationControal(Up, Vector3.right, -180));
                            isInputRestricted = false;
                            StopMoveing.SetActive(true);
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
                            StartCoroutine(RotationControal(Down, Vector3.left, 180));
                            isInputRestricted = false;
                            StopMoveing.SetActive(true);
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
                            StartCoroutine(RotationControal(Right, Vector3.back, -90));
                            isInputRestricted = false;
                            StopMoveing.SetActive(true);
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
                            StartCoroutine(RotationControal(left, Vector3.forward, 90));
                            isInputRestricted = false;
                            StopMoveing.SetActive(true);
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
                MoveUpSideCube = true;
                transform.Translate(0, force[i], 0);
            }
            else
            {
                StartCoroutine(RotationControal(Up, Vector3.right, -180));
                isInputRestricted = false;
                UpMove = false;
                MoveUpSideCube = false;
                StopMoveing.SetActive(true);
            }
        }

        if (DownMove == true)
        {
            if (DownSide.PlayerMove == false)
            {
                MoveUpSideCube = true;
                transform.Translate(0, force[i], 0);
            }
            else
            {
                StartCoroutine(RotationControal(Down, Vector3.left, 180));
                isInputRestricted = false;
                DownMove = false;
                MoveUpSideCube = false;
                StopMoveing.SetActive(true);
            }
        }

        if (LeftMove == true)
        {
            if(LeftSide.PlayerMove == false)
            {
                MoveUpSideCube = true;
                transform.Translate(0, force[i], 0);
            }
            else
            {
                StartCoroutine(RotationControal(left, Vector3.forward, 90));
                isInputRestricted = false;
                LeftMove = false;
                MoveUpSideCube = false;
                StopMoveing.SetActive(true);
            }
        }

        if (RightMove == true)
        {
            if (RightSide.PlayerMove == false)
            {
                MoveUpSideCube = true;
                transform.Translate(0, force[i], 0);
            }
            else
            {
                StartCoroutine(RotationControal(Right, Vector3.back, -90));
                isInputRestricted = false;
                RightMove = false;
                MoveUpSideCube = false;
                StopMoveing.SetActive(true);
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
                    if (UpStop == false)
                    {
                        StartCoroutine(RotationControal(Up, Vector3.right, -180));
                        isInputRestricted = false;
                        StopMoveing.SetActive(true);
                    }
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
                    if (DownStop == false)
                    {
                        StartCoroutine(RotationControal(Down, Vector3.left, 180));
                        isInputRestricted = false;
                        StopMoveing.SetActive(true);
                    }
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
                    if (RightStop == false)
                    {
                        StartCoroutine(RotationControal(Right, Vector3.back, -90));
                        isInputRestricted = false;
                        StopMoveing.SetActive(true);
                    }
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
                    if (LeftStop == false)
                    {
                        StartCoroutine(RotationControal(left, Vector3.forward, 90));
                        isInputRestricted = false;
                        StopMoveing.SetActive(true);
                    }
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

    IEnumerator RotationControal(GameObject move , Vector3 Rotation , float angle)
    {
        for (int i = 0; i < (180 / step); i++)
        {
            Player.transform.RotateAround(move.transform.position, Rotation, step);
            yield return new WaitForSeconds(speed);
        }
        Center.transform.position = Player.transform.position;
        CenterInside.transform.rotation = new Quaternion(0, 0, 0, angle);
    }

}
