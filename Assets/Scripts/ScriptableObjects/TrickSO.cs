using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrickCombo", menuName = "ScriptableObjects/TrickCombo", order = 1)]
public class TrickSO : ScriptableObject
{
    public List<TrickButtons> Combo;
}
