using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D playerBody;

    //add bounds for these 2
    public float speed;
    public float jumpPower;

    private bool canJump = false;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

        if (spriteRenderer != null && Mathf.Abs(playerBody.velocity.x) > 0.001f)
        {
            spriteRenderer.flipX = playerBody.velocity.x < 0.0f;
        }

        playerBody.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, playerBody.velocity.y);
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpPower);
            canJump = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("PlayerMovementController.OnCollisionEnter2D -- collided with " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("collidable"))
        {
            Collider2D collider = collision.collider;

            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collider.bounds.center;

            //bool right = contactPoint.x > center.x;
            //bool top = contactPoint.y > center.y;
            if(contactPoint.y > center.y)
            {
                canJump = true;
            }
            
        }
    }
}
