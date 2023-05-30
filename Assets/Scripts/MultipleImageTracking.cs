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
    public Text TrackedImageName;
    private float setTime;
    public Image blueprint;
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
        float blueX = blueprint.transform.position.x;
        float blueY = blueprint.transform.position.y;

        //���
        ImagePosition.text = ImageX.ToString() + " : " + ImageY.ToString() + "\n" +
                             blueX.ToString() + " : " + blueY.ToString() + "\n" +
                             blueprintNames[printnum].ToString();

        
        // �̹����� û���� ��ġ �¾����� //�״����� �´� �̹��� ���� Ȯ��
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if(blueprintNames[printnum] == ImageName)
            {
                TrackedImageName.text = trackedImage.referenceImage.name;
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

    public void ChangePrintNum(int num)
    {
        printnum = num;
    }
}
