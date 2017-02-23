using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [Header("Movement Values")]
    public float MovementSpeed = 50;
    public float JumpStrength = 10;
    public float DefaultJumpTime = 1;
    public float JumpBoosterDelay = 0.2f;

    Rigidbody rb;

    bool jump = false;
    bool holdJump = false;
    float jumpTimer;

    [SerializeField] bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Set the jump timer
        jumpTimer = DefaultJumpTime + JumpBoosterDelay;
    }

    void FixedUpdate()
    {
        if (!holdJump)
            return;

        //Jump code
        #region jumping
        //Down jump button
        if (isGrounded && !jump)
        {
            jump = true;
            rb.velocity = new Vector3(rb.velocity.x, JumpStrength, 0);
        }

        //Jump
        if (jump)
            {
                float _deltaTime = jumpTimer / DefaultJumpTime;

                jumpTimer -= Time.fixedDeltaTime;

                //Wait for delaytime
                if (jumpTimer < DefaultJumpTime)
                    rb.velocity = rb.velocity + new Vector3(0, JumpStrength * 2 * _deltaTime * Time.fixedDeltaTime, 0);

                //Stop when time is over
                if (jumpTimer <= 0)
                {
                    jump = false;
                    jumpTimer = DefaultJumpTime + JumpBoosterDelay;
                }
            }
        #endregion
    }

    /// <summary>
    /// Move Player
    /// </summary>
    /// <param name="_direction">The direction the player has to move towards</param>
    public void MovePlayer(float _direction)
    {
        float _movementValue = _direction * MovementSpeed * Time.deltaTime;
        rb.velocity = new Vector3(_movementValue, rb.velocity.y, 0);
    }

    /// <summary>
    /// Make the player Jump
    /// </summary>
    public void Jump()
    {
        holdJump = true;
    }

    /// <summary>
    /// Stop Jump
    /// </summary>
    public void StopJump()
    {
        holdJump = false;

        jump = false;
        jumpTimer = DefaultJumpTime + JumpBoosterDelay;
    }

    #region collision detection
    /// <summary>
    /// On Collision Enter
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter(Collision other)
    {
        //Reset is grounded
        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            isGrounded = true;
        }
    }

    /// <summary>
    /// On Collision Exit
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionExit(Collision other)
    {
        //Reset is grounded
        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            isGrounded = false;
        }
    }
    #endregion
}
