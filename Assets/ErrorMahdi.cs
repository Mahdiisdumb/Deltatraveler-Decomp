using System;
using UnityEngine;

public class ErrorMahdi : MonoBehaviour
{
    public SpriteRenderer Mahdi;
    public Sprite[] mahdispr;

    public Vector2 MahdiPos;

    private void Awake()
    {
        Mahdi = GetComponent<SpriteRenderer>();
        MahdiPos = Mahdi.transform.position;

        // Clapping sprites
        mahdispr = new Sprite[2]
        {
            Resources.Load<Sprite>("WOW1"),
            Resources.Load<Sprite>("WOW2")
        };
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
        float animationSpeed = 2f; // cycles per second
        float t = Time.time * animationSpeed;

        // Animate sprite
        Mahdi.sprite = mahdispr[(int)Mathf.Floor(t) % 2];
    }
}
