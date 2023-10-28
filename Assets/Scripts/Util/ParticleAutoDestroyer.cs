using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAutoDestroyer : MonoBehaviour
{
    private ParticleSystem _particle;


    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(!_particle.isPlaying) Destroy(gameObject);
    }
}