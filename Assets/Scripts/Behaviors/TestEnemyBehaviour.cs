using UnityEngine;

public class TestEnemyBehaviour : SpriteEnemyBehavior {

    public const float UDPATE_RADIUS = 45.0f;

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

        float playerDistance = (transform.position - FirstPersonPlayerComponent.player.transform.position).magnitude;

        // any detection of the player during the patrol stops and shoot
        if(enemyState != EnemyState.Attacking && playerDistance < SHOOT_RADIUS){
            enemyState = EnemyState.Attacking;
            StopMoving();
            // rotation.SetAnimationIndex(SHOOT_ANIMATION_INDEX);
        }

        // Pretty simple finite state machine
        if(enemyState == EnemyState.Idle){
            if(patrolIdleTimer.Finished()){
                enemyState = EnemyState.Patrolling;
                MoveToPosition(patrollingAToB ? patrolPointB.position : patrolPointA.position);
                // rotation.SetAnimationIndex(WALK_ANIMATION_INDEX);
                patrollingAToB = !patrollingAToB;
            }
        } else if(enemyState == EnemyState.Patrolling){
            if(AtGoal()){
                patrolIdleTimer.Start();
                enemyState = EnemyState.Idle;
                // rotation.SetAnimationIndex(IDLE_ANIMATION_INDEX);
            }
        } else if(enemyState == EnemyState.Attacking){
            if(playerDistance > SHOOT_RADIUS){
                enemyState = EnemyState.Idle;
                patrolIdleTimer.Start();
            }

            // bool hasShootToken = AttackTokenComponent.RequestToken(gameObject);

            // if(shootTimer.Finished() && hasShootToken){
            //     Vector3 toPlayer = FirstPersonPlayerComponent.player.transform.position - transform.position;
            //     toPlayer.y = 0.0f;
            //     transform.rotation = Quaternion.LookRotation(toPlayer);
            //
            //     gun.Shoot();
            //     shootTimer.Start();
            //     rotation.SetAnimationIndex(IDLE_ANIMATION_INDEX);
            // }
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
