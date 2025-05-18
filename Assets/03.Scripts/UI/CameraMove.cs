using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject gameCamera;
    Vector3 moveDir = Vector3.zero;
    public float moveSpeed = 5f;
    private float defaultDir;
    private bool isMoving = false;

    void Update()
    {
        CheckKeyboardInput();
        if (isMoving)
        {
            gameCamera.transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;
        }
    }

    private void CheckKeyboardInput()
    {
        bool a = Input.GetKey(KeyCode.A);
        bool d = Input.GetKey(KeyCode.D);

        if (a && !d)
        {
            isMoving = true;
            moveDir.x = -1;
        }
        else if (d && !a)
        {
            isMoving = true;
            moveDir.x = 1;
        }
        else
        {
            isMoving = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMoving = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMoving = false;
    }
}
