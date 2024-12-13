using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    public Vector3 direction;
    public float forwardSpeed;

    public float maxSpeed;

    private int desiredLane = 1; // 0 chap 1 o'rta 2 o'ng
    public float laneDistance = 4; //ikki tomonga harakat 

    public float jumpForce;
    public float Gravity = -20;

 



    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerManager.isGameStarted)
            return;
        //increase speed
        if(forwardSpeed < maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;



        direction.z = forwardSpeed;

       

        direction.y += Gravity * Time.deltaTime;

        if (controller.isGrounded)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
        }

        

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
            
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
            
        }

        //keyingi manzilini belgilash

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;

        }else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }


        //transform.position = targetPosition;
        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir= diff.normalized* 25* Time.deltaTime;
        if (moveDir.sqrMagnitude <diff.sqrMagnitude)
            controller.Move(moveDir);
        else 
            controller.Move(diff);
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;
        controller.Move(direction * Time.fixedDeltaTime);

    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }

   
}
