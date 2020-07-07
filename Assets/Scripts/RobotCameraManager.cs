using ROSBridgeLib.sensor_msgs;
using ROSBridgeLib.std_msgs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCameraManager : MonoBehaviour
{

    public Camera _cam;
    private ROS _robot;
    private CompressedImageMsg msg;
    //private int k = 0;

    void Start()
    {
        if (_robot == null)
        {
            _robot = FindObjectOfType<ROS>();
        }
        //SendImage();

    }

    /*private void Update()
    {
        StartCoroutine(SendImage());
    }*/

    private void FixedUpdate()
    {
        HeaderMsg header = new HeaderMsg(0, new TimeMsg(_robot.GetepochStart().Second, 0), "base_link");
        _cam.Render();
        RenderTexture.active = _cam.targetTexture;
        //msg = new ImageMsg(header, _cam.targetTexture);
        //string msg_string = msg.ToString();

        /*if(k == 0)
        {
            int i = 190000;
            byte[] _data = new byte[i];

            for (int j = 0; j < i; j++)
            {
                _data[j] = (byte)0;
            }
            System.IO.File.WriteAllBytes(Application.dataPath + "/msg", _data);
            Debug.Log("Imagen capturada");
            k = 1;
        }*/  
        //_robot.Publish(RobotCamera_pub.GetMessageTopic(), msg);
    }

    IEnumerator SendImage()
    {
        
        Debug.Log("Hola");
        //_robot.Publish(RobotCamera_pub.GetMessageTopic(), msg);
        yield return null;
    }

/*
    public void SendImage() {

        if (_robot == null)
        {
            _robot = FindObjectOfType<ROS>();
        }

        //HeaderMsg header = new HeaderMsg(0, new TimeMsg(_robot.GetepochStart().Second, 0), _robotLocation._id.ToString());
        HeaderMsg header = new HeaderMsg(0, new TimeMsg(_robot.GetepochStart().Second, 0), "base_link");
        //Debug.Log(_cam.targetTexture);
        _cam.Render();
        RenderTexture.active = _cam.targetTexture;
        ImageMsg msg = new ImageMsg(header, _cam.targetTexture);
        _robot.Publish(RobotCamera_pub.GetMessageTopic(), msg);

        Invoke("SendImage", 5f);
    }*/

}
