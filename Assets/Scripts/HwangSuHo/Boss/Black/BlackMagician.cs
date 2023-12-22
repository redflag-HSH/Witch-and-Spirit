using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMagician : Boss
{
    //퍼센트 체력바용 오버라이드 체력?
    bool _isTurning;
    float _angleToPlayer;
    [Header("Player chase parameter")]
    [SerializeField] float _rotationPow;
    [SerializeField] float _turnDegree;
    [SerializeField] float _stareDegree;
    
    public AudioClip ShootSound;
    public Projectile ShootProjectile;

    Player _player;

    public BossStateMachine StateMachine { get; private set; }

    [Header("Attack parameters")]
    public float FirstSlashReadyCount = 1.8f;
    public float SecondSlashReadyCount = 1.5f;
    public float ThirdSlashReadyCount = 3f;
    public float ThirdSlashAfterCount = 3f;
    public bool IsSlashing;

    [SerializeField] BlackLaserProjectile _projectile;

    public Animator MotionAnimator;
    public bool TestCase;

    protected override void OnDeath()
    {
        //컷씬 넘긴 이후 밑줄 실행
        GameManager.Instance.Player.OnClear();
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        transform.LookAt(_player.transform.position);
        StateMachine = GetComponent<BossStateMachine>();
        StateMachine.ChangeState(new BlackMagicianIdle());
    }

    // Update is called once per frame
    void Update()
    {
        StaringCheck();
        TurnCheck();
        RenderHealth();
    }
    private void RenderHealth()
    {
        BossHPGraphRenderer.Instance.Render(HP / (float)MaxHP);
    }
    //시야각 기록용 함수
    private void StaringCheck()
    {
        Vector3 targetDirection = _player.transform.position - transform.position;
        _angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
    }
    //각도 체크 및 뒤돌기 확인 함수
    private void TurnCheck()
    {
        if (!_isTurning && _angleToPlayer >= _turnDegree && _angleToPlayer >= 0 && !IsSlashing)
        {
            _isTurning = true;
            StartCoroutine(TurnAround());
        }
    }
    //뒷 모습 보일경우 호출
    IEnumerator TurnAround()
    {
        yield return new WaitForSeconds(.7f);
        print("TurnStart");
        while (true)
        {
            yield return null;
            if (IsSlashing)
                break;
            //플레이어가 일정 시야각 이내에 들어오지 않았을 경우
            else if (_angleToPlayer > _stareDegree)
            {
                transform.Rotate(Vector3.up * _rotationPow * Time.deltaTime);
                print("Turning");
            }
            /* else if (_angleToPlayer <= -_stareDegree && _angleToPlayer >= 0)
                 transform.Rotate(transform.right * _rotationPow);*/
            else
                break;
        }
        _isTurning = false;
        print("TurningEnd");
    }
    public void LaserSummon(bool switc)
    {
        _projectile.gameObject.SetActive(switc);
        _projectile.IsRotating = switc;
    }
    public void ResetPattern()
    {
        StateMachine.ChangeState(new BlackMagicianIdle());
    }
}
