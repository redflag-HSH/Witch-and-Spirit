using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGolem : MonoBehaviour
{
    [SerializeField] int Direction = -1;

    [SerializeField] float MoveDistance;
    [SerializeField] float MinMoveDistance;
    [SerializeField] float MaxMoveDistance;

    [SerializeField] float MoveSpeed;
    [SerializeField] float MoveWaitTime;
    [SerializeField] float MinMoveWaitTime;
    [SerializeField] float MaxMoveWaitTime;


    [SerializeField] float WarningWaitTime;    //��� ���ӽð�
    [SerializeField] float AttackWaitTime;

    [SerializeField] float WarningDuration;


    bool TopAttack;
    bool TopAttackStart;
    GameObject Player;
    GameObject WarningRotator;
    GameObject Warning;

    Vector3 SettedPlayerPosition;
    // Start is called before the first frame update
    void Start()
    {

        WarningRotator = transform.GetChild(0).gameObject;
        WarningRotator.SetActive(false);

        Player = GameObject.FindWithTag("Player");
        StartCoroutine(MobMove());
    }

    // Update is called once per frame
    void Update()
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
        SettedPlayerPosition = Player.transform.position;
        StartCoroutine(Attack()); // ù ���� �� �̵��� �ϰ� �̵� �� ���� �Լ� ����.
    }



    IEnumerator Attack()
    {
        WarningRotator.SetActive(true);
        TopAttack = true;
        WarningRotator.transform.position = new Vector3(SettedPlayerPosition.x,SettedPlayerPosition.y+ 20,SettedPlayerPosition.z);
        WarningRotator.transform.LookAt(SettedPlayerPosition);

        

        yield return new WaitForSeconds(WarningDuration); //��� �ð�.
        TopAttack= false;
        
        yield return new WaitForSeconds(3);
        WarningRotator.SetActive(false);
        StartCoroutine(MobMove());

    }
}
