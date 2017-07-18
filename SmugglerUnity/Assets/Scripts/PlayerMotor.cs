using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMotor : NetworkBehaviour {
    
    //Movement vars
    [Header("Movement Vars")]
    public float MovementSpeed = 50;
    [Range(0.1f, 3f)] public float ZMovementMultiplier = 0.5f;
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
    private TagDetector tagDetectorScript;
    public LayerMask tagDetectionObstaclesLayer;

    //Package Vars
    public GameObject Holder;

    //Debug vars
    [Header("Debug Vars:")]
    [SerializeField] bool isGrounded = true;
    [SerializeField] bool isNextToALedge = false;

    [SerializeField] bool debugMode;
    private Color debugHit = Color.green;
    private Color debugHidden = Color.red;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Get  Scripts
        tagDetectorScript = gameObject.GetComponentInChildren<TagDetector>();

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

        if (debugMode)
        {
            Debug.DrawLine(_playerCenter - new Vector3(PlayerWidth / 2 + 0.01f, 0, PlayerWidth / 2 + 0.01f), _playerCenter - new Vector3(PlayerWidth / 2 + 0.01f, 0, PlayerWidth / 2 + 0.01f) - transform.up * 3, Color.red);
            Debug.DrawLine(_playerCenter + new Vector3(PlayerWidth / 2 + 0.01f, 0, PlayerWidth / 2 + 0.01f), _playerCenter + new Vector3(PlayerWidth / 2 + 0.01f, 0, PlayerWidth / 2 + 0.01f) - transform.up * 3, Color.red);
            Debug.DrawLine(_playerCenter - new Vector3(PlayerWidth / 2 + 0.01f, 0, -PlayerWidth / 2 - 0.01f), _playerCenter - new Vector3(PlayerWidth / 2 + 0.01f, 0, -PlayerWidth / 2 - 0.01f) - transform.up * 3, Color.red);
            Debug.DrawLine(_playerCenter + new Vector3(PlayerWidth / 2 + 0.01f, 0, -PlayerWidth / 2 - 0.01f), _playerCenter + new Vector3(PlayerWidth / 2 + 0.01f, 0, -PlayerWidth / 2 - 0.01f) - transform.up * 3, Color.red);
            Debug.DrawLine(_playerCenter - new Vector3(PlayerWidth / 2 + 0.01f, 0, 0), _playerCenter - new Vector3(PlayerWidth / 2 + 0.01f, 0, 0) - transform.up * 3, Color.red);
            Debug.DrawLine(_playerCenter + new Vector3(PlayerWidth / 2 + 0.01f, 0, 0), _playerCenter + new Vector3(PlayerWidth / 2 + 0.01f, 0, 0) - transform.up * 3, Color.red);
            Debug.DrawLine(_playerCenter - new Vector3(0, 0, PlayerWidth / 2 + 0.01f), _playerCenter - new Vector3(0, 0, PlayerWidth / 2 + 0.01f) - transform.up * 3, Color.red);
            Debug.DrawLine(_playerCenter + new Vector3(0, 0, PlayerWidth / 2 + 0.01f), _playerCenter + new Vector3(0, 0, PlayerWidth / 2 + 0.01f) - transform.up * 3, Color.red);
        }

        //Raycast from every corner and inbetween corners downwards to see if the player is grounded
        if (Physics.Raycast(_playerCenter - new Vector3(PlayerWidth / 2 + 0.01f, 0, 0), -transform.up, out hit, PlayerHeight * PlayerCenter + 0.01f, GroundLayers))
            _rayGround = true;

        if (Physics.Raycast(_playerCenter + new Vector3(PlayerWidth / 2 + 0.01f, 0, 0), -transform.up, out hit, PlayerHeight * PlayerCenter + 0.01f, GroundLayers))
            _rayGround = true;

        if (Physics.Raycast(_playerCenter - new Vector3(0, 0, PlayerWidth / 2 + 0.01f), -transform.up, out hit, PlayerHeight * PlayerCenter + 0.01f, GroundLayers))
            _rayGround = true;

        if (Physics.Raycast(_playerCenter + new Vector3(0, 0, PlayerWidth / 2 + 0.01f), -transform.up, out hit, PlayerHeight * PlayerCenter + 0.01f, GroundLayers))
            _rayGround = true;

        if (Physics.Raycast(_playerCenter - new Vector3(PlayerWidth / 2 + 0.01f, 0, PlayerWidth / 2 + 0.01f), -transform.up, out hit, PlayerHeight * PlayerCenter + 0.01f, GroundLayers))
            _rayGround = true;

        if (Physics.Raycast(_playerCenter + new Vector3(PlayerWidth / 2 + 0.01f, 0, PlayerWidth / 2 + 0.01f), -transform.up, out hit, PlayerHeight * PlayerCenter + 0.01f, GroundLayers))
            _rayGround = true;

        if (Physics.Raycast(_playerCenter - new Vector3(PlayerWidth / 2 + 0.01f, 0, -PlayerWidth / 2 - 0.01f), -transform.up, out hit, PlayerHeight * PlayerCenter + 0.01f, GroundLayers))
            _rayGround = true;

        if (Physics.Raycast(_playerCenter + new Vector3(PlayerWidth / 2 + 0.01f, 0, -PlayerWidth / 2 - 0.01f), -transform.up, out hit, PlayerHeight * PlayerCenter + 0.01f, GroundLayers))
            _rayGround = true;

        if (_rayGround)
        {
            isGrounded = true;
            
            //Checks if the player is on a Ledge instead of on the ground
            if (hit.point.y > transform.position.y - PlayerHeight / 2 + 0.3f)
            {
                isNextToALedge = true;
            }
        }
        else
        {
            isGrounded = false;
            isNextToALedge = false;
        }

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

            if(isNextToALedge)
            {
                jump = false;
                jumpTimer = DefaultJumpTime + JumpBoosterDelay;
            }
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
    /// Move and Rotate Player
    /// </summary>
    /// <param name="_direction">The direction the player has to move towards</param>
    public void MovePlayer(Vector3 _direction)
    {
        Vector3 _movementValue = _direction * MovementSpeed * Time.deltaTime;

        if(_direction != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_direction), 10 * Time.deltaTime);

        rb.velocity = new Vector3(_movementValue.x, rb.velocity.y, _movementValue.z);
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
        if (!tagDetectorScript.isCollidingWithPlayer || tagDetectorScript.otherObject == null || transform.name != PlayerManager.instance.PlayerThatHoldsTheItem)
            return;
        
        RaycastHit _hit;

        Debug.DrawLine(transform.position, tagDetectorScript.otherObject.transform.position, debugHidden, 3);

        if (Physics.Linecast(transform.position, tagDetectorScript.otherObject.transform.position, out _hit, tagDetectionObstaclesLayer))
        {
            
            if (debugMode)
            {
                Debug.DrawLine(transform.position, _hit.point, debugHit, 3);
                Debug.DrawLine(_hit.point, tagDetectorScript.otherObject.transform.position, debugHidden, 3);
                Debug.Log(LayerMask.LayerToName(_hit.transform.gameObject.layer));
            }
            Debug.Log("blocked");
        }
        else
        {
            if (debugMode) { Debug.DrawLine(transform.position, tagDetectorScript.otherObject.transform.position, debugHit, 3); }

            Debug.Log("Item could be handed over");

            CmdHandOverItem(transform.name, tagDetectorScript.otherObject.name);
        }
    }

    [Command]
    public void CmdHandOverItem(string playerName, string otherPlayerName)
    {
        Debug.Log(playerName + " gave the item to " + otherPlayerName);

        //PlayerManager.instance.HandOverItemToPlayer(otherPlayerName);
        RpcHandOverPackage(otherPlayerName);
    }

    [ClientRpc]
    public void RpcHandOverPackage(string _playerName)
    {
        if (!PlayerManager.instance.Players.ContainsKey(_playerName))
            return;

        if (_playerName == PlayerManager.instance.PlayerThatHoldsTheItem)
            return;

        PlayerManager.instance.PlayerThatHoldsTheItem = _playerName;
        Debug.Log(PlayerManager.instance.PlayerThatHoldsTheItem);

        PlayerManager.instance.UpdatePackageVisuals();
    }
}
