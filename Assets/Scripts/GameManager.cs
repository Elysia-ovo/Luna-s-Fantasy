using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Ϸ�ܹ���
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject battleGo;//ս��������Ϸ����
    //luna����
    public int lunaHP;//�������ֵ
    public float lunaCurrentHP;//Luna�ĵ�ǰ����ֵ
    public int lunaMP;//�������
    public float lunaCurrentMP;//luna�ĵ�ǰ����
    //Monster����
    public int monsterCurrentHP;//���ﵱǰѪ��
    public int dialogInfoIndex;
    public bool canControlLuna;
    public bool hasPetTheDog;
    public int candleNum;
    public int killNum;
    public GameObject monstersGo;
    public NPCDialog npc;
    public bool enterBattle;
    public GameObject battleMonsterGo;
    public AudioSource audioSource;
    public AudioClip normalClip;
    public AudioClip battleClip;

    private void Awake()
    {
        Instance = this;
        lunaCurrentHP = 100;
        lunaCurrentMP = 100;
        lunaHP =100;
        lunaMP =100;
        monsterCurrentHP = 50;
    }

    private void Update()
    {
        if (!enterBattle)
        {
            if (lunaCurrentMP <= 100)
            {
                AddOrDecreaseMP(Time.deltaTime);
            }
            if (lunaCurrentHP <= 100)
            {
                AddOrDecreaseHP(Time.deltaTime);
            }
        }
    }

    //public void ChangeHeath(int amount)
    //{
    //    lunaCurrentHP = Mathf.Clamp(lunaCurrentHP + amount, 0, lunaHP);
    //    Debug.Log(lunaCurrentHP + "/" + lunaHP);
    //}

    public void EnterOrExitBattle(bool enter = true, int addKillNum = 0)
    {
        UIManager.Instance.ShowOrHideBattlePanel(enter);
        battleGo.SetActive(enter);
        if (!enter)//��ս��״̬������˵ս������
        {
            killNum += addKillNum;
            if (addKillNum > 0)
            {
                DestoryMonster();
            }
            monsterCurrentHP = 50;
            PlayMusic(normalClip);
            if (lunaCurrentHP <= 0)
            {
                lunaCurrentHP = 100;
                lunaCurrentMP = 0;
                battleMonsterGo.transform.position += new Vector3(0, 2, 0);
            }
        }
        else
        {
            PlayMusic(battleClip);
        }
        enterBattle = enter;
    }
    public void DestoryMonster()
    {
        Destroy(battleMonsterGo);
    }
    public void SetMonster(GameObject go)
    {
        battleMonsterGo = go;
    }

    /// <summary>
    /// LunaѪ���ı�
    /// </summary>
    /// <param name="value"></param>
    public void AddOrDecreaseHP(float value)
    {
        lunaCurrentHP += value;
        if (lunaCurrentHP>=lunaHP)
        {
            lunaCurrentHP = lunaHP;
        }
        if (lunaCurrentHP<=0)
        {
            lunaCurrentHP = 0;
        }
        UIManager.Instance.SetHPValue(lunaCurrentHP/lunaHP);
    }
    /// <summary>
    /// Luna�����ı�
    /// </summary>
    /// <param name="value"></param>
    public void AddOrDecreaseMP(float value)
    {
        lunaCurrentMP += value;
        if (lunaCurrentMP >= lunaMP)
        {
            lunaCurrentMP = lunaMP;
        }
        if (lunaCurrentMP <= 0)
        {
            lunaCurrentMP = 0;
        }
        UIManager.Instance.SetMPValue(lunaCurrentMP / lunaMP);
    }
    /// <summary>
    /// �Ƿ����ʹ����ؼ���
    /// </summary>
    /// <param name="value">���ܺķ�����</param>
    /// <returns></returns>
    public bool CanUsePlayerMP(int value)
    {
        return lunaCurrentMP >= value;
    }
    /// <summary>
    /// MonsterѪ���ı�
    /// </summary>
    /// <param name="value"></param>
    public int AddOrDecreaseMonsterHP(int value)
    {
        monsterCurrentHP += value;
        return monsterCurrentHP;
    }
    /// <summary>
    /// ��ʾ����
    /// </summary>
    public void ShowMonsters()
    {
        if (!monstersGo.activeSelf)
        {
            monstersGo.SetActive(true);
        }
    }
    /// <summary>
    /// ���������������
    /// </summary>
    public void SetContentIndex()
    {
        npc.SetContentIndex();
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (audioSource.clip != audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
