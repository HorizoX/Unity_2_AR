using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine;

public class AR_Placement : MonoBehaviour
{
    public GameObject AR_Object_To_Spawn;
    public GameObject placement_Indicator;
    private GameObject Spawn_Object;

    private Pose Placement_Pose;
    private ARRaycastManager AR_Raycast_Manager;
    private bool Placement_Valid = false;

    void Start()
    {
        AR_Raycast_Manager = FindObjectOfType<ARRaycastManager>();
    }

    // need to update placement inidcator, placement pose and spawn object

  void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (Placement_Valid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }
    void PlaceObject()
    {
        Spawn_Object = Instantiate(AR_Object_To_Spawn, Placement_Pose.position, Placement_Pose.rotation);
    }
    void UpdatePlacementIndicator()
    {
        if (Placement_Valid)
        {
            placement_Indicator.SetActive(true);
            placement_Indicator.transform.SetPositionAndRotation(Placement_Pose.position, Placement_Pose.rotation);
        }
        else
        {
            placement_Indicator.SetActive(false);
        }
    }
    void UpdatePlacementPose()
    {
        var screen_Center = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        AR_Raycast_Manager.Raycast(screen_Center, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        Placement_Valid = hits.Count > 0;
        if (Placement_Valid)
        {
            Placement_Pose = hits[0].pose;
            var camera_Forward = Camera.current.transform.forward;
            var camera_Bearing = new Vector3(camera_Forward.x, 0, camera_Forward.z).normalized;
            Placement_Pose.rotation = Quaternion.LookRotation(camera_Bearing);
        }
    }
}
