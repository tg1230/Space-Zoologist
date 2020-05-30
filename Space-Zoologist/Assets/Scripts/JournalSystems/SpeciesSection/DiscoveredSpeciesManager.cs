﻿using System.Collections.Generic;
using UnityEngine;

public class DiscoveredSpeciesManager : MonoBehaviour, ISetupSelectable
{
    [SerializeField] GameObject SpeciesDiscoveredContent = default;
    [SerializeField] GameObject SpeciesPopupPrefab = default;
    [SerializeField] SpeciesSectionManager speciesSectionManager = default;
    [Header("For testing")]
    [SerializeField] List<Species> LevelSpeciesPlaceholder = default;
    [Header("RemoveSelfFromList and whatever else should happen")]
    public ItemSelectedEvent SpeciesSelected = new ItemSelectedEvent();

    public void Start()
    {
        this.AddDiscoveredSpecies();
    }

    // Filters out species that have already been discovered
    public void AddDiscoveredSpecies()
    {
        foreach (var species in this.LevelSpeciesPlaceholder)
        {
            // Journal data exists, need to filter
            if (this.speciesSectionManager.journalEntries != null)
            {
                if (!this.speciesSectionManager.journalEntries.Entries.ContainsKey(species.SpeciesName))
                {
                    this.CreateNewDisplayObject(species);
                }
                else
                {
                    Debug.Log("Species \"" + species.SpeciesName + "\" already exists in journal");
                }
            }
            // No journal data yet, just add
            else
            {
                this.CreateNewDisplayObject(species);
            }
        }
    }

    private void CreateNewDisplayObject(Species species)
    {
        GameObject discoveredSpecies = Instantiate(this.SpeciesPopupPrefab, this.SpeciesDiscoveredContent.transform);
        discoveredSpecies.GetComponent<SpeciesEntryDisplayLogic>().Initialize(species);
        discoveredSpecies.GetComponent<SpeciesJournalData>().JournalEntry = new JournalEntry(species.SpeciesName);
        this.SetupItemSelectedHandler(discoveredSpecies, this.SpeciesSelected);
    }

    public void SetupItemSelectedHandler(GameObject item, ItemSelectedEvent action)
    {
        item.GetComponent<SelectableCanvasImage>().SetupItemSelectedHandler(action);
    }
}
