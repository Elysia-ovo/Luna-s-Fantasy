using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Animator lunaAnimator;
    public Transform lunaTrans;
    public Transform monsterTrans;
    private Vector3 monsterInitPos;
    private Vector3 lunaInitPos;
    public SpriteRenderer monsterSr;
    public SpriteRenderer lunaSr;
    public GameObject skillEffectGo;
    public GameObject healEffectGo;
    public AudioClip attackSound;
    public AudioClip lunaAttackSound;
    public AudioClip monsterAttackSound;
    public AudioClip skillSound;
    public AudioClip recoverSound;
    public AudioClip hitSound;
    public AudioClip dieSound;
    public AudioClip monsterDieSound;


    private void Awake()
    {
        monsterInitPos = monsterTrans.localPosition;
        lunaInitPos = lunaTrans.localPosition;
    }

    private void OnEnable()
    {
        monsterSr.DOFade(1,0.01f);
        lunaSr.DOFade(1,0.01f);
        lunaTrans.localPosition = lunaInitPos;
        monsterTrans.localPosition = monsterInitPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Luna攻击
    /// </summary>
    public void LunaAttack()
    {
        StartCoroutine(PerformAttackLogic());
    }

    IEnumerator PerformAttackLogic()
    {
        UIManager.Instance.ShowOrHideBattlePanel(false);
        lunaAnimator.SetBool("MoveState",true);
        lunaAnimator.SetFloat("MoveValue", -1);
        lunaTrans.DOLocalMove(monsterInitPos+new Vector3(1,0,0),0.5f).OnComplete
            (
                () => 
                {
                    GameManager.Instance.PlaySound(attackSound);
                    GameManager.Instance.PlaySound(lunaAttackSound);
                    lunaAnimator.SetBool("MoveState", false);
                    lunaAnimator.SetFloat("MoveValue", 0);
                    lunaAnimator.CrossFade("Attack",0);
                    monsterSr.DOFade(0.3f, 0.2f).OnComplete(() => { JudgeMonsterHP(-20); });
                }
            );
        yield return new WaitForSeconds(1.167f);
        lunaAnimator.SetBool("MoveState", true);
        lunaAnimator.SetFloat("MoveValue",1);
        lunaTrans.DOLocalMove(lunaInitPos, 0.5f).OnComplete
            (() => { lunaAnimator.SetBool("MoveState", false); });
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MonsterAttack());
    }

    IEnumerator MonsterAttack()
    {
        monsterTrans.DOLocalMove(lunaInitPos - new Vector3(1.5f, 0, 0), 0.5f);
        yield return new WaitForSeconds(0.5f);
        monsterTrans.DOLocalMove(lunaInitPos, 0.2f).OnComplete(()=>
        {
            GameManager.Instance.PlaySound(monsterAttackSound);
            monsterTrans.DOLocalMove(lunaInitPos - new Vector3(1.5f, 0, 0), 0.2f);
            lunaAnimator.CrossFade("Hit",0);
            GameManager.Instance.PlaySound(hitSound);
            lunaSr.DOFade(0.3f, 0.2f).OnComplete(() => { lunaSr.DOFade(1, 0.2f); });
            JudgePlayerHP(-20);
        }
        );
        yield return new WaitForSeconds(0.4f);
        monsterTrans.DOLocalMove(monsterInitPos, 0.5f).OnComplete(() => 
        {
            UIManager.Instance.ShowOrHideBattlePanel(true);
        });
    }
    /// <summary>
    /// Luna防御
    /// </summary>
    public void LunaDefend()
    {
        StartCoroutine(PerformDefendLogic());
    }
    IEnumerator PerformDefendLogic()
    {
        UIManager.Instance.ShowOrHideBattlePanel(false);
        lunaAnimator.SetBool("Defend",true);
        monsterTrans.DOLocalMove(lunaInitPos - new Vector3(1.5f, 0, 0), 0.5f);
        yield return new WaitForSeconds(0.5f);
        monsterTrans.DOLocalMove(lunaInitPos, 0.2f).OnComplete(() =>
        {
            monsterTrans.DOLocalMove(lunaInitPos - new Vector3(1.5f, 0, 0), 0.2f);
            lunaTrans.DOLocalMove(lunaInitPos+new Vector3(1,0,0),0.2f).OnComplete
            (
                () => { lunaTrans.DOLocalMove(lunaInitPos, 0.2f); }
            );
        }
        );
        yield return new WaitForSeconds(0.4f);
        monsterTrans.DOLocalMove(monsterInitPos, 0.5f).OnComplete(() =>
        {
            UIManager.Instance.ShowOrHideBattlePanel(true);
            GameManager.Instance.PlaySound(monsterAttackSound);
            lunaAnimator.SetBool("Defend", false);
        });
    }
    /// <summary>
    /// Luna使用技能
    /// </summary>
    public void LunaUseSkill()
    {
        if (!GameManager.Instance.CanUsePlayerMP(30))
        {
            return;
        }
        StartCoroutine(PerformSkillLogic());
    }
    IEnumerator PerformSkillLogic()
    {
        UIManager.Instance.ShowOrHideBattlePanel(false);
        lunaAnimator.CrossFade("Skill",0);
        GameManager.Instance.AddOrDecreaseMP(-30);
        yield return new WaitForSeconds(0.35f);
        GameObject go= Instantiate(skillEffectGo,monsterTrans);
        go.transform.localPosition = Vector3.zero;
        GameManager.Instance.PlaySound(lunaAttackSound);
        GameManager.Instance.PlaySound(skillSound);
        yield return new WaitForSeconds(0.4f);
        monsterSr.DOFade(0.3f,0.2f).OnComplete(()=>
        {
            JudgeMonsterHP(-40);
        });
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MonsterAttack());
    }
    /// <summary>
    /// Luna回血
    /// </summary>
    public void LunaRecoverHP()
    {
        if (!GameManager.Instance.CanUsePlayerMP(50))
        {
            return;
        }
        StartCoroutine(PerformRecoverHPLogic());
    }
    IEnumerator PerformRecoverHPLogic()
    {
        UIManager.Instance.ShowOrHideBattlePanel(false);
        lunaAnimator.CrossFade("RecoverHP",0);
        GameManager.Instance.AddOrDecreaseMP(-50);
        GameManager.Instance.PlaySound(lunaAttackSound);
        GameManager.Instance.PlaySound(recoverSound);
        yield return new WaitForSeconds(0.1f);
        GameObject go = Instantiate(healEffectGo, lunaTrans);
        go.transform.localPosition = Vector3.zero;
        GameManager.Instance.AddOrDecreaseHP(40);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MonsterAttack());
    }

    /// <summary>
    /// 改变玩家血量
    /// </summary>
    /// <param name="value"></param>
    private void JudgePlayerHP(int value)
    {
        GameManager.Instance.AddOrDecreaseHP(value);
        if (GameManager.Instance.lunaCurrentHP<=0)
        {
            GameManager.Instance.PlaySound(dieSound);
            lunaAnimator.CrossFade("Die",0);
            lunaSr.DOFade(0, 0.8f).OnComplete(() => { GameManager.Instance.EnterOrExitBattle(false); });
        }
    }
    /// <summary>
    /// 改变敌人血量
    /// </summary>
    /// <param name="value"></param>
    private void JudgeMonsterHP(int value)
    {
        if (GameManager.Instance.AddOrDecreaseMonsterHP(value)<= 0)
        {
            GameManager.Instance.PlaySound(monsterDieSound);
            monsterSr.DOFade(0, 0.4f).OnComplete(() => { GameManager.Instance.EnterOrExitBattle(false,1); });
        }
        else
        {
            monsterSr.DOFade(1, 0.2f);
        }
    }
    /// <summary>
    /// luna逃跑
    /// </summary>
    public void LunaEscape()
    {
        UIManager.Instance.ShowOrHideBattlePanel(false);
        lunaTrans.DOLocalMove(lunaInitPos+new Vector3(5,0,0),0.5f).OnComplete
            (
                () => { GameManager.Instance.EnterOrExitBattle(false); }
            );
        lunaAnimator.SetBool("MoveState", true);
        lunaAnimator.SetFloat("MoveValue", 1);
    }
}
