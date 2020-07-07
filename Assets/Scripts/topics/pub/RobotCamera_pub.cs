using ROSBridgeLib;
using ROSBridgeLib.sensor_msgs;

public class RobotCamera_pub : ROSBridgePublisher
{

    public new static string GetMessageTopic()
    {
        return "/active_perception/robot_camera";
    }

    public new static string GetMessageType()
    {
        return "sensor_msgs/CompressedImage";
    }

    public static string ToYAMLString(CompressedImageMsg msg)
    {
        return msg.ToYAMLString();
    }

}

