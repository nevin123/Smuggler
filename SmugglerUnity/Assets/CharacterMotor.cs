using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour {

    public LayerMask CollisionMask;

    const float skinWidth = 0.015f;
    public int HorizontalRayCounts = 4;
    public int VerticalRaycounts = 5;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    public float maxClimbAngle = 60f;

    CapsuleCollider collider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity) {
        UpdateRaycastOrigins();
        collisions.Reset();
        HorizontalCollisions(ref velocity);
        VerticalCollisions(ref velocity);

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float directionZ = Mathf.Sign(velocity.z);
        float rayLengthX = Mathf.Abs(velocity.x) + skinWidth;
        float rayLengthZ = Mathf.Abs(velocity.z) + skinWidth;

        for (int y = 0; y < VerticalRaycounts; y++)
        {
            Vector3 rayOrigin = raycastOrigins.floor;
            rayOrigin += new Vector3((float)Mathf.Cos(verticalRaySpacing * y) * raycastOrigins.radius, 0f, (float)Mathf.Sin(verticalRaySpacing * y) * raycastOrigins.radius);

            for (int i = 0; i < HorizontalRayCounts; i++)
            {
                rayOrigin += (i == 0)?Vector3.zero:new Vector3(0, horizontalRaySpacing, 0);

                RaycastHit hitX;
                RaycastHit hitZ;

                if (Physics.Raycast(rayOrigin, Vector3.right * directionX, out hitX, rayLengthX, CollisionMask))
                {
                    float slopeAngle = Vector3.Angle(hitX.normal, Vector3.up);
                    Vector2 slopeDirection = new Vector2(hitX.normal.x, hitX.normal.z).normalized * -1f;
                    
                    if (i == 0 && slopeAngle <= maxClimbAngle && !collisions.climbingSlope)
                    {
                        float distanceToSlopeStart = 0;
                        if(slopeAngle != collisions.slopeAngleOld)
                        {
                            distanceToSlopeStart = hitX.distance - skinWidth;
                            velocity.x -= distanceToSlopeStart * directionX;
                        }
                        ClimbSlope(ref velocity, slopeAngle, slopeDirection);
                        velocity.x += distanceToSlopeStart * directionX;
                    }

                    if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                    {
                        velocity.x = (hitX.distance - skinWidth) * directionX;
                        rayLengthX = hitX.distance;

                        if(collisions.climbingSlope)
                        {
                            velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                        }
                    }
                }

                if (Physics.Raycast(rayOrigin, Vector3.forward * directionZ, out hitZ, rayLengthZ, CollisionMask))
                {
                    float slopeAngle = Vector3.Angle(hitZ.normal, Vector3.up);
                    Vector2 slopeDirection = new Vector2(hitZ.normal.x, hitZ.normal.z).normalized * -1f;

                    if (i == 0 && slopeAngle <= maxClimbAngle && !collisions.climbingSlope)
                    {
                        float distanceToSlopeStart = 0;
                        if (slopeAngle != collisions.slopeAngleOld)
                        {
                            distanceToSlopeStart = hitZ.distance - skinWidth;
                            velocity.z -= distanceToSlopeStart * directionZ;
                        }
                        ClimbSlope(ref velocity, slopeAngle, slopeDirection);
                        velocity.z += distanceToSlopeStart * directionZ;
                    }

                    if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                    {
                        velocity.z = (hitZ.distance - skinWidth) * directionZ;
                        rayLengthZ = hitZ.distance;

                        if (collisions.climbingSlope)
                        {
                            velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.z);
                        }
                    }
                }

                Debug.DrawRay(rayOrigin, Vector3.right * directionX * rayLengthX, Color.red);
                Debug.DrawRay(rayOrigin, Vector3.forward * directionZ * rayLengthZ, Color.red);
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < VerticalRaycounts; i++)
        {
            Vector3 rayOrigin = (directionY == -1) ? raycastOrigins.floor : raycastOrigins.top;
            rayOrigin += new Vector3((float)Mathf.Cos(verticalRaySpacing * i) * raycastOrigins.radius, 0f, (float)Mathf.Sin(verticalRaySpacing * i) * raycastOrigins.radius);
            RaycastHit hit;
                
            if(Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, CollisionMask)) {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if(collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * (Mathf.Sign(velocity.x) * velocity.x);
                    velocity.z = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * (Mathf.Sign(velocity.z) * velocity.z);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }

            Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength, Color.red);
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle, Vector2 slopeDirection)
    {
        Vector2 playerDirection = new Vector2(velocity.x, velocity.z).normalized;

        float slopeDirectionAngle = Vector2.Angle(new Vector2(1, 0), slopeDirection);
        float playerDirectionAngle = Vector2.Angle(new Vector2(1, 0).normalized, playerDirection);

        float _dirValue = Mathf.Abs(slopeDirectionAngle - playerDirectionAngle) * Mathf.Deg2Rad;

        float moveDistance = Mathf.Abs(velocity.x) + Mathf.Abs(velocity.z);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY * Mathf.Clamp01(Mathf.Abs(Mathf.Cos(_dirValue)));

            float _newMoveDistance = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance;

            velocity.x = _newMoveDistance * playerDirection.x;
            velocity.z = _newMoveDistance * playerDirection.y;

            Debug.Log(playerDirection);

            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void UpdateRaycastOrigins()
    {
        raycastOrigins.height = Mathf.Abs(collider.bounds.min.y - collider.bounds.max.y) - skinWidth * 2;
        raycastOrigins.radius = Mathf.Abs(collider.bounds.min.x - collider.bounds.max.x)/2 - skinWidth;
        raycastOrigins.center = collider.bounds.center;
        raycastOrigins.floor = new Vector3(collider.bounds.center.x, collider.bounds.min.y + skinWidth, collider.bounds.center.z);
        raycastOrigins.top = new Vector3(collider.bounds.center.x, collider.bounds.max.y - skinWidth, collider.bounds.center.z);
    }

    void CalculateRaySpacing()
    {
        HorizontalRayCounts = Mathf.Clamp(HorizontalRayCounts, 4, int.MaxValue);
        VerticalRaycounts = Mathf.Clamp(VerticalRaycounts, 4, int.MaxValue);

        horizontalRaySpacing = (Mathf.Abs(collider.bounds.min.y - collider.bounds.max.y) - skinWidth * 2) / (HorizontalRayCounts-1);
        verticalRaySpacing = 2f * Mathf.PI / VerticalRaycounts;
    }

    struct RaycastOrigins
    {
        public float height;
        public float radius;
        public Vector3 center;
        public Vector3 floor;
        public Vector3 top;
    }

    public struct CollisionInfo
    {
        public bool above, below;

        public bool climbingSlope;
        public float slopeAngle, slopeAngleOld;

        public void Reset()
        {
            above = below = false;

            climbingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
