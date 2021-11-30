using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class OutlineHandler : MonoBehaviour
{
    //public List<Outline> childrenOutlines = new List<Outline>();
    public Outline[] childrenOutlines;

    // Start is called before the first frame update
    void Start()
    {
        /*
        foreach (Transform child in transform)
        {
            Outline outline = child.GetComponentsInChildren<Outline>();
            if (outline != null)
                childrenOutlines.Add(outline);
        }
        */

        childrenOutlines = GetComponentsInChildren<Outline>();

        SetOutline(true);
        SetOutline(false);
    }

    private void OnMouseEnter()
    {
        SetOutline(true);
    }

    private void OnMouseExit()
    {
        SetOutline(false);
    }

    public void SetOutline(bool val)
    {
        foreach (Outline outline in childrenOutlines)
            outline.enabled = val;
    }
}
