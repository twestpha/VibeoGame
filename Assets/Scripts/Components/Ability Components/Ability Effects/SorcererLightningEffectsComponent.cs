using UnityEngine;

public class SorcererLightningEffectsComponent : AbilityEffectsComponent {

    private const float MAX_DISTANCE = 10.0f;

    public Transform effectsRoot;
    public MeshRenderer endpointRenderer;

    public override void EffectsUpdate(){
        // Debug.DrawLine(transform.position, transform.position + (transform.forward * 3.0f), Color.red, 0.0f, false);

        RaycastHit hitInfo;
        float distance = MAX_DISTANCE;
        bool hit = false;

        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, MAX_DISTANCE, ~0, QueryTriggerInteraction.Ignore)){
            DamageableComponent hitDamageable = hitInfo.collider.gameObject.GetComponent<DamageableComponent>();

            if(hitDamageable != null){
                hitDamageable.DealDamage(3.0f * Time.deltaTime, DamageType.Projectile, hitInfo.point, castingAbilityComponent.gameObject);
            }

            distance = hitInfo.distance;
            hit = true;
        }

        endpointRenderer.enabled = hit;
        effectsRoot.localScale = new Vector3(
            1.0f,
            1.0f,
            (Mathf.Max(distance, 0.1f) / MAX_DISTANCE) * 0.85f
        );
    }

    public override void EffectsFinish(){
        Destroy(gameObject);
    }
}