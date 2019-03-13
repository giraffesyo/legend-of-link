using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    private float time;
    private float t;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        t = Mathf.Sin(time);
        if (t > 0)
        {
            this.transform.position += Vector3.right * Time.deltaTime * speed;
        }
        else
        {
            this.transform.position += Vector3.left * Time.deltaTime * speed;
        }

    }
}
