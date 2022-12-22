using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Quest")]
public class Quest : ScriptableObject {
    [SerializeField] private Objective[] _objectives;
}