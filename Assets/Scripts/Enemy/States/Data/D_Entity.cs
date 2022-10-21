using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class D_Entity : ScriptableObject
{
    public float wallCheckDistance;
    public float ledgeCheckDistance;

    public LayerMask groundMask;
}
