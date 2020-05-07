using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSourceManager : MonoBehaviour
{

    private List<FoodSource> foodSources = new List<FoodSource>();

    // Start is called before the first frame update
    void Start()
    {
        foodSources.AddRange(FindObjectsOfType<FoodSource>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
