using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    public GameObject UnitPrefab;
    public Text Price;
    public Barrack Barrack;

    private void Start()
    {
        Price.text = UnitPrefab.GetComponent<Unit>().Price.ToString();
    }

    public void TryBuy()
    {
        int price = UnitPrefab.GetComponent<Unit>().Price;

        if (FindObjectOfType<Resources>().Money >= price)
        {
            FindObjectOfType<Resources>().Money -= price;
            Barrack.CreateUnit(UnitPrefab);
        }
        else
        {
            Debug.Log("Not enough minerals");
        }
    }
}
