using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000290 RID: 656
	public abstract class MissionLogic : MissionBehavior
	{
		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002474 RID: 9332 RVA: 0x000848F2 File Offset: 0x00082AF2
		public override MissionBehaviorType BehaviorType
		{
			get
			{
				return MissionBehaviorType.Logic;
			}
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000848F5 File Offset: 0x00082AF5
		public virtual InquiryData OnEndMissionRequest(out bool canLeave)
		{
			canLeave = true;
			return null;
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x000848FB File Offset: 0x00082AFB
		public virtual bool MissionEnded(ref MissionResult missionResult)
		{
			return false;
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x000848FE File Offset: 0x00082AFE
		public virtual void OnBattleEnded()
		{
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x00084900 File Offset: 0x00082B00
		public virtual void ShowBattleResults()
		{
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x00084902 File Offset: 0x00082B02
		public virtual void OnRetreatMission()
		{
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x00084904 File Offset: 0x00082B04
		public virtual void OnSurrenderMission()
		{
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x00084906 File Offset: 0x00082B06
		public virtual void OnAutoDeployTeam(Team team)
		{
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x00084908 File Offset: 0x00082B08
		public virtual List<EquipmentElement> GetExtraEquipmentElementsForCharacter(BasicCharacterObject character, bool getAllEquipments = false)
		{
			return null;
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x0008490B File Offset: 0x00082B0B
		public virtual void OnMissionResultReady(MissionResult missionResult)
		{
		}
	}
}
