﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum NeedType { GasX, GasY, GasZ, SpaceMaple, LeafyBush, Sand, Grass, Dirt, Stone, RedLiquid, YellowLiquid, BlueLiquid }
public enum NeedCondition { Bad, Neutral, Good }

[CreateAssetMenu]
public class SpeciesNeed : ScriptableObject
{
    public NeedType Type => type;
    public int Severity => severity;

    [SerializeField] private NeedType type = default;
    [Range(1.0f, 10.0f)]
    [SerializeField] private int severity = 1;
    [SerializeField] private List<NeedCondition> conditions = default;
    [SerializeField] private List<float> thresholds = default;

    /// <summary>
    /// Compares a value with the condition thresholds and returns the associated condition.
    /// </summary>
    /// <param name="value">The value to compare to the need thresholds</param>
    /// <returns></returns>
    public NeedCondition GetCondition(float value)
    {
        // If there is only one condition, return it.
        if (conditions.Count == 1) return conditions[0];

        for (var i = 0; i < this.thresholds.Count; i++)
        {
            if (value < this.thresholds[i])
            {
                return this.conditions[i];
            }
        }

        return this.conditions[this.thresholds.Count];
    }

    public void OnValidate()
    {
        if (conditions.Count == 0)
        {
            conditions.Add(NeedCondition.Good);
        }

        while (conditions.Count < thresholds.Count + 1)
        {
            thresholds.RemoveAt(thresholds.Count - 1);
        }
        while (conditions.Count > thresholds.Count + 1)
        {
            if (thresholds.Count == 0)
            {
                thresholds.Add(0);
            }
            else
            {
                thresholds.Add(thresholds[thresholds.Count - 1] + 1);
            }
        }

        for (var i = 0; i < thresholds.Count - 1; i++)
        {
            if (thresholds[i + 1] <= thresholds[i])
            {
                thresholds[i + 1] = thresholds[i] + 1;
            }
        }

    }
}