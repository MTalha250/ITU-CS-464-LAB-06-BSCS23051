using UnityEngine;

// Lecture 07's moving hazard: a kinematic Rigidbody sliding back and forth. Hazard.cs on the same object does the damage.
[RequireComponent(typeof(Rigidbody))]
public class MovingHazard : MonoBehaviour
{
    [SerializeField] float speed = 2f, distance = 6f;
    [SerializeField] Vector3 axis = Vector3.right;
    Rigidbody rb;
    Vector3 start;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        start = rb.position;
    }

    void FixedUpdate()
    {
        float x = Mathf.PingPong(Time.time * speed, distance);
        rb.MovePosition(start + axis.normalized * x);
    }
}
