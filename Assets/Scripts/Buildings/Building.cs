using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Building : SelectableObject
{
    public int Price;
    public int XSize = 3;
    public int ZSize = 3;
    public Renderer Renderer;
    public GameObject MenuGameObject;

    private Color _startColor;

    protected override void Start()
    {
        base.Start();
        MenuGameObject.SetActive(false);
    }

    private void Awake()
    {
        _startColor = Renderer.material.color;
    }

    private void OnDrawGizmos()
    {
        float cellSize = FindObjectOfType<BuildingPlacer>().CellSize;
        for (int x = 0; x < XSize; x++)
        {
            for (int z = 0; z < ZSize; z++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x,0f,z) * cellSize, new Vector3(1f,0f,1f) * cellSize);
            }
        }
    }

    public override void Select()
    {
        base.Select();
        MenuGameObject.SetActive(true);
    }

    public override void Unselect()
    {
        base.Unselect();
        MenuGameObject.SetActive(false);
    }

    public void DisplayUnacceptablePosition()
    {
        Renderer.material.color = Color.red;
    }

    public void DisplayAcceptablePosition()
    {
        Renderer.material.color = _startColor;
    }
}
