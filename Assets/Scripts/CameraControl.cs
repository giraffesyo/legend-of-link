using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    public int timeSpeed;

    private Vector3 offset;
    private GameObject leftBound;
    private Camera cam;
    private Color baseColor;
    private bool darker;
    private Color currentColor;

    // Start is called before the first frame update
    void Start()
    {
        darker = true;
        cam = GetComponent<Camera>();
        Color baseColor = new Color(0, 198, 255);
        offset = transform.position - player.transform.position;
    }

    private void Update()
    {
        /*
        float delta = Time.deltaTime/100 * timeSpeed;
        if (darker)
        {
            currentColor.r -= baseColor.r * delta;
            currentColor.g -= baseColor.g * delta;
            currentColor.b -= baseColor.b * delta;
        }
        else
        {
            currentColor.r += baseColor.r * delta;
            currentColor.g += baseColor.g * delta;
            currentColor.b += baseColor.b * delta;
        }
        cam.backgroundColor = currentColor;
        if (currentColor.b <= 0.001)
        {
            darker = false;
        }
        else if (currentColor.b >= 253.999)
        {
            darker = true;
        }
        */
    }

    public void Adjust(Vector3 shift)
    {
        transform.position = transform.position + shift;
        //ChangeBackground(shift);
    }

    public void ChangeBackground(Vector3 shift)
    {
        float delta = shift.x / cam.pixelWidth * timeSpeed;
        currentColor.r = currentColor.r - baseColor.r / 10 * delta;
        currentColor.g = currentColor.g - baseColor.g / 10 * delta;
        currentColor.b = currentColor.b - baseColor.b / 10 * delta;
        cam.backgroundColor = currentColor;
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
