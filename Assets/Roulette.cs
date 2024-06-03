using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using Doozy.Engine.UI;
using UnityEngine.Events;
using UnityEngine;

public class Roulette : MonoBehaviour
{
	[SerializeField] private string iD;
	private RouletteDB.Row Data;
	[SerializeField] private ParticleImage particle1; 
	[SerializeField] private ParticleImage particle2; 
	
	private string needid;
	private int needhw;
	private string extragive;
	private string[] extragivehw;
	[SerializeField]
	private int extragivecount;
	private int roulettenum;
	private string[] Rare;
	private string[] Percent;
	
	[SerializeField]
	private	Transform			piecePrefab;				// �귿�� ǥ�õǴ� ���� ������
	[SerializeField]
	private	Transform			linePrefab;					// �������� �����ϴ� �� ������
	[SerializeField]
	private	Transform			pieceParent;				// �������� ��ġ�Ǵ� �θ� Transform
	[SerializeField]
	private	Transform			lineParent;					// ������ ��ġ�Ǵ� �θ� Transform
	[SerializeField]
	private	List<RoulettePieceData>	roulettePieceData = new List<RoulettePieceData>();			// �귿�� ǥ�õǴ� ���� �迭

	[SerializeField]
	private	int					spinDuration;				// ȸ�� �ð�
	[SerializeField]
	private	Transform			spinningRoulette;			// ���� ȸ���ϴ� ȸ���� Transfrom
	[SerializeField]
	private	AnimationCurve		spinningCurve;				// ȸ�� �ӵ� ��� ���� �׷���

	private	float				pieceAngle;					// ���� �ϳ��� ��ġ�Ǵ� ����
	private	float				halfPieceAngle;				// ���� �ϳ��� ��ġ�Ǵ� ������ ���� ũ��
	private	float				halfPieceAngleWithPaddings;	// ���� ���⸦ ����� Padding�� ���Ե� ���� ũ��
	
	private	int					accumulatedWeight;			// ����ġ ����� ���� ����
	private	bool				isSpinning = false;			// ���� ȸ��������
	private	int					selectedIndex = 0;			// �귿���� ���õ� ������

	public UIButton StartButton;

	public GameObject Block;
	public GameObject AutoStopBlock;
	
	[SerializeField] private UIToggle Toggle;

	[SerializeField] private gaugebarslot extragivebar;


	[SerializeField] private percentdataslot[] PercentData;
	[SerializeField] private UIView PercentPanel;


	public void Bt_ShowPercent()
	{
		for (int i = 0; i < PercentData.Length; i++)
		{
			PercentData[i].gameObject.SetActive(false);
		}

		for (int i = 0; i < id.Length; i++)
		{
			PercentData[i].Refresh(id[i],hw[i],Percent[i]);
			PercentData[i].gameObject.SetActive(true);
		}

		PercentPanel.Show(false);
	}

	
	string[] id;
	string[] hw;
	string[] chance;
	
	private void Awake()
	{
		Data = RouletteDB.Instance.Find_id(iD);

		
		needid = Data.needitem;
		needhw = int.Parse(Data.needhowmany);
		extragive = Data.extragive;
		Rare = Data.Rare.Split(';');
		extragivehw = Data.extragivehw.Split(';');
//		Debug.Log(Data.extragivecount);
		extragivecount = int.Parse(Data.extragivecount);
		roulettenum = int.Parse(Data.num);
		
		id = Data.reward.Split(';');
		hw= Data.howmany.Split(';');
		chance = Data.Chance.Split(';');
		Percent = Data.Percent.Split(';');
		
		for (int i = 0; i < id.Length; i++)
		{
			RoulettePieceData temp = new RoulettePieceData();
			temp.id = id[i];
			temp.hw = int.Parse(hw[i]);
			temp.chance = int.Parse(chance[i]);
			temp.rare = Rare[i];
			roulettePieceData.Add(temp);
		}
		
		pieceAngle					= 360 / roulettePieceData.Count;
		halfPieceAngle				= pieceAngle * 0.5f;
		halfPieceAngleWithPaddings	= halfPieceAngle - (halfPieceAngle * 0.25f);

		
		extragivebar.RefreshBar(PlayerBackendData.Instance.RouletteCount[roulettenum],extragivecount);
		SpawnPiecesAndLines();
		CalculateWeightsAndIndices();

		// Debug..
		//Debug.Log($"Index : {GetRandomIndex()}");

	
	}

	private void SpawnPiecesAndLines()
	{
		for ( int i = 0; i < roulettePieceData.Count; ++ i )
		{
			Transform piece = Instantiate(piecePrefab, pieceParent.position, Quaternion.identity, pieceParent);
			// ������ �귿 ������ ���� ���� (������, ����)
			piece.GetComponent<RoulettePiece>().SetUp(roulettePieceData[i]);
			// ������ �귿 ���� ȸ��
			piece.RotateAround(pieceParent.position, Vector3.back, (pieceAngle * i));

			Transform line = Instantiate(linePrefab, lineParent.position, Quaternion.identity, lineParent);
			// ������ �� ȸ�� (�귿 ���� ���̸� �����ϴ� �뵵)
			line.RotateAround(lineParent.position, Vector3.back, (pieceAngle * i) + halfPieceAngle);
		}
	}

	private void CalculateWeightsAndIndices()
	{
		for ( int i = 0; i < roulettePieceData.Count; ++ i )
		{
			roulettePieceData[i].index = i;

			// ����ó��. Ȥ�ö� chance���� 0 �����̸� 1�� ����
			if ( roulettePieceData[i].chance <= 0 )
			{
				roulettePieceData[i].chance = 1;
			}

			accumulatedWeight += roulettePieceData[i].chance;
			roulettePieceData[i].weight = accumulatedWeight;

		}
	}

	private int GetRandomIndex()
	{
		int weight = Random.Range(0, accumulatedWeight);

		for ( int i = 0; i < roulettePieceData.Count; ++ i )
		{
			if ( roulettePieceData[i].weight > weight )
			{
				return i;
			}
		}

		return 0;
	}

	public List<string> listid = new List<string>();
	public List<int> listhw = new List<int>();
	
	public List<string> listid2= new List<string>();
	public List<int> listhw2 = new List<int>();
	public int roulettecount;
	public void Bt_StartRecord()
	{
		listid2.Clear();
		listhw2.Clear();
		roulettecount = 0;
	}
	
	public void Bt_Spin()
	{
		if ( isSpinning == true ) return;

		listid.Clear();
		listhw.Clear();
		if (PlayerBackendData.Instance.CheckItemCount(needid) < needhw)
		{
			alertmanager.Instance.ShowAlert( Inventory.GetTranslate("UI5/������ ����"),alertmanager.alertenum.����);
			Toggle.IsOn = false;
			StartButton.Interactable = true;
			Block.SetActive(false);
			return;
		}

		if (Toggle.IsOn)
			AutoStopBlock.SetActive(true);
		else
		{
			AutoStopBlock.SetActive(false);
		}

		Block.SetActive(true);
		StartButton.Interactable = false;
		// �귿�� ��� �� ����
		selectedIndex = GetRandomIndex();
		// ���õ� ����� �߽� ����
		float angle			= pieceAngle * selectedIndex;
		// ��Ȯ�� �߽��� �ƴ� ��� �� ���� ���� ������ ���� ����
		float leftOffset	= (angle - halfPieceAngleWithPaddings) % 360;
		float rightOffset	= (angle + halfPieceAngleWithPaddings) % 360;
		float randomAngle	= Random.Range(leftOffset, rightOffset);

		// ��ǥ ����(targetAngle) = ��� ���� + 360 * ȸ�� �ð� * ȸ�� �ӵ�
		int	  rotateSpeed	= 2;
		float targetAngle	= (randomAngle + 360 * spinDuration * rotateSpeed);

		if (id.Equals("1000"))
		{
			TutorialTotalManager.Instance.CheckFinish();
		}
		
		switch (Rare[selectedIndex])
		{
			case "3":
				chatmanager.Instance.ChattoRoullet(roulettePieceData[selectedIndex].id,needid,roulettePieceData[selectedIndex].hw.ToString());
				break;
		}
		
		Inventory.Instance.AddItem(roulettePieceData[selectedIndex].id,roulettePieceData[selectedIndex].hw);
		listid.Add(roulettePieceData[selectedIndex].id);
		listhw.Add(roulettePieceData[selectedIndex].hw);
		listid2.Add(Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(roulettePieceData[selectedIndex].id).name));
		listhw2.Add(roulettePieceData[selectedIndex].hw);
		PlayerBackendData.Instance.RemoveItem(needid,needhw);

	
		
		PlayerBackendData.Instance.RouletteCount[roulettenum]++;
		if (PlayerBackendData.Instance.RouletteCount[roulettenum] >= extragivecount)
		{
			//�߰� ����
			int ran = Random.Range(int.Parse(extragivehw[0]), int.Parse(extragivehw[1]));
			Inventory.Instance.AddItem(extragive,ran);
			listid.Add(extragive);
			listhw.Add(ran);
			PlayerBackendData.Instance.RouletteCount[roulettenum] = 0;
		}
		//�α�
		roulettecount++;
		extragivebar.RefreshBar(PlayerBackendData.Instance.RouletteCount[roulettenum],extragivecount);
		Savemanager.Instance.SaveInventory();
		Savemanager.Instance.SaveRoulette();
		Savemanager.Instance.Save();
		TutorialTotalManager.Instance.CheckFinish();
		isSpinning = true;
		StartCoroutine(OnSpin(targetAngle));
	}
	private WaitForSeconds wait = new WaitForSeconds(0.1f);
	private WaitForSeconds wait2 = new WaitForSeconds(0.3f);
	private IEnumerator OnSpin(float end)
	{
		float current = 0;
		float percent = 0;
		float spinduration = Toggle.IsOn ? 1f : 5f;
		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / spinduration;

			float z = Mathf.Lerp(0, end, spinningCurve.Evaluate(percent));
			spinningRoulette.rotation = Quaternion.Euler(0, 0, z);
			yield return null;
		}
		yield return
			isSpinning = false;
		yield return wait;
		switch (Rare[selectedIndex])
		{
			//case "1":
				//particle1.Play();
				//break;
			case "2":
			case "3":
				particle2.Play();
				Soundmanager.Instance.PlayerSound("Attack/Magic/Buff5");
				break;
		}
		Inventory.Instance.ShowEarnItem3(listid.ToArray(), listhw.ToArray(), false);
		if (Toggle.IsOn)
		{
			yield return wait2;
			Bt_Spin();
		}
		else
		{
			if (roulettecount > 100)
			{
				LogManager.LogSave_Roulette(iD,listid2.ToArray(),listhw2.ToArray(),roulettecount);
			
				roulettecount = 0;
			}
			else
			{
				LogManager.LogSave_Roulette(iD,listid2.ToArray(),listhw2.ToArray(),roulettecount);
				roulettecount = 0;
			}
			Block.SetActive(false);
			alertmanager.Instance.NotiCheck_Roullet();
		}
		StartButton.Interactable = true;
	}
	public void bt_logData()
	{
		LogManager.LogSave_Roulette(iD,listid2.ToArray(),listhw2.ToArray(),roulettecount);
	}
	public void Bt_Exit()
	{
		LogManager.LogSave_Roulette(iD,listid2.ToArray(),listhw2.ToArray(),roulettecount);
		Settingmanager.Instance.SaveDataALl(false,false,false);
		roulettecount = 0;
		Toggle.IsOn = false;
		StartButton.Interactable = true;
		Block.SetActive(false);
		alertmanager.Instance.NotiCheck_Roullet();

	}
}

