using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterCameraScript : MonoBehaviour
{
    private CinemachineFreeLook camera;
    public GameControllerScript gameController;
    public float speedX = 2.0f;
    public float speedY = 2.0f;

    public float yaw = 0.0f;
    public float pitch = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        yaw += speedX * Input.GetAxis("Mouse X" ) * Time.deltaTime;
        pitch -= speedY * Input.GetAxis("Mouse Y") * Time.deltaTime;

        camera.m_XAxis.Value = yaw;
        camera.m_YAxis.Value = pitch;
        */
        if(gameController.getAllowInput() == false){
            print("gg");

            camera.m_YAxis.m_InputAxisName = "";
            camera.m_XAxis.m_InputAxisName = "";
        } else {
            camera.m_YAxis.m_InputAxisName = "Mouse Y";
            camera.m_XAxis.m_InputAxisName = "Mouse X";
        }
    }
}
