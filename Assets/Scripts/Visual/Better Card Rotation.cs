using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCardRotation : MonoBehaviour
{
    public Transform facePoint;
    public RectTransform faceSide;
    public RectTransform backSide;
    public Collider cardCol;
    private bool passedThoughColliderOnCard;
    private bool showingBack;

    void Update()
    {
        RaycastHit[] hits;
        if (Camera.main == null) return;
        Vector3 cameraToFacePoint = facePoint.transform.position - Camera.main.transform.position;
        hits = Physics.RaycastAll(Camera.main.transform.position,cameraToFacePoint,cameraToFacePoint.magnitude);

        passedThoughColliderOnCard = false;

        foreach (RaycastHit h in hits)
        {
            if (h.collider == cardCol)
                passedThoughColliderOnCard = true;
            break;
        }

        if(passedThoughColliderOnCard && !showingBack)
        {
            backSide.gameObject.SetActive(true);
            faceSide.gameObject.SetActive(false);
            showingBack = true;
        }
        else if (!passedThoughColliderOnCard && showingBack)
        {
            backSide.gameObject.SetActive(false);
            faceSide.gameObject.SetActive(true);
            showingBack = false;
        }
    }
}
