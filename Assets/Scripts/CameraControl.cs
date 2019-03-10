using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;
    private GameObject leftBound;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    public void Adjust(Vector3 shift)
    {
        transform.position = transform.position + shift;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (player.transform.position.x > 0)
        //{
        //    Vector3 temp = player.transform.position;
        //    temp.y = 0;
        //    transform.position = temp + offset;
        //}
    }
}
