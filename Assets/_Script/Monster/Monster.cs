using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Monster : MonoBehaviour
{
    [field: SerializeField]public List<Transform> Waypoints { get; set; }
    public MonsterStateMachine MonsterStateMachine { get; set; }
    public SearchState SearchState { get; set; }
    public ChaseState ChaseState { get; set; }
    public CatchState CatchState { get; set; }
    public NavMeshAgent NavAgent { get { return _navAgent; } set { } }
    public MonsterVision MonsterVision { get; set; }

    [SerializeField]private SpriteRenderer minimapIconSprite;
    private NavMeshAgent _navAgent;
    private Animator _anim;
    private AudioSource _audioSource;
    public bool IsPlayerWithinRange { get; set; }


    [Header("Search State fields")]
    [SerializeField] private float searchMovementSpeed;
    public float SearchMovementSpeed { get { return searchMovementSpeed; } set { } }


    [Header("Chase State fields")]
    [SerializeField] private float chaseMovementSpeed;
    public float ChaseMovementSpeed { get { return chaseMovementSpeed; } set { } }

    private void Awake()
    {
        MonsterVision = GetComponent<MonsterVision>();
        _navAgent = GetComponent<NavMeshAgent>();
        _anim = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
        MonsterStateMachine = new MonsterStateMachine();
        SearchState = new SearchState(MonsterStateMachine, this);
        ChaseState = new ChaseState(MonsterStateMachine, this);
        CatchState = new CatchState(MonsterStateMachine, this);
    }

    private void Start()
    {
        MonsterStateMachine.Initialize(SearchState);
        GameEvents.Instance.TriggerMonsterSpawned(this);
        InvokeRepeating("MonsterSound", 1, 5);
    }
    private void Update()
    {
        MonsterStateMachine.CurrentMonsterState.FrameUpdate();
    }
    private void FixedUpdate()
    {
        MonsterStateMachine.CurrentMonsterState.PhysicsUpdate();
    }
    private void OnEnable()
    {
        GameEvents.Instance.OnCardAbilityUsed += ApplyMonsterAbilityEffect;
    }
    private void OnDisable()
    {
        GameEvents.Instance.OnCardAbilityUsed -= ApplyMonsterAbilityEffect;
    }

    public void MoveTowards(Vector3 position, float speed)
    {
        NavAgent.speed = speed;
        NavAgent.SetDestination(position);
    }
    public void UpdateAnimations()
    {
        _anim.SetBool("Walking", _navAgent.velocity.magnitude > 0.02f);
    }

    public void SetAggroStatus(bool isPlayerWithinRange)
    {
        IsPlayerWithinRange = isPlayerWithinRange;
    }
    public void MonsterSound()
    {
        AudioManager.Instance.PlaySound(SoundList.MonsterSound, 1, _audioSource);
    }

    private void ApplyMonsterAbilityEffect(CardAbilityType type, float duration)
    {
        switch (type)
        {
            case CardAbilityType.Reveal:
                StartCoroutine(ApplyRevealCO(duration));
                break;
            case CardAbilityType.Stall:
                StartCoroutine(ApplySpeedDebuffCO(duration));
                break;
            default:
                break;
        }
    }
    IEnumerator ApplyRevealCO(float dur)
    {
        minimapIconSprite.gameObject.SetActive(true);
        yield return new WaitForSeconds(dur);
        minimapIconSprite.gameObject.SetActive(false);
    }
    IEnumerator ApplySpeedDebuffCO(float dur)
    {
        float originalSpeed = _navAgent.speed;
        _navAgent.speed = 0;
        _navAgent.enabled = false;
        yield return new WaitForSeconds(dur);
        _navAgent.enabled = true;
        _navAgent.speed = originalSpeed;
    }
    private void OnDrawGizmos()
    {
        foreach(Transform waypoint in Waypoints)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(waypoint.position, 1);
        }
    }
}
