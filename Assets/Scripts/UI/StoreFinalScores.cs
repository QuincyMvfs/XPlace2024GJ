using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreFinalScores : MonoBehaviour
{
    [SerializeField] private int _star1 = 0;
    [SerializeField] private int _star2 = 1000;
    [SerializeField] private int _star3 = 2000;

    public int star1 { get { return _star1; } }
    public int star2 { get { return _star2; } }
    public int star3 { get { return _star3; } }
}
