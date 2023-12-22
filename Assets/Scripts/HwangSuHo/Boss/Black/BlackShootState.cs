using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackShootState : BossBaseState
{
    public override void Enter()
    {
        int count = Random.Range(10, 20);
        GameManager.Instance.StartCoroutine(ShootDagger(count));
    }

    public IEnumerator ShootDagger(int count)
    {
        for(int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.5f);
            var dagger = Object.Instantiate(StateMachine.BlackMagician.ShootProjectile);
            float angle = Random.Range(0, 360 / 10) * 10f;
            dagger.transform.eulerAngles = new(0, angle, 0);
            var pos = StateMachine.BlackMagician.transform.position + dagger.transform.forward * 4f;
            pos.y = GameManager.Instance.Player.transform.position.y;
            dagger.transform.position = pos;
            GameManager.Instance.SoundManager.PlaySFX(StateMachine.BlackMagician.ShootSound, dagger.transform.position, 1, 1);
        }

        StateMachine.ChangeState(new BlackMagicianIdle());
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {

    }
}
