using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Test : MonoBehaviour
{
    //�̱��游���.
    private static Test _instance = null;
    public static Test Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Test)) as Test;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
        
    }
    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(720, 1280, true);
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    public void bt_guildjoin()
    {
        SendQueue.Enqueue(Backend.Guild.ApplyGuildV3,"2023-06-08T15:12:35.508Z", ( callback ) => 
        {
            // ���� ó��
            if (callback.IsSuccess())
            {
                Debug.Log("���Խ�û����");
                //��ð��Կ��� Ȯ���� 
                GuildManager.Instance.GuildJoinSucc();
                    
            }
        }); 
    }
    
    public void bt_guildjoinOn()
    {
        Backend.Guild.SetRegistrationValueV3(true); // ��� ���� ����
    }
    
    public void btUptime(float time)
    {
        Time.timeScale += time;
    }

    public void SetMyGuildToRandom()
    {
        SendQueue.Enqueue(Backend.RandomInfo.SetRandomData, RandomType.Guild, "cb5a9f10-069d-11ee-b7ff-45e1842bef31",  1, callback => 
        {
            if (callback.IsSuccess())
            {
                Debug.Log("����忡 �ֱ� ����");   
            }
            // ���� ó��
        });
    }
    public void Bt_GiveItem()
    {
        Inventory.Instance.AddItem("52",1000);
        Inventory.Instance.AddItem("53",1000);
        Inventory.Instance.AddItem("54",1000);
    }

    public TMP_InputField itemid;
    public TMP_InputField itemcount;
    public TMP_InputField PLayername;

    public GameObject Testobj;
    
    [Title("����")]
    [Button(ButtonSizes.Large),GUIColor(0,1,0)]
    public void Contribute()
    {
        Savemanager.Instance.Save();
    }
    
    public void Bt_Make()
    {
        if (itemid.text.Contains("E"))
        {
            //���
            PlayerBackendData.Instance.MakeEquipment(itemid.text��);
        }
        else
        {
            Inventory.Instance.AddItem(itemid.text,int.Parse(itemcount.text));
        }
    }
    
    public void GameDataGet()
    {
        
        var bro2 = Backend.Social.GetUserInfoByNickName(PLayername.text);
        
        //example
        string gamerIndate = bro2.GetReturnValuetoJSON()["row"]["inDate"].ToString();
        // key �÷��� ���� keyCode�� ������ �˻�
        Where where = new Where();
        where.Equal("owner_inDate",gamerIndate);
        
        
        // Step 3. �������� �ҷ����� �����ϱ�
        Debug.Log("���� ���� ��ȸ �Լ��� ȣ���մϴ�.");
        var bro = Backend.GameData.Get("PlayerData", where);
        if (bro.IsSuccess())
        {
            Debug.Log("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);

            PlayerBackendData userData = PlayerBackendData.Instance;
            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json���� ���ϵ� �����͸� �޾ƿɴϴ�.

            // �޾ƿ� �������� ������ 0�̶�� �����Ͱ� �������� �ʴ� ���Դϴ�.
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            }
            else
            {
                //����
                userData.SetLv(int.Parse(gameDataJson[0]["level"].ToString()));
                userData.AddExp(decimal.Parse(gameDataJson[0]["levelExp"].ToString()));
                userData.SetAchLv(int.Parse(gameDataJson[0]["level_ach"].ToString()));
                userData.AddAchExp(decimal.Parse(gameDataJson[0]["level_achExp"].ToString()));
                //����
                if (gameDataJson[0].ContainsKey("Altar_Lvs"))
                {
                    //Ŭ���� ������
                    for (int i = 0; i < gameDataJson[0]["Altar_Lvs"].Count; i++)
                    {
                        //  Debug.Log("�����Է�" + key);
                        PlayerBackendData.Instance.Altar_Lvs[i] = int.Parse(gameDataJson[0]["Altar_Lvs"][i].ToString());
                    }
                }
                else
                {
                    userData.SetAltarLv(int.Parse(gameDataJson[0]["level_altar"].ToString()));
                    PlayerBackendData.Instance.Altar_Lvs[0] = int.Parse(gameDataJson[0]["level_altar"].ToString());
                }

                userData.SetAdLv(int.Parse(gameDataJson[0]["level_adven"].ToString()));


                userData.SetFieldLV(int.Parse(gameDataJson[0]["FieldLv"].ToString()));

                //Debug.Log(gameDataJson[0]["inventory"]);

                //�κ��丮 ��������
                userData.ItemInventory.Clear();
                for (int i = 0; i < gameDataJson[0]["inventory"].Count; i++)
                {
                    userData.ItemInventory.Add(new ItemInven(gameDataJson[0]["inventory"][i]["Id"].ToString(),
                        int.Parse(gameDataJson[0]["inventory"][i]["Howmany"].ToString()),
                        gameDataJson[0]["inventory"][i]["Expiredate"].ToString()));
                }

                if (gameDataJson[0].ContainsKey("CollectionNew"))
                {
                    //Ŭ���� ������
                    for (int i = 0; i < gameDataJson[0]["CollectionNew"].Count; i++)
                    {
                        //  Debug.Log("�����Է�" + key);
                        PlayerBackendData.Instance.RenewalCollectData[i]
                            = bool.Parse(gameDataJson[0]["CollectionNew"][i].ToString());
                    }
                }

                for (int i = 0; i < gameDataJson[0]["EquipmentNow"].Count; i++)
                {
                    if (gameDataJson[0]["EquipmentNow"][i] != null)
                    {
                        try
                        {
                            //Debug.Log("Ű��");
                            //Debug.Log(gameDataJson[0]["EquipmentNow"][i]["Itemid"].ToString());
                            userData.EquipEquiptment0[i] = new EquipDatabase(gameDataJson[0]["EquipmentNow"][i]);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }



                //��� ��������
                userData.Equiptment0.Clear();
                foreach (string key in gameDataJson[0]["equipment_Weapon"].Keys)
                {
                    userData.Equiptment0.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Weapon"][key]));
                }

                userData.Equiptment1.Clear();
                foreach (string key in gameDataJson[0]["equipment_SubWeapon"].Keys)
                {
                    userData.Equiptment1.Add(key, new EquipDatabase(gameDataJson[0]["equipment_SubWeapon"][key]));
                }

                userData.Equiptment2.Clear();
                foreach (string key in gameDataJson[0]["equipment_Helmet"].Keys)
                {
                    userData.Equiptment2.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Helmet"][key]));
                }

                userData.Equiptment3.Clear();
                foreach (string key in gameDataJson[0]["equipment_Armor"].Keys)
                {
                    userData.Equiptment3.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Armor"][key]));
                }

                userData.Equiptment4.Clear();
                foreach (string key in gameDataJson[0]["equipment_Glove"].Keys)
                {
                    userData.Equiptment4.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Glove"][key]));
                }

                userData.Equiptment5.Clear();
                foreach (string key in gameDataJson[0]["equipment_Boot"].Keys)
                {
                    userData.Equiptment5.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Boot"][key]));
                }

                userData.Equiptment6.Clear();
                foreach (string key in gameDataJson[0]["equipment_Ring"].Keys)
                {
                    userData.Equiptment6.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Ring"][key]));
                }

                userData.Equiptment7.Clear();
                foreach (string key in gameDataJson[0]["equipment_Necklace"].Keys)
                {
                    userData.Equiptment7.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Necklace"][key]));
                }

                userData.Equiptment8.Clear();
                if (gameDataJson[0].ContainsKey("equipment_Wing"))
                {
                    foreach (string key in gameDataJson[0]["equipment_Wing"].Keys)
                    {
                        userData.Equiptment8.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Wing"][key]));
                    }
                }

                userData.Equiptment9.Clear();
                if (gameDataJson[0].ContainsKey("equipment_Pet"))
                {
                    foreach (string key in gameDataJson[0]["equipment_Pet"].Keys)
                    {
                        userData.Equiptment9.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Pet"][key]));
                    }
                }

                userData.Equiptment10.Clear();
                if (gameDataJson[0].ContainsKey("equipment_Rune"))
                {
                    foreach (string key in gameDataJson[0]["equipment_Rune"].Keys)
                    {
                        userData.Equiptment10.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Rune"][key]));
                    }
                }


                userData.Equiptment11.Clear();
                if (gameDataJson[0].ContainsKey("equipment_Insignia"))
                {
                    foreach (string key in gameDataJson[0]["equipment_Insignia"].Keys)
                    {
                        userData.Equiptment11.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Insignia"][key]));
                    }
                }

                //��ų
                if (gameDataJson[0].ContainsKey("SkillInven"))
                {
                    //Ŭ���� ������
                    for(int i = 0 ; i < gameDataJson[0]["SkillInven"].Count;i++)
                    {
                        //  Debug.Log("�����Է�" + key);
                        PlayerBackendData.Instance.Skills.Add(gameDataJson[0]["SkillInven"][i].ToString());
                    }
                }

                //���׷� ���ٸ�
                if (PlayerBackendData.Instance.Skills.Count == 0)
                {
                    foreach (var VARIABLE in PlayerBackendData.Instance.ClassData)
                    {
                        if (!VARIABLE.Value.Isown) continue;
                            PlayerBackendData.Instance.Skills.Add(ClassDB.Instance.Find_id(VARIABLE.Value.ClassId1)
                                .giveskill);
                    }
                }

                
                //��1ȸ ���� ����
                if (gameDataJson[0].ContainsKey("Ability"))
                {
                    Debug.Log("�����Ƽ �ҷ���");
                    //Ŭ���� ������
                    for(int i = 0 ; i < gameDataJson[0]["Ability"].Count;i++)
                    {
                        PlayerBackendData.Instance.Abilitys[i] = gameDataJson[0]["Ability"][i].ToString();
                    }
                }
                
                if (gameDataJson[0].ContainsKey("PetData"))
                {
                    Debug.Log("��ҷ���");
                    //Ŭ���� ������
                    foreach (string key in gameDataJson[0]["PetData"].Keys)
                    {
//                Debug.Log("�� �ҷ���" + gameDataJson[0]["PetData"][key].Count);
                        PlayerBackendData.Instance.PetData[key] = new petdatabase(gameDataJson[0]["PetData"][key]);
                    }

                    for (int i = 0; i < gameDataJson[0]["PetCount"].Count; i++)
                    {
                        PlayerBackendData.Instance.PetCount[i] = int.Parse(gameDataJson[0]["PetCount"][i].ToString());
                    }
                }
                if (gameDataJson[0].ContainsKey("nowPetid"))
                {
                    PlayerBackendData.Instance.nowPetid = gameDataJson[0]["nowPetid"].ToString();
                }
                
                //Ŭ���� ������
                foreach (string key in gameDataJson[0]["Class"].Keys)
                {
                    //  Debug.Log("�����Է�" + key);
                    PlayerBackendData.Instance.ClassData[key] = new ClassDatabase(gameDataJson[0]["Class"][key]);
                }


                if (gameDataJson[0].ContainsKey("CollectionR"))
                {
                    //Ŭ���� ������
                    foreach (string key in gameDataJson[0]["CollectionR"].Keys)
                    {
                        //  Debug.Log("�����Է�" + key);
                        PlayerBackendData.Instance.CollectData[key] = new CollectDatabase(gameDataJson[0]["CollectionR"][key]);
                    }
                
                }

                //����
                if (gameDataJson[0].ContainsKey("Achievement"))
                {
                    foreach (string key in gameDataJson[0]["Achievement"].Keys)
                    {
                        //  Debug.Log("�����Է�" + key);
                        PlayerBackendData.Instance.PlayerAchieveData[key] = new Achievedata(gameDataJson[0]["Achievement"][key]);
                    }
                }


                //  Debug.Log("Ŭ���� ���̵�" + gameDataJson[0]["NowClass"]["ClassId1"].ToString());
                PlayerBackendData.Instance.ClassId = gameDataJson[0]["NowClass"]["ClassId1"].ToString();

                if (gameDataJson[0].ContainsKey("Stage"))
                {
                    PlayerBackendData.Instance.nowstage = gameDataJson[0]["Stage"].ToString();
                    PlayerBackendData.Instance.spawncount = (int.Parse(gameDataJson[0]["FieldCount"].ToString()));
                }
                else
                {
                    PlayerBackendData.Instance.nowstage = "1000";
                    PlayerBackendData.Instance.spawncount = 1;

                }
                
                if (gameDataJson[0].ContainsKey("Gold"))
                {
                    PlayerBackendData.Instance.SetMoney(decimal.Parse(gameDataJson[0]["Gold"].ToString()));
                    PlayerBackendData.Instance.SetCash(decimal.Parse(gameDataJson[0]["Crystal"].ToString()));
                    
                }
                
                //��1ȸ ���� ����
                if (gameDataJson[0].ContainsKey("OnceShopData"))
                {
                    //Ŭ���� ������
                    for(int i = 0 ; i < gameDataJson[0]["OnceShopData"].Count;i++)
                    {
                        //  Debug.Log("�����Է�" + key);
                        Timemanager.Instance.OncePremiumPackage.Add(gameDataJson[0]["OnceShopData"][i].ToString());
                    }
                }
                //��1ȸ ���� ����
                if (gameDataJson[0].ContainsKey("OnceTradeData"))
                {
                    //Ŭ���� ������
                    for(int i = 0 ; i < gameDataJson[0]["OnceTradeData"].Count;i++)
                    {
                        //  Debug.Log("�����Է�" + key);
                        Timemanager.Instance.OnceTradePackage.Add(gameDataJson[0]["OnceTradeData"][i].ToString());
                    }
                }

                //TutoId
                if (gameDataJson[0].ContainsKey("tutoguideid"))
                {
                    //Ŭ���� ������
                    PlayerBackendData.Instance.tutoguideid = int.Parse(gameDataJson[0]["tutoguideid"].ToString());
                }
                //TutoId
                if (gameDataJson[0].ContainsKey("tutoguideisfinish"))
                {
                    //Ŭ���� ������
                    PlayerBackendData.Instance.tutoguideisfinish = bool.Parse(gameDataJson[0]["tutoguideisfinish"].ToString());
                }
                //TutoId
                if (gameDataJson[0].ContainsKey("tutoguidepremium"))
                {
                    //Ŭ���� ������
                    PlayerBackendData.Instance.tutoguidepremium = bool.Parse(gameDataJson[0]["tutoguidepremium"].ToString());
                }  
                
                
                //TutoId
                if (gameDataJson[0].ContainsKey("Tutoid"))
                {
                    //Ŭ���� ������
                    PlayerBackendData.Instance.tutoid = gameDataJson[0]["Tutoid"].ToString();
                }
     
                //������ ����ð�
                if (gameDataJson[0].ContainsKey("SavedCheckTime"))
                {
                    //Ŭ���� ������
                    PlayerBackendData.Instance.SavedCheckTime = gameDataJson[0]["SavedCheckTime"].ToString();
                }
                //����
                if (gameDataJson[0].ContainsKey("AdsFree"))
                {
                    //Ŭ���� ������
                    PlayerBackendData.Instance.isadsfree = bool.Parse(gameDataJson[0]["AdsFree"].ToString());
                }
                /*
                //�ð�
                if (gameDataJson[0].ContainsKey("PlayerTimes"))
                {
                    //Ŭ���� ������
                    for(int i = 0 ; i < gameDataJson[0]["PlayerTimes"].Count;i++)
                    {
                        //  Debug.Log("�����Է�" + key);
                        PlayerBackendData.Instance.PlayerTimes[i] = DateTime.Parse(gameDataJson[0]["PlayerTimes"][i].ToString());
                    }
                }*/

                PlayerBackendData.Instance.PassiveClassId.Initialize();
                if (gameDataJson[0].ContainsKey("Passive"))
                {
                    //Ŭ���� ������
                    for(int i = 0 ; i < gameDataJson[0]["Passive"].Count;i++)
                    {
                        try
                        {
                            PlayerBackendData.Instance.PassiveClassId[i] = gameDataJson[0]["Passive"][i].ToString() != "True" ?gameDataJson[0]["Passive"][i].ToString() : "";
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                        //  Debug.Log("�����Է�" + key);
                    }
                }
                
                if ((gameDataJson[0].ContainsKey("craftmakingid")))
                {
                    //������
                    for (int i = 0; i < gameDataJson[0]["craftmakingid"].Count; i++)
                    {
                        if (gameDataJson[0]["craftmakingid"][i] != null)
                        {
                            try
                            {
                                PlayerBackendData.Instance.craftmakingid[i] =
                                    gameDataJson[0]["craftmakingid"][i].ToString();

                                PlayerBackendData.Instance.craftdatetime[i] =
                                    gameDataJson[0]["craftdatetime"][i].ToString();

                                PlayerBackendData.Instance.craftdatecount[i] =
                                    int.Parse(gameDataJson[0]["craftdatecount"][i].ToString());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                    }
                }
                
                string loadstring = Timemanager.Instance.GetServerTime().ToString();
                Param param = new Param
                {
                    //����
                    { "PlayerCanLoadBool", "false" },
                    { "PlayerLoadUID", SystemInfo.deviceUniqueIdentifier }, 
                    { "PlayerLoadModel", SystemInfo.deviceModel }, //��
                    { "PlayerLoadTime", loadstring} //�ҷ��½ð�
                };
                
                PlayerBackendData.Instance.playersaveindate = loadstring;
                    Savemanager.Instance.SaveOnlyLv();
                    Savemanager.Instance. SaveLoadTime();
                    Savemanager.Instance.SaveClassData();
                    Savemanager.Instance.SaveEquip();
                    Savemanager.Instance.SaveExpData();
                    Savemanager.Instance.SaveInventory();
                    Savemanager.Instance.SaveSkillData();
                    Savemanager.Instance. SaveMoneyDataDirect();
                    Savemanager.Instance. SaveCash();
                    Savemanager.Instance. SaveCraft();
                    Savemanager.Instance. SaveAchieve();
               Savemanager.Instance. SaveStageData();
               Savemanager.Instance. SaveGuideQuest();
               Savemanager.Instance. SaveCollection();
               Savemanager.Instance. SaveShopData();
               Savemanager.Instance.  SaveadsFree();
               Savemanager.Instance.Save();
        // key �÷��� ���� keyCode�� ������ �˻�
                Where where2 = new Where();
                where2.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
            
                SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where2, param, ( callback ) =>
                {
                    Debug.Log(callback);
                    // ���� ó��
                    if (!callback.IsSuccess()) return;

                });
                

            }

        }
        else
        {
            Debug.LogError("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);
        }
        
        
        PlayerData.Instance.RefreshPlayerstat();
        PlayerData.Instance.RefreshPlayerstat_Equip();
        PlayerData.Instance.RefreshMyBattlePoint();
        Inventory.Instance.RefreshInventory();
    }
    [Button(Name = "����")]
    public void Up()
    {
        achievemanager.Instance.AddCount(Acheves.����óġ,10000000);
    }

    
    [Button(Name = "����������")]
    public void ShopGive()
    {
        Levelshop.Instance.GiveTime2(21, 2);
    }
    
    [Button(Name = "����������2")]
    public void ShopGive2()
    {
        Levelshop.Instance.GiveTime2(4,3);

    }
    
    [Button(Name = "�����׽���")]
    public void Test1()
    {
        chatmanager.Instance.ChattoSmeltUp(7,"E1000","6");

    }

    [Button(Name = "��")]
    public void GetPet()
    {
       petmanager.Instance.OpenStartPetBox(10);
    }
    public string username;
    [Button(Name = "��ȸ")] 
    public void Bt_1123()
    {
        otherusermanager.Instance.ShowPlayerData(username);
    }
    
    [Button(Name = "HP")] 
    public void Bt_11223()
    {
        MultiGage.Instance.ObserveStart(3);
    }
}
