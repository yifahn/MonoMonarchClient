using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingdomCameraController : MonoBehaviour
{
    [Header("Camera Pan Margins")]
    [SerializeField] private float panMarginHorizontal = 50f;
    [SerializeField] private float panMarginVertical = 20f;

    [Header("Camera Pan Speeds")]
    [Tooltip("Horizontal pan speed (X axis)")]
    [Range(48f, 192f)]
    [SerializeField] private float panSpeedHorizontal = 96f;
    [Tooltip("Vertical pan speed (Z axis)")]
    [Range(24f, 96f)]
    [SerializeField] private float panSpeedVertical = 48f;

    [Header("Camera Zoom Speeds")]
    [Tooltip("Zoom in speed (scroll up)")]
    [Range(2400f, 9600f)]
    [SerializeField] private float zoomInSpeed = 4800f;
    [Tooltip("Zoom out speed (scroll down)")]
    [Range(2400f, 9600f)]
    [SerializeField] private float zoomOutSpeed = 4800f;

    private Vector3 mapCenter;
    private float minPanX, maxPanX, minPanZ, maxPanZ;
    private float minY;
    private float maxY;
    private Vector3 dragOrigin;
    private bool isDragging = false;
    void Update()
    {
        HandleCameraPan();
        HandleCameraZoom();
    }
    void Start()
    {
        FitCameraToMap();
    }
    public void FitCameraToMap()
    {
        float minX = 3f;//needs dynamic map size handling
        float maxX = 180f;
        float minZ = -99f;
        float maxZ = -3f;
        float padding = 5f;//--

        float mapWidth = maxX - minX + padding * 2f;
        float mapHeight = Mathf.Abs(maxZ - minZ) + padding * 2f;

        mapCenter = new Vector3((minX + maxX) / 2f, 0f, (minZ + maxZ) / 2f);

        float aspect = (float)Screen.width / Screen.height;

        // Adjust pan margins based on aspect ratio
        panMarginHorizontal = 50f * Mathf.Lerp(0.7f, 1.5f, Mathf.InverseLerp(0.5f, 2.0f, aspect));
        panMarginVertical = 20f * Mathf.Lerp(1.5f, 0.7f, Mathf.InverseLerp(0.5f, 2.0f, aspect));

        minPanX = minX - panMarginHorizontal;
        maxPanX = maxX + panMarginHorizontal;
        minPanZ = minZ - panMarginVertical;
        maxPanZ = maxZ + panMarginVertical;

        Camera cam = Camera.main;
        cam.orthographic = false;

        float fov = cam.fieldOfView;
        float halfMapSize = Mathf.Max(mapWidth / aspect, mapHeight) / 2f;
        float cameraHeight = halfMapSize / Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);

        // --- Dynamic Zoom Clamping ---
        // Minimum Y: just enough to see the whole map (with a little padding)
        minY = cameraHeight * 0.7f; // allow zooming in a bit closer than "fit whole map"
                                    // Maximum Y: allow zooming out to see more, but not too far
        maxY = cameraHeight * 2.0f;

        cam.transform.position = mapCenter + new Vector3(0, cameraHeight, 0);
        cam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private void HandleCameraPan()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                isDragging = true;
            }
            if (Input.GetMouseButton(0) && isDragging)
            {
                Vector3 diff = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                Vector3 move = new Vector3(-diff.x * panSpeedHorizontal, 0, -diff.y * panSpeedVertical);

                Camera.main.transform.Translate(move, Space.World);

                Vector3 pos = Camera.main.transform.position;
                pos.x = Mathf.Clamp(pos.x, minPanX, maxPanX);
                pos.y = Mathf.Clamp(pos.y, minY, maxY);
                pos.z = Mathf.Clamp(pos.z, minPanZ, maxPanZ);
                Camera.main.transform.position = pos;

                dragOrigin = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }
    }

    private void HandleCameraZoom()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                Vector3 pos = Camera.main.transform.position;
                if (scroll > 0)
                    pos.y -= scroll * zoomInSpeed * Time.deltaTime;
                else
                    pos.y -= scroll * zoomOutSpeed * Time.deltaTime;

                pos.y = Mathf.Clamp(pos.y, minY, maxY);
                pos.x = Mathf.Clamp(pos.x, minPanX, maxPanX);
                pos.z = Mathf.Clamp(pos.z, minPanZ, maxPanZ);
                Camera.main.transform.position = pos;
            }
        }
    }
}
