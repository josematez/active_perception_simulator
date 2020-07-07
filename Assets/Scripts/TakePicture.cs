using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TakePicture : MonoBehaviour
{

    private Camera myCamera;

    private int resWidth = 256;
    private int resHeight = 256;

    void Awake()
    {
        myCamera = gameObject.GetComponent<Camera>();

        if(myCamera.targetTexture == null)
        {
            myCamera.targetTexture = RenderTexture.GetTemporary(myCamera.pixelWidth, myCamera.pixelHeight, 24);
        }
        else
        {
            resWidth = myCamera.targetTexture.width;
            resHeight = myCamera.targetTexture.height;
        }
    }

    void SavePNG(Texture2D image)
    {
        byte[] bytes = image.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot_11.png", bytes);
        Debug.Log("Screenshot saved.");
    }

    void SendImage()
    {
        Texture2D image = new Texture2D(myCamera.pixelWidth, myCamera.pixelHeight, TextureFormat.RGB24, false);
        myCamera.Render();
        RenderTexture.active = myCamera.targetTexture;
        Rect rect = new Rect(0, 0, myCamera.pixelWidth, myCamera.pixelHeight);
        image.ReadPixels(rect, 0, 0);

        SavePNG(image);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
