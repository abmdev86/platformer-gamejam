using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sluggagames.jumper
{


    public class PlayerController : MonoBehaviour
    {
        private CharacterController controller;
        private bool isGround = false;
        private Vector3 playerVelocity;
        [SerializeField]
        private float playerSpeed = 2.0f;
        [SerializeField]
        private float jumpHeight = 1.0f;
        [SerializeField]
        private float graviteValue = -9.81f;
        [SerializeField]
        float playerWeight = -3.0f;



        private void Start()
        {
            controller = GetComponent<CharacterController>();



        }

        private void Update()
        {
            isGround = controller.isGrounded;
            if (isGround && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            controller.Move(move * Time.deltaTime * playerSpeed);


            if (!isGround)
            {
                print("grounded");
                print($"CharacterController : {controller.isGrounded}");
            }

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }


            if (Input.GetButton("Jump") && isGround)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * playerWeight * graviteValue);
            }
            playerVelocity.y += graviteValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);




        }



    }
}
