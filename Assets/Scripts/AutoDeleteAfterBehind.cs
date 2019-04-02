using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeleteAfterBehind : MonoBehaviour
{

    public int deadZone = 400;
    public Transform gameAreaTransform;

    void Start()
    {
        this.gameAreaTransform = GameObject.Find("GameArea").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (this.transform.position.x < (gameAreaTransform.position.x - deadZone))
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
