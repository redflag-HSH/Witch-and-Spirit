using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    [SerializeField] private AudioClip _elementChangeSound;
    [SerializeField] private AudioClip _playerDamageSound, _playerHealSound;
    [SerializeField] private AudioSource _playerHeartbeat;
    [SerializeField] private PlayerElementData _fireElementData, _earthElementData, _waterElementData, _darkElementData;

    [SerializeField] private float _speed;
    [SerializeField] private float _aimDistance;
    [SerializeField] private Projectile _projectilePrefab;

    [SerializeField] private float _shootDistance;

    public Transform Boss;
    [SerializeField] private bool _isBossMoving;
    [SerializeField] private Light _orbLight;

    [SerializeField] private MeshRenderer WaterProtector;
    [SerializeField] private ParticleSystem SpeedUpParticle;

    public ParticleSystem DamageParticle;

    [SerializeField] private MeshRenderer _renderer;

    public PlayerElementData CurrentElement;

    [HideInInspector] public bool IsInvulnerable = false;

    public float AttackCooldown = 0.5f;
    public float SkillCooldown = 10f;

    private float _attackTimer = 0f;
    private float _fireSkillTimer = 0f, _waterSkillTimer = 0f, _earthSkillTimer = 0f;

    public float FireRemainCooldown { get => _fireSkillTimer / _fireElementData.SkillCooldown; }
    public float EarthRemainCooldown { get => _earthSkillTimer / _earthElementData.SkillCooldown; }
    public float WaterRemainCooldown { get => _waterSkillTimer / _waterElementData.SkillCooldown; }

    private bool _isClearing = false;

    [Header("skill")]
    [SerializeField] private float _invulnerableTime = 2f;
    [SerializeField] private float _speedUpTime = 2f;
    [SerializeField] private float _speedUpModifier = 2f;

    protected override void Awake()
    {
        base.Awake();
        ChangeElement(CurrentElement, true);
        WaterProtector.gameObject.SetActive(false);
        SpeedUpParticle.Stop();
    }

    private void Update()
    {
        MoveUpdate();
        ShootUpdate();
        ElementChangeUpdate();
        SkillUpdate();
        HeartBeatUpdate();
        TutorialCheck();

        if (Input.GetKeyDown(KeyCode.T)) OnClear();
    }

    private void TutorialCheck()
    {
        if(GameManager.Instance.BlackScreen.alpha <= 0.01f)
        {
            if (!PlayerPrefs.HasKey("ControllTutorial"))
            {
                PlayerPrefs.SetInt("ControllTutorial", 1);
                TutorialManager.Instance.ShowTutorial(TutorialManager.Instance.ControllTutorial);
            }

            if (!PlayerPrefs.HasKey("ElementTutorial") && StageManager.CurrentStageNumber == 2)
            {
                PlayerPrefs.SetInt("ElementTutorial", 1);
                TutorialManager.Instance.ShowTutorial(TutorialManager.Instance.ElementTutorial);
            }

        }
    }

    public static bool IsDominentTo(ElementType elementType, ElementType target)
    {
        if (elementType == ElementType.Fire && target == ElementType.Earth) return true;
        if (elementType == ElementType.Water && target == ElementType.Fire) return true;
        if (elementType == ElementType.Earth && target == ElementType.Water) return true;
        if (elementType == ElementType.Light && target == ElementType.Dark) return true;
        return false;
    }

    private void HeartBeatUpdate()
    {
        var hbDeltaTime = HP <= 1 ? Time.deltaTime : -Time.deltaTime;
        
        _playerHeartbeat.volume = Mathf.Clamp01(_playerHeartbeat.volume + hbDeltaTime);

        var col = GameManager.Instance.HeartBeatScreen.color;
        col.a = Mathf.Clamp(col.a + hbDeltaTime / 0.2f / 0.6f, 0f, 0.2f);
        GameManager.Instance.HeartBeatScreen.color = col;
    }

    private void ElementChangeUpdate()
    {
        if (StageManager.CurrentStageNumber > 1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                ChangeElement(_darkElementData);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                ChangeElement(_waterElementData);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                ChangeElement(_earthElementData);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                ChangeElement(_fireElementData);
            }
        }
    }

    private void SkillUpdate()
    {
        if (_fireSkillTimer > 0f) _fireSkillTimer -= Time.deltaTime;
        if (_earthSkillTimer > 0f) _earthSkillTimer -= Time.deltaTime;
        if (_waterSkillTimer > 0f) _waterSkillTimer -= Time.deltaTime;
    }

    public void ChangeElement(PlayerElementData data, bool slient = false)
    {
        _renderer.material = data.Material;
        _orbLight.color = data.LightColor;
        if(data.SpawnParticle != null && !slient) 
            ParticleManager.SpawnParticle(data.SpawnParticle, transform.position, transform);
        CurrentElement = data;
        
        if(!slient)
            GameManager.Instance.SoundManager.PlaySFX(_elementChangeSound, transform);

        switch(data.ElementType)
        {
            case ElementType.Fire:
                {
                    if (_fireSkillTimer <= 0f)
                    {
                        _fireSkillTimer = CurrentElement.SkillCooldown;
                        StartCoroutine(FireSkill());
                    }
                }
                break;
            case ElementType.Water:
                {
                    if (_waterSkillTimer <= 0f)
                    {
                        _waterSkillTimer = CurrentElement.SkillCooldown;
                        StartCoroutine(WaterSkill());
                    }

                }
                break;
            case ElementType.Earth:
                {
                    if (_earthSkillTimer <= 0f)
                    {
                        _earthSkillTimer = CurrentElement.SkillCooldown;
                        StartCoroutine(EarthSkill());
                    }

                }
                break;
        }
    }

    private IEnumerator FireSkill()
    {
        SpeedUpParticle.Play();
        AttackCooldown /= _speedUpModifier;
        yield return new WaitForSeconds(_speedUpTime);
        AttackCooldown *= _speedUpModifier;

        SpeedUpParticle.Stop();
    }

    private IEnumerator WaterSkill()
    {
        WaterProtector.gameObject.SetActive(true);
        for(float i = 0; i < 1f; i += Time.deltaTime / 0.3f)
        {
            WaterProtector.material.color = new Color(1, 1, 1, i);
            yield return null;
        }
        IsInvulnerable = true;
        yield return new WaitForSeconds(_invulnerableTime);
        IsInvulnerable = false;

        for (float i = 0; i < 1f; i += Time.deltaTime / 0.3f)
        {
            WaterProtector.material.color = new Color(1, 1, 1, 1 - i);
            yield return null;
        }

        WaterProtector.gameObject.SetActive(false);
    }

    private IEnumerator EarthSkill()
    {
        if (hp < MaxHP) hp++;
        GameManager.Instance.SoundManager.PlaySFX(_playerHealSound, transform.position, 1, 1);

        yield return null;
    }

    public override void Damage(int damage)
    {
        if (!IsInvulnerable)
        {
            base.Damage(damage);
            GameManager.Instance.DamageScreen.ShowDamage();
        }
        GameManager.Instance.SoundManager.PlaySFX(_playerDamageSound, transform.position, 1, 1);
        ParticleManager.SpawnParticle(DamageParticle, transform.position);
    }

    private void ShootUpdate()
    {
        if (TutorialManager.Instance.Showing) return;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var dir = ray.direction;
        transform.forward = dir;

        if (_attackTimer > 0) _attackTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && _attackTimer <= 0f)
        {
            _attackTimer = AttackCooldown;
            if (CurrentElement.ProjectParticle != null)
                ParticleManager.SpawnParticle(CurrentElement.ProjectParticle, transform.position, transform);
            var projectile = Instantiate(CurrentElement.Projectile, transform.position + transform.forward * _shootDistance, Quaternion.identity);
            projectile.transform.forward = dir;
            projectile.CameraOffset = projectile.transform.position - Camera.main.transform.position;
            projectile.Owner = this;
            GameManager.Instance.SoundManager.PlaySFX(CurrentElement.ShootSound, transform, CurrentElement.ShootVolume);
        }
    }

    protected override void OnDeath()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void OnClear()
    {
        if(_isClearing) return;
        _isClearing = true;
        StartCoroutine(OnClearRoutine());
    }

    public IEnumerator OnClearRoutine()
    {
        yield return GameManager.Instance.ShowBlack();
        PlayerPrefs.SetInt("Score", GameManager.Instance.scoreManager.score);
        var NextStage = PlayerPrefs.GetInt("RecentStage", 0) + 1;
        PlayerPrefs.SetInt("RecentStage", NextStage);
        SceneManager.LoadScene("StoryScene");
        _isClearing = false;
    }

    private void MoveUpdate()
    {
        if (TutorialManager.Instance.Showing) return;
        float xAxis = Input.GetAxisRaw("Horizontal");
        if (_isBossMoving)
        {
            var bossDir = (transform.position - Boss.position);
            bossDir.y = 0;
            var distance = bossDir.magnitude;
            var angle = Mathf.Atan2(bossDir.z, bossDir.x);
            // 2 * pi * r = 원주
            // 각도 * r = 움직이는 속도
            // 속도 / r = 각속도
            angle += xAxis * Time.deltaTime * _speed / distance;
            transform.position = new Vector3(Boss.position.x, transform.position.y, Boss.position.z) 
                + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized * distance;
        }
        else
        {
            transform.position += _speed * xAxis * Time.deltaTime * Vector3.right;
            var stage = StageManager.CurrentStageData;
            if(stage is NormalStageData normalStage)
            {
                var pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, normalStage.PlayerMinX, normalStage.PlayerMaxX);
                transform.position = pos;
            }
        }
    }
}