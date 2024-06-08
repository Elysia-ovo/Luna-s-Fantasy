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
    private float originalSize;//Ѫ��ԭʼ���
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
    /// Ѫ��UI�����ʾ
    /// </summary>
    /// <param name="fillPercent">���ٷֱ�</param>
    public void SetHPValue(float fillPercent)
    {
        hpMaskImage.rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,fillPercent*originalSize);
    }
    /// <summary>
    /// ����UI�����ʾ
    /// </summary>
    /// <param name="fillPercent">���ٷֱ�</param>
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
    /// ��ʾ�Ի����ݣ�����������л������ֵĸ������Ի����ݵĸ�����
    /// </summary>
    /// <param name="content"></param>
    /// <param name="name"></param>
    public void ShowDialog(string content = null, string name = null)
    {
        //�ر�
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
