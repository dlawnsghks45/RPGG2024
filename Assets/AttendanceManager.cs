using UnityEngine;
using BackEnd;
using UnityEngine.UI;

public class AttendanceManager : MonoBehaviour
{

    /*
    // �⼮üũ â
    public GameObject attendanceWindow;
    // �⼮üũ ��ư
    public GameObject attendanceButton;
    // ���� ������ ����Ʈ
    public RewardItem[] rewardItems;

    // ���� ������ ����ü
    [System.Serializable]
    public struct RewardItem
    {
        public int rewardCount;
        public string rewardName;
    }

    private int attendanceCount; // �⼮�� ��

    void Start()
    {
        // �ڳ����� �ʱ�ȭ

        // �⼮�� ���� ������
        BackendReturnObject bro = Backend.GameData.GetMyData("attendance_count",backend.Instance.);
            {
                if (callback.IsSuccess() == false)
                {
                    Debug.Log("Failed to get attendance count: " + callback.ToString());
                    return;
                }

                attendanceCount = callback.GetReturnValuetoJSON()["data"].AsInt;
                Debug.Log("Attendance count: " + attendanceCount);
            });

        // �⼮üũ â ��Ȱ��ȭ
        attendanceWindow.SetActive(false);

        // �⼮ ��ư Ŭ�� �̺�Ʈ ���
        attendanceButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            // �⼮üũ â Ȱ��ȭ
            attendanceWindow.SetActive(true);

            // ���� ������ ����Ʈ�� �ִ� ��� �������� ����ŭ ���� ����
            foreach (var rewardItem in rewardItems)
            {
                string itemName = rewardItem.rewardName;
                int itemCount = rewardItem.rewardCount;

                // �ڳ������� ���� ������ ���� ��û ����
                Backend.GameData.GiveWithOutAuth(itemName, itemCount, callback =>
                {
                    if (callback.IsSuccess() == false)
                    {
                        Debug.Log("Failed to give reward: " + callback.ToString());
                        return;
                    }

                    Debug.Log("Succeeded to give reward: " + callback.GetReturnValue());
                });
            }

            // �⼮�� �� ������Ʈ
            attendanceCount++;
            Backend.GameData.Update("attendance_count", attendanceCount, callback =>
            {
                if (callback.IsSuccess() == false)
                {
                    Debug.Log("Failed to update attendance count: " + callback.ToString());
                    return;
                }

                Debug.Log("Succeeded to update attendance count: " + callback.GetReturnValue());
            });
        });
    }
    */
}
