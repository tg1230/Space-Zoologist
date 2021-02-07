using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AddResourceCommand { addGrass, add5Grass, add10Grass, addDirt, add5Dirt, add10Dirt, addSand, add5Sand, add10Sand, addStone, add5Stone, add10Stone, };

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

    [SerializeField] List<ResourceData> resourceData = default;
    Dictionary<string, int> remainingResources = new Dictionary<string, int>();
    [SerializeField] LevelDataReference LevelDataRef = default;

    // Auto-generate the list of ResourceData for you if resourceData is empty
    public void OnValidate()
    {
        if (LevelDataRef == null) return;
        if (resourceData == null || resourceData.Count > 0) return;

        resourceData = new List<ResourceData>();
        foreach (Item item in LevelDataRef.LevelData.Items) {
            resourceData.Add(new ResourceData(item.ID));
        }
    }


    public void Start()
    {
        foreach (ResourceData data in resourceData) {
            remainingResources.Add(data.resourceName, data.initialAmount);
        }
    }

    public void AddResource(AddResourceCommand command) {

        switch (command) {
            case AddResourceCommand.addGrass:
                AddItems("Grass", 1);
                break;
            case AddResourceCommand.add5Grass:
                AddItems("Grass", 5);
                break;
            case AddResourceCommand.add10Grass:
                AddItems("Grass", 10);
                break;
            default:
                break;

        }

    }

    public void AddItems(Item item, int amount) {
        AddItems(item.ID, amount);
    }

    void AddItems(string itemName, int amount) {
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
        PlacedItems(item.ID, amount);
    }

    void PlacedItems(string itemName, int amount) {
        if (remainingResources.ContainsKey(itemName))
        {
            remainingResources[itemName] -= amount;
        }
        else
        {
            Debug.LogError("ResourceManager: " + itemName + " does not exist!");
        }
    }

    public int CheckRemainingAmount(string itemName) {
        if (remainingResources.ContainsKey(itemName))
        {
            return remainingResources[itemName];
        }
        else
        {
            Debug.LogError("ResourceManager: " + itemName + " does not exist!");
            return -1;
        }
    }
}
