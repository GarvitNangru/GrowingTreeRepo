using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlace : MonoBehaviour
{

    [SerializeField] private GameObject objectToInstatiate;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject startTxt;
    [SerializeField] private Toggle canPlace;

    

    private GameObject spawnedObject;

    private ARPlaneManager planeManager;
    private ARRaycastManager raycastManager;

    private Vector2 touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (canPlace.isOn)
        {
            planeManager.enabled = true;
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;



            if (!IsPointerOverUIObject())
            {
                if (raycastManager.Raycast(touchPosition, hits, trackableTypes: TrackableType.PlaneWithinPolygon))
                {
                    var hitPose = hits[0].pose;

                    if (spawnedObject == null)
                    {
                        spawnedObject = Instantiate(objectToInstatiate, hitPose.position, hitPose.rotation);
                        canPlace.isOn = false;
                        planeManager.enabled = false;
                        Destroy(startTxt);
                    }
                    else
                    {
                        spawnedObject.transform.position = hitPose.position;

                    }
                }
            }
        }
        else
            planeManager.enabled = false;
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(index: 0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    private bool IsPointerOverUIObject()
    {
        // get current pointer position and raycast it
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // check if the target is in the UI
        foreach (RaycastResult r in results)
        {
            bool isUIClick = r.gameObject.transform.IsChildOf(this.ui.transform);
            if (isUIClick)
            {
                return true;
            }
        }
        return false;
    }

}