using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameControls;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    [SerializeField]
    public Transform cam;

    [Header("Rig")]
    [SerializeField]
    private Vector3 offset = new Vector3(0,0, -10f);


    [Header("Zoom")]
    [SerializeField]
    private float minLocalZ = -10f;
    [SerializeField]
    private float maxLocalZ = 5f;
    [SerializeField]
    private float zoomSpeed = 15f;


    [Header("Orbit")]
    [SerializeField]
    private float orbitSpeed = 15f;
    [SerializeField]
    private Vector2 longitudeClamp = new Vector2(-90, 90);

    private GameControls controls;



    private void Awake()
    {
        controls = new GameControls();

        // Calculate Initial Position
        Renderer targetRenderer = target.GetComponent<Renderer>();
        Vector3 initialPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z - targetRenderer.bounds.size.z * 0.5f + offset.z);
        transform.position = initialPosition;
        cam.LookAt(target);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        Zoom(controls.Camera.Zoom.ReadValue<float>());
        Movement(controls.Camera.Movement.ReadValue<Vector2>());

        cam.LookAt(target);
    }


    private void Zoom(float delta)
    {
        float z = cam.transform.localPosition.z + delta * zoomSpeed * Time.deltaTime;
        z = Mathf.Clamp(z, minLocalZ, maxLocalZ);

        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, z);
    }

    private float currentVerticalRotation = 0f;
    private void Movement(Vector2 delta)
    {
        transform.RotateAround(target.position, Vector3.up, delta.x * orbitSpeed * Time.deltaTime);

        float newVerticalRotation = currentVerticalRotation + delta.y * orbitSpeed * Time.deltaTime;

        if (newVerticalRotation > longitudeClamp.x && newVerticalRotation < longitudeClamp.y)
        {
            transform.RotateAround(target.position, transform.right, -delta.y * orbitSpeed * Time.deltaTime);
            currentVerticalRotation = newVerticalRotation;
        }
    }
}