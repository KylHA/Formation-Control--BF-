using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownLevFwdBck : MonoBehaviour
{

    public float UpDownForce;
    FormationControl Fcontrol;

    void Start()
    {
        UpDownForce = 9.81f;
        Fcontrol = GameObject.Find("GameController").GetComponent<FormationControl>();
    }


    void FixedUpdate()
    {
        // GoPoz(Fcontrol.CalculateDirection(), 2);
    }

    //void UpDwnLev()
    //{
    //    if (Input.GetKeyDown(KeyCode.I))
    //        UpDownForce = 400f;

    //    else if (Input.GetKeyDown(KeyCode.K))
    //        UpDownForce = -200f;

    //    else if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        GetComponent<Rigidbody>().Sleep();
    //        GetComponent<Rigidbody>().WakeUp();
    //        UpDownForce = 9.81f;
    //    }

    //    else
    //        UpDownForce = 9.81f;
    //}

    void GoPoz(Vector3 _poz, float speed)
    {
        // GetComponent<Rigidbody>().MovePosition(transform.position + control.CalculateDirection() * Time.fixedDeltaTime * speed);
    }


}
