using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteMageWeakShotState : BossBaseState
{
    public override void Enter()
    {
        StateMachine.WhiteMagician.Phase2Animator.SetBool("Cast", true);
        //if(StateMachine.WhiteMagician.SecPhase)
        //�Ӽ� ����  

        //������Ʈ Ǯ������ ó���ϰ�
    }

    public override void Exit()
    {
        StateMachine.WhiteMagician.Phase2Animator.SetBool("Cast", false);
    }

    public override void Perform()
    {
        //��ٷȴٰ� idle��
    }
}
