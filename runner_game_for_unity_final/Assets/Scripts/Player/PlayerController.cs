

using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1;//0:left, 1:middle, 2:right
    public float laneDistance = 2.5f;//The distance between two lanes

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float Gravity = -20f;
    public float jumpHeight = 2;
    private Vector3 velocity;

    public Animator animator;
    private bool isSliding = false;

    public float slideDuration = 1.5f;

    public float jumpForce;

    bool toggle = false;
    //private object direction;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Time.timeScale = 1.2f;
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;

        // Increase Speed
        if (toggle)
        {
            toggle = false;
            if (forwardSpeed < maxSpeed)
                forwardSpeed += 0.1f * Time.fixedDeltaTime;
        }
        else
        {
            toggle = true;
            if (Time.timeScale < 2f)
                Time.timeScale += 0.005f * Time.fixedDeltaTime;
        }
    }

    void Update()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;

        // Har doim o'yinchini oldinga qarab turishiga ishonch hosil qiling
        transform.rotation = Quaternion.Euler(0, 0, 0);

        animator.SetBool("isGameStarted", true);
        direction.z = forwardSpeed;

        //// Ground Check
        //isGrounded = Physics.CheckSphere(groundCheck.position, 0.17f, groundLayer);
        //animator.SetBool("isGrounded", isGrounded);
        //if (isGrounded && velocity.y < 0)
        //    velocity.y = -2f; // -1f ni -2f ga o'zgartirdik, bu stabil harakat beradi.

        // Jump
        direction.z = forwardSpeed;

        direction.y += Gravity * Time.deltaTime;

        if(controller.isGrounded)
        {

            if (SwipeManager.swipeUp || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
        }

        if(SwipeManager.swipeDown || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(Slide());
        }

        //if (isGrounded)
        //{
        //    if (SwipeManager.swipeUp || Input.GetKeyDown(KeyCode.UpArrow))
        //    {
        //        Debug.Log("Jump Detected");
        //        Jump();
        //    }

        //    if ((SwipeManager.swipeDown || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding)
        //        StartCoroutine(Slide());
        //}
        //else
        //{
        //    velocity.y += Gravity * Time.deltaTime; // Gravitatsiyani qo‘shishda muammo yo‘q.
        //    if ((SwipeManager.swipeDown || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding)
        //    {
        //        StartCoroutine(Slide());
        //        velocity.y = -10; // Bu sakrashdan pastga tushish jarayonida yordam beradi.
        //    }
        //}

        controller.Move(velocity * Time.deltaTime);

        // Lane Change
        if (SwipeManager.swipeRight || Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (SwipeManager.swipeLeft || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        // Calculate Target Position
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
            targetPosition += Vector3.left * laneDistance;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * laneDistance;

        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 30 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.magnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }

        controller.Move(direction * Time.deltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;



        //if (!isGrounded) return; // Faqat yerga tegib turgan holatda sakrashga ruxsat bering.

        //StopCoroutine(Slide());
        //animator.SetBool("isSliding", false);
        //animator.SetTrigger("jump");
        //controller.center = Vector3.zero; // Normal holatni o'rnating.
        //controller.height = 2; // Harakatni davom ettirish uchun original balandlikka qayting.
        //isSliding = false;

        //velocity.y = Mathf.Sqrt(jumpHeight * -2f * Gravity); // Sakrashning vertikal tezligini hisoblash.
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }

    private IEnumerator Slide()
    {
        //isSliding = true;
        animator.SetBool("isSliding", true);
        yield return new WaitForSeconds(1f / Time.timeScale);
        //controller.center = new Vector3(0, -0.5f, 0);
        //controller.height = 1;

        //yield return new WaitForSeconds((slideDuration - 0.25f) / Time.timeScale);

        animator.SetBool("isSliding", false);

        //controller.center = Vector3.zero;
        //controller.height = 2;

        //isSliding = false;
    }
}
