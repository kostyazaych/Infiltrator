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
    Vector3 oldDirection = new Vector3(0f,0f,0f);
    
    private float speedSmoothVelocity = 0.05f;
    public float speedSmoothTime = 0.2f;
    private Rigidbody RootRigidbody;
    private float jumpHeight = 1.0f;
    public float gravity = -9.81f;

    [HideInInspector] public bool slowMovementMode = true;
    
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
       
        //TransitAnimaStateMachine(CharacterAnimator.GetCurrentAnimatorStateInfo(0));
        /*this.rotation = new Vector3(0, Input.GetAxisRaw("Mouse X") * mouseSense * Time.deltaTime, 0);
        this.transform.Rotate(this.rotation);
        */

        //Attacking
        /* if (Input.GetButton("Fire1"))
             GetComponent<CharacterBase>().CharacterAttack();
             */
    }

    public void PrintEvent(string s) {
        Debug.Log("PrintEvent: " + s + " called at: " + Time.time);
    }
    
    private void Move()
    {
        Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementInput.x = movementInput.x * -1;
        //Инвертирую направление движения, * -1 нужно, чтоб на "S" двигался вниз, а не вверх
       
        Vector3 desiredMoveDirection = new Vector3(movementInput.y, 0, movementInput.x);
        desiredMoveDirection.Normalize();
        desiredMoveDirection = transform.TransformDirection(desiredMoveDirection);       
        
        if ( desiredMoveDirection.magnitude != 0)
        {
            if (slowMovementMode)
            {
                movementType = 1;
                movementSpeed = 3f;
                SendParametersToAnimationNetwork(CharacterAnimator, movementType, desiredMoveDirection, movementSpeed );
            }
            else
            {
                movementType = 2;
                movementSpeed = 6f;
                SendParametersToAnimationNetwork(CharacterAnimator, movementType, desiredMoveDirection, movementSpeed );
            }
            //Debug.Log(movementSpeed);
            //CharacterAnimator.SetInteger(name: "Move", value: movementType);  
            oldDirection = desiredMoveDirection;
        }
        
        else
        {
            movementType = 0;
            SendParametersToAnimationNetwork(CharacterAnimator, movementType, desiredMoveDirection, movementSpeed );
           // CharacterAnimator.SetInteger(name: "Move", value: movementType);
            desiredMoveDirection = Vector3.SmoothDamp(oldDirection, new Vector3(0f,0f,0f), ref oldDirection, speedSmoothTime);      
        }       

        Vector3 gravityVector = Vector3.zero;

        if (!controller.isGrounded)
        {
            gravityVector.y -= gravity;
        }

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
        this.transform.Rotate(0, mouseX * mouseSense * Time.deltaTime, 0);
      }    
    
    public void SendParametersToAnimationNetwork (Animator inputCharacterAnimator, int inputMovementType, Vector3 inputDirection, float inputSpeed) //direction = 0f;  0 - forward; -90 - left; 90 - right | movementType = 0; 0 - stands; 1 - walk; 2 - run
    {
        
        Quaternion rotation = Quaternion.LookRotation(inputDirection, new Vector3(0f,1f,0f));
        float Look = rotation.z;
        //= Quaternion.Euler(rotation.y);
        
        
        inputCharacterAnimator.SetInteger(name: "Move", value: inputMovementType);          
        inputCharacterAnimator.SetFloat(name: "Direction", value: Look);
        inputCharacterAnimator.SetFloat(name: "Speed", value: inputSpeed);
        
        
    }    
    void TransitAnimaStateMachine(string Leg)
    {
        if (Leg == "Right")           
            CharacterAnimator.SetTrigger("RightLeg");
        else if(Leg == "Left")
            CharacterAnimator.SetTrigger("LeftLeg");
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
    