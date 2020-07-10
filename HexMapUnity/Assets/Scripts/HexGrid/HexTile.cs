using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexTile : MonoBehaviour
{
    public HexCoordinates coordinates;
    public int index;
    public HexTile[] neighbours;

    public int basicX;  //the coordinates of the original placement
    public int basicZ;

    public Vector3[] GetVertices()
    {
        Vector3[] vertices = new Vector3[HexMetrics.corners.Length + 1];
        vertices[0] = transform.position;

        for (int i = 0; i < vertices.Length - 1; i++)
        {
            vertices[i + 1] = transform.position + HexMetrics.corners[i];
        }
        return vertices;
    }

    public Vector3[] GetInnerVertices()
    {
        Vector3[] iC = HexMetrics.GetInnerCorners();

        Vector3[] rIC = new Vector3[iC.Length + 1];
        rIC[0] = transform.position;

        for (int i = 1; i < rIC.Length; i++)
        {
            rIC[i] = iC[i - 1] + transform.position;
        }
        return rIC;
    }
}


