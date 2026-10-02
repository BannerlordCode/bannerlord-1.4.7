using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x0200001F RID: 31
	public class MissionPreloadView : MissionView
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x00009DEC File Offset: 0x00007FEC
		public override void OnPreMissionTick(float dt)
		{
			if (!this._preloadDone)
			{
				List<BasicCharacterObject> list = new List<BasicCharacterObject>();
				foreach (PartyBase partyBase in MapEvent.PlayerMapEvent.InvolvedParties)
				{
					foreach (TroopRosterElement troopRosterElement in partyBase.MemberRoster.GetTroopRoster())
					{
						for (int i = 0; i < troopRosterElement.Number; i++)
						{
							list.Add(troopRosterElement.Character);
						}
					}
				}
				this._helperInstance.PreloadCharacters(list);
				SiegeDeploymentMissionController missionBehavior = base.Mission.GetMissionBehavior<SiegeDeploymentMissionController>();
				if (missionBehavior != null)
				{
					this._helperInstance.PreloadItems(missionBehavior.GetSiegeMissiles());
				}
				this._preloadDone = true;
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00009EDC File Offset: 0x000080DC
		public override void OnSceneRenderingStarted()
		{
			this._helperInstance.WaitForMeshesToBeLoaded();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00009EE9 File Offset: 0x000080E9
		public override void OnMissionStateDeactivated()
		{
			base.OnMissionStateDeactivated();
			this._helperInstance.Clear();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00009EFC File Offset: 0x000080FC
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			this._helperInstance.Clear();
		}

		// Token: 0x0400007A RID: 122
		private readonly PreloadHelper _helperInstance = new PreloadHelper();

		// Token: 0x0400007B RID: 123
		private bool _preloadDone;
	}
}
