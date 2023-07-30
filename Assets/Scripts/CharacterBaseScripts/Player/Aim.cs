using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField] private Sprite sprite;

    private Vector3 mousePoz;
    private Camera mainCamera;
    

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        mousePoz = mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        mousePoz.z = 0f;
        transform.position = mousePoz;
    }
}
