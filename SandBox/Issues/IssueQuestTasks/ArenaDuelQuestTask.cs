using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Missions.MissionLogics.Arena;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Issues.IssueQuestTasks
{
	// Token: 0x020000BC RID: 188
	public class ArenaDuelQuestTask : QuestTaskBase
	{
		// Token: 0x060007C2 RID: 1986 RVA: 0x00034564 File Offset: 0x00032764
		public ArenaDuelQuestTask(CharacterObject duelOpponentCharacter, Settlement settlement, Action onSucceededAction, Action onFailedAction, DialogFlow dialogFlow = null)
			: base(dialogFlow, onSucceededAction, onFailedAction, null)
		{
			this._opponentCharacter = duelOpponentCharacter;
			this._settlement = settlement;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00034580 File Offset: 0x00032780
		public void AfterStart(IMission mission)
		{
			if (Mission.Current.HasMissionBehavior<ArenaDuelMissionBehavior>() && PlayerEncounter.LocationEncounter.Settlement == this._settlement)
			{
				this.InitializeTeams();
				List<MatrixFrame> list = (from e in Mission.Current.Scene.FindEntitiesWithTag("sp_arena_respawn")
					select e.GetGlobalFrame()).ToList<MatrixFrame>();
				MatrixFrame matrixFrame = list[MBRandom.RandomInt(list.Count)];
				float maxValue = float.MaxValue;
				MatrixFrame matrixFrame2 = matrixFrame;
				foreach (MatrixFrame matrixFrame3 in list)
				{
					if ((in matrixFrame) != (in matrixFrame3))
					{
						Vec3 origin = matrixFrame3.origin;
						if (origin.DistanceSquared(matrixFrame.origin) < maxValue)
						{
							matrixFrame2 = matrixFrame3;
						}
					}
				}
				matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
				matrixFrame2.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
				this._playerAgent = this.SpawnArenaAgent(CharacterObject.PlayerCharacter, Mission.Current.PlayerTeam, matrixFrame);
				this._opponentAgent = this.SpawnArenaAgent(this._opponentCharacter, Mission.Current.PlayerEnemyTeam, matrixFrame2);
			}
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x000346C0 File Offset: 0x000328C0
		public override void SetReferences()
		{
			CampaignEvents.AfterMissionStarted.AddNonSerializedListener(this, new Action<IMission>(this.AfterStart));
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			CampaignEvents.MissionTickEvent.AddNonSerializedListener(this, new Action<float>(this.MissionTick));
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00034712 File Offset: 0x00032912
		public void OnGameMenuOpened(MenuCallbackArgs args)
		{
			if (Hero.MainHero.CurrentSettlement == this._settlement)
			{
				if (this._duelStarted)
				{
					if (this._opponentAgent.IsActive())
					{
						base.Finish(QuestTaskBase.FinishStates.Fail);
						return;
					}
					base.Finish(QuestTaskBase.FinishStates.Success);
					return;
				}
				else
				{
					this.OpenArenaDuelMission();
				}
			}
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00034754 File Offset: 0x00032954
		public void MissionTick(float dt)
		{
			if (Mission.Current.HasMissionBehavior<ArenaDuelMissionBehavior>() && PlayerEncounter.LocationEncounter.Settlement == this._settlement && ((this._playerAgent != null && !this._playerAgent.IsActive()) || (this._opponentAgent != null && !this._opponentAgent.IsActive())))
			{
				if (this._missionEndTimer != null && this._missionEndTimer.ElapsedTime > 4f)
				{
					Mission.Current.EndMission();
					return;
				}
				if (this._missionEndTimer == null && ((this._playerAgent != null && !this._playerAgent.IsActive()) || (this._opponentAgent != null && !this._opponentAgent.IsActive())))
				{
					this._missionEndTimer = new BasicMissionTimer();
				}
			}
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00034814 File Offset: 0x00032A14
		private void OpenArenaDuelMission()
		{
			Location locationWithId = this._settlement.LocationComplex.GetLocationWithId("arena");
			int num = (this._settlement.IsTown ? this._settlement.Town.GetWallLevel() : 1);
			SandBoxMissions.OpenArenaDuelMission(locationWithId.GetSceneName(num), locationWithId);
			this._duelStarted = true;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00034870 File Offset: 0x00032A70
		private void InitializeTeams()
		{
			Mission.Current.Teams.Add(BattleSideEnum.Defender, Hero.MainHero.MapFaction.Color, Hero.MainHero.MapFaction.Color2, null, true, false, true);
			Mission.Current.Teams.Add(BattleSideEnum.Attacker, Hero.MainHero.MapFaction.Color2, Hero.MainHero.MapFaction.Color, null, true, false, true);
			Mission.Current.PlayerTeam = Mission.Current.DefenderTeam;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x000348F8 File Offset: 0x00032AF8
		private Agent SpawnArenaAgent(CharacterObject character, Team team, MatrixFrame frame)
		{
			if (team == Mission.Current.PlayerTeam)
			{
				character = CharacterObject.PlayerCharacter;
			}
			Equipment randomElement = this._settlement.Culture.DuelPresetEquipmentRoster.AllEquipments.GetRandomElement<Equipment>();
			Mission mission = Mission.Current;
			AgentBuildData agentBuildData = new AgentBuildData(character).Team(team).ClothingColor1(team.Color).ClothingColor2(team.Color2)
				.InitialPosition(in frame.origin);
			Vec2 vec = frame.rotation.f.AsVec2;
			vec = vec.Normalized();
			Agent agent = mission.SpawnAgent(agentBuildData.InitialDirection(in vec).NoHorses(true).Equipment(randomElement)
				.TroopOrigin(new SimpleAgentOrigin(character, -1, null, default(UniqueTroopDescriptor)))
				.Controller((character == CharacterObject.PlayerCharacter) ? AgentControllerType.Player : AgentControllerType.AI), false);
			if (agent.IsAIControlled)
			{
				agent.SetWatchState(Agent.WatchState.Alarmed);
			}
			return agent;
		}

		// Token: 0x04000421 RID: 1057
		private Settlement _settlement;

		// Token: 0x04000422 RID: 1058
		private CharacterObject _opponentCharacter;

		// Token: 0x04000423 RID: 1059
		private Agent _playerAgent;

		// Token: 0x04000424 RID: 1060
		private Agent _opponentAgent;

		// Token: 0x04000425 RID: 1061
		private bool _duelStarted;

		// Token: 0x04000426 RID: 1062
		private BasicMissionTimer _missionEndTimer;
	}
}
