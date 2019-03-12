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
    private bool teleport;
    private Vector3 teleportVector;
    private bool cameraLeft;
    public bool freezeCamera;
    static float t = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        freezeCamera = false;
        teleport = false;
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
        if (!freezeCamera)
        {
            if (!cameraLeft && shift.x < 0)
            {
                shift.x = shift.x * -.25f;
            }
            transform.position = transform.position + shift;
        }
        //ChangeBackground(shift);
    }

    public void Teleport(Vector3 teleportVector)
    {
        this.teleportVector = teleportVector;
        teleport = true;
    }

    public void CamMode(bool left)
    {
        cameraLeft = left;
    }

    public void ChangeBackground(Vector3 shift)
    {
        float delta = shift.x / cam.pixelWidth * timeSpeed;
        currentColor.r = currentColor.r - baseColor.r / 10 * delta;
        currentColor.g = currentColor.g - baseColor.g / 10 * delta;
        currentColor.b = currentColor.b - baseColor.b / 10 * delta;
        cam.backgroundColor = currentColor;
    }

    public void AffixCamera()
    {
        freezeCamera = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (teleport)
        {
            cam.transform.position += teleportVector;
            teleport = false;
        }
        else if (freezeCamera)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, 411, t), Mathf.Lerp(transform.position.y, 9, t), transform.position.z);
            t += 0.020f * Time.deltaTime;
        }
    }
}
