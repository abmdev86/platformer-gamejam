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

        private Animator enemyAnimController;




        private void Start()
        {

            try
            {
                InitiateActor(actorData);
                enemyAgent = GetComponent<NavMeshAgent>();
                SetUpNavMeshAgent(enemyAgent, enemySpeed);



                enemyAnimController = GetComponentInChildren<Animator>();
                transform.forward = new Vector3(1, 0, 0);


            }
            catch (UnityException ex)
            {
                Debug.LogError(ex.Message);
            }


        }
        private void Update()
        {
            if (!enemyAgent.hasPath)
            {
                enemyAgent.SetDestination(GetRandomPoint.Instance.GetPoint());
            }
            else if (enemyAgent.velocity == Vector3.zero)
            {
                enemyAgent.ResetPath();
            }
            print(enemyAgent.hasPath + " has path");

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
