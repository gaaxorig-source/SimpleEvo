using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private static List<Behavior> units;
    private static List<Behavior> unSubbedUnits;
    void Awake()
    {
        units = new List<Behavior>(32);
        unSubbedUnits = new List<Behavior>(32);
    }

    void Update()
    {
        for (int i = unSubbedUnits.Count - 1; i >= 0; i--)
        {
            Behavior behavior = unSubbedUnits[i];
            Remove(behavior.managerIndex);
            unSubbedUnits.RemoveAt(i);
            Destroy(behavior.gameObject);
        }

        foreach (var unit in units)
        {
            unit.MyUpdate(Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        foreach (var unit in units)
        {
            unit.MyFixedUpdate(Time.fixedDeltaTime);
        }
    }

    public static void Sub(Behavior b)
    {
        b.managerIndex = units.Count;
        units.Add(b);
    }

    public static void UnSub(Behavior b)
    {
        unSubbedUnits.Add(b);
    }

    private static void Remove(int index)
    {
        units[index] = units[units.Count - 1];
        units.RemoveAt(units.Count - 1);
        units[index].managerIndex = index;
    }
}
