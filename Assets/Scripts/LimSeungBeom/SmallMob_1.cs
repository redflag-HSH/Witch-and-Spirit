using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SmallMob_1 : MonoBehaviour
{

    [SerializeField] int Direction = -1;

    [SerializeField] float MoveDistance;
    [SerializeField] float MinMoveDistance;
    [SerializeField] float MaxMoveDistance;

    [SerializeField] float MoveSpeed;
    [SerializeField] float MoveWaitTime;
    [SerializeField] float MinMoveWaitTime;
    [SerializeField] float MaxMoveWaitTime;

    float AttackWaitTime;
    [SerializeField] float MaxAttackWaitTime;
    [SerializeField] float MiNAttackWaitTime;

    [Header("��ź ������ ������ ����. 5 ����")]
    [SerializeField] public float Spread;
    [Header("��ź ���ݿ��� ������ �Ѿ� ��")]
    [SerializeField] public int _AmountOfBullet;
    [Header("��ź ���� �Ѿ� ���� �ֱ�. 0.05 ����")]
    [SerializeField] public float _SpreadCooltime;

    GameObject Player;
    [SerializeField] GameObject Projectile;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        StartCoroutine(MobMove());
        StartCoroutine(Attack());

    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(1 * Direction * MoveDistance, transform.position.y, transform.position.z), Time.deltaTime * MoveSpeed);
    }
    IEnumerator MobMove()
    {
        //------------------ ���� ����--------------
        Vector3 viewpos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position); // ī�޶� ���� ������ �����ִ��� Ȯ��
        if (viewpos.x <= 0) { Direction = 1; }
        if (viewpos.x >= 1) { Direction = -1; }
        if (0 < viewpos.x && viewpos.x < 1)
        {
            Direction = Random.Range(-1, 1);
            if (Direction == 0) Direction = 1;
        }

        MoveDistance = Random.Range(MinMoveDistance, MaxMoveDistance + 1); // �̵� �Ÿ�
        MoveWaitTime = Random.Range(MinMoveWaitTime, MaxMoveWaitTime + 1); // �̵� �� �����̵����� ���ð�
        Direction = Random.Range(-1, 1);
        if (Direction == 0) Direction = 1;


        yield return new WaitForSeconds(MoveWaitTime);                          //���� ��ٷ����� ���� �̵� ����
        StartCoroutine(MobMove());
    }

    IEnumerator Attack()
    {
        AttackWaitTime = Random.Range(MiNAttackWaitTime,MaxAttackWaitTime + 1);
        Vector3 SettedPlayerPosition = Player.transform.position;

        for(int i = 0; i < _AmountOfBullet;  i++)
        {
            GameObject bullet = Instantiate(Projectile, transform.position, Quaternion.identity);
            bullet.transform.LookAt(new Vector3((SettedPlayerPosition.x - Spread) + (i * (Spread * 2 / _AmountOfBullet)), SettedPlayerPosition.y, SettedPlayerPosition.z));
            yield return new WaitForSeconds(_SpreadCooltime);
        }



        yield return new WaitForSeconds(AttackWaitTime);
        StartCoroutine (Attack());
    }

}
