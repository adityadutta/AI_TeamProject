using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntercepterMissile : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public float maxAngularSpeed;
    Vector3 velocity = Vector3.zero;
    Vector3 orientation = Vector3.up;

    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Enemy")[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            float dt = Time.deltaTime;

            Vector3 targetVelocity = target.gameObject.GetComponent<MoveForwardShip>().GetVelocity();
            Vector3 vr = targetVelocity - velocity;
            Vector3 sr = target.transform.position - transform.position;
            float tc = sr.magnitude / vr.magnitude;
            Vector3 st = target.transform.position + targetVelocity * tc;

            Vector3 dir = st - transform.position;
            dir.Normalize();


            float dotProduct = Vector3.Dot(dir, orientation.normalized);
            float desiredAngle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg; // desired angle in degrees
            float actualAngle = maxAngularSpeed * dt; // actual angle in degrees


            if (desiredAngle > actualAngle)
            {
                Vector3 rightNormal = new Vector3(orientation.y, -orientation.x, 0);
                bool isRightDir = Vector3.Dot(rightNormal, dir) > 0;
                if (isRightDir)
                {
                    actualAngle = -actualAngle;
                }
                orientation = Quaternion.Euler(0, 0, actualAngle) * orientation;
            }
            else
            {
                orientation = dir;
            }
            UpdateOrientation();

            Vector3 pos = transform.position;
            velocity = orientation * speed;
            pos += velocity * dt;

            transform.position = pos;
        }
        else
        {
            target = GameObject.FindWithTag("Enemy");
        }
    }

    void UpdateOrientation()
    {
        Vector3 angle = new Vector3(0, 0, -Mathf.Atan2(orientation.x, orientation.y) * Mathf.Rad2Deg);
        transform.eulerAngles = angle;
    }
}
