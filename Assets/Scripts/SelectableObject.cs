using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] private GameObject _tag;

    protected virtual void Start()
    {
        _tag.SetActive(false);
    }
    
    public virtual void OnHover()
    {
        transform.localScale = Vector3.one * 1.1f;
    }

    public virtual void OnUnHover()
    {
        transform.localScale = Vector3.one;
    }

    public virtual void Select()
    {
        _tag.SetActive(true);
    }

    public virtual void Unselect()
    {
        _tag.SetActive(false); 
    }

    public virtual void WhenClickOnGround(Vector3 point)
    {
    }
    
}
