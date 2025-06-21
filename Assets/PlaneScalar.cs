using UnityEngine;

public class PlaneScaler : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Transform planeTransform;
    [SerializeField] private float planeY = 0f; // Y position of the plane

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
        if (planeTransform == null)
            planeTransform = transform;

        ScalePlaneToCamera();
    }

    void ScalePlaneToCamera()
    {
        float distance = Mathf.Abs(targetCamera.transform.position.y - planeY);
        float fov = targetCamera.fieldOfView;
        float aspect = (float)Screen.width / Screen.height;

        // Calculate the world height and width at the plane's Y, then double it
        float worldHeight = 2f * distance * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) * 2f;
        float worldWidth = worldHeight * aspect;

        // Unity's default plane is 10x10 units, so scale accordingly
        planeTransform.position = new Vector3(planeTransform.position.x, planeY, planeTransform.position.z);
        planeTransform.localScale = new Vector3(worldWidth / 10f, 1f, worldHeight / 10f);
    }
}