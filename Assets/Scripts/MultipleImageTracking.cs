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
    
    private void Awake()
    {
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

    // �� ������ ���� ȣ�� �Ǵ� �Լ�
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

        //�̹��� ��ġ �� û���� ��ġ �޾ƿ���
        float ImageX = trackedImage.transform.position.x;
        float ImageY = trackedImage.transform.position.y;    
        // �̹����� û���� ��ġ �¾����� //�״����� �´� �̹��� ���� Ȯ��
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if(blueprintNames[printnum] == ImageName)
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
        else
        {
            trackedObject.SetActive(false);
        }
    }

    public void ChangePrintNum(int num)
    {
        printnum = num;
    }
}
