using System.Collections;
using System.Collections.Generic;
using sluggagames.jumper.Data;
using sluggagames.jumper.Data.Helpers;
using UnityEngine;
using UnityEngine.AI;

namespace sluggagames.jumper.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]


    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private ActorSO actorData;
        [SerializeField]
        private float radius;
        private GameObject enemyModel;
        private float enemyHealth;
        private int enemyAttackPower;
        private int enemyDefense;
        private float enemySpeed;
        private NavMeshAgent enemyAgent;
        private EnemyVisibility vision;
        private Transform playerTransform;

        private Animator enemyAnimController;

        private bool isPlayerVisible = false;


        private void Start()
        {

            try
            {
                vision = GetComponentInChildren<EnemyVisibility>();
                InitiateActor(actorData);
                enemyAgent = GetComponent<NavMeshAgent>();
                SetUpNavMeshAgent(enemyAgent, enemySpeed);



                enemyAnimController = GetComponentInChildren<Animator>();
                transform.forward = enemyAgent.velocity.normalized;
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            }
            catch (UnityException ex)
            {
                Debug.LogError(ex.Message);
            }


        }
        private void Update()
        {
            isPlayerVisible = vision.CheckVisibilityOriginal();
            print($"Target visible? {isPlayerVisible}");
            if (!isPlayerVisible)
            {
                EnemyMover();

            }
            else
            {

                print("I see player and should attack!" + isPlayerVisible);
            }

        }

        private void EnemyMover()
        {

            if (!enemyAgent.hasPath)
            {
                enemyAgent.SetDestination(GetRandomPoint.Instance.GetPoint(transform, radius));

            }
            else if (enemyAgent.velocity == Vector3.zero)
            {
                enemyAgent.ResetPath();
            }
            transform.forward = enemyAgent.velocity;

        }

        private void InitiateActor(ActorSO actor)
        {
            enemyHealth = actor.ActorHealth;
            enemyModel = actor.ActorModel;
            enemyModel = Instantiate(enemyModel, transform.position, Quaternion.identity);
            enemyModel.transform.SetParent(this.transform);
            enemySpeed = actor.ActorSpeed;
            enemyDefense = actor.Defense;
            enemyAttackPower = actor.AttackPower;
        }

        private void SetUpNavMeshAgent(NavMeshAgent agent, float speed, float angularSpeedModifier = 10f)
        {
            // agent
            agent.speed = speed;
            agent.angularSpeed = agent.speed * angularSpeedModifier;
            agent.acceleration = agent.speed + 3;
            agent.stoppingDistance = 0.3f;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
#endif
    }
}
