using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class InputController : MonoBehaviour
{
    private CharacterController controller;
    private Animator CharacterAnimator;
   
    private Vector3 playerVelocity;
    [HideInInspector] public float movementSpeed = 3f;
    [HideInInspector] public int movementType = 0;
    private float readyMovementSpeed = 1.5f;
    private float currentSpeed = 0f;
    [HideInInspector] public float speedBuffCoef = 1f;
    private Vector3 rotation;
    
    private float speedSmoothVelocity = 0.05f;
    public float speedSmoothTime = 0.2f;
    private Rigidbody RootRigidbody;
    private float jumpHeight = 1.0f;
    public float gravity = -9.81f;

    private Camera PlayerCamera;
    private Vector3 TestVelocity;
    public float mouseSense = 500f;



    private Transform player;
    private Transform mainCameraTransform = null;
 

    
    /// /////////////////
 
    
    // Start is called before the first frame update
    void Awake()
    {
        player = this.gameObject.transform;
        Cursor.visible = true;
        controller = GetComponent<CharacterController>();
        CharacterAnimator = GetComponentInChildren<Animator>();
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
        Vector3 oldDirection = new Vector3(0f,0f,0f);
        movementInput.x = movementInput.x * -1;
        //Инвертирую направление движения, * -1 нужно, чтоб на "S" двигался вниз, а не вверх
       
        Vector3 desiredMoveDirection = new Vector3(movementInput.y, 0, movementInput.x);
        desiredMoveDirection.Normalize();
        desiredMoveDirection = transform.TransformDirection(desiredMoveDirection);       
        
        if ( desiredMoveDirection.magnitude != 0)
        {
            movementType = 1;
            CharacterAnimator.SetInteger(name: "Move", value: movementType);  
            oldDirection = desiredMoveDirection;
        }
        else
        {
            movementType = 0;
            CharacterAnimator.SetInteger(name: "Move", value: movementType);

            Vector3 emptyVec = new Vector3(0f,0f,0f); 
            desiredMoveDirection = Vector3.Lerp(oldDirection, emptyVec, 0.5f);
           
        }       
        
        Vector3 gravityVector = Vector3.zero;

        if (!controller.isGrounded)
        {
            gravityVector.y -= gravity;
        }

        
        Debug.Log(desiredMoveDirection);
        gravityVector.y = -9;                   
        float targetSpeed = movementSpeed * desiredMoveDirection.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);     
        
        controller.Move(currentSpeed * Time.deltaTime * desiredMoveDirection + gravityVector * Time.deltaTime);

      
        
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
        Vector3 rotation;

        this.transform.Rotate(0, mouseX * mouseSense * Time.deltaTime, 0);
        }       
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
    