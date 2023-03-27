using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "ARPG/Ability Data", order = 0)]
public class AbilityData : ScriptableObject {

    public enum AbilityType {
        SpawnObject,
        DealDamageWithShape
    }

    public enum DamageShape {
        Rectangle,
    }

    public AbilityType abilityType;
    public float castTime;
    public float cooldownTime;
    public bool continueCastingWhileHeld;

    [Header("SpawnObject attributes")]
    public GameObject objectToSpawn;
    public Vector3 spawnOffsetFromCastTransform;

    [Header("DealDamageWithShape attributes")]
    public DamageShape damageShape;
    public float amount;
}