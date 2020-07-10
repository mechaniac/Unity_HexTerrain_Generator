using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class HexMetrics
{
    public const float outerRadius = 10f;
    public const float innerRadius = outerRadius * 0.866025404f;

    public const float innerCornerRadius = innerRadius * .9f;

    public static Vector3[] corners = {
            new Vector3(0f, 0f, outerRadius),
            new Vector3(innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(0f, 0f, -outerRadius),
            new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(-innerRadius, 0f, 0.5f * outerRadius)
    };

    public static Vector3[] GetInnerCorners()
    {
        Vector3[] iC = new Vector3[6];

        iC[0] = new Vector3(0f, 0f, innerCornerRadius);

        for (int i = 1; i < iC.Length; i++)
        {
            iC[i] = Quaternion.Euler(0, 60, 0) * iC[i - 1];
        }

        return iC;
    }
}


