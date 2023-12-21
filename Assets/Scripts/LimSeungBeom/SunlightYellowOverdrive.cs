using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UIElements;

public class SunlightYellowOverdrive : MonoBehaviour
{


    [SerializeField] float SpreadX; // 좌우 퍼지는 정도
    [SerializeField] float SpreadY; // 상하 퍼지는 정도
    [SerializeField] float SpreadZ; // 전후 퍼지는 정도

    [SerializeField] float FistCoolTime;
    int FistsArrIndex = 0;
    float ex;
    float ex2;
    float ex3;
    [SerializeField] GameObject SunlightYellowOverdrive_Fist;

    GameObject[] Fists = new GameObject[30];


    // Start is called before the first frame update
    void Start()
    {
        /// <summary>
        /// 주먹 30개 생성 후 Fists 배열에 넣음.
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
    //주먹 발사하는 함수. x,y,z의 offset값을 인자로 받아주어야 함.
    IEnumerator ActivateFist(int Index)
    {
        ex = Random.Range(-SpreadX, SpreadX);
        ex2 = Random.Range(-SpreadY, SpreadY);
        ex3 = Random.Range(-SpreadZ, SpreadZ);
        if (Index >= Fists.Length)
            Index -= Fists.Length;
        GameObject ActivatedFist = Fists[Index];

        FistsArrIndex++;
        if (FistsArrIndex >= Fists.Length)
        {
            FistsArrIndex = 0;
        }
        ActivatedFist.SetActive(true);

        ActivatedFist.transform.position = new Vector3(this.transform.position.x + ex, this.transform.position.y + ex2, this.transform.position.z + ex3);
        ActivatedFist.transform.LookAt(GameManager.Instance.Player.transform.position);

        print("주먹 방향 변경 : " + ActivatedFist.transform.eulerAngles);

        yield return new WaitForSeconds(5);

        ActivatedFist.SetActive(false);
        ActivatedFist.transform.position = this.transform.position;
    }
}
