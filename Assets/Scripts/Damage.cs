using UnityEngine;

[System.Serializable]
public class Damage
{
    public int initialDamage;

    [Space()]

    public bool causeBleeding;
    public int damagePerTick;
    public int totalTicks;
    public float tickInterval;
}
