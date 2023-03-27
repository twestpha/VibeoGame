using UnityEngine;

public class SorcererLightningEffectsComponent : AbilityEffectsComponent {

    private const float MAX_DISTANCE = 5.0f;

    public override void EffectsUpdate(){
        // Debug.DrawLine(transform.position, transform.position + (transform.forward * 3.0f), Color.red, 0.0f, false);

        RaycastHit hitInfo;
        float distance = MAX_DISTANCE;
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, MAX_DISTANCE, ~0, QueryTriggerInteraction.Ignore)){
            DamageableComponent hitDamageable = hitInfo.collider.gameObject.GetComponent<DamageableComponent>();

            if(hitDamageable != null){
                hitDamageable.DealDamage(10.0f, DamageType.Projectile, hitInfo.point, castingAbilityComponent.gameObject);
            }

            distance = hitInfo.distance;
        }

        // distance
    }

    public override void EffectsFinish(){
        Destroy(gameObject);
    }
}