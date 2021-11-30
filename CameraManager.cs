using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject cameraHolder;

    public float cameraSpeed = 3.0f;

    Vector2 minMaxX = new Vector2(0, 100);
    Vector2 minMaxZ = new Vector2(-130, -50);

    float zoomPercent;
    float camZoom = 50;
    Vector2 minMaxZoom = new Vector2(10, 35);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CamMove();

        CamZoom();
    }

    void CamMove(bool upDown = false)
    {
        float y = 0;
        if (upDown)
        {
            y = Input.GetKey(KeyCode.Space) ? 0.6f : 0;
            y -= Input.GetKey(KeyCode.LeftShift) ? 0.6f : 0;
        }


        Vector3 input = new Vector3(
            (Input.GetAxis("Horizontal")),
            y,
            Input.GetAxis("Vertical")
        );

        //var cap = Mathf.Clamp(cameraHolder.transform.position.y / 65, 0.2f, 1f);

        cameraHolder.transform.Translate(input * cameraSpeed, Space.Self);

        //var yCap = Mathf.Clamp(cameraHolder.transform.position.y, minMaxHeight.x, minMaxHeight.y);

        cameraHolder.transform.position = new Vector3(
            Mathf.Clamp(cameraHolder.transform.position.x, minMaxX.x, minMaxX.y),
            cameraHolder.transform.position.y,
            Mathf.Clamp(cameraHolder.transform.position.z, minMaxZ.x, minMaxZ.y));
    }

    void CamZoom()
    {
        zoomPercent += Input.GetAxis("Mouse ScrollWheel") * 3;

        if (Input.GetKey(KeyCode.PageDown))
        {
            zoomPercent += 0.1f;
        }
        if (Input.GetKey(KeyCode.PageUp))
        {
            zoomPercent -= 0.1f;
        }

        zoomPercent = Mathf.Clamp01(zoomPercent);

        camZoom = minMaxZoom.y - zoomPercent * (minMaxZoom.y - minMaxZoom.x);

        GetComponent<Camera>().fieldOfView = camZoom;
    }
}
