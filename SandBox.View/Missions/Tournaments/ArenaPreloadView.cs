using System;
using System.Collections.Generic;
using SandBox.Missions.MissionLogics.Arena;
using SandBox.Tournaments.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions.Tournaments
{
	// Token: 0x02000028 RID: 40
	internal class ArenaPreloadView : MissionView
	{
		// Token: 0x06000111 RID: 273 RVA: 0x0000CA9C File Offset: 0x0000AC9C
		public override void OnPreMissionTick(float dt)
		{
			if (!this._preloadDone)
			{
				List<BasicCharacterObject> list = new List<BasicCharacterObject>();
				if (Mission.Current.GetMissionBehavior<ArenaPracticeFightMissionController>() != null)
				{
					foreach (CharacterObject characterObject in ArenaPracticeFightMissionController.GetParticipantCharacters(Settlement.CurrentSettlement))
					{
						list.Add(characterObject);
					}
					list.Add(CharacterObject.PlayerCharacter);
				}
				TournamentBehavior missionBehavior = Mission.Current.GetMissionBehavior<TournamentBehavior>();
				if (missionBehavior != null)
				{
					foreach (CharacterObject characterObject2 in missionBehavior.GetAllPossibleParticipants())
					{
						list.Add(characterObject2);
					}
				}
				this._helperInstance.PreloadCharacters(list);
				this._preloadDone = true;
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000CB80 File Offset: 0x0000AD80
		public override void OnSceneRenderingStarted()
		{
			this._helperInstance.WaitForMeshesToBeLoaded();
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000CB8D File Offset: 0x0000AD8D
		public override void OnMissionStateDeactivated()
		{
			base.OnMissionStateDeactivated();
			this._helperInstance.Clear();
		}

		// Token: 0x04000080 RID: 128
		private readonly PreloadHelper _helperInstance = new PreloadHelper();

		// Token: 0x04000081 RID: 129
		private bool _preloadDone;
	}
}
