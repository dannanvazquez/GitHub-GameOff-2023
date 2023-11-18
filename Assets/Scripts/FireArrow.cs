using UnityEngine;
using System.Collections;

public class FireArrow : BasicArrow {

    public override void OnHit(Collider target)
    {
        if (target.transform.root.TryGetComponent(out Fire fire))
        {
            fire.SetOnFire();
        }
    }

}
