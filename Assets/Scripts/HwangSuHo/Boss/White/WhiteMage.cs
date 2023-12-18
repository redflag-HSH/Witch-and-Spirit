using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteMage : Entity
{
    BossStateMachine _stateMachine;

    [SerializeField] GameObject _phase1Model;
    public Animator MotionAnimator;
    [SerializeField] GameObject _phase2Model;

    public SunlightYellowOverdrive SYO;
    public BossLaser2 ULTLsr2;

    [SerializeField] bool TestCase;

    public bool Ultmode { get; set; }
    int _UltStack;
    [SerializeField] int _maxUltStack;
    protected override void OnDeath()
    {
        //�ƾ� �ѱ� ���� ���� ����
        GameManager.Instance.Player.OnClear();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (TestCase)
            hp = 160;
        _stateMachine = GetComponent<BossStateMachine>();
        _stateMachine.ChangeState(new WhiteMageIdleState());
    }
    private void Update()
    {
        RenderHealth();
    }
    private void RenderHealth()
    {
        BossHPGraphRenderer.Instance.Render(HP / (float)MaxHP);
    }

    //50% �������� ���� �ƴ��� Ȯ��
    public bool SecondPhaseCheck()
    {
        if (HP < 150)
        {
            StartCoroutine(PhaseTwoIn());
            return true;
        }
        else
            return false;
    }
    IEnumerator PhaseTwoIn()
    {
        MotionAnimator.gameObject.SetActive(true);
        _phase1Model.SetActive(false);
        yield return new WaitForSeconds(1);
        MotionAnimator.gameObject.SetActive(false);
        _phase2Model.SetActive(true);
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
}
