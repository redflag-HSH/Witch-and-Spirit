using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;


public class TextUiEx : MonoBehaviour
{
    public Image FirstCharacterImage;
    public Image SecondCharacterImage;
    public Image BackgroundImage;
    public Image TalkBox;
    public Image NameBack;
    public Sprite NameNull;
    public Sprite NameBackSprite;
    public AudioSource talkAudio;
    public CanvasGroup BlackScreen;

    public TMP_Text ChatText;
    public TMP_Text DisplayNameText;

    private List<Dictionary<string, string>> _chatScriptData;
    private readonly List<Chat> _chatList = new();

    private bool _isTexting = false;
    private bool _waitForSkip = false;
    private bool ending = false;

    private StageData _recentStageData = null;
    private AudioSource _audioSource = null;

    void Start()
    {
        _isTexting = false;
        _waitForSkip = false;

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null) _audioSource = gameObject.AddComponent<AudioSource>();

        var recentStageNum = StageManager.CurrentStageNumber;
        _recentStageData = StageManager.CurrentStageData;

        _chatScriptData = CSVReader.Read("RawData/Scripts");
        foreach(var line in _chatScriptData)
        {
            line.TryGetValue("Stage", out var stageStr);

            if (!int.TryParse(stageStr, out int stage) || stage != recentStageNum) continue;

            line.TryGetValue("DisplayName", out var displayName);
            line.TryGetValue("BGImage", out var bgImageStr);
            line.TryGetValue("FirstCharacterImage", out var firstCharImageStr);
            line.TryGetValue("SecondCharacterImage", out var secondCharImageStr);
            line.TryGetValue("Talkbox", out var talkboxImageStr);
            line.TryGetValue("Shake", out var shakeStr);
            line.TryGetValue("Script", out var script);

            bool shake = shakeStr.ToUpper() == "Y";
            var bgImage = Resources.Load<Sprite>("Story/Background/" + bgImageStr);
            var firstCharImage = Resources.Load<Sprite>("Story/Character/" + firstCharImageStr);
            var secondCharImage = Resources.Load<Sprite>("Story/Character/" + secondCharImageStr);
            var talkboxImage = Resources.Load<Sprite>("Story/Talkbox/" + talkboxImageStr);

            _chatList.Add(new()
            {
                Script = script,
                DisplayName = displayName,
                BGImage = bgImage,
                FirstCharacterImage = firstCharImage,
                SecondCharacterImage = secondCharImage,
                TalkboxImage = talkboxImage,
                Shake = shake,
            });
        }

        StartCoroutine(TextPractice());
    }
    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && _isTexting)
        {
            _waitForSkip = true;
        }
        if (DisplayNameText.text == "")
        {
            NameBack.sprite = NameNull;
        }
        else
        {
            NameBack.sprite = NameBackSprite;
        }
    }

    IEnumerator TextPractice()
    {
        if(_chatList.Count > 0) ApplyChatSituation(_chatList[0]);
        for (var i = 0f; i <= 1f; i += Time.deltaTime)
        {
            BlackScreen.alpha = 1 - i;
            yield return null;
        }
        BlackScreen.alpha = 0f;

        foreach (var chat in _chatList)
        {
            yield return StartCoroutine(NormalChat(chat));
        }


        for (var i = 0f; i <= 1f; i += Time.deltaTime)
        {
            BlackScreen.alpha = i;
            yield return null;
        }
        BlackScreen.alpha = 1f;

        if (_recentStageData == null)
        {
            SceneManager.LoadSceneAsync("EndingScene");
        }
        else if(_recentStageData is NormalStageData)
        {
            SceneManager.LoadSceneAsync("NormalStage");
        }
        else if(_recentStageData is BossStageData)
        {
            SceneManager.LoadSceneAsync("BossStage");
        }
    }

    private void ApplyChatSituation(Chat chat)
    {
        DisplayNameText.SetText(chat.DisplayName);
        BackgroundImage.sprite = chat.BGImage;
        FirstCharacterImage.sprite = chat.FirstCharacterImage;
        SecondCharacterImage.sprite = chat.SecondCharacterImage;
        TalkBox.sprite = chat.TalkboxImage;

        FirstCharacterImage.gameObject.SetActive(FirstCharacterImage.sprite != null);
        SecondCharacterImage.gameObject.SetActive(SecondCharacterImage.sprite != null);
    }

    IEnumerator NormalChat(Chat chat)
    {
        if (ending == true)
        {
            for (var i = 0f; i <= 1f; i += Time.deltaTime)
            {
                BlackScreen.alpha = i;
                yield return null;
            }
            BlackScreen.alpha = 1f;
        }
        ApplyChatSituation(chat);
        if (ending == true)
        {
            for (var i = 0f; i <= 1f; i += Time.deltaTime)
            {
                BlackScreen.alpha = 1 - i;
                yield return null;
            }
            BlackScreen.alpha = 0f;
            ending = false;
        }

        if (chat.Shake)
            TalkBox.GetComponent<UIPanelShake>().StartShake();

        var writerText = "";
        _isTexting = true;
        talkAudio.Play();
        foreach (var c in chat.Script)
        {
            writerText += c;
            ChatText.text = writerText;
            if (_waitForSkip) continue;
            yield return new WaitForSeconds(.05f);
        }
        _waitForSkip = false;
        _isTexting = false;
        talkAudio.Stop();

        TalkBox.GetComponent<UIPanelShake>().EndShake();

        if (DisplayNameText.text == "")
        {
            ending = true;
        }

        yield return null;
        while (!(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))) yield return null;
    }
}

public struct Chat
{
    public string Script, DisplayName;
    public Sprite FirstCharacterImage, SecondCharacterImage, BGImage, TalkboxImage;
    public bool Shake;
}