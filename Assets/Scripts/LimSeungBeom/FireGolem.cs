using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGolem : Enemy
{

    GameObject Player;
    [SerializeField] ParticleSystem Warning;
    [SerializeField] ParticleSystem Meteor;
    [Header("���׿� ���� �� ��� �ð� (��)")]  //������ ��ƼŬ ���ӽð��� ���� ���� �� �־�� ��. ���� ������ �� ������ ��.
    [SerializeField] float AttackWarningTime;


    bool AttackAiming;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        Player = GameManager.Instance.Player.gameObject;
    }
    void Start()
    {
        Warning.gameObject.SetActive(false);
        Meteor.gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(AttackAiming)
        {
            Warning.transform.position = new Vector3(Player.transform.position.x,Player.transform.position.y - 0.7f,Player.transform.position.z);
        }

    }
    protected override void Attack()
    {
        StartCoroutine(FireGolemAttack());
    }
    IEnumerator FireGolemAttack()
    {
        //StartCoroutine(Stop(AttackWarningTime + 3));

        Warning.gameObject.SetActive(true);
        AttackAiming = true;
        yield return new WaitForSeconds(AttackWarningTime);

        Meteor.gameObject.SetActive(true);
        Meteor.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - 0.5f, Player.transform.position.z);
        Meteor.transform.parent = null;
        Warning.transform.parent = null;
        AttackAiming = false;

        yield return new WaitUntil(() => !Meteor.isPlaying && !Warning.isPlaying);
        Meteor.transform.parent = transform;
        Warning.transform.parent = transform;
        Meteor.gameObject.SetActive(false);
        Warning.gameObject.SetActive(false);
    }
}
