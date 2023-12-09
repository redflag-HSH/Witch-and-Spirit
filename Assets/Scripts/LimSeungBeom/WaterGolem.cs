using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class WaterGolem : Enemy
{
    GameObject Player;
    GameObject WarningRotator;
    GameObject Warning;
    bool IsAiming;
    Vector3 SettedPlayerPosition;
    
    void Start()
    {
        WarningRotator = transform.GetChild(0).gameObject;
        WarningRotator.SetActive(false);

        Player = GameManager.Instance.Player.gameObject;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (IsAiming)
        {
            WarningRotator.transform.LookAt(new Vector3(Player.transform.position.x, Player.transform.position.y - 0.3f, Player.transform.position.z));
        }
        if(IsAiming == false)
        {
            WarningRotator.transform.LookAt(new Vector3(SettedPlayerPosition.x, SettedPlayerPosition.y - 0.3f, SettedPlayerPosition.z));
        }
    }
    protected override void Attack()
    {
        StartCoroutine(WaterGolemAttack());
    }

    IEnumerator WaterGolemAttack()
    {
        StartCoroutine(Stop(8));
        IsAiming = true;
        WarningRotator.SetActive(true);

        yield return new WaitForSeconds(3); //3�ʵ��� �غ� �� ȸ�� ����.-------------------------------------------------
        IsAiming = false;
        SettedPlayerPosition = Player.transform.position;

        yield return new WaitForSeconds(2); // 2���� ���� �÷��̾ �ִ� ��ġ�� �߻�.
        Projectile bullet = Instantiate(Data.projectile,transform.position, Quaternion.identity);
        bullet.IsEnemyProjectile = true;
        bullet.transform.LookAt(SettedPlayerPosition);
        WarningRotator.transform.LookAt(new Vector3(SettedPlayerPosition.x, SettedPlayerPosition.y - 1, SettedPlayerPosition.z));

        yield return new WaitForSeconds(1);  // ���� ������ �ٲ㼭 �� ���� ��. 
        WarningRotator.SetActive(false);

    }
}
