using UnityEngine;
using System.Collections;

public class FireArrow : BasicArrow {

    public override void OnHit(Collider target)
    {
        if (target.transform.TryGetComponent(out Fire fire))
        {
            fire.SetOnFire();
        }
    }

}
