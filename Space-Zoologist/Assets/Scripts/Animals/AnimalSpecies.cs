using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using System;

[CreateAssetMenu]
public class AnimalSpecies : ScriptableObject
{
    // Getters
    public string SpeciesName => speciesName;
    public int Dominance => dominance;
    public float GrowthRate => growthRate;
    public float Size => size;
    public List<BehaviorScriptTranslation> Behaviors => needBehaviorSet;
    public List<TileType> AccessibleTerrain => accessibleTerrain;
    public Sprite Icon => icon;
    public Sprite Sprite => icon;
    public float Range => range;
    public Sprite Representation => representation;
    // TODO setup tile weights for species
    public Dictionary<TileType, byte> TilePreference = default;
    public AnimatorController AnimatorController => animatorController;

    // Values
    [SerializeField] private AnimatorController animatorController = default;
    [SerializeField] private string speciesName = default;
    [Range(0.0f, 10.0f)]
    [SerializeField] private int dominance = default;
    [Range(30f, 120f)]
    [SerializeField] private float growthRate = 30f;

    [SerializeField] private float range = default;
    [Header("Behavior displayed when need isn't being met")]
    [SerializeField] private List<BehaviorScriptTranslation> needBehaviorSet = default;
    [Range(0.0f, 10.0f)]
    [SerializeField] private float size = default;
    [SerializeField] private List<TileType> accessibleTerrain = default;
    [SerializeField] private Sprite icon = default;

    [SerializeField]
    private List<NeedTypeConstructData> needsList = new List<NeedTypeConstructData>();

    // Replace later with actual representation/animations/behaviors
    [SerializeField] private Sprite representation = default;

    public Dictionary<string, Need> SetupNeeds()
    {
        Dictionary<string, Need> needs = new Dictionary<string, Need>();
        foreach (NeedTypeConstructData needData in needsList)
        {
            foreach (NeedConstructData need in needData.Needs)
            {
                // Use the NeedData to create Need
                needs.Add(need.NeedName, new Need(needData.NeedType, need));
                //Debug.Log($"Add {need.NeedName} Need for {this.SpeciesName}");
            }
        }
        return needs;
    }
    public void SetupData(string name, int dominance, float growthRate, List<string> accessibleTerrain, List<NeedTypeConstructData> needsList)
    {
        // TODO setup behaviors and accessible terrain
        this.speciesName = name;
        this.dominance = dominance;
        this.growthRate = growthRate;
        this.accessibleTerrain = new List<TileType>();
        foreach (string tileType in accessibleTerrain)
        {
            if (tileType.Equals("Sand", StringComparison.OrdinalIgnoreCase))
            {
                this.accessibleTerrain.Add(TileType.Sand);
            }
            if (tileType.Equals("Grass", StringComparison.OrdinalIgnoreCase))
            {
                this.accessibleTerrain.Add(TileType.Grass);
            }
            if (tileType.Equals("Dirt", StringComparison.OrdinalIgnoreCase))
            {
                this.accessibleTerrain.Add(TileType.Dirt);
            }
            if (tileType.Equals("Liquid", StringComparison.OrdinalIgnoreCase))
            {
                this.accessibleTerrain.Add(TileType.Liquid);
            }
            if (tileType.Equals("Rock", StringComparison.OrdinalIgnoreCase))
            {
                this.accessibleTerrain.Add(TileType.Rock);
            }
            if (tileType.Equals("Wall", StringComparison.OrdinalIgnoreCase))
            {
                this.accessibleTerrain.Add(TileType.Wall);
            }
        }
        //this.accessibleTerrain = accessibleTerrain;
        this.needsList = needsList;
    }
}