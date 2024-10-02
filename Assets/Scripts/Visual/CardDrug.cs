using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrug : MonoBehaviour
{
    public bool UsePointerDisplacement = true;

    private bool dragging = false;
    private float zDisplacement;
    private Vector3 pointerDisPlacement = Vector3.zero;

    private void OnMouseDown()
    {
        Debug.Log("点中此卡");
        dragging = true;
        zDisplacement = transform.position.z - Camera.main.transform.position.z;
        if (UsePointerDisplacement)
            pointerDisPlacement = transform.position - MouseInWorldCoords();
        else
            pointerDisPlacement = Vector3.zero;
    }

    private void Update()
    {
        if (dragging)
        {
            var mousePos = MouseInWorldCoords();
            transform.position = new Vector3(mousePos.x + pointerDisPlacement.x, mousePos.y + pointerDisPlacement.y, transform.position.z);
        }
    }

    private void OnMouseUp()
    {
        Debug.Log("释放此卡");
        if(dragging)
            dragging = false;
    }

    private Vector3 MouseInWorldCoords() 
    {
        Vector3 screenMousePos = Input.mousePosition;
        screenMousePos.z = zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    } 
    

}
