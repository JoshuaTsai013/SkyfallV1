using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
  Transform PlayerPos;
  NavMeshAgent agent;
  public Animator animator;
  private CharacterGeneral characterGeneral;
  public float sightRange = 10f;
  public float attackRange = 10f;
  [SerializeField]
  private bool Hit = false;
  public float walkPointRange;
  public Transform patrolCenter;
  public float patrolRadius = 10f;
  public float minStopTime = 2f;
  public float maxStopTime = 5f;
  private bool isPatrollingStopped = false;

  public GameObject MiniGun;

  private void Start()
  {
    PlayerPos = PlayerManager.instance.player.transform;
    characterGeneral = GetComponent<CharacterGeneral>();
    if (characterGeneral)
    {
      characterGeneral.OnHit.AddListener(HandleHit);
    }
    agent = GetComponent<NavMeshAgent>();
    Patroling();
    MiniGun.SetActive(false);
  }

  private void Update()
  {
    float distance = Vector3.Distance(PlayerPos.position, transform.position);

    if (distance <= attackRange || Hit)
    {
      Attacking();
    }
    else if (distance <= sightRange)
    {
      Chasing();
    }
    else if (distance >= sightRange)
    {
      Patroling();
    }
  }

  private void Patroling()
  {
    MiniGun.SetActive(false);
 
    if (isPatrollingStopped)
    {
      agent.isStopped = true;
      animator.SetBool("Walk", false);
      return;
    }

    if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
    {
      Vector3 randomPoint = RandomPointAroundCenter();

      if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
      {
        agent.isStopped = false;
        agent.SetDestination(hit.position); 
        animator.SetBool("Walk", true);
      }

      StartRandomPatrolStop();
    }
  }

  private void StartRandomPatrolStop()
  {
    if (!isPatrollingStopped)
    {
      isPatrollingStopped = true;
      agent.isStopped = true;     
      float stopDuration = Random.Range(minStopTime, maxStopTime); 
      Invoke(nameof(ResumePatrolling), stopDuration); 
    }
  }

  private void ResumePatrolling()
  {
    if (agent.isOnNavMesh) 
    {
      isPatrollingStopped = false;
      agent.isStopped = false;
      animator.SetBool("Walk", true);
      Patroling();
    }
    else
    {
      Debug.Log("Agent is not on a NavMesh. Cannot resume patrolling.");
    }
  }

  Vector3 RandomPointAroundCenter()
  {
    float angle = Random.Range(0f, Mathf.PI * 2);
    Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * patrolRadius;
    return patrolCenter.position + offset;
  }

  private void Attacking()
  {
    agent.SetDestination(transform.position);
    transform.LookAt(PlayerManager.instance.player.transform);
    animator.SetBool("Walk", false);
    animator.SetBool("Attack", true);
    MiniGun.SetActive(true);
  }

  public void Chasing()
  {
    agent.SetDestination(PlayerPos.position);
    animator.SetBool("Walk", true);
    animator.SetBool("Attack", false);
    MiniGun.SetActive(false);
  }

  private void HandleHit()
  {
    Hit = true;
    Invoke(nameof(ResetAttacking), 2.0f);
  }

  private void ResetAttacking()
  {
    Hit = false;
  }
}