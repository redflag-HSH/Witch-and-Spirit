using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class WhiteMageWeakS : Projectile
{
    bool _isDroping = false;
    [SerializeField] float _readyCount;
    [SerializeField] GameObject _warn;
    [Header("Model Parameter")]
    [SerializeField] GameObject[] _models;

    void Start()
    {
        StartCoroutine(Motor());
    }

    IEnumerator Motor()
    {
        _warn.SetActive(true);
        yield return new WaitForSeconds(_readyCount);
        _warn.SetActive(false);
        _isDroping = true;
    }

    protected override void Update()
    {
        if (_isDroping)
            base.Update();
    }

    public void ModelSet(int index)
    {
        _models[index].SetActive(true);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        print("WeakTrigger " + other.gameObject);

    }
}
