using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class InputController : MonoBehaviour
{
    private CharacterController controller;
    // private Animator animator;

    private float movementSpeed = 3f;
    private float readyMovementSpeed = 1.5f;
    private float currentSpeed = 0f;
    [HideInInspector] public float speedBuffCoef = 1f;
    private float speedSmoothVelocity = 0.05f;
    private float speedSmoothTime = 0.01f;
    private Rigidbody RootRigidbody;
    private float jumpHeight = 1.0f;
    private Vector3 playerVelocity;
    //private float rotationSpeed = 0.1f;
    public float gravity = -9.81f;

    private Camera PlayerCamera;
    private Vector3 TestVelocity;
    public float mouseSense = 0.1f;



    private Transform player;
    private Transform mainCameraTransform = null;

    private bool ReadyWeaponMovement = true;
    private Vector3 rotation;

    // Start is called before the first frame update
    void Awake()
    {
        player = this.gameObject.transform;
        Cursor.visible = true;
        controller = GetComponent<CharacterController>();
       // animator = GetComponent<Animator>();
        PlayerCamera = GetComponentInChildren<Camera>();
        RootRigidbody = GetComponentInChildren<Rigidbody>();
        mainCameraTransform = PlayerCamera.transform;


    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetAxisRaw("Jump") > 0f)
            Jump();

        //ReadyWeapon();
         RotateCharacter();
        
        /*this.rotation = new Vector3(0, Input.GetAxisRaw("Mouse X") * mouseSense * Time.deltaTime, 0);
        this.transform.Rotate(this.rotation);
        */
        
        //Attacking
        /* if (Input.GetButton("Fire1"))
             GetComponent<CharacterBase>().CharacterAttack();
             */
    }


    private void Move()
    {
        Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        movementInput.x = movementInput.x * -1;
        //Инвертирую направление движения, * -1 нужно, чтоб на "S" двигался вниз, а не вверх

        Vector3 desiredMoveDirection = new Vector3(movementInput.y, 0, movementInput.x);
        desiredMoveDirection.Normalize();
        //transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);
        Vector3 gravityVector = Vector3.zero;
      

        if (!controller.isGrounded)
        {
            gravityVector.y -= gravity;
        }
        Vector3 kekVec = new Vector3(1f, 0f, 1f);
        float targetSpeed;
        if (ReadyWeaponMovement == false)
        {
            targetSpeed = movementSpeed * desiredMoveDirection.magnitude;     
        }
        else
        {
            targetSpeed = readyMovementSpeed * desiredMoveDirection.magnitude;
        }

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);     
        controller.Move(desiredMoveDirection * currentSpeed * Time.deltaTime);
        //controller.Move(gravityVector * Time.deltaTime);
    }


    void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        controller.Move(playerVelocity * Time.deltaTime);
Debug.Log("JUMP");
    }

   private void RotateCharacter()
     {
         float mouseX = Input.GetAxis("Mouse X");
         float mouseY = Input.GetAxis("Mouse Y");
         float mouseSenseReady = mouseSense * 0.5f;
         float rotAmountX = 0f;
         float rotAmountY = 0f;
         Vector3 rotation;
    
         transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime, 0);
      

         /* this.rotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * mouseSense * Time.deltaTime, 0);
          player.transform.Rotate(this.rotation123);   
           */
         /*if (ReadyWeaponMovement == false)
         {
             rotAmountX = mouseX * mouseSense;
             rotAmountY = mouseY * (mouseSense * -1.4f);
         }
         else
         {
             rotAmountX = mouseX * mouseSenseReady;
             rotAmountY = mouseY; //* (mouseSenseReady * -1.4f);
         }

           Vector3 rotPlayer = player.transform.rotation.eulerAngles;

           rotPlayer.y += rotAmountX;
           rotPlayer.z = 0;

           if (ReadyWeaponMovement == true)
           {
               rotPlayer.x += rotAmountY;

               if (rotPlayer.x < 320f && rotPlayer.x > 65f)
               {
                   rotPlayer.x = 320f;
               }
               if (rotPlayer.x > 59f && rotPlayer.x < 320f)
               {
                   rotPlayer.x = 55f;
               }
           }
            else
           {
               rotPlayer.x = 0;
           }

          if (Input.GetButtonUp("Turn"))
               rotPlayer.y = rotPlayer.y - 180f;

           player.rotation = Quaternion.Euler(rotPlayer);*/


       }

     


    /*void ReadyWeapon()
    {

        if (Input.GetButton("Fire2") && ReadyWeaponMovement == false)
        {
            GetComponent<CharacterBase>().ReadyCharacterWeapon();
            ReadyWeaponMovement = true;

        }
        if ((Input.GetButtonUp("Fire2") && ReadyWeaponMovement == true))
        {
            GetComponent<CharacterBase>().ReadyCharacterWeapon();
            ReadyWeaponMovement = false;

        }
    }*/
}