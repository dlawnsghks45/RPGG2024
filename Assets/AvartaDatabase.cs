using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvartaDatabase
{
     string BodyAvarta;
     string WeaponAvarta;
     string SubWeaponAvarta;

    public AvartaDatabase(string bodyAvarta, string weaponAvarta, string subWeaponAvarta)
    {
        BodyAvarta = bodyAvarta;
        WeaponAvarta = weaponAvarta;
        SubWeaponAvarta = subWeaponAvarta;
    }
    public AvartaDatabase()
    {
        BodyAvarta = "";
        WeaponAvarta = "";
        SubWeaponAvarta = "";
    }

    public string BodyAvarta1 { get => BodyAvarta; set => BodyAvarta = value; }
    public string WeaponAvarta1 { get => WeaponAvarta; set => WeaponAvarta = value; }
    public string SubWeaponAvarta1 { get => SubWeaponAvarta; set => SubWeaponAvarta = value; }
}
