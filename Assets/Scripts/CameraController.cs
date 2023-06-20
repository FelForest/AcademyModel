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

        // 스크린 크기 만큼의 Texture2D를 생성
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // 생성한 Texture2D에 현재 화면 캡처
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        // 이미지를 갤러리에 저장
        NativeGallery.SaveImageToGallery(screenshotTexture, "MyApp", "Screenshot.png");

        
        canvas.SetActive(true);
    }
}
