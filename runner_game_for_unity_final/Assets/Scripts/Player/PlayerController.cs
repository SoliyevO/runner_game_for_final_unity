

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
    public float speedIncreaseRate = 0.1f; // Har sekundda oshish miqdori
    private bool isSpeedBoosted = false; // Tezlik oshirilganligini tekshirish
    private float speedBeforeBoost; // Boostdan oldingi forwardSpeed qiymati

    private int desiredLane = 1;//0:chap, 1:o'rta, 2:o'ng
    public float laneDistance = 2.5f;

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

    public bool isGameStarted = false; // O'yin boshlanish flagi



    void Start()
    {
        controller = GetComponent<CharacterController>();
        Time.timeScale = 1.2f;

        //playerHealth = GetComponent<PlayerHealth>();

        //// Xatolikni oldini olish uchun tekshiramiz
        //if (playerHealth == null)
        //{
        //    Debug.LogError("PlayerHealth komponenti topilmadi!");
        //}

    }

    //private void FixedUpdate()
    //{
    //    if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
    //        return;

    //    // Increase Speed
    //    if (toggle)
    //    {
    //        toggle = false;
    //        if (forwardSpeed < maxSpeed)
    //            forwardSpeed += 0.1f * Time.fixedDeltaTime;
    //    }
    //    else
    //    {
    //        toggle = true;
    //        if (Time.timeScale < 2f)
    //            Time.timeScale += 0.005f * Time.fixedDeltaTime;
    //    }
    //}

    private void FixedUpdate()
    {
        // Agar o'yin boshlanmagan yoki tugagan bo'lsa, hech narsa qilmaslik
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;

        // Tezlikni oshirishni faqat forwardSpeed < maxSpeed bo'lganda qilish
        //if (forwardSpeed < maxSpeed)
        //{
        //    forwardSpeed += 0.1f * Time.fixedDeltaTime; // Tezlikni oshirish
        //}

        // Time.timeScale ni oshirish (agar kerak bo'lsa)
        if (Time.timeScale < 2f)
        {
            Time.timeScale += 0.005f * Time.fixedDeltaTime; // TimeScale boshqaruvi
        }
    }




    void Update()
    {

        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;

        if (!isSpeedBoosted && forwardSpeed < maxSpeed)
        {
            // ForwardSpeed har sekundda o‘sib boradi
            forwardSpeed += speedIncreaseRate * Time.deltaTime;
            forwardSpeed = Mathf.Clamp(forwardSpeed, 0, maxSpeed); // MaxSpeeddan oshmaydi
        }

        // Har doim o'yinchini oldinga qarab turishiga ishonch hosil qiling
        transform.rotation = Quaternion.Euler(0, 0, 0);

        animator.SetBool("isGameStarted", true);
        direction.z = forwardSpeed;

        if (isGameStarted)
        {
            forwardSpeed += speedIncreaseRate * Time.deltaTime;
            forwardSpeed = Mathf.Clamp(forwardSpeed, 0, maxSpeed); // MaxSpeed'ni cheklash
        }

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


    public void IncreaseSpeedTemporarily(float amount, float duration)
    {
        if (!isSpeedBoosted)
        {
            speedBeforeBoost = forwardSpeed; // Boostdan oldingi tezlikni saqlaymiz
        }

        forwardSpeed += amount; // Tezlikni oshirish
        forwardSpeed = Mathf.Clamp(forwardSpeed, 0, maxSpeed); // Maksimal tezlikni oshirmaslik
        isSpeedBoosted = true; // Tezlik oshirilganligini belgilash

        StartCoroutine(ResetSpeedAfterDuration(duration));
    }

    private IEnumerator ResetSpeedAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration); // Boost vaqti tugashini kutamiz
        forwardSpeed = speedBeforeBoost; // Avvalgi forwardSpeed qiymatini tiklaymiz
        isSpeedBoosted = false; // Tezlik yana o‘sib borishi uchun imkon beramiz
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


    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.transform.CompareTag("Obstacle")) // Obstaclega tegganda
    //    {
    //        if (playerHealth != null)
    //        {
    //            bool isGameOver = playerHealth.TakeDamage(); // Jonni kamaytirish

    //            if (isGameOver) // Jon tugadi
    //            {
    //                PlayerManager.gameOver = true;
    //                Debug.Log("Game Over!");
    //                FindObjectOfType<AudioManager>().PlaySound("GameOver");
    //            }
    //            else
    //            {
    //                Debug.Log("Jon kamaydi! Qolgan jon: " + playerHealth.GetCurrentHealth());
    //                FindObjectOfType<AudioManager>().PlaySound("Hit");
    //            }
    //        }
    //    }
    //}





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


    public void IncreaseSpeed(float amount)
    {
        forwardSpeed += amount; // Tezlikni oshirish
        forwardSpeed = Mathf.Clamp(forwardSpeed, 0, maxSpeed); // Maksimal tezlikni oshirmaslik
    }

    public bool IsMaxSpeedReached()
    {
        return forwardSpeed >= maxSpeed;
    }

    //public void IncreaseSpeed(float amount)
    //{
    //    forwardSpeed = amount;
    //    Debug.Log("Tezlik 15 ga teng!");
    //    StartCoroutine(ResetSpeedAfterDelay(3.0f));
    //}

    //private IEnumerator ResetSpeedAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    forwardSpeed = forwardSpeed;
    //    Debug.Log("Tezlik o'z holatiga qaytdi.");
    //}

    public void StartGame()
    {
        isGameStarted = true; // O'yinni boshlash
    }
}
