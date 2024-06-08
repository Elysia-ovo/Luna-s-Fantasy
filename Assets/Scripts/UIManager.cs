using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Image hpMaskImage;
    public Image mpMaskImage;
    private float originalSize;//血条原始宽度
    public GameObject battlePanelGo;

    public GameObject TalkPanelGo;
    public Image characterImage;
    public Sprite[] characterSprtes;
    public Text nameText;
    public Text contentText;

    void Awake()
    {
        Instance = this;
        originalSize = hpMaskImage.rectTransform.rect.width;
        SetHPValue(1); 
    }
    /// <summary>
    /// 血条UI填充显示
    /// </summary>
    /// <param name="fillPercent">填充百分比</param>
    public void SetHPValue(float fillPercent)
    {
        hpMaskImage.rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,fillPercent*originalSize);
    }
    /// <summary>
    /// 蓝条UI填充显示
    /// </summary>
    /// <param name="fillPercent">填充百分比</param>
    public void SetMPValue(float fillPercent)
    {
        mpMaskImage.rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal, fillPercent * originalSize);
    }

    public void ShowOrHideBattlePanel(bool show)
    {
        battlePanelGo.SetActive(show);
    }
    /// <summary>
    /// 显示对话内容（包含人物的切换，名字的更换，对话内容的更换）
    /// </summary>
    /// <param name="content"></param>
    /// <param name="name"></param>
    public void ShowDialog(string content = null, string name = null)
    {
        //关闭
        if (content == null)
        {
            TalkPanelGo.SetActive(false);
        }
        else
        {
            TalkPanelGo.SetActive(true);
            if (name != null)
            {
                if (name == "Luna")
                {
                    characterImage.sprite = characterSprtes[0];
                }
                else
                {
                    characterImage.sprite = characterSprtes[1];
                }
                characterImage.SetNativeSize();
            }
            contentText.text = content;
            nameText.text = name;
        }
    }
}
