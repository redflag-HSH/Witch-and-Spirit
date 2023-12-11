using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteMage : Entity
{
    BossStateMachine _stateMachine;
    public Animator MotionAnimator;
    public SunlightYellowOverdrive SYO;
    public BossLaser2 ULTLsr2;
    protected override void OnDeath()
    {
        //�ƾ� �ѱ� ���� ���� ����
        GameManager.Instance.Player.OnClear();
    }

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = GetComponent<BossStateMachine>();
        _stateMachine.ChangeState(new WhiteMageIdleState());
    }
    private void Update()
    {
        RenderHealth();
    }
    private void RenderHealth()
    {
        BossHPGraphRenderer.Instance.Render(HP / MaxHP);
    }

    //50% �������� ���� �ƴ��� Ȯ��
    public bool SecondPhaseCheck()
    {
        if (HP < 500)
            return true;
        else
            return false;
    }
}
