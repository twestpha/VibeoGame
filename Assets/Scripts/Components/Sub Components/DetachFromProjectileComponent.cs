using UnityEngine;

//##################################################################################################
// Sub Components are a toolbox of simple, common building block behaviours, meant to do a single
// thing within the constraints of the other existing components.
//
// Detach From Projectile Component
// Once the bullet this is on is destroyed, unattach ourselves. This is commonly used for trails or
// effects, that shouldn't get destroyed along with a bullet.
//##################################################################################################
public class DetachFromProjectileComponent : MonoBehaviour {

    public ProjectileComponent bulletParent;

    void Start(){
        bulletParent.RegisterOnProjectileDestroyedDelegate(ProjectileDestroyed);
    }

    public void ProjectileDestroyed(){
        transform.parent = null;
    }
}
