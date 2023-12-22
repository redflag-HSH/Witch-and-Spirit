using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player Player;
    public Boss Boss;
    public DamageScreen DamageScreen;
    public Image HeartBeatScreen;
    public CanvasGroup BlackScreen;
    public ScoreManager scoreManager;
    public SoundManager SoundManager;
    public TextMeshProUGUI BossTitle;

    public BossCameraMove BossCamera;

    private void Awake()
    {
        Instance = this;

        Instantiate(StageManager.CurrentStageData.MapObject, Vector3.zero, Quaternion.identity);
        if(StageManager.CurrentStageData is BossStageData bossStageData)
        {
            Boss = Instantiate(bossStageData.Boss, Vector3.zero, Quaternion.identity);
            Player.Boss = Boss.transform;
            BossCamera.Boss = Boss.transform;
            BossTitle.SetText(Boss.BossName);
        }

        RenderSettings.skybox = StageManager.CurrentStageData.Skybox;
        RenderSettings.fogDensity = StageManager.CurrentStageData.FogDensity;
        RenderSettings.fogColor = StageManager.CurrentStageData.FogColor;

        StartCoroutine(HideBlack());
    }

    private IEnumerator HideBlack()
    {
        for (var i = 0f; i <= 1f; i += Time.deltaTime)
        {
            BlackScreen.alpha = 1 - i;
            yield return null;
        }
        BlackScreen.alpha = 0f;
    }

    public IEnumerator ShowBlack()
    {
        for (var i = 0f; i <= 1f; i += Time.deltaTime)
        {
            BlackScreen.alpha = i;
            yield return null;
        }
        BlackScreen.alpha = 1f;
    }

    private void Start()
    {
        SoundManager.PlayBGM(StageManager.CurrentStageData.BackgroundMusic);
    }

}