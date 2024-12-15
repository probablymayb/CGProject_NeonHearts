using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace monogon_common
{
	public class ThirdPerson : MonoBehaviour
	{
		public static ThirdPerson _instance;
		[HideInInspector] public bool _canMove;
		[Header("")]

		[SerializeField] private float MoveSpeed = 4; 
		[SerializeField] private float runSpeed = 8;
		[SerializeField] private float rotationSpeed = 10;
		[SerializeField] float JumpHeight = 5;
		[SerializeField] private Transform cam;
		
		Rigidbody rb;
		private bool IsWalking;
		
		[HideInInspector] public Animator playerAnim;
		private float actualSpeed;
		//[SerializeField] private Transform cam;



		void Start()
		{
			_instance = this;
			actualSpeed = MoveSpeed;
			playerAnim = GetComponent<Animator>();
			rb = GetComponent<Rigidbody>();

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

void Update()
{
    //inputs
    float horInput = Input.GetAxisRaw("Horizontal");
    float verInput = Input.GetAxisRaw("Vertical");

    //camera dir
    Vector3 camFoward = cam.forward;
    Vector3 camRight = cam.right;
    camFoward.y = 0;
    camRight.y = 0;

    //creating relate cam direction
    Vector3 forwardRelative = verInput * camFoward;
    Vector3 rightRelative = horInput * camRight;
    Vector3 moveDir = forwardRelative + rightRelative;

    //movement
    rb.velocity = new Vector3(moveDir.x * actualSpeed, rb.velocity.y, moveDir.z * actualSpeed);

    if (Input.GetButtonDown("Jump") && Mathf.Approximately(rb.velocity.y, 0))
        rb.velocity = new Vector3(rb.velocity.x, JumpHeight, rb.velocity.z);

    if (horInput != 0 || verInput != 0)
    {
        IsWalking = true;

        // Smoothly rotate the character towards the movement direction
        Vector3 lookDirection = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    else
    {
        IsWalking = false;
    }

    playerAnim.SetBool("Walk", IsWalking);
}


		public void Sprint()
		{

			if (IsWalking)
			{
				if (Input.GetKeyDown(KeyCode.LeftShift))
				{
					actualSpeed = actualSpeed + runSpeed;
					playerAnim.SetFloat("WalkSpeed", 1.5f);

				}
				if (Input.GetKeyUp(KeyCode.LeftShift))
				{
					actualSpeed = MoveSpeed;
					playerAnim.SetFloat("WalkSpeed", 1);

				}
			}
		}

	}
}