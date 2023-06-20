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
        GameObject cloud = spawnedObjs["cloud"];
        GameObject sea = spawnedObjs["sea"];
        float moveSpeed = 0.1f;
        
        // �̹����� û���� ��ġ �¾����� //�״����� �´� �̹��� ���� Ȯ��
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if(blueprintNames[printnum] == ImageName)
            {
                trackedObject.transform.position = trackedImage.transform.position;
                trackedObject.transform.rotation = trackedImage.transform.rotation;
                trackedObject.SetActive(true);

                sea.transform.position = trackedObject.transform.position;
                sea.transform.rotation = trackedObject.transform.rotation * Quaternion.Euler(-90f, 0f, 0f);
                sea.SetActive(true);

                if (!cloudstart)
                {
                    cloud.transform.position = trackedObject.transform.position + new Vector3(0f,0f,1f);
                    cloud.transform.rotation = trackedObject.transform.rotation;
                    cloud.SetActive(true);
                }
                

                if (blueprintNames[printnum] == "ship")
                {
                    cloud.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                }
                if(blueprintNames[printnum] == "plane")
                {
                    cloud.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                trackedObject.SetActive(false);
                sea.SetActive(false);
                cloud.SetActive(false);
                cloudstart = false;
            }
        }
        else
        {
            trackedObject.SetActive(false);
            sea.SetActive(false);
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
