using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrickSet", menuName = "ScriptableObjects/TrickSet", order = 2)]
public class TrickSetSO : ScriptableObject
{
    public List<TrickSO> TrickCombos;
}
