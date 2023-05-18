using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class MultipleImageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Objs;

    private Dictionary<string, GameObject> spawnedObjs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager ARTrackedImageManager;

    public Image blueprint;
    public float margin;
    public Text PositionCheck;
    private void Awake()
    {
        ARTrackedImageManager = GetComponent<ARTrackedImageManager>();
        margin = 0.5f;
        //Debug.Log(margin);
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
        bool objectSetActive = false;

        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            PositionCheck.text = trackedImage.transform.position.x.ToString() + " : " + trackedImage.transform.position.y.ToString();
            if (blueprint.transform.position.x <= trackedImage.transform.position.x + margin && blueprint.transform.position.x >= trackedImage.transform.position.x - margin && blueprint.transform.position.y <= trackedImage.transform.position.y + margin && blueprint.transform.position.y >= trackedImage.transform.position.y - margin)
            {
                objectSetActive = true;
            }
            if (objectSetActive)
            {
                trackedObject.transform.position = trackedImage.transform.position;
                trackedObject.transform.rotation = trackedImage.transform.rotation;
                trackedObject.SetActive(true);
            }
        }
        else
        {
            trackedObject.SetActive(false);
            objectSetActive = false;
        }
    }
}