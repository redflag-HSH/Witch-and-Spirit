using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMagician : Entity
{
    //�ۼ�Ʈ ü�¹ٿ� �������̵� ü��?
    bool _isTurning;
    float _angleToPlayer;
    [Header("Player chase parameter")]
    [SerializeField] float _rotationPow;
    [SerializeField] float _turnDegree;
    [SerializeField] float _stareDegree;

    Player _player;

    public BossStateMachine StateMachine { get; private set; }

    [SerializeField] BlackLaserProjectile _projectile;

    public Animator MotionAnimator;
    public bool TestCase;
    protected override void OnDeath()
    {
        //�ƾ� �ѱ� ���� ���� ����
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
    //�þ߰� ��Ͽ� �Լ�
    private void StaringCheck()
    {
        Vector3 targetDirection = _player.transform.position - transform.position;
        _angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
    }
    //���� üũ �� �ڵ��� Ȯ�� �Լ�
    private void TurnCheck()
    {
        if (!_isTurning && _angleToPlayer >= _turnDegree && _angleToPlayer >= 0)
        {
            _isTurning = true;
            StartCoroutine(TurnAround());
        }
    }
    //�� ��� ���ϰ�� ȣ��
    IEnumerator TurnAround()
    {
        yield return new WaitForSeconds(.7f);
        print("TurnStart");
        while (true)
        {
            yield return null;
            //�÷��̾ ���� �þ߰� �̳��� ������ �ʾ��� ���
            if (_angleToPlayer > _stareDegree)
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
