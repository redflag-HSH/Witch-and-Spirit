using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYODFist : MonoBehaviour
{
    [SerializeField] float speed;

    void Update()
    {
        transform.position += Time.deltaTime * speed * transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�÷��̾� ������ �ִ� �ڵ�.
            this.gameObject.SetActive(false);
            Debug.Log("�÷��̾� �ǰ�!");
        }
    }
}
