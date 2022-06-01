using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    private RaycastHit2D landingHit;
    private RaycastHit2D leftHit;
    private RaycastHit2D rightHit;
    private RaycastHit2D topHit;
    private BoxCollider2D playerColider;
    Vector3 m_Size;
    float deltax;
    float deltay;
    float extraHeight = 0.01f;

    float rightPositionX;
    float topPositionY;
    float bottomPositionY;
    float leftPositionX;
    Rigidbody2D playerBody;

    public float speed;
    public float jumpHeight;

    public bool allowedToJump;
    public bool touchingWall;

    // Start is called before the first frame update
    void Start()
    {
        allowedToJump = true;

        playerBody = GetComponent<Rigidbody2D>();
        playerColider = GetComponent<BoxCollider2D>();
        rightPositionX = playerColider.bounds.max.x + .1f;
        topPositionY = playerColider.bounds.max.y + .1f;
        bottomPositionY = playerColider.bounds.min.y  - .1f;
        leftPositionX = playerColider.bounds.min.x - .1f;
        m_Size = playerColider.bounds.size;
        deltax = 0.5f * m_Size[0];
        deltay = 0.5f * m_Size[1];
    }

    void Update()
    {

        GetInput();
        //Debug.Log(new Vector2(this.transform.position.x, bottomPositionY + transform.position.y));
        //Debug.Log(new Vector2(this.transform.position.x, bottomPositionY + transform.position.y));
        //Debug.Log(m_Size);
        //Debug.Log(deltax);
        //Debug.Log(playerColider.bounds.center);
        //Debug.DrawRay(transform.position, forward, Color.green);
        //Debug.DrawRay(new Vector2(this.transform.position.x, transform.position.y), new Vector2(0, -0.1f-deltay), Color.red);
        //Debug.DrawRay ( playerColider.bounds.center, Vector2.down * ( playerColider.bounds.extents.y + extraHeight ),  Color.red, duration: 2f, depthTest: false);

        Debug.DrawRay ( playerColider.bounds.center, new Vector2(-extraHeight-deltax, 0.0f),  Color.red, duration: 1e22f, depthTest: false);

        //Debug.Log(playerColider.bounds.center);
        landingHit = Physics2D.Raycast(playerColider.bounds.center, new Vector2(0.0f, -extraHeight-deltay), 0.001f);
        leftHit = Physics2D.Raycast(playerColider.bounds.center, new Vector2(-extraHeight-deltax, 0.0f), 0.01f);
        rightHit = Physics2D.Raycast(playerColider.bounds.center, new Vector2(extraHeight+deltax, 0.0f),0.01f);
        topHit = Physics2D.Raycast(new Vector2(this.transform.position.x, topPositionY + transform.position.y), new Vector2(transform.position.x, 0.2f),0.2f);

        //Debug.DrawRay(new Vector2(this.transform.position.x, topPositionY + transform.position.y), new Vector2(0, 0.2f), Color.red);
        //Debug.DrawRay(new Vector2(leftPositionX + transform.position.x, this.transform.position.y), new Vector2(leftPositionX - 0.2f, 0), Color.red);
        //Debug.DrawRay(new Vector2(rightPositionX + transform.position.x, this.transform.position.y), new Vector2(0.2f, 0), Color.red);
        //Debug.Log(landingHit.collider.tag);
        //Debug.Log(landingHit.collider.tag);

        Debug.Log(playerColider.bounds.center);
        Debug.Log(new Vector2(this.transform.position.x, this.transform.position.y));
        //Debug.Log(new Vector2(-extraHeight-deltax, 0.0f));

        if (landingHit.collider.tag == "floor")
        {
            allowedToJump = true;
            Debug.Log("Hit the floor");
        }
        if (topHit.collider != null)
        {
            if (topHit.collider.tag == "floor")
            {
                allowedToJump = false;
                Debug.Log("Hit the top");
            }
        }
        if (leftHit.collider != null)
        {
            if (leftHit.collider.tag == "wall")
            {
                touchingWall = true;
                Debug.Log("Hit the left wall");
            }
        }
        if (rightHit.collider != null)
        {
            if (rightHit.collider.tag == "wall")
            {
                touchingWall = true;
                Debug.Log("Hit the right wall");
            }
        }
        if(rightHit.collider == null && leftHit.collider == null)
        {
            touchingWall = false;
        }

        if(touchingWall)
        {
            allowedToJump = true;
        }



     /*    Debug.DrawRay(new Vector2(this.transform.position.x, bottomPositionY + transform.position.y), new Vector2(0, -0.5f), Color.red);
    Debug.DrawRay(new Vector2(this.transform.position.x, topPositionY + transform.position.y), new Vector2(0, 0.2f), Color.red);
    Debug.DrawRay(new Vector2(leftPositionX + transform.position.x, this.transform.position.y), new Vector2(leftPositionX - 0.2f, 0), Color.red);
    Debug.DrawRay(new Vector2(rightPositionX + transform.position.x, this.transform.position.y), new Vector2(0.2f, 0), Color.red);   */

    }
    void FixedUpdate()
    {

    }
    private void GetInput()
    {

        float direction = Input.GetAxisRaw("Horizontal");


        float move = 0.0f;
        if(direction != 0.0f) {
            move = direction * speed;
            playerBody.velocity = new Vector2(move, playerBody.velocity.y);
        }
        if (Input.GetKey(KeyCode.Space) && allowedToJump)
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpHeight);
            allowedToJump = false;
        }
    }
}
