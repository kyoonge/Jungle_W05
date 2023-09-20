using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    public float minx;
    public float maxx;

    private void Update()
    {
        if(transform.position.x < minx|| transform.position.x > maxx)
        {
            transform.DOMoveX(transform.position.x, 1f);
        }
        
    }
}