using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UIElements;

public class SunlightYellowOverdrive : MonoBehaviour
{


    [SerializeField] float SpreadX; // �¿� ������ ����
    [SerializeField] float SpreadY; // ���� ������ ����
    [SerializeField] float SpreadZ; // ���� ������ ����

    [SerializeField] float FistCoolTime;
    [SerializeField] GameObject Player;
    int FistsArrIndex = 0;
    float ex;
    float ex2;
    float ex3;
    [SerializeField] GameObject SunlightYellowOverdrive_Fist;

    Transform SavedPlayerPosition;

    GameObject[] Fists = new GameObject[30];


    // Start is called before the first frame update
    void Start()
    {
        /// <summary>
        /// �ָ� 30�� ���� �� Fists �迭�� ����.
        /// </summary>
        for (int i = 0; i < Fists.Length; i++)
        {
            GameObject FistInstances = Instantiate(SunlightYellowOverdrive_Fist);
            Fists[i] = FistInstances;
            FistInstances.SetActive(false);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            StartFist();

            //StartCoroutine(ActivateFist(FistsArrIndex, 0, 0));

        }


        /*ex = Random.Range(-SpreadX, SpreadX);
        ex2 = Random.Range(-SpreadY, SpreadY);
        ex3 = Random.Range(-SpreadZ, SpreadZ);*/
    }
    public void StartFist()
    {
        float a;
        a = Random.Range(0, 30);
        int aa = Mathf.FloorToInt(a);
        StartCoroutine(FistMotor(FistsArrIndex, aa));
    }
    IEnumerator FistMotor(int index, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(Random.Range(.1f, .7f));
            StartCoroutine(ActivateFist(index + i));
            
        }
    }
    //�ָ� �߻��ϴ� �Լ�. x,y,z�� offset���� ���ڷ� �޾��־�� ��.
    IEnumerator ActivateFist(int Index)
    {
        ex = Random.Range(-SpreadX, SpreadX);
        ex2 = Random.Range(-SpreadY, SpreadY);
        ex3 = Random.Range(-SpreadZ, SpreadZ);
        //SavedPlayerPosition = Player.transform;
        GameObject ActivatedFist = Fists[Index];

        FistsArrIndex++;
        if (FistsArrIndex >= Fists.Length)
        {
            FistsArrIndex = 0;
        }
        ActivatedFist.SetActive(true);
        ActivatedFist.transform.LookAt(Player.transform.position);
        ActivatedFist.transform.position = new Vector3(this.transform.position.x + ex, this.transform.position.y + ex2, this.transform.position.z + ex3);

        yield return new WaitForSeconds(5);

        ActivatedFist.SetActive(false);
        ActivatedFist.transform.position = this.transform.position;
    }
}
