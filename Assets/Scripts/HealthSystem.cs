using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public int maxHP = 3;
    public int hp;
    public Action<int, int> onChange;
    public Action onDeath;

    void Awake() { hp = maxHP; }

    public void Damage(int d)
    {
        hp = Mathf.Max(0, hp - d);
        onChange?.Invoke(hp, maxHP);
        if (hp <= 0) onDeath?.Invoke();
    }

    public void ResetHP()
    {
        hp = maxHP;
        onChange?.Invoke(hp, maxHP);
    }
}
