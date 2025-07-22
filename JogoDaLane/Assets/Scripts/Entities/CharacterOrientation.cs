using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOrientation : MonoBehaviour
{
    // public Vector3 lastOrientation;
    public Transform handTransform;
    public Vector2 lastOrientation;
    //private string spriteOrientation;
    //public SpriteChanger sc;
    // public Animator animator;

    // void Start()
    // {
    //     lastOrientation = new Vector3(0, 0, -1);
    //     // spriteOrientation = "forward";
    //     animator = transform.GetComponent<Animator>();
    //     spriteRenderer = transform.GetComponent<SpriteRenderer>();
    // }

    void Awake()
    {
        lastOrientation = Vector2.zero;
        // spriteOrientation = "forward";
        //animator = transform.GetComponent<Animator>();
    }


    public void ChangeOrientation(Vector3 targetPoint)
    {
        Vector3 relativePos = targetPoint - transform.position;

        //Debug.Log(relativePos);
        //Debug.DrawLine(targetPoint, transform.position, Color.red, 50);

        if(relativePos != Vector3.zero)
        {
            // AngleCheck(relativePos);

            handTransform.up = -relativePos.normalized;


            lastOrientation = relativePos;
            // CheckSpriteOrientation(relativePos); --- Old way of changing sprites
        }
        else
        {
            handTransform.up = -lastOrientation.normalized;
            // spriteHandler.ChangeSprite(lastOrientation.normalized);
            // AngleCheck(lastOrientation);
        }
    }
}