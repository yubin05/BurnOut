using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

public class GameManager : GlobalSingleton<GameManager>
{
    // 게임 시작 시, 자동으로 생성
    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        instance = Instance;
        Debug.Log(Application.persistentDataPath);

        GameApplication.Instance.Init();
        Pooling.Instance.Init();
    }

    private void Start()
    {
        // test
        var language = GameApplication.Instance.GameModel.ClientData.PlayerLanguage.language;
        var textInfo = GameApplication.Instance.GameModel.PresetData.ReturnData<TextInfo>(nameof(TextInfo), 1);
        string languageStr = "";
        switch (language)
        {
            case TextInfo.LanguageTypes.English: languageStr = textInfo.NameEn; break;
            default: languageStr = textInfo.NameKr; break;  // 한국어
        }
        Debug.Log(languageStr);

        // test
        // GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(10001, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.F4))
        {
            QuitGame();
        }

        // test
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(10002, Vector3.zero, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var soundInfos = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<SoundInfo>(nameof(SoundInfo));
            if (soundInfos != null) Debug.Log(soundInfos.Length);
        }
    }

    // 언어 변경
    public void ChanageLanauage(TextInfo.LanguageTypes language)
    {
        GameApplication.Instance.GameModel.ClientData.PlayerLanguage.language = language;

        var fileName = GameApplication.Instance.GameModel.ClientData.PlayerLanguageFileName;
        var extension = GameApplication.Instance.GameModel.JsonTable.Extension;

        var path = Application.persistentDataPath + "/" + DataTablePath.JsonFilePath + fileName + extension;
        var dataStr = "[" + JsonConvert.SerializeObject(GameApplication.Instance.GameModel.ClientData.PlayerLanguage) + "]";
        File.WriteAllText(path, dataStr);
    }

    // 게임 종료
    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
