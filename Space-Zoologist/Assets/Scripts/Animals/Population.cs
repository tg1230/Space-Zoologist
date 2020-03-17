﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{
    private Species species = default;
    public Species Species { get => species; }
    public string SpeciesName { get => species.SpeciesName; }
    private Dictionary<NeedType, float> Needs = new Dictionary<NeedType, float>();
    public int Count { get; private set; }
    private Sprite sprite;
    public Sprite Sprite { get { return species.Sprite; } private set => sprite = value; }
    private Vector2Int origin = Vector2Int.zero;

    public void Initialize(Species species, Vector2Int origin, NeedSystemManager needSystemManager)
    {
        this.species = species;
        this.origin = origin;

        this.transform.position = GridUtils.Vector2IntToVector3Int(origin);
        this.sprite = species.Sprite;
        foreach(SpeciesNeed need in Species.Needs)
        {
            Needs.Add(need.Type, 0);
            needSystemManager.RegisterPopulation(this, need.Type);
        }
    }

    // Called whenever an event triggers a system to update its value
    // or when a system calls this delegated method
    public void UpdateNeed(NeedType need, float value)
    {
        if (Needs.ContainsKey(need))
        {
            Needs[need] = value;
            // UpdateGrowthConditions();
        }
        else
        {
            Debug.Log("Need not found");
        }
    }

    public float GetNeedStatus(NeedType need)
    {
        if (!Needs.ContainsKey(need))
        {
            Debug.Log($"Tried to access nonexistent need '{need}' in a { SpeciesName } population");
            return 0;
        }

        return Needs[need];
    }

    // TODO: Implement
    /// <summary>
    /// Gets need conditions for each need based on the current values and sends them along with the need's severity to the growth formula system.
    /// </summary>
    public void UpdateGrowthConditions()
    {
        throw new System.NotImplementedException();
    }
}

