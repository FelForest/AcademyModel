using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlanceTouchPlace : MonoBehaviour
{
    // ȭ�鿡 ����� ������Ʈ
    public GameObject obj;

    // ARRaycastManager
    ARRaycastManager arRaycastManager;

    // raycast �۵��� ����Ʈ
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // ȭ�� ��ġ�� ��ǥ
    private Vector2 touchPos;

    // ������Ʈ ���� ���� Ȯ��
    bool isSpawned;

    // ȭ�鿡 ������ ������Ʈ
    private GameObject spawnedObj;

    // Start is called before the first frame update
    void Start()
    {

        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CreateObjectWhereTouched();
    }

    // ȭ����ġ�� ������Ʈ ���� �Լ�
    private void CreateObjectWhereTouched()
    {
        // ��ġ��
        if(Input.touchCount > 0)
        {
            // ��� ��ġ�� �� ���� ������ ���߿��� ù��°�� ��ġ�Ȱ� ��ġ �޾ƿ���
            touchPos = Input.GetTouch(0).position;

            // ��ġ�� ���� �ٰ��� �� ����
            if(arRaycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
            {
                // �̰� �𸣰ڴ�
                var hitPos = hits[0].pose;

                // ������Ʈ �ʱ� ������
                if (!isSpawned)
                {
                    //������Ʈ ����
                    spawnedObj = Instantiate(obj, hitPos.position, hitPos.rotation);
                    isSpawned = true;
                }
                else
                {
                    // ������Ʈ ��ġ ����
                    spawnedObj.transform.position = hitPos.position;
                }
            }
        }
    }
}
