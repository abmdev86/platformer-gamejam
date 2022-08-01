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

        Vector3 move;

        private void Start()
        {
            try
            {
                controller = GetComponent<CharacterController>();
                animController = GetComponentInChildren<Animator>();
                animController.SetFloat("walk", 0);
                transform.forward = new Vector3(1, 0, 0);

            }
            catch (UnityException ex)
            {
                Debug.LogError(ex.Message);
            }


        }

        private void Update()
        {

            // moving
            isGrounded = controller.isGrounded;
            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            controller.Move(move * Time.deltaTime * playerSpeed);



            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }


            if (Input.GetButton("Jump") && isGrounded)
            {
                animController.ResetTrigger("jumpAttack");

                playerVelocity.y += Mathf.Sqrt(jumpHeight * playerWeight * graviteValue);
                animController.SetTrigger("jump");
            }
            playerVelocity.y += graviteValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);


            // attacking
            if (Input.GetKey(KeyCode.E) && isGrounded)
            {
                print("attack");
                animController.SetTrigger("attack");
            }
            else if (Input.GetKey(KeyCode.E) && !isGrounded)
            {
                print("Jump Attack");
                animController.SetTrigger("jumpAttack");
            }

            //skills 

            if (isGrounded)
            {

                if (Input.GetKey(KeyCode.Alpha1))
                {
                    animController.SetTrigger("skill1");
                }
                else if (Input.GetKey(KeyCode.Alpha2))
                {
                    animController.SetTrigger("skill2");

                }
                else if (Input.GetKey(KeyCode.Alpha3))
                {
                    animController.SetTrigger("skill3");

                }
            }


            // dodging 
            if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
            {
                animController.SetTrigger("dodge");

            }

            // move animations

            //animController.SetFloat("walk", Mathf.Abs(move.x));

            Walk2DAnim("walk", move.x);
        }

        void Walk2DAnim(string animName, float value)
        {
            animController.SetFloat(animName, Mathf.Abs(value));

        }



    }
}
