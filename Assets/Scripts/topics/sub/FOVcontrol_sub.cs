using ROSBridgeLib;
using ROSBridgeLib.std_msgs;
using SimpleJSON;
using UnityEngine;

public class FOVcontrol_sub : ROSBridgeSubscriber
{
    public new static string GetMessageTopic()
    {
        return "active_perception/FOVcontrol";
    }

    public new static string GetMessageType()
    {
        return "std_msgs/Float32";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new Float32Msg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg, string host)
    {
        Object.FindObjectOfType<CameraRGBDSensor>().ChangeFoV(((Float32Msg)msg).GetData());
    }
}
