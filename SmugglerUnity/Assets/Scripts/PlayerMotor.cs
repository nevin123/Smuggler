using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMotor : NetworkBehaviour {

    //Movement vars
    [Header("Movement Vars")]
    public float MovementSpeed = 50;
    public float JumpStrength = 10;
    public float DefaultJumpTime = 1;
    public float JumpBoosterDelay = 0.2f;
    public LayerMask GroundLayers;

    Rigidbody rb;

    //Jump vars
    bool jump = false;
    bool holdJump = false;
    float jumpTimer;

    //Ground detection vars
    [Header("Player Ground Detection:")]
    [Range(0.1f, 3f)][SerializeField] float PlayerWidth = 1;
    [Range(0.1f, 3f)][SerializeField] float PlayerHeight = 1;
    [Range(0, 1)][SerializeField]float PlayerCenter = 0.5f;

    //Player detection vars
    [Header("Player Detection:")]
    private PlayerDetector playerDetectorScript;
    public LayerMask PlayerDetectionObstaclesLayer;

    //Debug vars
    [Header("Debug Vars:")]
    [SerializeField] bool isGrounded = true;
    [SerializeField] bool debugMode;
    private Color debugHit = Color.green;
    private Color debugHidden = Color.red;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Get  Scripts
        playerDetectorScript = gameObject.GetComponentInChildren<PlayerDetector>();

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

        if (Physics.Raycast(_playerCenter - new Vector3(PlayerWidth / 2 + 0.01f, 0, 0), -transform.up, out hit, PlayerHeight * PlayerCenter + 0.01f, GroundLayers))
                _rayGround = true;

        if (Physics.Raycast(_playerCenter + new Vector3(PlayerWidth / 2 + 0.01f, 0, 0), -transform.up, out hit, PlayerHeight * PlayerCenter + 0.01f, GroundLayers))
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

    /// <summary>
    /// Hand over the item to the other player
    /// </summary>
    public void HandOverItem()
    {
        if (!playerDetectorScript.isCollidingWithPlayer || playerDetectorScript.OtherPlayer == null)
            return;
        
        RaycastHit _hit;

        Debug.DrawLine(transform.position, playerDetectorScript.OtherPlayer.transform.position, debugHidden, 3);

        if (Physics.Linecast(transform.position, playerDetectorScript.OtherPlayer.transform.position, out _hit, PlayerDetectionObstaclesLayer))
        {
            
            if (debugMode)
            {
                Debug.DrawLine(transform.position, _hit.point, debugHit, 3);
                Debug.DrawLine(_hit.point, playerDetectorScript.OtherPlayer.transform.position, debugHidden, 3);
                Debug.Log(LayerMask.LayerToName(_hit.transform.gameObject.layer));
            }
            Debug.Log("blocked");
        }
        else
        {
            if (debugMode) { Debug.DrawLine(transform.position, playerDetectorScript.OtherPlayer.transform.position, debugHit, 3); }

            Debug.Log("Item could be handed over");

            CmdHandOverItem(transform.name, playerDetectorScript.OtherPlayer.name);
        }
    }

    [Command]
    void CmdHandOverItem(string playerName, string otherPlayerName)
    {
        Debug.Log(playerName + " gave the item to " + otherPlayerName);
    }
}
