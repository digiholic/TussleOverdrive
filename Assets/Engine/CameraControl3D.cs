using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl3D : MonoBehaviour {
    public List<Transform> follows;

    public float padding = 8.0f; //The amount of space to add around the fighters
    public float xAdjust = 0.0f; //The amount to shift the camera left or right
    public float yAdjust = 1.5f; //The amount to shift the camera up or down

    public float minDist = 2.0f;
    public float dampTime = 0.2f;

    public bool angle_camera = true;
    public float camera_angle_speed = 1.0f;
    private Camera m_Camera;
    private Vector3 moveVelocity;

    public static CameraControl3D current_camera = null;

	// Use this for initialization
	void Awake () {
        current_camera = this;
        m_Camera = GetComponentInChildren<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void TrackObjects()
    {
        Vector3 center_point = GetCenterPoint();
        if (angle_camera)
        {
            Vector3 world_center = center_point;
            world_center.z = 0;
            Quaternion targetRotation = Quaternion.LookRotation(world_center - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, camera_angle_speed * Time.deltaTime);
        } else
        {
            transform.eulerAngles = Vector3.zero;
        }
        transform.position = Vector3.SmoothDamp(transform.position, center_point, ref moveVelocity, dampTime);
    }

    private Vector3 GetCenterPoint()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        // Go through all the targets and add their positions together.
        for (int i = 0; i < follows.Count; i++)
        {

            // If the target isn't active, go on to the next one.
            if (!follows[i].gameObject.activeSelf)
                continue;

            // Add to the average and increment the number of targets in the average.
            averagePos += follows[i].position;
            numTargets++;
        }

        // If there are targets divide the sum of the positions by the number of them to find the average.
        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.x += xAdjust;
        averagePos.y += yAdjust;

        float distance = RequiredHeight() * 0.5f / Mathf.Tan(m_Camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        distance = Mathf.Max(distance, minDist); //Clamp it to the given minimum distance

        averagePos.z = -distance;

        return averagePos;
    }

    private float RequiredHeight()
    {
        float minX = Mathf.Infinity;
        float maxX = -Mathf.Infinity;
        float minY = Mathf.Infinity;
        float maxY = -Mathf.Infinity;

        foreach (Transform current_object in follows)
        {
            minX = Mathf.Min(minX, current_object.position.x);
            maxX = Mathf.Max(maxX, current_object.position.x);

            minY = Mathf.Min(minY, current_object.position.y);
            maxY = Mathf.Max(maxY, current_object.position.y);
        }

        float diffX = (maxX - minX) + padding;
        float diffY = (maxY - minY) + padding;

        float widthPreferredHeight = diffX / m_Camera.aspect; //The height required for the width the be what it wants to be

        return Mathf.Max(widthPreferredHeight, diffY); //If the width demands more height than the height differential, return it. Otherwise, return the height differential.
    }
}
