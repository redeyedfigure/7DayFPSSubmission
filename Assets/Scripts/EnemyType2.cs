using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyType2 : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //PlayerInfo
    public HealthBar healthSter;
    public AudioSource gunsound;
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    //Attacking
    public float sightRange, attackRange;
    private float countDownToShoot;
    public int attackDamage;
    public float timeBetweenAttacks;
    public bool playerInSightRange, playerInAttackRange;
    public bool alreadyAttacked;
    public LayerMask viewMask;
    public bool CanSeePlayer;
    public float pullTriggerTime;
    

    private void Awake(){
        player = GameObject.Find("player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update(){
        if(!Physics.Linecast(transform.position, player.position, viewMask)){
            CanSeePlayer = true;
        }
        else if(Physics.Linecast(transform.position, player.position, viewMask)){
            CanSeePlayer = false;
        }
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange) Patroling();
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if(playerInSightRange && playerInAttackRange && CanSeePlayer) AttackPlayer();
        if(!CanSeePlayer && playerInAttackRange && playerInSightRange) ChasePlayer();
    }
    private void Patroling(){
        if(!walkPointSet) SearchWalkPoint();
        if(walkPointSet)
        agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if(distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint(){
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    private void ChasePlayer(){
        agent.SetDestination(player.position);
    }
    private void AttackPlayer(){
        ///Attack code
        //AttackAI
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if(CanSeePlayer){
            countDownToShoot += Time.deltaTime;
        }
        else if(!CanSeePlayer){
            countDownToShoot -= Time.deltaTime;
        }
        if(!alreadyAttacked && CanSeePlayer == true && countDownToShoot >= pullTriggerTime){
            healthSter.SubtractHealth(attackDamage);
            gunsound.Play();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack(){
        alreadyAttacked = false;
    }
}
