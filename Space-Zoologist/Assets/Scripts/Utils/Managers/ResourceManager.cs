using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [System.Serializable]
    public class ResourceData {
        public string resourceName;
        public int initialAmount;

        public ResourceData(string ID) {
            resourceName = ID;
        }
    }

    [SerializeField] LevelDataReference LevelDataRef = default;
    [SerializeField] List<ResourceData> resourceData = default;
    Dictionary<string, int> remainingResources;

    // Auto-generate the list of ResourceData for you if resourceData is empty
    public void OnValidate()
    {
        if (LevelDataRef == null) return;
        if (resourceData == null || resourceData.Count > 0) return;

        resourceData = new List<ResourceData>();
        foreach (Item item in LevelDataRef.LevelData.Items) {
            resourceData.Add(new ResourceData(item.ID));
        }
        foreach (AnimalSpecies species in LevelDataRef.LevelData.AnimalSpecies) {
            resourceData.Add(new ResourceData(species.SpeciesName));
        }
    }


    public void Start()
    {
        remainingResources = new Dictionary<string, int>();
        foreach (ResourceData data in resourceData) {
            remainingResources.Add(data.resourceName, data.initialAmount);
        }
    }

    public void AddItem(Item item, int amount) {
        AddItem(item.ID, amount);
    }

    void AddItem(string itemName, int amount) {
        if (remainingResources.ContainsKey(itemName))
        {
            remainingResources[itemName] += amount;
        }
        else
        {
            Debug.LogError("ResourceManager: " + itemName + " does not exist!");
        }
    }

    public void Placed(Item item, int amount)
    {
        PlacedItem(item.ID, amount);
    }

    public void Placed(AnimalSpecies species, int amount)
    {
        PlacedItem(species.SpeciesName, amount);
    }

    void PlacedItem(string itemName, int amount) {
        if (remainingResources.ContainsKey(itemName))
        {
            remainingResources[itemName] -= amount;
        }
        else
        {
            Debug.LogError("ResourceManager: " + itemName + " does not exist!");
        }
    }

    public int CheckRemainingResource(Item item) {
        if (remainingResources.ContainsKey(item.ID))
        {
            return remainingResources[item.ID];
        }
        else
        {
            Debug.LogError("ResourceManager: " + item.ID + " does not exist!");
            return -1;
        }
    }

    public int CheckRemainingResource(AnimalSpecies species)
    {
        if (remainingResources.ContainsKey(species.SpeciesName))
        {
            return remainingResources[species.SpeciesName];
        }
        else
        {
            Debug.LogError("ResourceManager: " + species.SpeciesName + " does not exist!");
            return -1;
        }
    }
}
