using UnityEngine;

[CreateAssetMenu(fileName = "Element Data", menuName = "ScriptableObjects/Player Element", order = 0)]
public class PlayerElementData : ScriptableObject
{
    public ElementType ElementType;
    public Material Material;
    public Color LightColor;
    public ParticleSystem SpawnParticle;
    public ParticleSystem ProjectParticle;
    public Projectile Projectile;
    public AudioClip ShootSound;
    public float CircleRotation;
    public float SkillCooldown;
    public float ShootVolume = 1f;
}

public enum ElementType
{
    Fire, Water, Earth, Dark, Light
}