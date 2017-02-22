using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [Header("Movement Vars")]
    public float MovementSpeed = 50;
    public float JumpStrength = 10;
    public float DefaultJumpTime = 1;
    public float JumpBoosterDelay = 0.2f;

    Rigidbody rb;

    bool jump = false;
    bool holdJump = false;
    float jumpTimer;

    [Header("Player isGrounded Vars")]
    [Range(0.1f, 3f)][SerializeField] float PlayerWidth = 1;
    [Range(0.1f, 3f)][SerializeField] float PlayerHeight = 1;
    [Range(0, 1)][SerializeField]float PlayerCenter = 0.5f;

    [Header("Debug Vars")]
    [SerializeField] bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Set the jump timer
        jumpTimer = DefaultJumpTime + JumpBoosterDelay;
    }

    void FixedUpdate()
    {
        //Check if the player is grounded
        #region isGrounded
        //Test if the left/right corner of the player hits the ground
        bool _rayGround = false;

        RaycastHit hit;

        Vector3 _playerCenter = new Vector3(transform.position.x, transform.position.y - PlayerHeight / 2 + PlayerHeight * PlayerCenter, transform.position.z);

        if (Physics.Raycast(_playerCenter - new Vector3(PlayerWidth / 2 + 0.01f, 0, 0), -transform.up, out hit, PlayerHeight * PlayerCenter + 0.01f))
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Environment"))
                _rayGround = true;

        if (Physics.Raycast(_playerCenter + new Vector3(PlayerWidth / 2 + 0.01f, 0, 0), -transform.up, out hit, PlayerHeight * PlayerCenter + 0.01f))
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Environment"))
                _rayGround = true;

        if (_rayGround)
            isGrounded = true;
        else
            isGrounded = false;

        #endregion

        //Jump code
        #region jumping
        if (!holdJump)
        return;

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
}
