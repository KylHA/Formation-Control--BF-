﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class FormationControl
{
    public List<GameObject> TargetObjects = new List<GameObject>();
    public List<GameObject> DroneObjects = new List<GameObject>();

    public List<DTWList> ListOfDTW = new List<DTWList>();
    public List<DTList> MinTargets = new List<DTList>();


    public void FillDTWList()
    {
        foreach (var Drone in DroneObjects)
        {
            ListOfDTW.Add(new DTWList(Drone, TargetObjects, CalcDistance(TargetObjects, Drone)));
        }
        Debug.Log("" + LtoT(ListOfDTW));
        WeightCalc();
        Debug.Log("" + LtoT(ListOfDTW));
    }

    public void SortNStack()
    {
        int IterationCount = 1;
        while (MinTargets.Count != DroneObjects.Count)
        {
            Debug.LogWarning("{"+IterationCount+"}. Iteration");
            List<DTW> StackDTW = new List<DTW>();

            foreach (var item in ListOfDTW)
            {
                StackDTW.Add(new DTW(item.Drone, item.TargetList[MinPos(item.WeightList)], item.WeightList[MinPos(item.WeightList)]));
            }

            Debug.Log("" + DTWtoT(StackDTW));

            if (!Repeats(StackDTW))
            {
                foreach (var item in StackDTW)
                {
                    MinTargets.Add(new DTList(item.Drone, item.Target));
                }

                if (MinTargets.Count == DroneObjects.Count)
                    break;
            }

            for (int i = 0; i < StackDTW.Count; i++)
            {
                if (!Repeats(StackDTW))
                    break;

                if (RepeatPos(StackDTW, i).Count > 1)
                {
                    MinTargets.Add(FindMin(StackDTW, RepeatPos(StackDTW, i)));
                    Debug.Log("Removed From Original List ( " + 
                        MinTargets[MinTargets.Count - 1].Drone.name + ", "+MinTargets[MinTargets.Count - 1].Target.name+" )");
                    RemoveFromLists(StackDTW, RepeatPos(StackDTW, i));
                    i = 0;
                }
            }
            IterationCount++;
            Debug.Log("" + LtoT(ListOfDTW));
        }

        
        Debug.Log("" + DTtoT(MinTargets));
    }

    void WeightCalc()
    {
        foreach (var item in ListOfDTW)
        {
            float sum = 0;
            float mean = 0;

            foreach (var WList in item.WeightList)
            {
                sum += WList;
            }

            mean = sum / TargetObjects.Count;

            for (int i = 0; i < item.WeightList.Count; i++)
            {
                item.WeightList[i] = item.WeightList[i] / mean;
            }

        }
    }
    void RemoveFromLists(List<DTW> _StackDTW, List<int> posL)
    {

        int removeat = 0;
        for (int j = 0; j < ListOfDTW[0].WeightList.Count; j++)
        {
            if (ListOfDTW[0].TargetList[j] == MinTargets[MinTargets.Count - 1].Target)
            {
                removeat = j;
                break;
            }
        }


        ListOfDTW.RemoveAll(x => x.Drone == MinTargets[MinTargets.Count - 1].Drone);

        ListOfDTW[0].TargetList.RemoveAll(x => x == MinTargets[MinTargets.Count - 1].Target);

        foreach (var item in ListOfDTW)
        {
            item.WeightList.RemoveAt(removeat);
        }
        posL.Reverse();
        foreach (var item in posL)
        {
            _StackDTW.RemoveAt(item);
        }

    }
    List<int> RepeatPos(List<DTW> _StackDTW, int pos)
    {
        List<int> _temp = new List<int>();

        int count = 0;

        for (int i = 0; i < _StackDTW.Count; i++)
        {
            if (_StackDTW[pos].Target == _StackDTW[i].Target)
            {
                count++;
                _temp.Add(i);
            }
        }
        return _temp;
    }
    DTList FindMin(List<DTW> _StackDTW, List<int> posL)
    {
        int minpos = posL[0];

        DTList _temp = new DTList();

        foreach (var pos in posL)
            if (_StackDTW[pos].Weight < _StackDTW[minpos].Weight)
                minpos = pos;

        _temp = new DTList(_StackDTW[minpos].Drone, _StackDTW[minpos].Target);
        return _temp;
    }
    bool Repeats(List<DTW> _DTW)
    {
        int count = 0;

        for (int i = 0; i < _DTW.Count; i++)
        {
            foreach (var item in _DTW)
            {
                if (_DTW[i].Target == item.Target)
                    count++;
            }

            if (count >= 2)
                return true;
            else
                count = 0;
        }

        return false;
    }
    int MinPos(List<float> _WeightList)
    {
        if (_WeightList.Count == 1)
            return 0;

        int minpos = 0;

        for (int pos = 1; pos < _WeightList.Count; pos++)
            if (_WeightList[pos] < _WeightList[minpos])
                minpos = pos;

        return minpos;
    }
    List<float> CalcDistance(List<GameObject> Target, GameObject _Drone)
    {
        List<float> weight = new List<float>();

        foreach (var _Target in Target)
        {
            float Distance = Mathf.Pow(
            (Mathf.Pow(_Target.transform.position.x - _Drone.transform.position.x, 2) +
            Mathf.Pow(_Target.transform.position.y - _Drone.transform.position.y, 2))
            , 0.5f);
            weight.Add(Distance);
        }

        return weight;
    }

    string LtoT(List<DTWList> _list)
    {
        
        string rt = "Drone, Target, Weight Lists \n";

        foreach (var item in _list)
        {
            rt += item.Drone.name;

            for (int i = 0; i < TargetObjects.Count; i++)
            {
                rt += "  ( " + item.TargetList[i].name + ", " + item.WeightList[i] + " )    /        ";
            }
            rt += "\n";
        }
        return rt;
    }
    string DTWtoT(List<DTW> _list)
    {
        
        string rt = "Minimum Drone, Target, Weigth \n";

        foreach (var item in _list)
        {
            rt += item.Drone.name + "  ( " + item.Target.name + ", " + item.Weight + " )";
            rt += "\n";
        }
        return rt;
    }
    string DTtoT(List<DTList> _list)
    {
        
        string rt = "Drone Target Match List \n";

        foreach (var item in _list)
        {
            rt += item.Drone.name + "  ( " + item.Target.name + " )";
            rt += "\n";
        }
        return rt;
    }
}