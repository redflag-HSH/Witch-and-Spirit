using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMagicianClaw : BossBaseState
{
    float _count;
    float _readyCount=0.5f;
    float _afterCount=3.5f;
    public override void Enter()
    {

    }

    public override void Exit()
    {
        StateMachine.BlackMagician.IsSlashing = false;
    }

    public override void Perform()
    {
        if (!StateMachine.BlackMagician.IsSlashing)
        {
            if (_count < _readyCount)
                _count += Time.deltaTime;
            else
            {
                StateMachine.BlackMagician.IsSlashing = true;
                StateMachine.BlackMagician.MotionAnimator.SetTrigger("Claw");
                _count = 0;
            }
        }
        else
        {
            if (_count < _afterCount)
                _count += Time.deltaTime;
            else
            {
                StateMachine.ChangeState(new BlackMagicianIdle());
            }
        }
    }
}
