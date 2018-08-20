using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : PhysicsObject {

    private float gsp = 0;
    public float acc = 2;
    public float dec = 20;
    public float frc = 8;
    public float topSpeed = 50;
    public float rotSpd = 5;
    private GameObject sprite;

    void OnEnable ()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.GetChild(0).gameObject;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (gsp > 0) gsp -= dec * Time.deltaTime;
            else gsp -= acc * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (gsp < 0) gsp += dec * Time.deltaTime;
            else gsp += acc * Time.deltaTime;
        }
        else
        {
            if (Mathf.Abs(gsp) < frc) gsp = 0;
            else gsp -= frc * Mathf.Sign(gsp) * Time.deltaTime;
        }

        if (Mathf.Abs(gsp) > topSpeed) gsp = topSpeed * Mathf.Sign(gsp);

        if (grounded) targetV = groundNormal * grv;
        else
        {
            targetV += Vector2.up * grv * Time.deltaTime;
            groundNormal = new Vector2(0, 1);
        }
            


        target.x = gsp;
        //if (Mathf.Abs(groundNormal.x) > .5f) transform.up = new Vector2(Mathf.Sign(groundNormal.x), 0);
        //else if (Mathf.Abs(groundNormal.x) < .8f) transform.up = new Vector2(0, -1);
        sprite.transform.up = Vector2.Lerp(new Vector2(sprite.transform.up.x, sprite.transform.up.y), groundNormal, rotSpd * Time.deltaTime);


    }
}
