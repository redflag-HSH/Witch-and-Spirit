using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WhiteMage : Boss
{
    bool _isTurning;
    float _angleToPlayer;
    [Header("Player chase parameter")]
    [SerializeField] float _rotationPow;
    [SerializeField] float _turnDegree;
    [SerializeField] float _stareDegree;
    Transform _playerPos;

    BossStateMachine _stateMachine;

    [SerializeField] GameObject _phase1Model;
    public Animator MotionAnimator;
    [SerializeField] GameObject _phase2Model;
    public Animator Phase2Animator { get => _phase2Model.GetComponent<Animator>(); }

    [SerializeField] float _weakHeight;
    [SerializeField] Projectile _weakDagger;
    public AudioClip ShootSound;
    public Projectile ShootDagger;

    public SunlightYellowOverdrive SYO;
    public BossLaser2 ULTLsr2;

    public bool SecPhase = false;
    private bool _isPhaseChanging = false;

    [SerializeField] bool TestCase;

    public bool Ultmode { get; set; }
    int _UltStack;
    [SerializeField] int _maxUltStack;
    protected override void OnDeath()
    {
        //컷씬 넘긴 이후 밑줄 실행
        GameManager.Instance.Player.OnClear();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (TestCase)
            hp = 160;
        _stateMachine = GetComponent<BossStateMachine>();
        _stateMachine.ChangeState(new WhiteMageIdleState());
        _playerPos = GameManager.Instance.Player.transform;
    }

    private void Update()
    {
        StaringCheck();
        TurnCheck();
        RenderHealth();
    }
    private void RenderHealth()
    {
        BossHPGraphRenderer.Instance.Render(HP / (float)MaxHP);
    }

    //50% 남았을때 인지 아닌지 확인
    public bool SecondPhaseCheck()
    {
        if (HP < MaxHP * 0.6f && !_isPhaseChanging)
        {
            _isPhaseChanging = true;
            _stateMachine.ChangeState(new WhiteMageSecondIdle());
            _phase2Model.gameObject.SetActive(true);
            _phase1Model.SetActive(false);
            _phase2Model.GetComponent<Animator>().SetTrigger("In");
            SecPhase = true;
            return true;
        }
        else
            return false;
    }
    public void DaggerSummon()
    {
        GameObject ii = Instantiate(_weakDagger, new Vector3(_playerPos.position.x, _weakHeight, _playerPos.position.z), transform.rotation).gameObject;
        ii.transform.forward = Vector3.down;
        int a = Random.Range(0, 4);
        if (SecPhase)
            a = 3;
        ii.GetComponent<WhiteMageWeakS>().ModelSet(a);
        if (a == 3)
            a = 4;
        ii.GetComponent<WhiteMageWeakS>().ElementType = (ElementType)a;
        ii.GetComponent<WhiteMageWeakS>().Owner = this;
    }
    public override void Damage(int damage)
    {
        base.Damage(damage);
        if (Ultmode)
        {
            _UltStack += damage;
            if (_UltStack >= _maxUltStack)
            {
                _UltStack = 0;
                Ultmode = false;
                _stateMachine.ChangeState(new WhiteMageSecondIdle());
            }
        }
    }

    private void StaringCheck()
    {
        Vector3 targetDirection = _playerPos.position - transform.position;
        _angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
    }
    //각도 체크 및 뒤돌기 확인 함수
    private void TurnCheck()
    {
        if (!_isTurning && _angleToPlayer >= _turnDegree && _angleToPlayer >= 0)
        {
            _isTurning = true;
            StartCoroutine(TurnAround());
        }
    }
    //뒷 모습 보일경우 호출
    IEnumerator TurnAround()
    {
        yield return new WaitForSeconds(.7f);
        while (true)
        {
            yield return null;
            //플레이어가 일정 시야각 이내에 들어오지 않았을 경우
            if (_angleToPlayer > _stareDegree)
            {
                var euler = transform.eulerAngles;
                euler.y = Mathf.MoveTowardsAngle(euler.y, _angleToPlayer, _rotationPow * Time.deltaTime);
                transform.eulerAngles = euler;
            }
            else
                break;
        }
        _isTurning = false;
    }
}
