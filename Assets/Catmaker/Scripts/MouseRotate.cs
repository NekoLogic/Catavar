using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseRotate : MonoBehaviour
{
    private Vector3 startRotation;
    private float mouseStartX;

    private bool isDragging;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startRotation = transform.rotation.eulerAngles;
            mouseStartX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            float currentMouse = mouseStartX - Input.mousePosition.x;
            transform.rotation = Quaternion.Euler(startRotation.x, currentMouse + startRotation.y, startRotation.z);
        }
    }
}
