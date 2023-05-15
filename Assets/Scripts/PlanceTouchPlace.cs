using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlanceTouchPlace : MonoBehaviour
{
    // 화면에 띄워줄 오브젝트
    public GameObject obj;

    // ARRaycastManager
    ARRaycastManager arRaycastManager;

    // raycast 작동시 리스트
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // 화면 터치시 좌표
    private Vector2 touchPos;

    // 오브젝트 생성 여부 확인
    bool isSpawned;

    // 화면에 생성된 오브젝트
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

    // 화면터치시 오브젝트 생성 함수
    private void CreateObjectWhereTouched()
    {
        // 터치시
        if(Input.touchCount > 0)
        {
            // 어러개 터치가 될 수도 있지만 그중에서 첫번째로 터치된곳 위치 받아오기
            touchPos = Input.GetTouch(0).position;

            // 터치한 곳이 다각형 및 평면시
            if(arRaycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
            {
                // 이건 모르겠다
                var hitPos = hits[0].pose;

                // 오브젝트 초기 생성시
                if (!isSpawned)
                {
                    //오브젝트 생성
                    spawnedObj = Instantiate(obj, hitPos.position, hitPos.rotation);
                    isSpawned = true;
                }
                else
                {
                    // 오브젝트 위치 변경
                    spawnedObj.transform.position = hitPos.position;
                }
            }
        }
    }
}
