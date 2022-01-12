using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButtons : MonoBehaviour
{
    public BuildingPlacer BuildingPlacer;
    public GameObject BuildingPrefab;

    public void TryBuy()
    {
        int price = BuildingPrefab.GetComponent<Building>().Price;
        if (FindObjectOfType<Resources>().Money >= price)
        {
            FindObjectOfType<Resources>().Money -= price;
            BuildingPlacer.CreateBuilding(BuildingPrefab);
        }
        else
        {
            Debug.Log("Not enough Minerals");
        }
    }
}
