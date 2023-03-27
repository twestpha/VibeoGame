using UnityEngine;

public class TestEnemyBehaviour : SpriteEnemyBehavior {

    public const float UDPATE_RADIUS = 800.0f;

    public const float SHOOT_RADIUS = 12.0f;

    public const int IDLE_ANIMATION_INDEX = 0;
    public const int WALK_ANIMATION_INDEX = 1;
    public const int SHOOT_ANIMATION_INDEX = 2;

    public enum EnemyState {
        Idle,
        Patrolling,
        Attacking,
    }

    public EnemyState enemyState;

    public float patrolIdleTime;
    public float shootCooldown;

    public Transform patrolPointA;
    public Transform patrolPointB;

    public Transform muzzleTransform;

    public BarkComponent bark;

    public GameObject bodyToSpawn;

    private bool patrollingAToB;

    private Timer patrolIdleTimer;
    private Timer shootTimer;
    private Timer updateTimer = new Timer(2.0f);

    // private GunComponent gun;

    //##############################################################################################
    // Make sure to call base start, then setup some simple state
    //##############################################################################################
    protected override void Start(){
        base.Start();

        patrollingAToB = true;

        patrolIdleTimer = new Timer(patrolIdleTime);
        shootTimer = new Timer(shootCooldown);

        // gun = GetComponent<GunComponent>();

        patrolIdleTimer.Start();
    }

    //##############################################################################################
    // If we ever encounter the player, shoot at them. Otherwise, patrol from A to B, wait, then
    // patrol back.
    //##############################################################################################
    public override void EnemyUpdate(){
        base.EnemyUpdate();

        Vector3 toPlayer = transform.position - FirstPersonPlayerComponent.player.transform.position;
        float playerDistance = (toPlayer).magnitude;

        if(updateTimer.Finished()){
            updateTimer.Start();
            MoveToPosition(FirstPersonPlayerComponent.player.transform.position);
        }

        // Make sure to re-register for next frames update if we're still near enough!
        if(playerDistance < UDPATE_RADIUS){
            EnemyManagerComponent.RegisterUpdate(this);
        }
    }

    //##############################################################################################
    // Play a grunt bark when damaged
    //##############################################################################################
    public override void PlayDamagedSequence(){
        bark.Bark();

        base.PlayDamagedSequence();
    }

    //##############################################################################################
    // Spawn a body prefab when killed
    //##############################################################################################
    public override void PlayDeathSequence(){
        GameObject spawnedBody = GameObject.Instantiate(bodyToSpawn);
        spawnedBody.transform.position = transform.position;

        base.PlayDeathSequence();
    }
}
