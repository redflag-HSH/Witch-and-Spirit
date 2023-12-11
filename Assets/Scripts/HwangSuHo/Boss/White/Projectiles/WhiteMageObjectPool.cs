using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteMageObjectPool : MonoBehaviour
{
    float _radius;
    [Header("WeakShot POOL")]
    [SerializeField] WhiteMagicianNormalShot _daggerPrefab;
    [SerializeField] List<WhiteMagicianNormalShot> _daggers;
    [SerializeField] int _daggersCount;
    public int _daggerActiveIndex;

    [Header("Collar POOL")]
    [SerializeField] WhiteMagicianCollar _collarPrefab;
    [SerializeField] List<WhiteMagicianCollar> _collars;
    [SerializeField] int _collarsCount;
    public int _collarActiveIndex;

    [Header("Meteor POOL")]
    [SerializeField] WhiteMagicianMeteor _MeteorPrefab;
    [SerializeField] List<WhiteMagicianMeteor> _Meteors;
    [SerializeField] int _MeteorsCount;
    public int _meteorActiveIndex;

    [Header("Fist POOL")]
    [SerializeField] WhiteMagicianFist _FistPrefab;
    [SerializeField] List<WhiteMagicianFist> _Fists;
    [SerializeField] int _FistsCount;
    public int _fistActiveIndex;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void GenratePool()
    {
        int i;
        for (i = 0; i < 5; i++)
        {
            _daggers.Add(Instantiate(_daggerPrefab, transform.position, _daggerPrefab.transform.rotation));
            _daggersCount++;
        }
        for (i = 0; i < 5; i++)
        {
            _collars.Add(Instantiate(_collarPrefab, transform.position, _daggerPrefab.transform.rotation));
            _collarsCount++;
        }
        for (i = 0; i < 5; i++)
        {
            _Meteors.Add(Instantiate(_MeteorPrefab, transform.position, _daggerPrefab.transform.rotation));
            _MeteorsCount++;
        }
        for (i = 0; i < 5; i++)
        {
            _Fists.Add(Instantiate(_FistPrefab, transform.position, _daggerPrefab.transform.rotation));
            _FistsCount++;
        }

    }
    public void UseDaggerPool()
    {
        float count = Random.Range(3, _daggersCount + .1f);
        _daggerActiveIndex = Mathf.FloorToInt(count);
        for (int i = 0; i < count; i++)
        {
            //������ ���缭 �����ϰ� ���� ��ġ
        }
        StartCoroutine(DaggerPattern(count));
    }
    IEnumerator DaggerPattern(float count)
    {
        //for������ .n�ʸ��� Ȱ��ȭ
        yield return new WaitForSeconds(3.0f);
        //��� ����
        yield return new WaitForSeconds(3.0f);
        //for�� �������
        yield return new WaitForSeconds(3.0f);
        //��
    }

    public void UseCollarPool()
    {
        float count = Random.Range(3, _collarsCount + .1f);
        _collarActiveIndex = Mathf.FloorToInt(count);
        for (int i = 0; i < count; i++)
        {
            //������ ���缭 �����ϰ� ���� ��ġ
        }
        StartCoroutine(CollarPattern(count));
    }
    IEnumerator CollarPattern(float count)
    {
        //for������ .n�ʸ��� Ȱ��ȭ
        yield return new WaitForSeconds(3.0f);
        //��� ����
        yield return new WaitForSeconds(3.0f);
        //for�� �������
        yield return new WaitForSeconds(3.0f);
        //��
    }
    public void UseMeteorPool()
    {
        float count = Random.Range(3, _MeteorsCount + .1f);
        _meteorActiveIndex = Mathf.FloorToInt(count);
        for (int i = 0; i < count; i++)
        {
            //������ ���缭 �����ϰ� ���� ��ġ
        }
        StartCoroutine(MeteorPattern(count));
    }
    IEnumerator MeteorPattern(float count)
    {
        //for������ .n�ʸ��� Ȱ��ȭ
        yield return new WaitForSeconds(3.0f);
        //��� ����
        yield return new WaitForSeconds(3.0f);
        //for�� �������
        yield return new WaitForSeconds(3.0f);
        //��
    }
    public void UseFistPool()
    {
        float count = Random.Range(3, _FistsCount + .1f);
        _fistActiveIndex = Mathf.FloorToInt(count);
        for (int i = 0; i < count; i++)
        {
            //������ ���缭 �����ϰ� ���� ��ġ
        }
        StartCoroutine(FistPattern(count));
    }
    IEnumerator FistPattern(float count)
    {
        //for������ ������ �ð����� Ȱ��ȭ �ϰ� ����
        yield break;
        //��
    }
}
