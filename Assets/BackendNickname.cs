using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using TMPro;
using I2.Loc;
public class BackendNickname : MonoBehaviour
{
    public GameObject Nickname_logoPanel;
    public TMP_InputField NicknameInput_logo;
    public Text caution;
    public Animator cauani;
    private static readonly int Show = Animator.StringToHash("show");

    private void OnDestroy()
    {
        Nickname_logoPanel = null;
        NicknameInput_logo = null;
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    //ÀÔ·Â
    public void WriteString(string name)
    {
        NicknameInput_logo.text = name;
    }

    //´Ð³×ÀÓ »ý¼º¹öÆ°
    public void Bt_CreateNickanme()
    {
        //´Ð³×ÀÓÀÌ ¿Ç¹Ù¸¥Áö Ã¼Å©
        if (!CheckispossibleNickname(NicknameInput_logo.text))
        {
            //caution_logo.text = TranslateManager.Instance.GetTranslate("UI/NoSpecialChar");// "´Ð³×ÀÓÀº ÇÑ±Û, ¿µ¾î, ¼ýÀÚ¸¸ °¡´ÉÇÕ´Ï´Ù.";
            return;
        }

        BackendReturnObject bro = Backend.BMember.CreateNickname(NicknameInput_logo.text);
        if (bro.IsSuccess())
        {
                //Debug.Log("ÀÎµ¥" + bro.GetInDate());
            PlayerBackendData.Instance.nickname = NicknameInput_logo.text;
            Nickname_logoPanel.SetActive(false);
            GetComponent<BackendLogin>().Bt_SetClass();
           // PlayerBackendData.Instance.initNewPlayerData();
        }
        else
        {
            switch (bro.GetStatusCode())
            {
                case "409": //Áßº¹µÈ ´Ð³×ÀÓ
                     caution.text = TranslateManager.Instance.GetTranslate("UI/Áßº¹´Ð³×ÀÓ");
                    break;

                case "400": //´Ð³×ÀÓ¿¡ ¾Õ/µÚ °ø¹éÀÌ ÀÖ´Â °æ¿ì
                   caution.text = TranslateManager.Instance.GetTranslate("UI/ºó´Ð³×ÀÓÄ­");
                    break;
            }
            cauani.SetTrigger(Show);
        }
    }

    static bool CheckispossibleNickname(string nickname)
    {
        return Regex.IsMatch(nickname, "^[0-9a-zA-Z°¡-ÆR]*$");
    }

    public static bool CheckispossibleGuildName(string nickname)
    {
        return Regex.IsMatch(nickname, "^[a-zA-Z°¡-ÆR]*$");
    }
}
