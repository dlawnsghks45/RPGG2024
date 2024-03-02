using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class 
    ClassDatabase
{
    private bool isown;
    private string ClassId;
    private int Lv;
    private string[] Skills;
    private string equippedavarta;
    private bool ispasstive;

    public ClassDatabase()
    {

    }

   

    public ClassDatabase(bool isown,string classId, int lv, string[] skills, string equippedavarta,bool ispasstive)
    {
        Isown = isown;
        ClassId = classId;
        Lv = lv;
        Skills = skills;
        this.equippedavarta = equippedavarta;
        Ispasstive = ispasstive;
    }

    public ClassDatabase(JsonData data)
    {
        ClassId1 = data["ClassId1"].ToString();
        Ispasstive = bool.Parse(data["Ispasstive"].ToString());
        Skills1 = new string[12];
        for (int i = 0; i < data["Skills1"].Count; i++)
        {
            try
            {
                if (data["Skills1"][i].ToString() == "True")
                {
                    Skills1[i] = null;
                }
                else
                {
                    Skills1[i] = data["Skills1"][i].ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        Lv1 = int.Parse(data["Lv1"].ToString());
        Isown =  bool.Parse(data["Isown"].ToString());
        Equippedavarta = data["Equippedavarta"].ToString();

    }
    public string ClassId1 { get => ClassId; set => ClassId = value; }
    public int Lv1 { get => Lv; set => Lv = value; }
    public string[] Skills1 { get => Skills; set => Skills = value; }
    public string Equippedavarta { get => equippedavarta; set => equippedavarta = value; }
    public bool Isown { get => isown; set => isown = value; }
    public bool Ispasstive { get => ispasstive; set => ispasstive = value; }
}
