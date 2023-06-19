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

    private bool IsCorrect;
    public string[] blueprintNames;
    public int printnum;
    private string ImageName;
    private bool cloudstart;

    private void Awake()
    {
        cloudstart = false;
        ARTrackedImageManager = GetComponent<ARTrackedImageManager>();
        foreach (GameObject prefab in Objs)
        {
            GameObject clone = Instantiate(prefab);
            spawnedObjs.Add(prefab.name, clone);
            clone.SetActive(false);
        }
    }

    void OnEnable()
    {
        ARTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        ARTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // 매 프레임 마다 호출 되는 함수
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
        ImageName = trackedImage.referenceImage.name;
        GameObject trackedObject = spawnedObjs[ImageName];
        GameObject cloud = spawnedObjs["cloud"];
        float moveSpeed = 0.1f;
        
        // 이미지랑 청사진 위치 맞아햐함 //그다음에 맞는 이미지 인지 확인
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if(blueprintNames[printnum] == ImageName)
            {
                trackedObject.transform.position = trackedImage.transform.position;
                trackedObject.transform.rotation = trackedImage.transform.rotation;
                trackedObject.SetActive(true);
                if (ImageName == "ship")
                {
                    if (!cloudstart)
                    {
                        cloud.transform.position = trackedObject.transform.position;
                        cloud.transform.rotation = trackedObject.transform.rotation;
                        cloud.SetActive(true);
                        cloudstart = true;
                    }
                    cloud.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                }
                else
                {
                    cloud.SetActive(false);
                    cloudstart = false;
                }
                

            }
            else
            {
                trackedObject.SetActive(false);
                cloud.SetActive(false);
                cloudstart = false;
            }
        }
        else
        {
            trackedObject.SetActive(false);
            cloud.SetActive(false);
            cloudstart = false;
        }
    }

    public void ChangePrintNum(int num)
    {
        printnum = num;
        cloudstart = false;
    }
}
