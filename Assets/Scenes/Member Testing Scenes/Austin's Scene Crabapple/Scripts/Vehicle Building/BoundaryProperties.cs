using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

using Vector2 = UnityEngine.Vector2 ;

public class BoundaryProperties : MonoBehaviour
{
    [SerializeField] Vector2 boundaryCenter ;
    [SerializeField] Vector2 boundDims ;

    // Return the center of the boundary
    public Vector2 GetBoundCenter()
    {
        return boundaryCenter ;
    }

    // Return the dimensions of the boundary
    public Vector2 GetBoundDims()
    {
        return boundDims ;
    }
}
