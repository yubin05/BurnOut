using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GlobalSingleton<GameManager>
{
    // 게임 시작 시, 자동으로 생성
    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        instance = Instance;
        Debug.Log(Application.persistentDataPath);

        GameApplication.Instance.Init();
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
    }
}
