using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteMageIdleState : BossBaseState
{
    float _count = 0, _maxcount = 1;
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Perform()
    {
        if (StateMachine.WhiteMagician.SecondPhaseCheck())
            StateMachine.ChangeState(new WhiteMageSecondIdle());
        else if (_count < _maxcount)
            _count += Time.deltaTime;
        else
        {
            int a = 1;
            switch (a)
            {
                case 0:
                    //StateMachine.ChangeState();
                    break;
            }
            //���̵� ī��Ʈ if ��
        }
        //���� ��� �ִϸ��̼� ���
        //���� ����ġ�� Ʋ��
    }
}
