using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMagicianClaw : BossBaseState
{
    float _count;
    float _readyCount;
    float _afterCount;
    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Perform()
    {
        if (_count < _readyCount)
            _count += Time.deltaTime;
        else
        {

            //�� �ֵθ��� �ִϸ��̼�
            //Hovl ��Ʃ����� ���� ��ũ���̺� �ִϸ��̼�
            //���� �ִϸ��̼�
            //dile�� �ѱ��
        }
    }
}
