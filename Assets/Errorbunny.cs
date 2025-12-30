using System;
using UnityEngine;

public class Errorbunny : MonoBehaviour
{
    public float rotationSpeed = 360f; // Degrees per second
    public Vector2 bunnyPos;

    private void Awake()
    {
        bunnyPos = transform.position;
    }

    private void Start()
    {
        if ((bool)Util.GameManager())
        {
            Destroy(Util.GameManager().gameObject);
        }
    }

    private void Update()
    {
        // Rotate 360 degrees continuously on Y axis
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}