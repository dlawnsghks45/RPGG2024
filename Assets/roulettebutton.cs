using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

public class roulettebutton : MonoBehaviour
{
    [SerializeField]
    private	Roulette	roulette;
    [SerializeField]
    private UIButton	buttonSpin;

    

    private void EndOfSpin(RoulettePieceData selectedData)
    {
        buttonSpin.Interactable = true;
    }

    public void Bt_EndSpin()
    {
        buttonSpin.Interactable = true;
    }
}
