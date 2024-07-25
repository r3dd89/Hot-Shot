using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class Pickups : ScriptableObject
{
    public GameObject AmmoBox;
    public GameObject HealthBox;
    public int HealAmount;
    public int AmmoAmount;
}
