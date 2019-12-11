using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 acceleration = Vector3.zero;
    static float desiredSeparion = 1.5f;
    static float neighborDistance = 3f;
    float maxForce = 1f;
    float maxSpeed = 2f;
    public Flock flock;

    float maxX = 15.0f;
    float maxY = 6f;

    // Use this for initialization
    void Start()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 sep = Separate(flock.GetBoids());
        Vector3 ali = Align(flock.GetBoids());
        Vector3 coh = Cohesion(flock.GetBoids());

        acceleration = Vector3.zero;
        acceleration += sep * 1.5f;
        acceleration += ali;
        acceleration += coh;

        Vector3 avoid = AvoidObstacles(flock.GetObstacles());
        acceleration += avoid;


        float dt = Time.deltaTime;
        dt *= 5;

        Vector3 pos = transform.position;
        pos += velocity * dt + 0.5f * acceleration * dt * dt;
        velocity += acceleration * dt;
        acceleration = Vector3.zero;

        if (pos.x > maxX)
            velocity.x *= -1f;
        if (pos.x < -maxX)
            velocity.x *= -1f;
        if (pos.y > maxY)
            velocity.y *= -1f;
        if (pos.y < -maxY)
            velocity.y *= -1f;

        transform.position = pos;


        float angle = Mathf.Acos(Vector3.Dot(velocity.normalized, Vector3.up));

        transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle * (velocity.x > 0 ? -1 : 1));
    }

    Vector3 Separate(List<GameObject> boids)
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (GameObject other in boids)
        {
            if (!other) break;

            if (other == gameObject) continue;

            float d = Vector3.Distance(transform.position, other.transform.position);
            if ((d > 0) && (d < desiredSeparion))
            {
                // Calculate vector pointing away from neighbor
                Vector3 diff = transform.position - other.transform.position;
                diff.Normalize();
                diff /= d;
                steer += diff;
                count++;
            }
        }
        if (count > 0)
        {
            steer /= count;
        }

        if (steer.magnitude > 0)
        {
            steer.Normalize();
            steer *= maxSpeed;
            steer -= velocity;
            if (steer.magnitude > maxForce)
            {
                steer.Normalize();
                steer *= maxForce;
            }
        }
        return steer;
    }

    Vector3 Align(List<GameObject> boids)
    {
        // For every nearby boid in the system, calculate the average velocity
        Vector3 sum = Vector3.zero;
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (GameObject other in boids)
        {
            if (!other) break;

            if (other == gameObject) continue;

            float d = Vector3.Distance(transform.position, other.transform.position);
            if ((d > 0) && (d < neighborDistance))
            {
                // To do : Calculate the sum of all the velocities of the boids                
                sum += other.GetComponent<Boid>().velocity;
                count++;
            }
        }

        if (count > 0)
        {
            // To do: Implement Rynolds: Steering = Desired - Velocity
            // The magnitude of steer should not be greater than maxForce
            sum = sum.normalized;
            sum *= maxSpeed;
            steer = sum - velocity;
            if (steer.magnitude > maxForce)
            {
                steer.Normalize();
                steer *= maxForce;
            }
        }

        return steer;
    }

    Vector3 Cohesion(List<GameObject> boids)
    {
        // For the average position (i.e. center) of all nearby boids, calculate steering vector towards that position
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (GameObject other in boids)
        {
            if (!other) break;

            if (other == gameObject) continue;

            float d = Vector3.Distance(transform.position, other.transform.position);
            if ((d > 0) && (d < neighborDistance))
            {
                // To do : Calculate the sum of all the positions of the boids    
                sum += other.transform.position;
                count++;
            }
        }

        if (count > 0)
        {
            Vector3 averagePos = Vector3.zero;
            // To do: Calculate the average position and steer toward the position
            sum /= count;
            return Seek(sum);
        }

        return Vector3.zero;
    }



    Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - transform.position;
        desired.Normalize();
        desired /= maxSpeed;

        Vector3 steer = desired - velocity;
        if (steer.magnitude > maxForce)
        {
            steer.Normalize();
            steer *= maxForce;
        }

        return steer;
    }

    Vector3 AvoidObstacles(List<GameObject> obstacles)
    {
        Vector3 steer = Vector3.zero;
        float collision_visibilty = 20;
        float obstacle_radius = 5;

        foreach (GameObject obstacle in obstacles)
        {
            if (!obstacle) break;

            Vector3 a = obstacle.transform.position - transform.position;
            Vector3 u = velocity.normalized;
            Vector3 v = u * collision_visibilty;
            Vector3 p = Vector3.Dot(a, u) * u;
            Vector3 b = p - a;

            if ((b.magnitude < obstacle_radius) && (p.magnitude < v.magnitude))
            {
                Vector3 n = new Vector3(a.y, -a.x, 0);
                Vector3 desired = Vector3.zero;
                float dir = Vector3.Dot(n, v);
                steer = n.normalized * maxSpeed * dir / Mathf.Abs(dir);
                if (steer.magnitude > maxForce)
                {
                    steer.Normalize();
                    steer *= maxForce;
                }
            }
        }

        return steer;
    }
}
