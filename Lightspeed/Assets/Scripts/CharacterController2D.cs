using UnityEngine;
using UnityEngine.Events;

//Code based on: https://github.com/Brackeys/2D-Character-Controller

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Transform m_LeftCheck;                             // A position marking where to check is touching something to their left
    [SerializeField] private Transform m_RightCheck;                            // A position marking where to check is touching something to their right

    const float k_PlayerWidthRadius = .2f;                                      // Radius of the overlap area for player width
    const float k_PlayerHeightRadius = 1.081f;                                  // Radius of the overlap area for player height
    const float k_AreaCheckWidth = 0.05f;
    private bool m_Grounded;                                                    // Whether or not the player is grounded.
    private bool m_TouchingCeiling;                                             // Whether or not the player is touching something above them
    private bool m_TouchingLeft;                                                // Whether or not the player is touching something to the left of them
    private bool m_TouchingRight;                                               // Whether or not the player is touching something to the right of them
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;                                          // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;


    private AnimationController animationController;



    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnJumpEvent;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnJumpEvent == null)
            OnJumpEvent = new BoolEvent();

        animationController = GetComponent<AnimationController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("root tag ------> " + collision.transform.root.tag);
        Debug.Log("tag ------> " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("platform"))
        {
            collision.gameObject.GetComponent<PlatformMovementController>().dontMove = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("platform"))
        {
            collision.gameObject.GetComponent<PlatformMovementController>().dontMove = false;
        }
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = IsPlayerTouchingGround();
        m_TouchingCeiling = IsPlayerTouchingCeiling();
        m_TouchingLeft = IsPlayerTouchingLeft();
        m_TouchingRight = IsPlayerTouchingRight();

        if(m_Grounded && !wasGrounded) OnLandEvent.Invoke();

        animationController.SetIsJumping(!m_Grounded);
    }


    public void Move(float move, bool jump)
    {
        if (!m_AirControl && !m_Grounded) move = 0f; 
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            animationController.SetIsJumping(true);
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public bool IsPlayerTouchingGround()
    {
        Collider2D[] colliders = new Collider2D[16];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(m_WhatIsGround);
        m_GroundCheck.GetComponent<BoxCollider2D>().OverlapCollider(filter, colliders);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == null) return false;
            if (!colliders[i].gameObject.CompareTag("checkObject") && colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsPlayerTouchingCeiling()
    {
        Collider2D[] colliders = new Collider2D[16];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(m_WhatIsGround);
        m_CeilingCheck.GetComponent<BoxCollider2D>().OverlapCollider(filter, colliders);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == null) return false;
            if (!colliders[i].gameObject.CompareTag("checkObject") && colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsPlayerTouchingLeft()
    {
        Collider2D[] colliders = new Collider2D[16];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(m_WhatIsGround);
        m_LeftCheck.GetComponent<BoxCollider2D>().OverlapCollider(filter, colliders);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == null) return false;
            if (!colliders[i].gameObject.CompareTag("checkObject") && colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsPlayerTouchingRight()
    {
        Collider2D[] colliders = new Collider2D[16];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(m_WhatIsGround);
        m_RightCheck.GetComponent<BoxCollider2D>().OverlapCollider(filter, colliders);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == null) return false;
            if (!colliders[i].gameObject.CompareTag("checkObject") && colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsColliderTouchingAbove(Collider2D col)
    {
        return m_CeilingCheck.GetComponent<BoxCollider2D>().IsTouching(col);
    }
    private bool IsColliderTouchingBelow(Collider2D col)
    {
        return m_GroundCheck.GetComponent<BoxCollider2D>().IsTouching(col);
    }
    private bool IsColliderTouchingLeft(Collider2D col)
    {
        return m_LeftCheck.GetComponent<BoxCollider2D>().IsTouching(col);
    }
    private bool IsColliderTouchingRight(Collider2D col)
    {
        return m_RightCheck.GetComponent<BoxCollider2D>().IsTouching(col);
    }

    public bool IsPlayerSquished(GameObject collisionObject, Vector3 moveAmount)
    {
        Vector3 posOnTile = collisionObject.GetComponent<BoxCollider2D>().bounds.ClosestPoint(transform.position);
        Vector3 collisionPos = posOnTile + moveAmount;
        if      (IsColliderTouchingAbove(collisionObject.GetComponent<BoxCollider2D>()) && m_Grounded)  return true;
        else if (IsColliderTouchingBelow(collisionObject.GetComponent<BoxCollider2D>()) && m_TouchingCeiling) return true;
        else if (IsColliderTouchingLeft(collisionObject.GetComponent<BoxCollider2D>())  && m_TouchingRight)   return true;
        else if (IsColliderTouchingRight(collisionObject.GetComponent<BoxCollider2D>()) && m_TouchingLeft)    return true;
        return false;
    }
}