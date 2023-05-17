using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Objs;

    private Dictionary<string, GameObject> spawnedObjs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager ARTrackedImageManager;

    public GameObject blueprint;
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


        if (trackedImage.trackingState == TrackingState.Tracking)
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
}