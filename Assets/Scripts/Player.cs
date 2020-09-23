using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    // Config 
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float jumpPushForward = 3f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);

    // wall jump
    public Vector2 wallJumpClimb;

	// public Vector2 wallJumpOff;

    public float wallJumpOff = 20f;
	public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3f;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

    bool wallSliding;
	int wallDirX;

    // Dust
    public ParticleSystem dust;

    // State
    bool isAlive = true;
    //bool wallSliding = false;
    [SerializeField] bool wallJump = false;
    [SerializeField] float wallJumpSpeed = 3f;

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;

    // Message then methods
    void Start() {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update(){
        if (!isAlive) { return; }

        // bool wallSliding = false;
        // wall sliding
        HandleWallSliding ();

        Run();
        ClimbLadder();
        Jump();
        FlipSprite();
        Die();
    }

    void HandleWallSliding() {
        // change how the wall sees left, and replace here
		// wallDirX = (controller.collisions.left) ? -1 : 1;
        wallDirX = -(int)(Mathf.Sign(myRigidBody.velocity.x));

		wallSliding = false;
        // replace left, right, and below
		if ((myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Wall"))) 
            && (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) 
            && myRigidBody.velocity.y < 0) {
			wallSliding = true;

			if (myRigidBody.velocity.y < -wallSlideSpeedMax) {

                Vector2 newWallSpeed = new Vector2(0f, -wallSlideSpeedMax);
                myRigidBody.velocity = newWallSpeed;
				//myRigidBody.velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				// velocityXSmoothing = 0;

				//myRigidBody.velocity.x = 0;
                myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);

				if (CrossPlatformInputManager.GetAxis("Horizontal") != wallDirX 
                    && CrossPlatformInputManager.GetAxis("Horizontal") != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}

		}

	}

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is betweeen -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void ClimbLadder()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);

    }

    private void Jump()
    {
        // Wall jump 
        // if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){
        //     wallJump = true;
        // } else {
        //     wallJump = false;
        // }

        // if(CrossPlatformInputManager.GetButtonDown("Jump")){
        //     if(wallJump) {
        //         float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        //         Vector2 jumpVelocityToAdd = new Vector2((-(controlThrow * 100)), jumpSpeed);
        //         myRigidBody.gravityScale = 0f;
        //         Debug.Log(jumpVelocityToAdd);

        //         //Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
        //         myRigidBody.velocity += jumpVelocityToAdd;
        //         //myRigidBody.velocity += jumpVelocityToAdd;
        //     } 
        // }
        
        if (wallSliding) {
			// if (wallDirX == directionalInput.x) {
			// 	velocity.x = -wallDirX * wallJumpClimb.x;
			// 	velocity.y = wallJumpClimb.y;
			// }
			// else if (directionalInput.x == 0) {
			// 	velocity.x = -wallDirX * wallJumpOff.x;
			// 	velocity.y = wallJumpOff.y;
			// }
			// else {
			// 	velocity.x = -wallDirX * wallLeap.x;
			// 	velocity.y = wallLeap.y;
			// }

            if (CrossPlatformInputManager.GetButtonDown("Jump")) {
                float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
                Vector2 jumpOffVelocityToAdd = new Vector2(controlThrow, wallJumpOff);
                myRigidBody.velocity = jumpOffVelocityToAdd;
                // CreateDust();
            }
		}

        // CreateDust();
        // StopDust();

        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }



        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
                Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
                // Vector2 jumpVelocityToAdd = new Vector2((myRigidBody.velocity.x * jumpPushForward), jumpSpeed);
                // Debug.Log(myRigidBody.velocity.x);
                // Debug.Log(jumpVelocityToAdd);
                myRigidBody.velocity += jumpVelocityToAdd;
                // CreateDust();
        }
    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void FlipSprite()
    {
        // Debug.log('Created Dust')

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        // if(Mathf.Abs(myRigidBody.velocity.x) === 0){
            // CreateDust();
        // }

        if (playerHasHorizontalSpeed)
        {
            // CreateDust();
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    public void CreateDust() 
    {
        Debug.Log('d');
        // dust.Emit();
        dust.Play();
    }
        public void StopDust() 
    {
        Debug.Log('s');
        dust.Stop();
    }

}
