using System;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000065 RID: 101
	public class CampaignSiegeStateHandler : MissionLogic
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x000174F0 File Offset: 0x000156F0
		public bool IsSiege
		{
			get
			{
				return this._mapEvent.IsSiegeAssault;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x000174FD File Offset: 0x000156FD
		public bool IsSallyOut
		{
			get
			{
				return this._mapEvent.IsSallyOut;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0001750A File Offset: 0x0001570A
		public Settlement Settlement
		{
			get
			{
				return this._mapEvent.MapEventSettlement;
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00017517 File Offset: 0x00015717
		public CampaignSiegeStateHandler()
		{
			this._mapEvent = PlayerEncounter.Battle;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001752A File Offset: 0x0001572A
		public override void OnRetreatMission()
		{
			this._isRetreat = true;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00017533 File Offset: 0x00015733
		public override void OnMissionResultReady(MissionResult missionResult)
		{
			this._defenderVictory = missionResult.BattleState == BattleState.DefenderVictory;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00017544 File Offset: 0x00015744
		public override void OnSurrenderMission()
		{
			PlayerEncounter.PlayerSurrender = true;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001754C File Offset: 0x0001574C
		protected override void OnEndMission()
		{
			if (this.IsSiege && this._mapEvent.PlayerSide == BattleSideEnum.Attacker && !this._isRetreat && !this._defenderVictory)
			{
				this.Settlement.SetNextSiegeState();
			}
		}

		// Token: 0x04000211 RID: 529
		private readonly MapEvent _mapEvent;

		// Token: 0x04000212 RID: 530
		private bool _isRetreat;

		// Token: 0x04000213 RID: 531
		private bool _defenderVictory;
	}
}
