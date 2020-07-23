using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedSystemsTester : MonoBehaviour
{

    [SerializeField] private Text populationStats = default;
    [SerializeField] private Text foodSourceStats = default;

    private void Update()
    {
        string populationStatsText = "";
        foreach (Population population in PopulationManager.ins.Populations)
        {
            populationStatsText += $"***{ population.Species.SpeciesName } {population.GetInstanceID()}; Count: {population.Count}; Dominance: {population.Dominance}***\n";
            foreach (KeyValuePair<string, float> needValue in population.NeedsValues)
            {
                populationStatsText += $"- {needValue.Key}: { needValue.Value } -- Condition: {population.Species.Needs[needValue.Key].GetCondition(needValue.Value)}\n";
            }
            populationStatsText += "\n";
        }
        populationStats.text = populationStatsText;

        string foodSourceStatsText = "";
        foreach (FoodSource foodSource in FoodSourceManager.ins.FoodSources)
        {
            foodSourceStatsText += $"***{ foodSource.Species.SpeciesName }; Food output: {foodSource.FoodOutput}***\n";
            foreach (KeyValuePair<string, float> needValue in foodSource.NeedsValues)
            {
                foodSourceStatsText += $"- {needValue.Key}: { needValue.Value } -- Condition: {foodSource.Species.Needs[needValue.Key].GetCondition(needValue.Value)}\n";
            }
            foodSourceStatsText += "\n";
        }
        foodSourceStats.text = foodSourceStatsText;
    }
}