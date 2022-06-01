using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerMovement : MonoBehaviour
{ 
    //[SerializeField] private float moveSpeed;
	private Rigidbody2D body;
	private CircleCollider2D playerColider;
	private float horizontal;
		
    public float moveSpeed = 10.0f;
	public float airMoveSpeed = 10.0f;
    public float jumpHeight = 17.5f;
    public float wallJumpForce = 17.5f;
	public float bounceDist = 10.0f;
	public float wallSlideSpeed=1.0f;
	public Vector2 wallJumpAngle;
    public LayerMask groundLayer;
	
	private float wallJumpForceTemp=0.0f;
	private float wallJumpDirection = -1.0f;
	private float previousWall = 0.0f;
	private float previousWallTemp=0.0f;
	private float deltax;
	private float deltay;
	private float distance = 0.51f;
	private float crossdistance = 0.71f;
	private Vector3 m_Size;
/* 	private RaycastHit2D landingHit;
	private RaycastHit2D rightHit;
	private RaycastHit2D leftHit;
	private RaycastHit2D lowerLeftHit;
	private RaycastHit2D lowerRightHit;
	private RaycastHit2D upperLeftHit;
	private RaycastHit2D upperRightHit; */

	
	bool allowedToJump;
	private bool allowedToWallJump;
    private bool touchingRightWall;
    private bool touchingLeftWall;
	private bool touchingGround;
	private bool touchingWall;
	private bool isWallSliding;
	
	//float rightPositionX;
	
	
	
    private void Start()
    {
        allowedToJump = true;
		allowedToWallJump = true;
		touchingGround = false;
		body = GetComponent<Rigidbody2D>();
		playerColider = GetComponent<CircleCollider2D>();
		m_Size = playerColider.bounds.size;
        deltax = 0.5f * m_Size[0];
        deltay = 0.5f * m_Size[1];
				
    }
 
    private void Update()
    {
		
		drawRayCast();
		checkObstacle();
		
    }
	private void FixedUpdate()
    {
		// GetInput(); 
		Movement();
		WallSlide();
		WallJump();
    }
	
	void drawRayCast()
	{
		Vector2 position = transform.position;
		Debug.DrawRay ( position, new Vector2(-distance, 0.0f), Color.red);
		Debug.DrawRay ( position, new Vector2(distance, 0.0f), Color.green);
		
		Debug.DrawRay ( position, new Vector2(0.0f, distance), Color.blue);
		Debug.DrawRay ( position, new Vector2(0.0f, -distance), Color.black);
		
		Debug.DrawRay ( position, new Vector2(-distance, -distance), Color.yellow);
		Debug.DrawRay ( position, new Vector2(distance, -distance), Color.yellow);
	}
	
	void checkObstacle()
	{
		allowedToJump = false;
		touchingGround = false;
		touchingRightWall = false;
		touchingLeftWall = false;
		touchingWall = false;
		allowedToWallJump = true;
		Vector2 position = transform.position;
		
		RaycastHit2D landingHit = Physics2D.Raycast(position, Vector2.down, distance, groundLayer);
		RaycastHit2D rightHit = Physics2D.Raycast(position, new Vector2(1, 0), distance, groundLayer);
		RaycastHit2D leftHit = Physics2D.Raycast(position, new Vector2(-1, 0), distance, groundLayer);
		RaycastHit2D lowerLeftHit = Physics2D.Raycast(position, new Vector2(-1, -1), crossdistance, groundLayer);
		RaycastHit2D lowerRightHit = Physics2D.Raycast(position, new Vector2(1, -1), crossdistance, groundLayer);
		RaycastHit2D upperLeftHit = Physics2D.Raycast(position, new Vector2(-1, 1), crossdistance, groundLayer);
		RaycastHit2D upperRightHit = Physics2D.Raycast(position, new Vector2(1, 1), crossdistance, groundLayer);
		
		if (landingHit.collider != null) 
		{
			if (landingHit.collider.tag == "floor" || landingHit.collider.tag == "wall")
			{
				allowedToJump = true;
				touchingGround = true;
				//Debug.Log("Hit the floor");
			}
		}
		if (lowerLeftHit.collider != null)
		{
			if (lowerLeftHit.collider.tag == "floor" || lowerLeftHit.collider.tag == "wall")
			{
				touchingGround = true;
				//Debug.Log("Left corner hit the floor");
			}
		}
		if (lowerRightHit.collider != null)
		{
			if (lowerRightHit.collider.tag == "floor" || lowerRightHit.collider.tag == "wall")
			{
				touchingGround = true;
				//Debug.Log("Right corner hit the floor");
			}
		}
		if (upperLeftHit.collider != null)
		{
			if (upperLeftHit.collider.tag == "floor" || upperLeftHit.collider.tag == "wall")
			{
				touchingGround = false;
				//Debug.Log("Left corner hit the floor");
			}
		}
		if (upperRightHit.collider != null)
		{
			if (upperRightHit.collider.tag == "floor" || upperRightHit.collider.tag == "wall")
			{
				touchingGround = false;
				//Debug.Log("Left corner hit the floor");
			}
		}
        if (rightHit.collider != null)
        {
            if (rightHit.collider.tag == "wall" || rightHit.collider.tag == "floor")
            {
                touchingRightWall = true;
                //Debug.Log("Hit the right wall");
            }
        }
		if (leftHit.collider != null)
        {
            if (leftHit.collider.tag == "wall" || leftHit.collider.tag == "floor")
            {
                touchingLeftWall = true;
                //Debug.Log("Hit the left wall");
            }
        }
		if(touchingLeftWall || touchingRightWall)
        {
			touchingWall = true;
        }
		
	}

	void Movement()
	{
		Debug.Log("Touch the ground: " + touchingGround);
		Debug.Log("Touch the wall: " + touchingWall);
		//Debug.Log("Allow to jump: " + allowedToJump);
		
		// check touch wall
		if (touchingLeftWall)
		{
			float direction = Mathf.Clamp(Input.GetAxis("Horizontal"),0,100);
			body.velocity = new Vector2(direction * moveSpeed, body.velocity.y);
		}
		else if (touchingRightWall)
		{
			float direction = Mathf.Clamp(Input.GetAxis("Horizontal"),-100,0);
			body.velocity = new Vector2(direction * moveSpeed, body.velocity.y);
		}
		
		// check touch ground
		if (Input.GetKey(KeyCode.Space) && allowedToJump)
		{	
			body.velocity = new Vector2(body.velocity.x, jumpHeight);
			
		}
		else
		{
			body.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, body.velocity.y);
		}
	}
	
	void WallSlide()
	{
		if (touchingWall && !touchingGround && body.velocity.y <0.0f)
		{
			isWallSliding = true;
		}
		else
		{
			isWallSliding = false;
		}
		// Debug.Log("Sliding from wall: "+isWallSliding);
		if (isWallSliding)
		{
			body.velocity = new Vector2(body.velocity.x, -wallSlideSpeed);
		}
	}
	
    void WallJump()
    {
		//Debug.Log("Left / Right: "+touchingLeftWall+" / "+touchingRightWall);
		Debug.Log("Touching Ground: "+touchingGround);
		//allowedToWallJump
        if (Input.GetKey(KeyCode.Space) && isWallSliding) // && allowedToWallJump
        {
			if (touchingLeftWall)
			{
				wallJumpDirection = 1.0f;
				previousWallTemp = -1.0f;
			}
			else if (touchingRightWall)
			{
				wallJumpDirection = -1.0f;
				previousWallTemp = 1.0f;
			}
			Debug.Log(wallJumpDirection +"/"+ previousWall);
			if (wallJumpDirection != previousWall)
			{
				//Debug.Log("Jump from the same wall");
				wallJumpForceTemp = 0.5f;
			}
			else
			{
				wallJumpForceTemp = wallJumpForce;
			}
			Debug.Log(wallJumpForceTemp);
			Vector2 force = new Vector2(wallJumpForceTemp * wallJumpAngle.x * wallJumpDirection, wallJumpForceTemp * wallJumpAngle.y);
            body.AddForce(force,ForceMode2D.Impulse);
            previousWall = previousWallTemp;
			
        }   
    }
	
/* 	public void isGrounded(bool isgrounded)
	{
		if (isgrounded) 
		{
			Debug.Log("Hit the kinematic ground.");
			allowedToJump = true;
		}
	} */
}

