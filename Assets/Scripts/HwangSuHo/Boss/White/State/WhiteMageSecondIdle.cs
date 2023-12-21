using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteMageSecondIdle : BossBaseState
{
    float _count = 0, _maxcount = 1;
    public override void Enter()
    {
        Debug.Log("세컨드 입장");
    }
    public override void Exit()
    {
    }

    public override void Perform()
    {
        if (_count < _maxcount)
            _count += Time.deltaTime;
        else
        {
            float aa = Random.Range(0, 3);
            int a = Mathf.FloorToInt(aa);
            switch (a)
            {
                case 0:
                    StateMachine.ChangeState(new WhiteMageSYOState());
                    break;
                case 1:
                    StateMachine.ChangeState(new WhiteMageUltimateLaser());
                    break;
                case 2:
                    StateMachine.ChangeState(new WhiteMageWeakShotState());
                    break;
            }
            _count = 0;
            //아이들 카운트 if 문
        }
    }
}
