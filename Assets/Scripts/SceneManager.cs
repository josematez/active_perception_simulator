using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{

    public GameObject _robot;
    public Text _robotName;

    // Initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    public void NewRobot(Text TxIp)
    {
        GameObject robot = Instantiate(_robot);
        robot.name = _robotName.text + "_ws://" + TxIp.text;
        var ros = robot.GetComponent<ROS>();
        ros.SetIP(TxIp.text);
        ros._pubPackages = new List<string>() { "RobotCamera_pub" };
        ros._subPackages = new List<string>() { "Tf_sub" };

        ros.Connect();
    }
}
