using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMob_2 : MonoBehaviour
{
    [SerializeField] int Direction = -1;

    [SerializeField] float MoveDistance;
    [SerializeField] float MinMoveDistance;
    [SerializeField] float MaxMoveDistance;

    [SerializeField] float MoveSpeed;
    [SerializeField] float MoveWaitTime;
    [SerializeField] float MinMoveWaitTime;
    [SerializeField] float MaxMoveWaitTime;

    // Start is called before the first frame update
    void Start()
    {
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
        if(0 < viewpos.x && viewpos.x < 1)
        {
            Direction = Random.Range(-1, 1);
            if (Direction == 0) Direction = 1;
        }
        
        MoveDistance = Random.Range(MinMoveDistance, MaxMoveDistance + 1); // �̵� �Ÿ�
        MoveWaitTime = Random.Range(MinMoveWaitTime, MaxMoveWaitTime + 1); // �̵� �� �����̵����� ���ð�
        Direction = Random.Range(-1, 1);
        if (Direction == 0) Direction = 1;

        
        yield return new WaitForSeconds(MoveWaitTime);
        StartCoroutine(MobMove());
    }
}
