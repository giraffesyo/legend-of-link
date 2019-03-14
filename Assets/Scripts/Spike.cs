using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float waitTime, moveSpeed, moveHeight, startDelay;
    private int state = 0;
    private float timeNext, basePos;

    // Start is called before the first frame update
    void Start()
    {
        timeNext = Time.time + startDelay;
        basePos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < timeNext)
        {
          return;
        }

        switch (state) // will cycle between moving up, moving down, or sitting still in either position
        {
          case 0:
            if (transform.position.y + moveSpeed * Time.deltaTime > basePos + moveHeight) // if desired height reached, then stop
            {
              transform.position = new Vector3(transform.position.x, basePos + moveHeight, 0); // set height to desired
              timeNext = Time.time + waitTime; // update time to wait for
              state = 1; // progress to next step
            }
            else // move in direction of desired height
            {
              transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            }
            break;
          case 1:
            if (transform.position.y - moveSpeed * Time.deltaTime < basePos) // if desired height reached, then stop
            {
              transform.position = new Vector3(transform.position.x, basePos, 0); // set height to desired
              timeNext = Time.time + waitTime; // update time to wait for
              state = 0; // progress to next step
            }
            else // move in direction of desired height
            {
              transform.position += new Vector3(0, -moveSpeed * Time.deltaTime, 0);
            }
            break;
        }
    }


}
