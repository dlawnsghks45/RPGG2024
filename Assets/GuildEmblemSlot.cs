using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildEmblemSlot : MonoBehaviour
{
    public Image Flag;
    public Image Banner;



    public void SetEmblem(int F, int B)
    {
        Flag.sprite = GuildManager.Instance.Flags[F];
        Banner.sprite = GuildManager.Instance.Banners[B];
    }
}
