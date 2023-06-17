using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CameraController : MonoBehaviour
{
    public Canvas canvas;
    public void CaptureScreenshot()
    {
        StartCoroutine(CaptureScreenshotCoroutine());
    }

    private IEnumerator CaptureScreenshotCoroutine()
    {
        canvas.enabled = false;
        yield return new WaitForEndOfFrame();
        canvas.enabled = true;

        // ��ũ���� ĸó
        Texture2D screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();

        // Texture2D�� PNG ���Ϸ� ����
        string filename = "AR_Screenshot_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        string imagePath = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllBytes(imagePath, screenshotTexture.EncodeToPNG());

        // �������� ����
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(imagePath, "MyAlbum", filename);
        if (permission == NativeGallery.Permission.Granted)
        {
            Debug.Log("��ũ������ ����Ǿ����ϴ�. ���: " + imagePath);
        }
        else
        {
            Debug.Log("���� ������ �����ϴ�.");
        }

        // ���� ����
        File.Delete(imagePath);
    }
}
