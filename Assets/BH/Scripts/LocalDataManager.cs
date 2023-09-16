using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDataManager : MonoBehaviour
{
    public static LocalDataManager Instance;

    bool _isTutorialCleared = false;
    public bool IsTutorialCleared
    {
        get
        {
            return TutorialCanSkip();     
        }
    }

    public string RedMageName
    {
        get
        {
            return "�г��� �Ǵ� ������";
        }
    }

    public string BlueKnightName
    {
        get
        {
            return "������ û���� ���";
        }
    }

    public string SoulTreeName
    {
        get
        {
            return "�г��� ȥ�ŵι�";
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        TutorialCanSkip();
    }

    void Start()
    {
        
    }



    public void SetTrophy(string bossname)
    {
        PlayerPrefs.SetInt(bossname, 1);
    }

    public void SetPerfect(string bossname)
    {
        PlayerPrefs.SetInt(bossname + "Perfect", 1);
    }

    bool TutorialCanSkip()
    {
        return PlayerPrefs.GetInt("TutorialClear") == 1 ? true : false;
    }

    public void ClearTutorial()
    {
        PlayerPrefs.SetInt("TutorialClear", 1);
    }

    void DebugDeleteAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    void DebugClearTutorial()
    {
        PlayerPrefs.SetInt("TutorialClear", 1);
    }

    void DebugClearRedMage()
    {
        PlayerPrefs.SetInt(RedMageName, 1);
    }

    void DebugClearBlueKnight()
    {
        PlayerPrefs.SetInt(BlueKnightName, 1);
    }

    void DebugClearSoulTree()
    {
        PlayerPrefs.SetInt(SoulTreeName, 1);
    }

    void DebugPerfectRedMage()
    {
        PlayerPrefs.SetInt(RedMageName+"Perfect", 1);
    }

    void DebugPerfectBlueKnight()
    {
        PlayerPrefs.SetInt(BlueKnightName + "Perfect", 1);
    }

    void DebugPerfectSoulTree()
    {
        PlayerPrefs.SetInt(SoulTreeName + "Perfect", 1);
    }
}
