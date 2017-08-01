using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class Player : MonoBehaviour {

    public float JumpHeight = 2f;
    public float TimeToJump = .5f;
    public float MoveSpeed = 4f;
    public float AccelerationTimeGrounded = .1f;
    public float AccelerationTimeAirborne = 0.3f;

    float jumpVelocity;
    float gravity;
    Vector3 velocity;

    float velocityXSmoothing;
    float velocityZSmoothing;

    CharacterMotor motor;

	void Start () {
        motor = GetComponent<CharacterMotor>();

        gravity = -(2 * JumpHeight) / Mathf.Pow(TimeToJump, 2f);
        jumpVelocity = Mathf.Abs(gravity) * TimeToJump;

        print("Gravity: " + gravity + "\n Jump Velocity: " + jumpVelocity);
	}

    void Update()
    {
        if(motor.collisions.above || motor.collisions.below)
        {
            velocity.y = 0;
        }

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetButton("Jump") && motor.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        Vector2 targetVelocity = new Vector2(input.x * MoveSpeed, input.z * MoveSpeed);

        velocity.x = Mathf.SmoothDamp(velocity.x , targetVelocity.x, ref velocityXSmoothing, (motor.collisions.below)?AccelerationTimeGrounded:AccelerationTimeAirborne);
        velocity.z = Mathf.SmoothDamp(velocity.z, targetVelocity.y, ref velocityZSmoothing, (motor.collisions.below) ? AccelerationTimeGrounded : AccelerationTimeAirborne);
        velocity.y += gravity * Time.fixedDeltaTime;

        motor.Move(velocity * Time.fixedDeltaTime);
    }
}
