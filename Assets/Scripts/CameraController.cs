using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject canvas;

    public void CaptureAndSaveScreenshot()
    {
        StartCoroutine(TakeScreenshot());
    }

    private IEnumerator TakeScreenshot()
    {
        canvas.SetActive(false);
        yield return new WaitForEndOfFrame();

        // ��ũ�� ũ�� ��ŭ�� Texture2D�� ����
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // ������ Texture2D�� ���� ȭ�� ĸó
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        // �̹����� �������� ����
        NativeGallery.SaveImageToGallery(screenshotTexture, "MyApp", "Screenshot.png");

        
        canvas.SetActive(true);
    }
}
