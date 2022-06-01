using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlaneController : MonoBehaviour
{
	public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
	
	Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
	
    // Start is called before the first frame update
    void Start()
    {
		rigidbody2D = GetComponent<Rigidbody2D>();
		timer = changeTime;
        
    }

    // Update is called once per frame
    void Update()
    {
		timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
	
	void FixedUpdate()
	{
		Vector2 position = rigidbody2D.position;
		Vector2 position2 = rigidbody2D.position;
		if (vertical)
        {
            position2.y = position.y + Time.deltaTime * speed * direction;

        }
        else
        {
            position2.x = position.x + Time.deltaTime * speed * direction;
        }

	    rigidbody2D.velocity = (position2-position)/Time.deltaTime;
	}
	
/* 	void OnCollisionEnter2D(Collision2D other)
	{
		//Debug.Log("Hit the kinematic ground.");
		PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
		if(player!=null)
		{
			Debug.Log(player);
			player.isGrounded(true);
			//Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>());
			//Physics2D.IgnoreCollision( GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
		}
	}
 */
}

