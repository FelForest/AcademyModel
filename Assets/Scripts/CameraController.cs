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

        // 스크린샷 캡처
        Texture2D screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();

        // Texture2D를 PNG 파일로 저장
        string filename = "AR_Screenshot_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        string imagePath = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllBytes(imagePath, screenshotTexture.EncodeToPNG());

        // 갤러리로 복사
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(imagePath, "MyAlbum", filename);
        if (permission == NativeGallery.Permission.Granted)
        {
            Debug.Log("스크린샷이 저장되었습니다. 경로: " + imagePath);
        }
        else
        {
            Debug.Log("저장 권한이 없습니다.");
        }

        // 파일 삭제
        File.Delete(imagePath);
    }
}
