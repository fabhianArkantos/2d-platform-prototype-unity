using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Movement
    [Header("Horizontal Movement")]
    [Range (5f,15f)][SerializeField]
    private float moveSpeed = 10f;
    private Vector2 direction = Vector2.zero;
    [Header("Vertical Movement")]
    [Range (5f,15f)][SerializeField]
    private float jumpForce = 15f;
    [SerializeField]
    private float jumpDelay = 0.25f;
    private float jumpTimer;
    #endregion

    #region Components
    [Header("Components")]
    private Rigidbody2D rb2d;
    private SpriteRenderer sprite;
    [SerializeField]
    private LayerMask groundLayer;
    #endregion

    #region Physics
    [Header("Physics")]
    [SerializeField]
    private float maxSpeed = 7f;
    [SerializeField]
    private float linearDrag = 4f;
    [SerializeField]
    private float gravity = 1;
    [SerializeField]
    private float fallMultipler = 5f;
    #endregion

    #region Collision
    [Header("Collision")]
    [SerializeField]
    private bool onGround = false;
    [SerializeField]
    private float groundLength = 0.6f;
    [SerializeField]
    private Vector3 colliderOffset;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }

        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        if (jumpTimer > Time.time && onGround)
        {
            Jump();
        }
        ModifyPhysics();
    }

    private void Jump()
    {
        //Debug.Log("Deberia saltar!");
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpTimer = 0;
    }

    private void ModifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb2d.velocity.x < 0) || (direction.x < 0 && rb2d.velocity.x > 0);

        //rb2d.drag = Mathf.Abs(direction.x) < 0.4f || changingDirections ? linearDrag : 0f;

        if (onGround)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb2d.drag = linearDrag;
            }
            else
            {
                rb2d.drag = 0f;
            }
            rb2d.gravityScale = 0;
        }
        else
        {
            rb2d.gravityScale = gravity;
            rb2d.drag = linearDrag * 0.15f;
            //bool isJumpButtonHeld = !Input.GetButton("Jump"); //Si mantiene o no el botón apretado
            if (rb2d.velocity.y < 0)
            {
                rb2d.gravityScale = gravity * fallMultipler;
            }
            else if(rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb2d.gravityScale = gravity * (fallMultipler / 2);
            }
        }
    }

    private void MoveCharacter()
    {
        rb2d.AddForce(Vector2.right * direction.x * moveSpeed);
        CheckDirectionToFlipSprite(direction.x);
        LimitRigidbody2DSpeed();
    }

    private void LimitRigidbody2DSpeed()
    {
        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
        {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
        }
    }

    private void CheckDirectionToFlipSprite(float axis)
    {
        if (axis > 0)
        {
            sprite.flipX = false;
        }
        else if (axis < 0)
        {
            sprite.flipX = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
}
