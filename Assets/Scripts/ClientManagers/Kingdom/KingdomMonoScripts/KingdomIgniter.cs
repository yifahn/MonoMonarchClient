using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ClientManagers.Kingdom.KingdomMonoScripts
{
    public class KingdomIgniter : MonoBehaviour
    {

       

        void Start()
        {
            KingdomManager.Instance.KingdomMapGenerate();
        }



       
        ///// /////////////////////////////////////////////////////////////////////////

        //[Header("Camera Pan Margins")]
        //[SerializeField] private float panMarginHorizontal = 50f;
        //[SerializeField] private float panMarginVertical = 20f;

        //[Header("Camera Pan Speeds")]
        //[Tooltip("Horizontal pan speed (X axis)")]
        //[Range(48f, 192f)]
        //[SerializeField] private float panSpeedHorizontal = 96f;
        //[Tooltip("Vertical pan speed (Z axis)")]
        //[Range(24f, 96f)]
        //[SerializeField] private float panSpeedVertical = 48f;

        //[Header("Camera Zoom Speeds")]
        //[Tooltip("Zoom in speed (scroll up)")]
        //[Range(2400f, 9600f)]
        //[SerializeField] private float zoomInSpeed = 4800f;
        //[Tooltip("Zoom out speed (scroll down)")]
        //[Range(2400f, 9600f)]
        //[SerializeField] private float zoomOutSpeed = 4800f;

        //private Vector3 mapCenter;
        //private float minPanX, maxPanX, minPanZ, maxPanZ;
        //private float minY;
        //private float maxY;
        //private Vector3 dragOrigin;
        //private bool isDragging = false;

        //void Start()
        //{
        //    KingdomManager.Instance.KingdomMapGenerate();
        //    FitCameraToMap();

        //    minY = GetMapY();
        //    maxY = Camera.main.transform.position.y * 2f;
        //}

        //void Update()
        //{
        //    HandleCameraPan();
        //    HandleCameraZoom();
        //}

        //public void FitCameraToMap()
        //{
        //    float minX = 3f;
        //    float maxX = 180f;
        //    float minZ = -99f;
        //    float maxZ = -3f;
        //    float padding = 5f;

        //    float mapWidth = maxX - minX + padding * 2f;
        //    float mapHeight = Mathf.Abs(maxZ - minZ) + padding * 2f;

        //    mapCenter = new Vector3((minX + maxX) / 2f, 0f, (minZ + maxZ) / 2f);

        //    minPanX = minX - panMarginHorizontal;
        //    maxPanX = maxX + panMarginHorizontal;
        //    minPanZ = minZ - panMarginVertical;
        //    maxPanZ = maxZ + panMarginVertical;

        //    Camera cam = Camera.main;
        //    cam.orthographic = false;

        //    float fov = cam.fieldOfView;
        //    float aspect = (float)Screen.width / Screen.height;
        //    float halfMapSize = Mathf.Max(mapWidth / aspect, mapHeight) / 2f;
        //    float cameraHeight = halfMapSize / Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);

        //    cam.transform.position = mapCenter + new Vector3(0, cameraHeight, 0);
        //    cam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        //}

        //private float GetMapY()
        //{
        //    return 0f;
        //}

        //private void HandleCameraPan()
        //{
        //    if (Input.GetKey(KeyCode.LeftShift))
        //    {
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            dragOrigin = Input.mousePosition;
        //            isDragging = true;
        //        }
        //        if (Input.GetMouseButton(0) && isDragging)
        //        {
        //            Vector3 diff = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        //            Vector3 move = new Vector3(-diff.x * panSpeedHorizontal, 0, -diff.y * panSpeedVertical);

        //            Camera.main.transform.Translate(move, Space.World);

        //            Vector3 pos = Camera.main.transform.position;
        //            pos.x = Mathf.Clamp(pos.x, minPanX, maxPanX);
        //            pos.y = Mathf.Clamp(pos.y, 20f, 150f);
        //            pos.z = Mathf.Clamp(pos.z, minPanZ, maxPanZ);
        //            Camera.main.transform.position = pos;

        //            dragOrigin = Input.mousePosition;
        //        }
        //        if (Input.GetMouseButtonUp(0))
        //        {
        //            isDragging = false;
        //        }
        //    }
        //}

        //private void HandleCameraZoom()
        //{
        //    float scroll = Input.GetAxis("Mouse ScrollWheel");
        //    if (Mathf.Abs(scroll) > 0.01f)
        //    {
        //        Vector3 pos = Camera.main.transform.position;
        //        if (scroll > 0)
        //            pos.y -= scroll * zoomInSpeed * Time.deltaTime;
        //        else
        //            pos.y -= scroll * zoomOutSpeed * Time.deltaTime;

        //        pos.y = Mathf.Clamp(pos.y, 20f, 150f);
        //        pos.x = Mathf.Clamp(pos.x, minPanX, maxPanX);
        //        pos.z = Mathf.Clamp(pos.z, minPanZ, maxPanZ);
        //        Camera.main.transform.position = pos;
        //    }
        //}

        /////////////////////////////////////////////////



        //private float panMarginHorizontal = 50f; // Allow more horizontal panning
        //private float panMarginVertical = 20f;   // Allow less vertical panning
        //private Vector3 mapCenter;
        //private float minPanX, maxPanX, minPanZ, maxPanZ;

        //private float minY;
        //private float maxY;
        //private float panSpeed = 24f;
        //private float zoomSpeed = 2400f;
        //private Vector3 dragOrigin;
        //private bool isDragging = false;

        //void Start()
        //{
        //    KingdomManager.Instance.KingdomMapGenerate();
        //    FitCameraToMap();

        //    // Set zoom limits after camera is positioned
        //    minY = GetMapY(); // The y position of the map (usually 0)
        //    maxY = Camera.main.transform.position.y * 2f;
        //}

        //void Update()
        //{
        //    HandleCameraPan();
        //    HandleCameraZoom();
        //}

        //public void FitCameraToMap()
        //{
        //    float minX = 3f;
        //    float maxX = 180f;
        //    float minZ = -99f;
        //    float maxZ = -3f;
        //    float padding = 5f;

        //    float mapWidth = maxX - minX + padding * 2f;
        //    float mapHeight = Mathf.Abs(maxZ - minZ) + padding * 2f;

        //    mapCenter = new Vector3((minX + maxX) / 2f, 0f, (minZ + maxZ) / 2f);

        //    // Set pan limits based on map size and margins
        //    minPanX = minX - panMarginHorizontal;
        //    maxPanX = maxX + panMarginHorizontal;
        //    minPanZ = minZ - panMarginVertical;
        //    maxPanZ = maxZ + panMarginVertical;

        //    Camera cam = Camera.main;
        //    cam.orthographic = false;

        //    float fov = cam.fieldOfView;
        //    float aspect = (float)Screen.width / Screen.height;
        //    float halfMapSize = Mathf.Max(mapWidth / aspect, mapHeight) / 2f;
        //    float cameraHeight = halfMapSize / Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);

        //    cam.transform.position = mapCenter + new Vector3(0, cameraHeight, 0);
        //    cam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        //}

        //private float GetMapY()
        //{
        //    // If your map is at y=0, just return 0.
        //    // If not, calculate or fetch the actual y position of your map.
        //    return 0f;
        //}
        //private void HandleCameraPan()
        //{
        //    if (Input.GetKey(KeyCode.LeftShift))
        //    {
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            dragOrigin = Input.mousePosition;
        //            isDragging = true;
        //        }
        //        if (Input.GetMouseButton(0) && isDragging)
        //        {
        //            Vector3 diff = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        //            Vector3 move = new Vector3(-diff.x * panSpeed * 2f, 0, -diff.y * panSpeed);

        //            Camera.main.transform.Translate(move, Space.World);

        //            // Clamp camera position after panning
        //            Vector3 pos = Camera.main.transform.position;
        //            pos.x = Mathf.Clamp(pos.x, minPanX, maxPanX);
        //            pos.y = Mathf.Clamp(pos.y, 20f, 150f);
        //            pos.z = Mathf.Clamp(pos.z, minPanZ, maxPanZ);
        //            Camera.main.transform.position = pos;

        //            dragOrigin = Input.mousePosition;
        //        }
        //        if (Input.GetMouseButtonUp(0))
        //        {
        //            isDragging = false;
        //        }
        //    }
        //}

        //private void HandleCameraZoom()
        //{
        //    float scroll = Input.GetAxis("Mouse ScrollWheel");
        //    if (Mathf.Abs(scroll) > 0.01f)
        //    {
        //        Vector3 pos = Camera.main.transform.position;
        //        pos.y -= scroll * zoomSpeed * Time.deltaTime;
        //        pos.y = Mathf.Clamp(pos.y, 20f, 150f);
        //        // Clamp X and Z as well
        //        pos.x = Mathf.Clamp(pos.x, minPanX, maxPanX);
        //        pos.z = Mathf.Clamp(pos.z, minPanZ, maxPanZ);
        //        Camera.main.transform.position = pos;
        //    }
        //}
        //private void HandleCameraPan()
        //{
        //    if (Input.GetKey(KeyCode.LeftShift))
        //    {
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            dragOrigin = Input.mousePosition;
        //            isDragging = true;
        //        }
        //        if (Input.GetMouseButton(0) && isDragging)
        //        {
        //            Vector3 diff = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        //            Vector3 move = new Vector3(-diff.x * panSpeed, 0, -diff.y * panSpeed);

        //            Camera.main.transform.Translate(move, Space.World);
        //            dragOrigin = Input.mousePosition;
        //        }
        //        if (Input.GetMouseButtonUp(0))
        //        {
        //            isDragging = false;
        //        }
        //    }
        //}

        //private void HandleCameraZoom()
        //{
        //    float scroll = Input.GetAxis("Mouse ScrollWheel");
        //    if (Mathf.Abs(scroll) > 0.01f)
        //    {
        //        Vector3 pos = Camera.main.transform.position;
        //        pos.y -= scroll * zoomSpeed * Time.deltaTime;
        //        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        //        Camera.main.transform.position = pos;
        //    }
        //}
        ///////////////
        //void Start()
        //{
        //    KingdomManager.Instance.KingdomMapGenerate();
        //    FitCameraToMap();
        //}
        //public void FitCameraToMap()
        //{
        //    float minX = 3f;
        //    float maxX = 180f;
        //    float minZ = -99f;
        //    float maxZ = -3f;
        //    float padding = 5f;

        //    float mapWidth = maxX - minX;
        //    float mapHeight = Mathf.Abs(maxZ - minZ);

        //    float aspect = (float)Screen.width / Screen.height;
        //    float sizeBasedOnWidth = mapWidth / (2f * aspect);
        //    float sizeBasedOnHeight = mapHeight / 2f;

        //    Camera.main.orthographic = true;
        //    Camera.main.orthographicSize = Mathf.Max(sizeBasedOnWidth, sizeBasedOnHeight) + padding;
        //    Camera.main.transform.position = new Vector3((minX + maxX) / 2f, 100f, (minZ + maxZ) / 2f);
        //    Camera.main.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Top-down
        //}
        //public void FitCameraToMap()
        //{
        //    float minX = 3f;
        //    float maxX = 180f;
        //    float minZ = -99f;
        //    float maxZ = -3f;
        //    float padding = 5f;

        //    float mapWidth = maxX - minX + padding * 2f;
        //    float mapHeight = Mathf.Abs(maxZ - minZ) + padding * 2f;

        //    // Center of the map
        //    Vector3 center = new Vector3((minX + maxX) / 2f, 0f, (minZ + maxZ) / 2f);

        //    // Camera settings
        //    Camera cam = Camera.main;
        //    cam.orthographic = false;

        //    // Set camera angle (top-down, but tilted for perspective)
        //    float cameraAngle = 60f; // degrees from horizontal (30-60 is typical for strategy games)
        //    float radians = Mathf.Deg2Rad * cameraAngle;

        //    // Calculate required distance from center to fit the map vertically
        //    float halfHeight = mapHeight / 2f;
        //    float distance = halfHeight / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

        //    // Adjust for aspect ratio (width)
        //    float halfWidth = mapWidth / 2f;
        //    float aspect = (float)Screen.width / Screen.height;
        //    float distanceWidth = halfWidth / (Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * aspect);

        //    // Use the greater distance to ensure both width and height fit
        //    float finalDistance = Mathf.Max(distance, distanceWidth);

        //    // Set camera position (above the center, looking down at an angle)
        //    cam.transform.position = center + new Vector3(0, finalDistance * Mathf.Sin(radians), -finalDistance * Mathf.Cos(radians));
        //    cam.transform.LookAt(center);
        //}
        //public void FitCameraToMap()
        //{
        //    float minX = 3f;
        //    float maxX = 180f;
        //    float minZ = -99f;
        //    float maxZ = -3f;
        //    float padding = 5f;

        //    float mapWidth = maxX - minX + padding * 2f;
        //    float mapHeight = Mathf.Abs(maxZ - minZ) + padding * 2f;

        //    Vector3 center = new Vector3((minX + maxX) / 2f, 0f, (minZ + maxZ) / 2f);

        //    Camera cam = Camera.main;
        //    cam.orthographic = false;

        //    // Calculate required height to fit the map vertically in perspective
        //    float fov = cam.fieldOfView;
        //    float aspect = (float)Screen.width / Screen.height;

        //    // Use the larger of width or height to determine camera height
        //    float halfMapSize = Mathf.Max(mapWidth / aspect, mapHeight) / 2f;
        //    float cameraHeight = halfMapSize / Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);

        //    // Place camera directly above the center
        //    cam.transform.position = center + new Vector3(0, cameraHeight, 0);
        //    cam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        //}
    }
}

