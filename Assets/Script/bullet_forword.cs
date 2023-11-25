using System.Linq;
using UnityEngine;

public class bullet_forword : MonoBehaviour
{
    public Weapon _weapon;
    public Sentry_Gun sentry;
    public bool _is_Sentry_Gun;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _is_Sentry_Gun ? sentry.raycastHit.point : _weapon.raycastHit.point, 0.1f);
    }
}
