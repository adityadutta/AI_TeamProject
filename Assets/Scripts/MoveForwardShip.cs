using UnityEngine;
using System.Collections;

public class MoveForwardShip : MonoBehaviour {

    public float maxYSpeed = 5.0f;
    public float maxXSpeed;

    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    public float timeRangemin;
    public float timeRangeMax;
    private float timeRange;
    private float timer;

    Vector3 velocity;

    private void Start()
    {
        timeRange = Random.Range(timeRangemin, timeRangeMax);
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update() {

        //Move ship forwards
        Vector3 pos = transform.position;
        velocity = new Vector3(maxXSpeed * Time.deltaTime, maxYSpeed * Time.deltaTime, 0);
        pos += transform.rotation * velocity;
        transform.position = pos;


        Debug.Log("X Location: " + transform.position.x + " | Y Location: " + transform.position.y);

        if(transform.position.y < yMin)
        {
            ResetYLocation();
        }

        //Check the location of the ship in the x location
        if (xMin > transform.position.x || xMax < transform.position.x)
        {
            ResetXDirection();
        }

        //Update timer for location
        timer += Time.deltaTime;

        if (timer > timeRange)
        {
            ResetXDirection();
        }

    }

    void ResetYLocation()
    {
        Vector3 newPos = new Vector3(transform.position.x, yMax, 0.0f);
        transform.position = newPos;
    }
    

    void ResetXDirection()
    {
        maxXSpeed *= -1.0f;
        timer = 0.0f;
        timeRange = Random.Range(timeRangemin, timeRangeMax);
    }


    public Vector3 GetVelocity()
    {
        return velocity;
    }


}
