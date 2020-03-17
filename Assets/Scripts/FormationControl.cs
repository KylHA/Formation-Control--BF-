using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationControl : MonoBehaviour
{
    public Transform Target;
    [SerializeField] Transform DronePosition;

    public GameObject TargetOBJ;
    List<GameObject> Targets = new List<GameObject>();

    private void Start()
    {
        Spawn_Targets_to_Formation();
    }

    void Spawn_Targets_to_Formation()
    {
        int DroneCount = GameObject.FindGameObjectsWithTag("Player").Length;
        int countobj = 0;
        for (int i = 0; i < DroneCount; i++)
        {
            Targets.Add(Instantiate(TargetOBJ, TargetOBJ.transform.position, TargetOBJ.transform.rotation));
        }

        foreach (var _target in Targets)
        {
            _target.transform.position = GameObject.FindGameObjectsWithTag("Player")[countobj].transform.position;

            if (countobj < DroneCount)
                countobj++;
        }
    }

    public Vector3 CalculateDirection(Vector3 _Target, Vector3 _Obj)
    {
        Vector3 direction = _Target - _Obj;

        return direction.normalized;
    }

    void PreDetermined_V_Formation()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            //int targetCount = Targets.Count;

            //if (targetCount % 2 != 0)
            //    for (int i = 1; i <= targetCount; i++)
            //    {
            //        Targets[targetCount].transform.position=Vector3.up;
            //    }

            foreach (var item in Targets)
            {
                item.transform.position = new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5));
            }

        }
    }

    Vector3 FindClosestTarget()
    {
        return Vector3.up;
    }
}