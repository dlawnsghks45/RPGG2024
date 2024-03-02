using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class petdatabase
{
    string petid;
    int petstar;
    int havecount;
    bool isequip;
    private bool ishave;

   public petdatabase()
   {
      
   }

   public petdatabase(JsonData data)
   {
      Petid = data["Petid"].ToString();
      Isequip = bool.Parse(data["Isequip"].ToString());
      Ishave = bool.Parse(data["Ishave"].ToString());
      Petstar = int.Parse(data["Petstar"].ToString());
      Havecount = int.Parse(data["Havecount"].ToString());

   }
   
   public petdatabase(string petid, int petstar, int havecount, bool isequip)
   {
      this.petid = petid;
      this.petstar = petstar;
      this.havecount = havecount;
      this.isequip = isequip;
   }

   public petdatabase(string petid, int petstar, int havecount, bool isequip, bool ishave)
   {
      this.petid = petid;
      this.petstar = petstar;
      this.havecount = havecount;
      this.isequip = isequip;
      this.ishave = ishave;
   }

   public bool Ishave
   {
      get => ishave;
      set => ishave = value;
   }

   public bool Isequip
   {
      get => isequip;
      set => isequip = value;
   }

   public int Havecount
   {
      get => havecount;
      set => havecount = value;
   }

   public int Petstar
   {
      get => petstar;
      set => petstar = value;
   }

   public string Petid
   {
      get => petid;
      set => petid = value;
   }
   
}
