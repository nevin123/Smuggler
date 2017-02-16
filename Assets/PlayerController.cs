﻿using UnityEngine;

[RequireComponent (typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    PlayerMotor Motor;

    bool jump;
    float jumpTimer;

    void Start()
    {
        Motor = GetComponent<PlayerMotor>();

        jumpTimer = Motor.DefaultJumpTime + Motor.JumpBoosterDelay;
    }

	void FixedUpdate()
    {
        //Move Player
        Motor.MovePlayer(Input.GetAxis("Horizontal"));

        //Jump
        if (Input.GetButton("Jump"))
        {
            //Down jump button
            if (Motor.isGrounded && !jump)
            {
                jump = true;
                Motor.Jump(0, true);
            }
        } else 
        {
            //Release jump button
            jump = false;
            jumpTimer = Motor.DefaultJumpTime + Motor.JumpBoosterDelay;
        }
        
        //Jump
        if (jump)
        {
            float _deltaTime = jumpTimer / Motor.DefaultJumpTime;

            jumpTimer -= Time.fixedDeltaTime;

            //Wait for delaytime
            if(jumpTimer < Motor.DefaultJumpTime)
                Motor.Jump(_deltaTime, false);

            //Stop when time is over
            if (jumpTimer <= 0)
            {
                jump = false;
                jumpTimer = Motor.DefaultJumpTime + Motor.JumpBoosterDelay;
            }
        }
    }
}