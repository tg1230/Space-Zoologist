﻿using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add new entries and search for existing ones
/// </summary>
/// 4 TODO refactor so everything works with new JournalData setup
public class SpeciesSectionManager : MonoBehaviour, ISetupSelectable
{
    [SerializeField] private GameObject SpeciesDisplayPrefab = default;
    [SerializeField] private GameObject SpeciesContent = default;
    [SerializeField] private SpeciesReferenceData SpeciesData = default;
    public JournalData journalEntries => this.JournalEntries;
    private JournalData JournalEntries = default;
    private List<GameObject> JournalEntriesDisplay = new List<GameObject>();
    [Header("What should happen when a species is clicked")]
    public ItemSelectedEvent OnItemSelectedEvent = new ItemSelectedEvent();

    // Load Journal data and populate Journal with entries
    public void Start()
    {
        // this.JournalEntries = SaveSystem.LoadJournal();
        if (this.JournalEntries != null)
        {
            foreach(KeyValuePair<string, JournalEntry> entry in this.JournalEntries.Entries)
            {
                this.CreateJournalEntryDisplay(entry.Value);
            }
        }
        else
        {
            this.JournalEntries = new JournalData();
        }
    }

    // Called by adding species from the discovered species popup
    public void CreateJournalEntry(GameObject species)
    {
        // The popup should have this component attached
        SpeciesJournalData speciesData = null;
        if (species.TryGetComponent(out speciesData))
        {
            Species s = SpeciesData.FindSpecies(speciesData.JournalEntry.DiscoveredSpecies);
            this.JournalEntries.Entries.Add(speciesData.JournalEntry.DiscoveredSpecies, speciesData.JournalEntry);
            this.CreateJournalEntryDisplay(speciesData.JournalEntry);
        }
        else
        {
            Debug.Log("Attempted to add invalid gameobject to journal");
        }
    }

    public void CreateJournalEntryDisplay(JournalEntry entry)
    {   
        Species species = SpeciesData.FindSpecies(entry.DiscoveredSpecies);
        GameObject newEntry = Instantiate(this.SpeciesDisplayPrefab, this.SpeciesContent.transform);
        newEntry.GetComponent<SpeciesEntryDisplayLogic>().Initialize(species);
        this.SetupItemSelectedHandler(newEntry, this.OnItemSelectedEvent);
        newEntry.GetComponent<SpeciesJournalData>().JournalEntry = entry;
        // For searching by GameObject name
        newEntry.name = species.SpeciesName;
        newEntry.SetActive(true);
        this.JournalEntriesDisplay.Add(newEntry);
    }

    public void SetupItemSelectedHandler(GameObject entry, ItemSelectedEvent itemSelected)
    {
        entry.GetComponent<SelectableCanvasImage>().SetupItemSelectedHandler(itemSelected);
    }

    // Searching by actual GameObject name
    public void SearchJournal(string speciesSearch)
    {
        if (speciesSearch.Equals(""))
        {
            foreach(GameObject entry in this.JournalEntriesDisplay)
            {
                entry.SetActive(true);
            }
        }
        else 
        {
            foreach(GameObject entry in this.JournalEntriesDisplay)
            {
                if (speciesSearch.Length > entry.name.Length)
                {
                    continue;
                }
                if (!(entry.name.ToLower().Substring(0, speciesSearch.Length).Equals(speciesSearch.ToLower().Substring(0, speciesSearch.Length))))
                {
                    entry.SetActive(false);
                }
                else
                {
                    entry.SetActive(true);
                }
            }
        }
    }


}
