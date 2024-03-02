using UnityEngine;
using BackEnd;
using UnityEngine.UI;

public class AttendanceManager : MonoBehaviour
{

    /*
    // 출석체크 창
    public GameObject attendanceWindow;
    // 출석체크 버튼
    public GameObject attendanceButton;
    // 보상 아이템 리스트
    public RewardItem[] rewardItems;

    // 보상 아이템 구조체
    [System.Serializable]
    public struct RewardItem
    {
        public int rewardCount;
        public string rewardName;
    }

    private int attendanceCount; // 출석일 수

    void Start()
    {
        // 뒤끝서버 초기화

        // 출석일 수를 가져옴
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

        // 출석체크 창 비활성화
        attendanceWindow.SetActive(false);

        // 출석 버튼 클릭 이벤트 등록
        attendanceButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            // 출석체크 창 활성화
            attendanceWindow.SetActive(true);

            // 보상 아이템 리스트에 있는 모든 아이템의 수만큼 보상 지급
            foreach (var rewardItem in rewardItems)
            {
                string itemName = rewardItem.rewardName;
                int itemCount = rewardItem.rewardCount;

                // 뒤끝서버에 보상 아이템 지급 요청 보냄
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

            // 출석일 수 업데이트
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
