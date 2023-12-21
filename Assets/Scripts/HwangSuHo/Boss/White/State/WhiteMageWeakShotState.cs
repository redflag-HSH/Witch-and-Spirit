using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteMageWeakShotState : BossBaseState
{
    int _index;
    bool _isLight = false;
    float _count;
    float _eachCount;
    public override void Enter()
    {
        StateMachine.WhiteMagician.Phase2Animator.SetBool("Cast", true);

        if (StateMachine.WhiteMagician.SecPhase)
            _isLight = true;
        float count = Random.Range(0, 10);
        _index = Mathf.FloorToInt(count);
    }

    public override void Exit()
    {
        StateMachine.WhiteMagician.Phase2Animator.SetBool("Cast", false);
    }

    public override void Perform()
    {
        if (_index <= 0)
        {
            if(StateMachine.WhiteMagician.SecPhase)
                StateMachine.ChangeState(new WhiteMageSecondIdle());
            else
                StateMachine.ChangeState(new WhiteMageIdleState());
        }
        else if (_count < .3f)
            _count += Time.deltaTime;
        else
        {
            _count = 0;
            _index--;
            StateMachine.WhiteMagician.DaggerSummon();
        }
        //기다렸다가 idle로
    }
}
