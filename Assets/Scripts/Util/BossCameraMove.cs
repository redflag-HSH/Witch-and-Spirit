using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BossCameraMove : MonoBehaviour
{
    public Transform Boss;
    [SerializeField] private Player _player;
    [SerializeField] private float _smoothTime = 0.5f;

    private Vector3 _vel;
    private float _distance;

    private void Awake()
    {
        StartCoroutine(InitAwake());
    }

    private IEnumerator InitAwake()
    {
        yield return new WaitUntil(() => Boss != null);
        var bossDir = (transform.position - Boss.position);
        _distance = bossDir.magnitude;

    }

    private void LateUpdate()
    {
        if (Boss == null) return;
        var cameraBossDir = (Boss.position - transform.position);
        var angles = transform.eulerAngles;
        angles.y = Mathf.Atan2(cameraBossDir.x, cameraBossDir.z) * Mathf.Rad2Deg;
        transform.eulerAngles = angles;

        var bossDir = (_player.transform.position - Boss.position);
        bossDir.y = 0;
        var angle = Mathf.Atan2(bossDir.z, bossDir.x);
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(Boss.position.x, transform.position.y, Boss.position.z)
            + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized * _distance, ref _vel, _smoothTime);
    }
}
