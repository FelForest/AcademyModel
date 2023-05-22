using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
public class MultipleImageTracking : MonoBehaviour
{
    public GameObject[] Objs;
    private Dictionary<string, GameObject> spawnedObjs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager ARTrackedImageManager;
    public Text ImagePosition;
    public float margin;
    private bool IsCorrect;
    public Text countdown;
    private float setTime;
    public Image blueprint;
    private void Awake()
    {
        ARTrackedImageManager = GetComponent<ARTrackedImageManager>();
        foreach (GameObject prefab in Objs)
        {
            GameObject clone = Instantiate(prefab);
            spawnedObjs.Add(prefab.name, clone);
            clone.SetActive(false);
        }
        margin = 0.05f;
        IsCorrect = false;
        setTime = 3.0f;
    }

    private void Update()
    {
        countdown.text = setTime.ToString();
    }
    void OnEnable()
    {
        ARTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        ARTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            spawnedObjs[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        GameObject trackedObject = spawnedObjs[trackedImage.referenceImage.name];

        float ImageX = trackedImage.transform.position.x;
        float ImageY = trackedImage.transform.position.y;
        float blueX = blueprint.transform.position.x;
        float blueY = blueprint.transform.position.y;

        ImagePosition.text = ImageX.ToString() + " : " + ImageY.ToString() + "\n" +
                             blueX.ToString() +    " : " + blueY.ToString();
        
        // 이미지랑 청사진 위치 맞아햐함 //그다음에 맞는 이미지 인지 확인
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            IsCorrect = CountDown();
        }
        else
        {
            setTime = 3.0f;
            IsCorrect = false;
        }

        if (IsCorrect)
        {
            trackedObject.transform.position = trackedImage.transform.position;
            trackedObject.transform.rotation = trackedImage.transform.rotation;
            trackedObject.SetActive(true);
        }
        else
        {
            trackedObject.SetActive(false);
        }
    }

    bool CountDown()
    {
        if (setTime > 0)
        {
            setTime -= Time.deltaTime;
            return false;
        }
        else
        {
            setTime = 0f;
            return true;
        }
    }
}