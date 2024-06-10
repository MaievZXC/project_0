using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Constants")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float coyoteTime;
    [SerializeField] private int extraJumps;
    [SerializeField] private float WallJumpX;
    [SerializeField] private float WallJumpY;
    private int extraJumpsCounter;
    private float coyoteTimer;
    private float wallJumpCooldown;
    private float horizontalInput;
    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;


    private PlayerAttack playerAttack;
    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private Animator anim;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if(horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if(horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);


        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
        anim.SetBool("fall", body.velocity.y < 0);

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (onWall() && !isGrounded())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            if (isGrounded())
            {
                coyoteTimer = coyoteTime;
                extraJumpsCounter = extraJumps;
            }
            else
            {
                coyoteTimer -= Time.deltaTime;
            }
        }
    }

    private void Jump()
    {
        if (coyoteTimer < 0 && !onWall() && extraJumpsCounter <= 0) return;

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall()) 
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                if(coyoteTimer > 0) 
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else if(extraJumpsCounter > 0)
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    extraJumpsCounter--;
                }
                
            }
            coyoteTimer = 0;
        }
    }

    //looks pretty dummy, gotta find better solution
    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * WallJumpX, WallJumpY));
        wallJumpCooldown = 0;
    }


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y - 0.5f,
            boxCollider.bounds.size.z), 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return !onWall() || isGrounded();
    }

    public void setVelosity0()
    {
        body.velocity = new Vector2(0, body.velocity.y);
    }
}