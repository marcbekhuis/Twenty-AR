using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceGame : MonoBehaviour
{
    [SerializeField] GameObject gameController;
    [SerializeField] GameObject cubes;
    private ARRaycastManager arRaycastManager;
    [SerializeField] Camera camera;
    [SerializeField] ARPlaneManager arPlaneManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlaceObject(Input.mousePosition);
        }
        else
        {
            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    PlaceObject(Input.mousePosition);
                }
            }
        }
    }

    public void PlaceObject(Vector2 touchPosition)
    {
        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            cubes.transform.eulerAngles = new Vector3(0, camera.transform.eulerAngles.y, 0);
            cubes.transform.position = hitPose.position;
            gameController.SetActive(true);
            Destroy(this);

            arPlaneManager.enabled = false;
            foreach (var item in arPlaneManager.trackables)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
