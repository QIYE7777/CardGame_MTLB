using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameDistanceChildren : MonoBehaviour
{
    public Transform[] children;

    [HideInInspector]
    public Vector3 dist;

    private void Awake()
    {

            Vector3 firstChildPos = children[0].transform.position;
            Vector3 lastChildPos = children[children.Length - 1].transform.position;
            float xDist = (lastChildPos.x - firstChildPos.x) / (children.Length - 1);
            float yDist = (lastChildPos.y - firstChildPos.y) / (children.Length - 1);
            float zDist = (lastChildPos.z - firstChildPos.z) / (children.Length - 1);

            dist = new Vector3(xDist, yDist, zDist);

            for (int i = 1; i < children.Length; i++)
            {
                children[i].transform.position = children[i - 1].transform.position + dist;
            }
    }

}
