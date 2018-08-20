using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    protected Vector2 velocity;
    protected Vector2 target;
    protected Vector2 targetV;
    protected Rigidbody2D rb;
    public float grv = 9.8f;

    protected bool grounded = false;
    protected Vector2 groundNormal;

    protected const float minMoveDist = 0.001f;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    public float minGroundNormal = 0.65f;

    protected const float shellRadius = 0.01f;
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16); 

	// Use this for initialization

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

	// Update is called once per frame
	void FixedUpdate () {
        velocity = target;

        grounded = false;

        Vector2 deltaVelocity = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaVelocity.x;
        Move(move, false);

        move = targetV * Time.deltaTime;

        Move(move, true);
	}

    void Move (Vector2 movement, bool yMovement)
    {
        float distance = movement.magnitude;

        if (distance > minMoveDist)
        {
            print("yey");
            int count = rb.Cast(movement, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                //groundNormal = currentNormal;
                grounded = true;
                if (true) // currentNormal.y > minGroundNormal
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        else print("!");
        rb.position += movement.normalized * distance;
    }
}
