using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int max = 3;
    [SerializeField] float iFrames = 1f;
    int current;
    float invulnerableUntil;

    public int Current => current;
    public int Max => max;

    void Awake() => current = max;

    public bool TakeDamage(int amount)
    {
        if (current <= 0 || Time.time < invulnerableUntil) return false;
        invulnerableUntil = Time.time + iFrames;
        current -= amount;
        GameManager.Instance.UpdateUI();
        if (current <= 0) GameManager.Instance.PlayerDied();   // <= 0, because a hit of 2 can skip 0
        return true;
    }
}
