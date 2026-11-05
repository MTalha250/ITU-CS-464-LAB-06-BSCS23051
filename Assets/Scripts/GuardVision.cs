using UnityEngine;

// Lecture 05's CanSee on a guard. Being seen costs a whole life; the spotlight on the guard shows the same cone.
public class GuardVision : MonoBehaviour
{
    [SerializeField] float viewDistance = 6f, halfAngle = 30f;
    [SerializeField] LayerMask obstacles = ~0;
    Health player;

    void Start() => player = FindAnyObjectByType<Health>();

    void Update()
    {
        if (CanSee(player.transform)) player.TakeDamage(player.Max);
    }

    bool CanSee(Transform target)
    {
        Vector3 toTarget = target.position - transform.position;
        float distance = toTarget.magnitude;
        if (distance > viewDistance) return false;   // 1 range

        toTarget /= distance;                         // 2 length 1
        float dot = Vector3.Dot(transform.forward, toTarget);
        if (dot < Mathf.Cos(halfAngle * Mathf.Deg2Rad)) return false;   // 3 angle

        // 4 walls: stop just short of the player so the ray doesn't hit the player itself
        return !Physics.Raycast(transform.position, toTarget, distance - 0.6f, obstacles, QueryTriggerInteraction.Ignore);
    }
}
