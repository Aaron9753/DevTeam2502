using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [Header("----- Camera Settings -----")]
    [Tooltip("Sensitivity of the camera rotation.")]
    [SerializeField] int sens;

    [Tooltip("Minimum vertical rotation of the camera (negative values look down).")]
    [SerializeField] int lockVertMin;

    [Tooltip("Maximum vertical rotation of the camera (positive values look up).")]
    [SerializeField] int lockVertMax;

    [Tooltip("Invert the vertical mouse movement.")]
    [SerializeField] bool invertY;

    float rotX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //get input
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

        //tie the mouse input to the rotX
        if (!invertY)
            rotX -= mouseY;
        else
            rotX += mouseY;

        //clamp the camera x rot
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        //rotate the camera on the x-axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        //rotate the player on the y-axis
        transform.parent.Rotate(Vector3.up * mouseX);


    }
}
