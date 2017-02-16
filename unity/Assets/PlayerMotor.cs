using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour {

    [Header("Movement Values")]
    public float MovementSpeed = 50;
    public float JumpStrength = 10;
    public float DefaultJumpTime = 1;
    public float JumpBoosterDelay = 0.2f;

    Rigidbody2D rb;

    [HideInInspector]
    public bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Move Player
    /// </summary>
    /// <param name="_direction">The direction the player has to move towards</param>
    public void MovePlayer(float _direction)
    {
        float _movementValue = _direction * MovementSpeed * Time.deltaTime;
        rb.velocity = new Vector2(_movementValue, rb.velocity.y);
    }

    /// <summary>
    /// Make the player Jump
    /// </summary>
    public void Jump(float _deltaTime, bool _start)
    {
        if (_start)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpStrength);
        } else
        {
            rb.velocity = rb.velocity + new Vector2(0, JumpStrength * 2 * _deltaTime * Time.fixedDeltaTime);
        }
        
        Debug.Log(rb.velocity.y);
    }

    /// <summary>
    /// On Collision Enter
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter2D(Collision2D other)
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
    void OnCollisionExit2D(Collision2D other)
    {
        //Reset is grounded
        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            isGrounded = false;
        }
    }
}
