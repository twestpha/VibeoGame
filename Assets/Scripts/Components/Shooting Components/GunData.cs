using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoType {
    AmmoType0,
    AmmoType1,
    AmmoType2,
    AmmoType3,
    AmmoType4,
    AmmoType5,
    AmmoType6,
    AmmoType7,
    AmmoType8,
    // ...
}

[CreateAssetMenu(fileName = "GunData", menuName = "Shooter/Gun Data", order = 0)]
public class GunData : ScriptableObject {
    [Header("Gun Characteristics")]
    public float damage = 1.0f;
    public DamageType damageType;
    public float coolDown = 1.0f;
    public float spread = 0.0f;
    public int shots = 1;

    // This is specially used by the player input to decide whether to use
    // getInput or getInputDown (i.e. requiring a re-press of the input)
    public bool automaticAction = true;

    [Header("Ammo Characteristics")]
    public bool useAmmo = false;

    // Yes, I know not all guns use magazines; this is for abstraction
    // A "magazine" here denotes one reload's worth of ammo
    // A "box" here is the pool of bullets that reload pulls from
    public int startingMagazineAmmoCount = 1;
    public int maxMagazineAmmoCount = 1;

    public int startingBoxAmmoCount = 1;
    public int maxBoxAmmoCount = 1;

    public float reloadTime = 1.0f;

    // Allows player to manually reload using input
    // useAmmo must be set true for this to have an effect
    public bool manualReload;

    // First, makes reloading progressive (one bullet at a time) and uses reloadTime as that time
    // Then, it allows shooting to interrupt the reloading (possible resulting in a non-full reload)
    // useAmmo and manualReload must be set true for this to have an effect
    public bool progressiveReloadInterruption;

    // This is for ammo pickups to tell which ammo goes where
    public AmmoType ammoType;

    [Header("Zoom Characteristics")]
    public bool useZoom;
    public float zoomTime = 0.15f;
    public float zoomedFieldOfView = 15.0f;
    public float zoomMovementModifier = 1.0f;
    public float zoomLookModifier = 1.0f;

    [Header("Recoil")]
    public float momentumRecoil = 0.0f;
    public float aimRecoil = 0.0f;

    [Header("Bullet Characteristics")]
    public float muzzleVelocity = 100.0f;
    public GameObject bulletPrefab;
    public Vector3 muzzleOffset;

    [Header("Effects")]
    public GameObject firingEffectsPrefab;
    public Vector3 firingEffectsOffset;

    [Header("Sounds")]
    public SoundAsset fireSound;
    public SoundAsset reloadSound;

    [Header("Meta Characteristics")]
    public bool usePooledBullets;
    public string poolIdentifier;
    public int poolSize;
}
