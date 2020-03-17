using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turnpropeller : MonoBehaviour
{
    public float rotation_multiplayer;

    private void Start()
    {
        rotation_multiplayer = 8f;
    }
    void FixedUpdate()
    {
        Vector3 rZ = new Vector3(0, 0, 3.14f);
        transform.Rotate(rZ * rotation_multiplayer);
    }
}
