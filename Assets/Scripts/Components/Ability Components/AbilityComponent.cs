using UnityEngine;

public class AbilityComponent : MonoBehaviour {

    private static PooledGameObjectManager pooledManager;

    [Header("Ability Component Data")]
    public AbilityData[] abilityDatas;
    public Transform castTransform;

    public int selectedAbilityIndexA;
    public int selectedAbilityIndexB;

    private enum CastingState {
        idle,
        precasting,
        coolingDown,
    }

    private class RuntimeAbilityData {
        public CastingState castingState;
        public Timer castTimer;
        public Timer cooldownTimer;
    }

    private RuntimeAbilityData[] runtimeAbilityData;

    protected FirstPersonPlayerComponent player;

    protected void Awake(){

    }

	protected void Start(){
        if(castTransform == null){
            Logger.Error("Muzzle Actor on " + gameObject.name + "'s GunComponent cannot be null");
        }

        runtimeAbilityData = new RuntimeAbilityData[abilityDatas.Length];
        for(int i = 0, count = abilityDatas.Length; i < count; ++i){
            runtimeAbilityData[i] = new RuntimeAbilityData();
            runtimeAbilityData[i].cooldownTimer = new Timer(abilityDatas[i].castTime);
            runtimeAbilityData[i].castTimer = new Timer(abilityDatas[i].cooldownTime);
        }

        player = GetComponent<FirstPersonPlayerComponent>();
	}

    void Update(){
        // Capture player inputs
        if(Input.GetKeyUp(KeyCode.Mouse0) && CanCast(selectedAbilityIndexA)){
            StartCasting(selectedAbilityIndexA);
        }

        if(Input.GetKeyUp(KeyCode.Mouse1) && CanCast(selectedAbilityIndexB)){
            StartCasting(selectedAbilityIndexB);
        }

        // Update abilities as needed
        for(int i = 0, count = runtimeAbilityData.Length; i < count; ++i){
            RuntimeAbilityData rad = runtimeAbilityData[i];

            if(rad.castTimer.Finished() && rad.castingState == CastingState.precasting){
                ExecuteCast(i);
                rad.castingState = CastingState.coolingDown;
                rad.cooldownTimer.Start();
            }
            if(rad.cooldownTimer.Finished() && rad.castingState == CastingState.coolingDown){
                rad.castingState = CastingState.idle;
            }
        }
    }

    public bool CanCast(int abilityIndex){
        return runtimeAbilityData[abilityIndex].castingState == CastingState.idle;
    }

    public void StartCasting(int abilityIndex){
        if(CanCast(abilityIndex)){
            RuntimeAbilityData rad = runtimeAbilityData[abilityIndex];

            rad.castTimer.Start();
            rad.castingState = CastingState.precasting;
        }
    }

    private void ExecuteCast(int abilityIndex){
        AbilityData abilityData = abilityDatas[abilityIndex];

        if(abilityData.abilityType == AbilityData.AbilityType.SpawnObject){
            Vector3 spawnPosition = castTransform.position
                                    + (castTransform.right * abilityData.spawnOffsetFromCastTransform.x)
                                    + (castTransform.up * abilityData.spawnOffsetFromCastTransform.y)
                                    + (castTransform.forward * abilityData.spawnOffsetFromCastTransform.z);

            GameObject spawnedObject = GameObject.Instantiate(abilityData.objectToSpawn);
            spawnedObject.transform.position = spawnPosition;
            spawnedObject.transform.rotation = castTransform.rotation;

            ProjectileComponent projectile = spawnedObject.GetComponent<ProjectileComponent>();
            if(projectile != null){
                // TODO figure out how to source this data
                projectile.Fire(1.0f, DamageType.Projectile, castTransform.forward * 10.0f, gameObject);
            }
        } else if(abilityData.abilityType == AbilityData.AbilityType.DealDamageWithShape){
            // TODO
        }
    }
}
