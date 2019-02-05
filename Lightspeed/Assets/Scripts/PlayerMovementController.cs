using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D playerBody;
    private List<GameObject> blocksStandingOn = new List<GameObject>();

    //add bounds for these 2
    public float speed;
    public float jumpPower;


    private AnimationController animController;
    private bool canJump = false;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerBody = GetComponent<Rigidbody2D>();
        animController = gameObject.GetComponent<AnimationController>();
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
            animController.SetIsJumping(true);
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpPower);
            canJump = false;
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("PlayerMovementController.OnCollisionEnter2D -- collided with " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("collidable"))
        {
            Collider2D col = collision.collider;

            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = col.bounds.center;

            //bool right = contactPoint.x > center.x;
            //bool top = contactPoint.y > center.y;
            if(contactPoint.y > center.y)
            {
                animController.SetIsJumping(false);
                blocksStandingOn.Add(collision.gameObject);
                canJump = true;
            }
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("collidable"))
        {
            if (blocksStandingOn.Contains(collision.gameObject))
            {
                blocksStandingOn.Remove(collision.gameObject);
            }
            if(blocksStandingOn.Count == 0)
            {
                animController.SetIsJumping(true);
            }

        }
    }
}
