using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sluggagames.jumper
{


    public class PlayerController : MonoBehaviour
    {
        private CharacterController controller;
        private bool isGrounded = false;
        private Vector3 playerVelocity;
        [SerializeField]
        private float playerSpeed = 2.0f;
        [SerializeField]
        private float jumpHeight = 1.0f;
        [SerializeField]
        private float graviteValue = -9.81f;
        [SerializeField]
        float playerWeight = -3.0f;
        Animator animController;



        private void Start()
        {
            try
            {
                controller = GetComponent<CharacterController>();
                animController = GetComponentInChildren<Animator>();
                animController.SetFloat("walk", 0);

            }
            catch (UnityException ex)
            {
                Debug.LogError(ex.Message);
            }


        }

        private void Update()
        {
            isGrounded = controller.isGrounded;
            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            controller.Move(move * Time.deltaTime * playerSpeed);



            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            print(isGrounded);
            if (Input.GetButton("Jump") && isGrounded)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * playerWeight * graviteValue);
                animController.SetTrigger("jump");
            }
            playerVelocity.y += graviteValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

            // animations

            //animController.SetFloat("walk", Mathf.Abs(move.x));

            Walk2DAnim("walk", move.x);



        }

        void Walk2DAnim(string animName, float value)
        {
            animController.SetFloat(animName, Mathf.Abs(value));

        }



    }
}
