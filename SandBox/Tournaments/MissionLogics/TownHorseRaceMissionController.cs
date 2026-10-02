using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Tournaments.AgentControllers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Tournaments.MissionLogics
{
	// Token: 0x02000030 RID: 48
	public class TownHorseRaceMissionController : MissionLogic, ITournamentGameBehavior
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000B884 File Offset: 0x00009A84
		// (set) Token: 0x060001BF RID: 447 RVA: 0x0000B88C File Offset: 0x00009A8C
		public List<TownHorseRaceMissionController.CheckPoint> CheckPoints { get; private set; }

		// Token: 0x060001C0 RID: 448 RVA: 0x0000B895 File Offset: 0x00009A95
		public TownHorseRaceMissionController(CultureObject culture)
		{
			this._culture = culture;
			this._agents = new List<TownHorseRaceAgentController>();
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000B8B0 File Offset: 0x00009AB0
		public override void AfterStart()
		{
			base.AfterStart();
			this.CollectCheckPointsAndStartPoints();
			foreach (TownHorseRaceAgentController townHorseRaceAgentController in this._agents)
			{
				townHorseRaceAgentController.DisableMovement();
			}
			this._startTimer = new BasicMissionTimer();
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000B918 File Offset: 0x00009B18
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._startTimer != null && this._startTimer.ElapsedTime > 3f)
			{
				foreach (TownHorseRaceAgentController townHorseRaceAgentController in this._agents)
				{
					townHorseRaceAgentController.Start();
				}
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000B98C File Offset: 0x00009B8C
		private void CollectCheckPointsAndStartPoints()
		{
			this.CheckPoints = new List<TownHorseRaceMissionController.CheckPoint>();
			foreach (WeakGameEntity weakGameEntity in base.Mission.ActiveMissionObjects.Select<MissionObject, WeakGameEntity>((MissionObject amo) => amo.GameEntity))
			{
				VolumeBox firstScriptOfType = weakGameEntity.GetFirstScriptOfType<VolumeBox>();
				if (firstScriptOfType != null)
				{
					this.CheckPoints.Add(new TownHorseRaceMissionController.CheckPoint(firstScriptOfType));
				}
			}
			this.CheckPoints = this.CheckPoints.OrderBy<TownHorseRaceMissionController.CheckPoint, string>((TownHorseRaceMissionController.CheckPoint x) => x.Name).ToList<TownHorseRaceMissionController.CheckPoint>();
			this._startPoints = base.Mission.Scene.FindEntitiesWithTag("sp_horse_race").ToList<GameEntity>();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000BA78 File Offset: 0x00009C78
		private MatrixFrame GetStartFrame(int index)
		{
			MatrixFrame matrixFrame;
			if (index < this._startPoints.Count)
			{
				matrixFrame = this._startPoints[index].GetGlobalFrame();
			}
			else
			{
				matrixFrame = ((this._startPoints.Count > 0) ? this._startPoints[0].GetGlobalFrame() : MatrixFrame.Identity);
			}
			matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			return matrixFrame;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000BADC File Offset: 0x00009CDC
		private void SetItemsAndSpawnCharacter(CharacterObject troop)
		{
			int count = this._agents.Count;
			Equipment equipment = new Equipment();
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.ArmorItemEndSlot, new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>("charger"), null, null, false));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.HorseHarness, new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>("horse_harness_e"), null, null, false));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>("horse_whip"), null, null, false));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Body, new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>("short_padded_robe"), null, null, false));
			MatrixFrame startFrame = this.GetStartFrame(count);
			AgentBuildData agentBuildData = new AgentBuildData(troop).Team(this._teams[count]).InitialPosition(in startFrame.origin);
			Vec2 vec = startFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).Equipment(equipment).Controller((troop == CharacterObject.PlayerCharacter) ? AgentControllerType.Player : AgentControllerType.AI);
			Agent agent = base.Mission.SpawnAgent(agentBuildData2, false);
			agent.Health = (float)agent.Monster.HitPoints;
			agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
			this._agents.Add(this.AddHorseRaceAgentController(agent));
			if (troop == CharacterObject.PlayerCharacter)
			{
				base.Mission.PlayerTeam = this._teams[count];
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000BC4D File Offset: 0x00009E4D
		private TownHorseRaceAgentController AddHorseRaceAgentController(Agent agent)
		{
			return agent.AddController(typeof(TownHorseRaceAgentController)) as TownHorseRaceAgentController;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000BC64 File Offset: 0x00009E64
		private void InitializeTeams(int count)
		{
			this._teams = new List<Team>();
			for (int i = 0; i < count; i++)
			{
				this._teams.Add(base.Mission.Teams.Add(BattleSideEnum.None, uint.MaxValue, uint.MaxValue, null, true, false, true));
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000BCAA File Offset: 0x00009EAA
		public void StartMatch(TournamentMatch match, bool isLastRound)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000BCB1 File Offset: 0x00009EB1
		public void SkipMatch(TournamentMatch match)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000BCB8 File Offset: 0x00009EB8
		public bool IsMatchEnded()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000BCBF File Offset: 0x00009EBF
		public void OnMatchEnded()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400009E RID: 158
		public const int TourCount = 2;

		// Token: 0x040000A0 RID: 160
		private readonly List<TownHorseRaceAgentController> _agents;

		// Token: 0x040000A1 RID: 161
		private List<Team> _teams;

		// Token: 0x040000A2 RID: 162
		private List<GameEntity> _startPoints;

		// Token: 0x040000A3 RID: 163
		private BasicMissionTimer _startTimer;

		// Token: 0x040000A4 RID: 164
		private CultureObject _culture;

		// Token: 0x02000143 RID: 323
		public class CheckPoint
		{
			// Token: 0x1700012D RID: 301
			// (get) Token: 0x06000E0C RID: 3596 RVA: 0x00064B18 File Offset: 0x00062D18
			public string Name
			{
				get
				{
					return this._volumeBox.GameEntity.Name;
				}
			}

			// Token: 0x06000E0D RID: 3597 RVA: 0x00064B38 File Offset: 0x00062D38
			public CheckPoint(VolumeBox volumeBox)
			{
				this._volumeBox = volumeBox;
				this._bestTargetList = GameEntity.CreateFromWeakEntity(this._volumeBox.GameEntity).CollectChildrenEntitiesWithTag("best_target_point");
				this._volumeBox.SetIsOccupiedDelegate(new VolumeBox.VolumeBoxDelegate(this.OnAgentsEnterCheckBox));
			}

			// Token: 0x06000E0E RID: 3598 RVA: 0x00064B8C File Offset: 0x00062D8C
			public Vec3 GetBestTargetPosition()
			{
				Vec3 vec;
				if (this._bestTargetList.Count > 0)
				{
					vec = this._bestTargetList[MBRandom.RandomInt(this._bestTargetList.Count)].GetGlobalFrame().origin;
				}
				else
				{
					vec = this._volumeBox.GameEntity.GetGlobalFrame().origin;
				}
				return vec;
			}

			// Token: 0x06000E0F RID: 3599 RVA: 0x00064BE9 File Offset: 0x00062DE9
			public void AddToCheckList(Agent agent)
			{
				this._volumeBox.AddToCheckList(agent);
			}

			// Token: 0x06000E10 RID: 3600 RVA: 0x00064BF7 File Offset: 0x00062DF7
			public void RemoveFromCheckList(Agent agent)
			{
				this._volumeBox.RemoveFromCheckList(agent);
			}

			// Token: 0x06000E11 RID: 3601 RVA: 0x00064C08 File Offset: 0x00062E08
			private void OnAgentsEnterCheckBox(VolumeBox volumeBox, List<Agent> agentsInVolume)
			{
				foreach (Agent agent in agentsInVolume)
				{
					agent.GetController<TownHorseRaceAgentController>().OnEnterCheckPoint(volumeBox);
				}
			}

			// Token: 0x0400066C RID: 1644
			private readonly VolumeBox _volumeBox;

			// Token: 0x0400066D RID: 1645
			private readonly List<GameEntity> _bestTargetList;
		}
	}
}
