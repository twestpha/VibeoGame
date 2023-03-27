using UnityEngine;

public class AbilityEffectsComponent : MonoBehaviour {

    protected AbilityComponent castingAbilityComponent;
    protected AbilityData abilityData;

    public void Setup(AbilityComponent castingAbilityComponent_, AbilityData abilityData_){
        castingAbilityComponent = castingAbilityComponent_;
        abilityData = abilityData;
    }

    public virtual void EffectsStart(){}

    public virtual void EffectsUpdate(){}

    public virtual void EffectsFinish(){}
}