using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Managment : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private SelectableObject _hovered;

    private Vector2 _frameStart;
    private Vector2 _frameEnd;
    private bool _selection = false;

    public Image FrameImage;

    public List<SelectableObject> ListOfSelected = new List<SelectableObject>();

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<SelectableCollider>())
            {
                SelectableObject hitSelectable = hit.collider.GetComponent<SelectableCollider>().SelectableObject;
                if (_hovered)
                {
                    if (_hovered != hitSelectable)
                    {
                        _hovered.OnUnHover();
                        _hovered = hitSelectable;
                        _hovered.OnHover();
                    }
                }
                else
                {
                    _hovered = hitSelectable;
                    _hovered.OnHover();
                }
            }
            else
            {
                UnHoverCurrent();
            }
        }
        else
        {
            UnHoverCurrent();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_hovered)
            {
                if (Input.GetKey(KeyCode.LeftShift) == false)
                {
                    UnSelectAll();
                }    
                Select(_hovered);
            }

            if (hit.collider.tag == "Ground" && !_selection) 
            {
                UnSelectAll();
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            int rowNumber = Mathf.CeilToInt(Mathf.Sqrt(ListOfSelected.Count));
            for (int i = 0; i < ListOfSelected.Count; i++)
            {
                int row = i / rowNumber;
                int column = i % rowNumber;

                Vector3 point = hit.point + new Vector3(row, 0f, column);
                ListOfSelected[i].WhenClickOnGround(point);
            }
        }
        
        // Выделение рамкой 
        if (Input.GetMouseButtonDown(0))
        {
            _frameStart = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {

            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);
            Vector2 size = max - min;

            if (size.magnitude > 10f)
            {
                FrameImage.enabled = true;
                _selection = true;
                
                FrameImage.rectTransform.anchoredPosition = min;
            
                FrameImage.rectTransform.sizeDelta = size;
            
                Rect rect = new Rect(min, size);
                UnSelectAll();
                Unit[] allUnits = FindObjectsOfType<Unit>();
                for (int i = 0; i < allUnits.Length; i++)
                {
                    Vector2 screenPosition = _camera.WorldToScreenPoint(allUnits[i].transform.position);
                    if (rect.Contains(screenPosition))
                    {
                        Select(allUnits[i]);
                    }
                }   
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            FrameImage.enabled = false;
            _selection = false;
        }
    }

    private void Select(SelectableObject selectableObject)
    {
        if (ListOfSelected.Contains(selectableObject) == false)
        {
            ListOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    public void UnSelect(SelectableObject selectableObject)
    {
        if (ListOfSelected.Contains(selectableObject))
        {
            ListOfSelected.Remove(selectableObject);
        }
    }

    private void UnSelectAll()
    {
        for (int i = 0; i < ListOfSelected.Count; i++)
        {
            ListOfSelected[i].Unselect();
        }
        ListOfSelected.Clear();
    }

    private void UnHoverCurrent()
    {
        if (_hovered)
        {
            _hovered.OnUnHover();
            _hovered = null;
        }
    }
}