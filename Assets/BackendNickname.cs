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

    //�Է�
    public void WriteString(string name)
    {
        NicknameInput_logo.text = name;
    }

    //�г��� ������ư
    public void Bt_CreateNickanme()
    {
        //�г����� �ǹٸ��� üũ
        if (!CheckispossibleNickname(NicknameInput_logo.text))
        {
            //caution_logo.text = TranslateManager.Instance.GetTranslate("UI/NoSpecialChar");// "�г����� �ѱ�, ����, ���ڸ� �����մϴ�.";
            return;
        }

        BackendReturnObject bro = Backend.BMember.CreateNickname(NicknameInput_logo.text);
        if (bro.IsSuccess())
        {
                //Debug.Log("�ε�" + bro.GetInDate());
            PlayerBackendData.Instance.nickname = NicknameInput_logo.text;
            Nickname_logoPanel.SetActive(false);
            GetComponent<BackendLogin>().Bt_SetClass();
           // PlayerBackendData.Instance.initNewPlayerData();
        }
        else
        {
            switch (bro.GetStatusCode())
            {
                case "409": //�ߺ��� �г���
                     caution.text = TranslateManager.Instance.GetTranslate("UI/�ߺ��г���");
                    break;

                case "400": //�г��ӿ� ��/�� ������ �ִ� ���
                   caution.text = TranslateManager.Instance.GetTranslate("UI/��г���ĭ");
                    break;
            }
            cauani.SetTrigger(Show);
        }
    }

    static bool CheckispossibleNickname(string nickname)
    {
        return Regex.IsMatch(nickname, "^[0-9a-zA-Z��-�R]*$");
    }

    public static bool CheckispossibleGuildName(string nickname)
    {
        return Regex.IsMatch(nickname, "^[a-zA-Z��-�R]*$");
    }
}
