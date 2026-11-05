using UnityEngine;

// Visual only: the slab drops while the crusher's collider is on, and lifts while it rests.
public class CrusherSlab : MonoBehaviour
{
    [SerializeField] Collider zone;
    [SerializeField] float upY = 3f, downY = 0.6f, speed = 25f;

    void Update()
    {
        Vector3 p = transform.localPosition;
        p.y = Mathf.MoveTowards(p.y, zone.enabled ? downY : upY, speed * Time.deltaTime);
        transform.localPosition = p;
    }
}
