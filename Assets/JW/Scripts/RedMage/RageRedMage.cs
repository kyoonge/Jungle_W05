using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageRedMage : Boss
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	[SerializeField] private GameObject halo;
	#endregion

	#region PublicMethod
	public override void Initialize()
	{
		patternIndex = -1;
	}
	public void SetHpSameWithMain(int _hpCurrent, int _hpMax)
	{
		hpMax = _hpMax;
		hpCurrent = _hpCurrent;
	}
	public override void BossKilled()
	{
		base.BossKilled();
		halo.SetActive(false);
	}
	#endregion

	#region PrivateMethod
	protected override void OnEnable()
	{
		BossHpGUI.instance.SetBossNameText(bossName);
		BossHpGUI.instance.SetMaxHp(hpMax);
		BossHpGUI.instance.SetHp(hpCurrent);
		GameManager.instance.SetBoss(this);
		Initialize();
	}
	#endregion
}
