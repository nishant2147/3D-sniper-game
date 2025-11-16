using UnityEngine;
using UnityEngine.AI;

public class EnemyRandomMovement : MonoBehaviour
{

    public Transform[] waypoints;
    public float sampleRadius = 2f;
    public float waypointReachThreshold = 0.5f;
    public float waitAtWaypoint = 2f;
    public bool loop = true;
    public bool randomize = false;

    [Header("Movement Settings")]
    public float movementSpeed;

    [Header("Animation Settings")]
    public Animator animator;
    public string walkAnim = "isWalking";

    private NavMeshAgent agent;
    private int currentIndex = 0;
    private int direction = 1;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        agent.speed = movementSpeed;

        if (animator != null)
            animator.SetBool(walkAnim, true);
        GoToNextWaypoint(true);
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + waypointReachThreshold)
        {
            if (!isWaiting)
            {
                if (animator != null)
                    animator.SetBool(walkAnim, false);

                agent.isStopped = true;
                isWaiting = true;
                waitTimer = 0f;
            }

            if (isWaiting)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitAtWaypoint)
                {
                    isWaiting = false;
                    agent.isStopped = false;
                    GoToNextWaypoint();

                    if (animator != null)
                        animator.SetBool(walkAnim, true);
                }
            }
        }
    }

    void GoToNextWaypoint(bool first = false)
    {
        if (waypoints.Length == 0) return;

        if (randomize && !first)
        {
            currentIndex = Random.Range(0, waypoints.Length);
        }
        else if (!first)
        {
            currentIndex += direction;
            if (currentIndex >= waypoints.Length)
            {
                if (loop) currentIndex = 0;
                else
                {
                    currentIndex = waypoints.Length - 2;
                    direction = -1;
                }
            }
            else if (currentIndex < 0)
            {
                if (loop) currentIndex = waypoints.Length - 1;
                else
                {
                    currentIndex = 1;
                    direction = 1;
                }
            }
        }

        Vector3 target = waypoints[currentIndex].position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, sampleRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            agent.SetDestination(target);
        }
    }
    void OnEnable()
    {
        Movement.OnBulletFired += StopMovement;
        Movement.OnBulletDestroyed += StartMovement;
    }

    void OnDisable()
    {
        Movement.OnBulletFired -= StopMovement;
        Movement.OnBulletDestroyed -= StartMovement;
    }

    void StopMovement()
    {
        if (agent != null)
            agent.isStopped = true;
        if (animator != null)
            animator.SetBool(walkAnim, false);
    }

    void StartMovement()
    {
        if (agent != null)
            agent.isStopped = false;
        if (animator != null)
            animator.SetBool(walkAnim, true);
    }

}
