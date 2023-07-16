using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivitiy;

    private Transform parent;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        RotationControls();
    }

    private void RotationControls()
    {
        float rotate = Input.GetAxis("Mouse X") * mouseSensitivitiy * Time.deltaTime;
        parent.Rotate(Vector3.up, rotate);
    }
}
