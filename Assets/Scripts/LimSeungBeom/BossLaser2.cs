using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BossLaser2 : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject Player;

    //---------------------------------------------------------------- �Ʒ��� �ʿ���.
    [SerializeField] GameObject Laser2Warn;
    [SerializeField] GameObject Laser2Laser;


    [Header("���� �ð� : �������� �÷��̾ �����ϴ� �ð� (��) ")]
    [SerializeField] float AimingTIme; 
    Vector3 Laser2SavedPlayerPosition;
    bool Laser2AimingPlayer;
    bool Laser2Attacking;
    Player ps;
    [SerializeField] float Laser2Radius = 2;

    private void Awake()
    {
        Laser2Warn = Instantiate(Laser2Warn);
        Laser2Laser = Instantiate(Laser2Laser);
    }
    void Start()
    {

        Player = GameManager.Instance.Player.gameObject;
        ps = Player.GetComponent<Player>();
        //---------------------------------------------------------------- �Ʒ��� �ʿ���.
        Laser2Laser.SetActive(false);
        Laser2Warn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartLaserAttack();
        }

        if (Laser2AimingPlayer)
        {
            Laser2Warn.transform.position = Player.transform.position;
        }
        if(Laser2Attacking)
        {
            Vector3 center = Laser2Laser.transform.position;

            Collider[] colliders = Physics.OverlapSphere(center, Laser2Radius);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject == Player)
                {
                    ps.Damage(1);
                }
            }
        }

        
    }
    public void StartLaserAttack()
    {
        StartCoroutine(LaserAttack2());
    }
    IEnumerator LaserAttack2()
    {
        Laser2Warn.SetActive(true);
        Laser2AimingPlayer = true;

        yield return new WaitForSeconds(AimingTIme);
        Laser2AimingPlayer = false;
        Laser2SavedPlayerPosition = Player.transform.position;

        yield return new WaitForSeconds(5 - AimingTIme);

        Laser2Laser.SetActive(true);
        Laser2Laser.transform.position = new Vector3(Laser2SavedPlayerPosition.x, Laser2SavedPlayerPosition.y - 0.4f, Laser2SavedPlayerPosition.z);

        Laser2Attacking = true;
        
        yield return new WaitForSeconds(2.5f);
        Laser2Attacking = false;
        
        yield return new WaitForSeconds(1f);
        Laser2Warn.SetActive(false);
        Laser2Laser.SetActive(false);
    }
}
