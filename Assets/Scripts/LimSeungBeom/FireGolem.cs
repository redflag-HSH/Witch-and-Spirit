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
    [SerializeField] float TopAttackWaitSpeed;
    [SerializeField] float UnjiSpeed;
    [SerializeField] float ReturnSpeed;

    bool TargetSet;
    bool TopAttack; 
    bool TopAttackStart;
    bool Return;
    GameObject Player;
    GameObject WarningRotator;
    GameObject Warning;

    Vector3 SettedPlayerPosition;
    Vector3 SavedEnemyPosition;
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

        if(TopAttack)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(SettedPlayerPosition.x, SettedPlayerPosition.y + 10, SettedPlayerPosition.z), Time.deltaTime * TopAttackWaitSpeed);
        }
        if(TopAttackStart)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(SettedPlayerPosition.x, SettedPlayerPosition.y - 10, SettedPlayerPosition.z), Time.deltaTime * UnjiSpeed);
        }
        if(Return)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(SavedEnemyPosition.x,SavedEnemyPosition.y,SavedEnemyPosition.z), Time.deltaTime * ReturnSpeed);
        }
        if(TargetSet)
        {
            WarningRotator.transform.position = new Vector3(SettedPlayerPosition.x, SettedPlayerPosition.y + 20, SettedPlayerPosition.z); ;
        }
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

        SavedEnemyPosition = this.gameObject.transform.position;

        WarningRotator.SetActive(true);
        TargetSet = true;
        TopAttack = true;
        WarningRotator.transform.LookAt(SettedPlayerPosition);

        yield return new WaitForSeconds(WarningDuration); //��� �ð�.
        TopAttack= false;
        Debug.Log("�̵� �Ϸ�, ���� ����");
        TopAttackStart = true;
        
        
        yield return new WaitForSeconds(1);
        TopAttackStart = false;
        Return = true;
        yield return new WaitForSeconds(2);
        TargetSet = false;
        Return = false;
        WarningRotator.SetActive(false);
        StartCoroutine(MobMove());


    }
}
