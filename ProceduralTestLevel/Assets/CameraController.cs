using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CollisionHandler collisionHandler = new CollisionHandler();

    public Vector3 origin;

    public bool drawDesiredCameraLines = true;
    public bool drawadjustedCameraLines = true;

    // Start is called before the first frame update
    private void OnEnable()
    {
        origin = transform.parent.parent.position;

        collisionHandler.Initialize(Camera.main);
        collisionHandler.UpdateCameraClipPoints(transform.position, transform.rotation, ref collisionHandler.adjustedCameraClipPoints);
        collisionHandler.UpdateCameraClipPoints(origin, transform.rotation, ref collisionHandler.desiredCameraClipPoints);
    }

    private void FixedUpdate()
    {
        origin = transform.parent.parent.position;

        collisionHandler.UpdateCameraClipPoints(transform.position, transform.rotation, ref collisionHandler.adjustedCameraClipPoints);
        collisionHandler.UpdateCameraClipPoints(origin, transform.rotation, ref collisionHandler.desiredCameraClipPoints);

        collisionHandler.CheckColliding(origin);

        if (collisionHandler.colliding)
        {

        }

        for (int i = 0; i < 5; i++)
        {
            if (drawDesiredCameraLines)
            {
                Debug.DrawLine(transform.forward, collisionHandler.desiredCameraClipPoints[i], Color.green);
            }
            if (drawadjustedCameraLines)
            {
                Debug.DrawLine(transform.forward, collisionHandler.adjustedCameraClipPoints[i], Color.white);
            }
        }
    }

    public class CollisionHandler
    {
        public LayerMask collisionLayer;

        public bool colliding = false;

        public Vector3[] adjustedCameraClipPoints;
        public Vector3[] desiredCameraClipPoints;

        Camera camera;

        public void Initialize(Camera c)
        {
            // Set the camera
            camera = c;

            // The 4 clip points on the near clip plane and the cameras position
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        // Updates the Clip points
        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] inArray)
        {
            // Return is we dont have a camera
            if (!camera)
                return;

            // Clear the inArray
            inArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 3.41f) * z;
            float y = x / camera.aspect;

            // Top left
            // Adds and rotates the point relative to the camera
            inArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition;

            // Top right
            // Adds and rotates the point relative to the camera
            inArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition;

            // Bottom left
            // Adds and rotates the point relative to the camera
            inArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition;

            // Botton right
            // Adds and rotates the point relative to the camera
            inArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition;

            // camera position
            inArray[4] = cameraPosition = camera.transform.forward;
        }

        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                // Look for collision 
                Ray ray = new Ray(fromPosition, clipPoints[i] = fromPosition);
                float distance = Vector3.Distance(clipPoints[i], fromPosition);
                if (Physics.Raycast(ray, distance, collisionLayer))
                {
                    return true;
                }
            }

            return false;
        }

        // Returns the distance the camera needs to be from your target
        public float GetAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1;

            for (int i = 0; i < desiredCameraClipPoints.Length; i++)
            {
                Ray ray = new Ray(from, desiredCameraClipPoints[i]);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (distance == -1)
                        distance = hit.distance;
                    else
                    {
                        if (hit.distance < distance)
                            distance = hit.distance;
                    }
                }
            }

            if (distance == -1)
                return 0;
            else return distance;
        }

        public void CheckColliding(Vector3 target)
        {
            if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, target))
            {
                colliding = true;
            }
            else
            {
                colliding = false;
            }
        }
    }
}
