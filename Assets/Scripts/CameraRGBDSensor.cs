using ROSBridgeLib.sensor_msgs;
using ROSBridgeLib.std_msgs;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CameraRGBDSensor : MonoBehaviour

{
    public bool debug;
    public float _frecuency = 1;
    public ROS _ros;
    public Vector2Int _size;
    public float _fieldOfViewInicial = 60;

    private Camera[] _cameras;
    private Rect rect;

    #region Unity Functions
    private void Awake()
    {
        _cameras = GetComponentsInChildren<Camera>();
        _ros = GetComponent<ROS>();
        foreach (Camera c in _cameras) { c.fieldOfView = _fieldOfViewInicial; }
        rect = new Rect(0, 0, _size.x, _size.y);
    }

    void Start()
    {
        StartCoroutine(SendImages());
    }

    void Update()
    {
        if (debug && Input.GetMouseButton(0))
        {
            Vector3 screenPoint = Input.mousePosition;
            var image = RTImageDepth(_cameras[1]);
            var data = image.GetPixel((int)screenPoint.x, (int)screenPoint.y);
            Log("Data: " + data);
        }
    }

    #endregion

    #region Public Functions
    public void ChangeFoV(float fieldOfView)
    {
        Debug.Log(fieldOfView);
        foreach (Camera c in _cameras) { c.fieldOfView = (fieldOfView / (float)10.0); }
    }
    #endregion

    #region Private Functions
    IEnumerator SendImages()
    {
        while (true)
        {
            Texture2D rgb = RTImageColor(_cameras[0]);
            Texture2D depth = RTImageDepth(_cameras[1]);
            //Color c;
            //for (int i = 0; i < rgb.width; i++) {
            //    for (int j = 0; j < rgb.height; j++) {
            //        c = rgb.GetPixel(i, j);
            //        c.a = depth.GetPixel(i, j).a;
            //        rgb.SetPixel(i, j, c);
            //    }
            //}​

            Color[] pxs = rgb.GetPixels();
            Color[] pxsDepth = depth.GetPixels();

            for (int i = 0; i < pxs.Length; i++)
            {
                pxs[i].a = pxsDepth[i].a;
            }

            rgb.SetPixels(pxs);

            HeaderMsg _head = new HeaderMsg(0, new TimeMsg(_ros.GetepochStart().Second, 0), "map");

            //RGB
            CompressedImageMsg compressedImg = new CompressedImageMsg(_head, "png", rgb.EncodeToPNG());
            _ros.Publish(RobotCamera_pub.GetMessageTopic(), compressedImg);

            //Depth
            //compressedImg = new CompressedImageMsg(_head, "png", RTImageDepth(_cameras[1]).EncodeToPNG());
            //_ros.Publish(CameraDepth_pub.GetMessageTopic(), compressedImg);
            yield return new WaitForSeconds(_frecuency);
        }
    }

    private Texture2D RTImageColor(Camera camera)
    {
        RenderTexture renderTexture = new RenderTexture(_size.x, _size.y, 24);
        Texture2D screenShot = new Texture2D(_size.x, _size.y, TextureFormat.RGBA32, false);
        camera.targetTexture = renderTexture;
        camera.Render();
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null; //Clean
        Destroy(renderTexture); //Free memory​

        return screenShot;
    }

    private Texture2D RTImageDepth(Camera camera)
    {
        RenderTexture renderTexture = new RenderTexture(_size.x, _size.y, 24);
        Texture2D screenShot = new Texture2D(_size.x, _size.y, TextureFormat.Alpha8, false);
        camera.depthTextureMode = DepthTextureMode.Depth;
        camera.targetTexture = renderTexture;
        camera.Render();
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null; //Clean
        Destroy(renderTexture); //Free memory​

        return screenShot;
    }

    private void Log(string msg)
    {
        if (!debug)
            return;
        Debug.Log("[Camera RGBD sensors]: " + msg);
    }
    #endregion

}

