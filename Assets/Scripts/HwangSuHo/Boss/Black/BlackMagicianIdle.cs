using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMagicianIdle : BossBaseState
{
    float _readyCount;
    float _countFull = 5;
    public override void Enter()
    {
        _readyCount = 0;
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if (_readyCount < _countFull)
            _readyCount += Time.deltaTime;
        else
            SelectNextPattern();
    }
    private void SelectNextPattern()
    {
        int state = Random.Range(0, 2);
        switch (state)
        {
            case 0:
                StateMachine.ChangeState(new BlackMagicianSlash());
                break;
            case 1:
                StateMachine.ChangeState(new BlackMagicianClaw());
                break;
        }
    }
}
