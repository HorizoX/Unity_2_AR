using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System;

public class GameManager : MonoBehaviour
{
    // Rotation for horizontal surfaces
    private Quaternion horizontalRotation = Quaternion.Euler(0f, -90f, 0f);
    private GameObject selectedPrefab; // Currently selected prefab
    public Button transitionUpButton; // UI button for transitioning up
    public Button transitionDownButton; // UI button for transitioning down
    public Button transitionLeftButton; // UI button for transitioning left
    public Button transitionRightButton; // UI button for transitioning right
    public Button rotateRightButton; // UI button for rotating right
    public Button rotateLeftButton; // UI button for rotating left
    public Button deleteObjectButton; // UI button for deleting an object
    public Camera camera; // Reference to the camera
    private int index; // Index of the selected prefab in the dropdown
    private float distanceMoved = 1f; // Distance to move the prefab
    private float rotationStep = 45f; // Rotation step for the prefab
    public List<GameObject> prefabs; // List of available prefabs
    public TMP_Dropdown dropdownMenu; // Dropdown menu for selecting prefabs


    private void Start()
    {
        dropdownMenu.onValueChanged.AddListener(Menu_Choice_Changed);
        rotateRightButton.onClick.AddListener(RotateRightPressed);
        rotateLeftButton.onClick.AddListener(RotateLeftPressed);
        transitionRightButton.onClick.AddListener(TransitionRightPressed);
        transitionLeftButton.onClick.AddListener(TransitionLeftPressed);
        transitionUpButton.onClick.AddListener(TransitionUpPressed);
        transitionDownButton.onClick.AddListener(TransitionDownPressed);
        deleteObjectButton.onClick.AddListener(DeletePressed);
    }
    private void Update()
    {
        Vector2 location;

        #if UNITY_EDITOR
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        location = Mouse.current.position.ReadValue();
        #else
        // Handle touch input on devices
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            return;

        location = Input.GetTouch(0).position;
        #endif

        Ray ray = camera.ScreenPointToRay(location);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            bool isPrefabPlaceable = IsPrefabPutDown(hit.collider.gameObject, hit.transform.rotation);
            if (isPrefabPlaceable && hit.collider.gameObject == null && index >= 0 && index < prefabs.Count)
            {
                // Instantiate the selected prefab
                GameObject selectedPrefab = prefabs[index];
                GameObject createdObject = Instantiate(selectedPrefab, hit.point + new Vector3(0, 0.2f, 0), Quaternion.identity);
            }

            GameObject pressedPrefab = hit.transform.gameObject;
        }
    }

   

    private bool IsHorizontal(Quaternion rotation)
    {
        return rotation.eulerAngles == horizontalRotation.eulerAngles;
    }

    private bool IsPrefabPutDown(GameObject New_Prefab, Quaternion Prefab_Rotation)
    {
        if (IsHorizontal(Prefab_Rotation))
        {
            if (index >= 0 && index < prefabs.Count)
            {
                GameObject prefab = prefabs[index];
                return prefab;
            }
        }
        else
        {
            if (index >= 0 && index < prefabs.Count)
            {
                GameObject prefab = prefabs[index];
                return prefab;
            }
        }

        return false;
    }

    private void ChoosePrefab(GameObject prefabChoice)
    {
        if (selectedPrefab != null)
        {
            // Deselect the currently selected prefab if there is one
            UnchoosePrefab();
        }

        selectedPrefab = prefabChoice;

        Renderer prefabColor = selectedPrefab.GetComponent<Renderer>();
        prefabColor.material.color = Color.white;
    }

    private void UnchoosePrefab()
    {
        Renderer prefabColor = selectedPrefab.GetComponent<Renderer>();
        prefabColor.material.color = Color.blue;

        selectedPrefab = null;
    }

    private void RotateRightPressed()
    {
        if (selectedPrefab != null)
        {
            selectedPrefab.transform.Rotate(Vector3.up, rotationStep);
        }
    }

    private void RotateLeftPressed()
    {
        if (selectedPrefab != null)
        {
            selectedPrefab.transform.Rotate(Vector3.down, rotationStep);
        }
    }
    private void TransitionRightPressed()
    {
        if (selectedPrefab != null)
        {
            selectedPrefab.transform.Translate(Vector3.right * distanceMoved);
        }
    }

    private void TransitionLeftPressed()
    {
        if (selectedPrefab != null)
        {
            selectedPrefab.transform.Translate(Vector3.left * distanceMoved);
        }
    }

    private void TransitionUpPressed()
    {
        if (selectedPrefab != null)
        {
            selectedPrefab.transform.Translate(Vector3.forward * distanceMoved);
        }
    }

    private void TransitionDownPressed()
    {
        if (selectedPrefab != null)
        {
            selectedPrefab.transform.Translate(Vector3.back * distanceMoved);
        }
    }
    private void DeletePressed()
    {
        if (selectedPrefab != null)
        {
            Destroy(selectedPrefab);
            selectedPrefab = null;
        }
    }
    private void Menu_Choice_Changed(int value)
    {
        index = value;
    }
}


