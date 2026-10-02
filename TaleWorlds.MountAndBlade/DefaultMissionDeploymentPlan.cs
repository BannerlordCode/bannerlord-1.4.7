using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000210 RID: 528
	public class DefaultMissionDeploymentPlan : IMissionDeploymentPlan
	{
		// Token: 0x06001EA4 RID: 7844 RVA: 0x0006A2AC File Offset: 0x000684AC
		public DefaultMissionDeploymentPlan(Mission mission)
		{
			this._mission = mission;
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x0006A2D4 File Offset: 0x000684D4
		public void Initialize()
		{
			foreach (Team team in this._mission.Teams)
			{
				if (team.Side != BattleSideEnum.None)
				{
					DefaultTeamDeploymentPlan defaultTeamDeploymentPlan = new DefaultTeamDeploymentPlan(this._mission, team);
					this._teamDeploymentPlans.Add(new ValueTuple<Team, DefaultTeamDeploymentPlan>(team, defaultTeamDeploymentPlan));
				}
			}
			for (int i = 0; i < 2; i++)
			{
				this._playerSpawnFrames[i] = null;
			}
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x0006A36C File Offset: 0x0006856C
		public void ClearDeploymentPlan(Team team)
		{
			this.GetTeamPlan(team).ClearPlan(false);
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x0006A37B File Offset: 0x0006857B
		public void ClearReinforcementPlan(Team team)
		{
			this.GetTeamPlan(team).ClearPlan(true);
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x0006A38A File Offset: 0x0006858A
		public bool HasPlayerSpawnFrame(BattleSideEnum battleSide)
		{
			return this._playerSpawnFrames[(int)battleSide] != null;
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x0006A3A0 File Offset: 0x000685A0
		public bool GetPlayerSpawnFrame(BattleSideEnum battleSide, out WorldPosition position, out Vec2 direction)
		{
			WorldFrame? worldFrame = this._playerSpawnFrames[(int)battleSide];
			if (worldFrame != null)
			{
				Scene scene = Mission.Current.Scene;
				UIntPtr zero = UIntPtr.Zero;
				WorldFrame worldFrame2 = worldFrame.Value;
				position = new WorldPosition(scene, zero, worldFrame2.Origin.GetGroundVec3(), false);
				worldFrame2 = worldFrame.Value;
				direction = worldFrame2.Rotation.f.AsVec2.Normalized();
				return true;
			}
			position = WorldPosition.Invalid;
			direction = Vec2.Invalid;
			return false;
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x0006A432 File Offset: 0x00068632
		public static bool HasSignificantMountedTroops(int footTroopCount, int mountedTroopCount)
		{
			return (float)mountedTroopCount / Math.Max((float)(mountedTroopCount + footTroopCount), 1f) >= 0.1f;
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x0006A44F File Offset: 0x0006864F
		public void ClearAddedTroops(Team team, bool isReinforcement = false)
		{
			this.GetTeamPlan(team).ClearAddedTroops(isReinforcement);
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x0006A460 File Offset: 0x00068660
		public void ClearAll()
		{
			foreach (ValueTuple<Team, DefaultTeamDeploymentPlan> valueTuple in this._teamDeploymentPlans)
			{
				DefaultTeamDeploymentPlan item = valueTuple.Item2;
				item.ClearAddedTroops(false);
				item.ClearPlan(false);
				item.ClearAddedTroops(true);
				item.ClearPlan(true);
			}
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x0006A4CC File Offset: 0x000686CC
		public void AddTroops(Team team, FormationClass formationClass, int footTroopCount, int mountedTroopCount = 0, bool isReinforcement = false)
		{
			BattleSideEnum side = team.Side;
			this.GetTeamPlan(team).AddTroops(formationClass, footTroopCount, mountedTroopCount, isReinforcement);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x0006A4E7 File Offset: 0x000686E7
		public void SetSpawnWithHorses(Team team, bool spawnWithHorses)
		{
			this.GetTeamPlan(team).SetSpawnWithHorses(spawnWithHorses);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x0006A4F8 File Offset: 0x000686F8
		public void MakeDefaultDeploymentPlans()
		{
			foreach (ValueTuple<Team, DefaultTeamDeploymentPlan> valueTuple in this._teamDeploymentPlans)
			{
				Team item = valueTuple.Item1;
				this.MakeDeploymentPlan(item, 0f, 0f);
				this.MakeReinforcementDeploymentPlan(item);
			}
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x0006A564 File Offset: 0x00068764
		public void MakeDeploymentPlan(Team team, float spawnPathOffset = 0f, float targetOffset = 0f)
		{
			if (!this.IsPlanMade(team))
			{
				this.MakeDeploymentPlanAux(team, false, spawnPathOffset, targetOffset);
				bool flag;
				if (this.IsPlanMade(team, out flag))
				{
					this._mission.OnDeploymentPlanMade(team, flag);
				}
			}
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x0006A59C File Offset: 0x0006879C
		public void MakeReinforcementDeploymentPlan(Team team)
		{
			if (!this.IsReinforcementPlanMade(team))
			{
				this.MakeDeploymentPlanAux(team, true, 0f, 0f);
			}
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x0006A5BC File Offset: 0x000687BC
		public bool RemakeDeploymentPlan(Team team)
		{
			this.IsPlanMade(team);
			float spawnPathOffset = this.GetSpawnPathOffset(team);
			float targetOffset = this.GetTargetOffset(team);
			ValueTuple<int, int>[] array = new ValueTuple<int, int>[11];
			foreach (Agent agent2 in this._mission.AllAgents.Where<Agent>((Agent agent) => agent.IsHuman && agent.Team != null && agent.Team == team && agent.Formation != null))
			{
				int formationIndex = (int)agent2.Formation.FormationIndex;
				ValueTuple<int, int> valueTuple = array[formationIndex];
				array[formationIndex] = (agent2.HasMount ? new ValueTuple<int, int>(valueTuple.Item1, valueTuple.Item2 + 1) : new ValueTuple<int, int>(valueTuple.Item1 + 1, valueTuple.Item2));
			}
			if (!this.IsInitialPlanSuitableForFormations(team, array))
			{
				this.ClearAddedTroops(team, false);
				this.ClearDeploymentPlan(team);
				for (int i = 0; i < 11; i++)
				{
					ValueTuple<int, int> valueTuple2 = array[i];
					int item = valueTuple2.Item1;
					int item2 = valueTuple2.Item2;
					if (item + item2 > 0)
					{
						this.AddTroops(team, (FormationClass)i, item, item2, false);
					}
				}
				this.MakeDeploymentPlan(team, spawnPathOffset, targetOffset);
				return this.IsPlanMade(team);
			}
			return false;
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x0006A738 File Offset: 0x00068938
		public bool IsPositionInsideDeploymentBoundaries(Team team, in Vec2 position)
		{
			ValueTuple<string, MBList<Vec2>> valueTuple;
			return this.GetTeamPlan(team).IsPositionInsideDeploymentBoundaries(in position, out valueTuple);
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x0006A754 File Offset: 0x00068954
		public Vec2 GetClosestDeploymentBoundaryPosition(Team team, in Vec2 position)
		{
			return this.GetTeamPlan(team).GetClosestDeploymentBoundaryPosition(in position);
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x0006A763 File Offset: 0x00068963
		public bool SupportsReinforcements()
		{
			return true;
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x0006A766 File Offset: 0x00068966
		public bool SupportsNavmesh(Team team)
		{
			return true;
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x0006A769 File Offset: 0x00068969
		public bool GetPathDeploymentBoundaryIntersection(Team team, in WorldPosition startPosition, in WorldPosition endPosition, out WorldPosition intersection)
		{
			return this.GetTeamPlan(team).GetPathDeploymentBoundaryIntersection(in startPosition, in endPosition, out intersection);
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x0006A77C File Offset: 0x0006897C
		public bool IsPositionInsideSiegeDeploymentBoundaries(in Vec2 position)
		{
			bool flag = false;
			foreach (ICollection<Vec2> collection in this._mission.Boundaries.Values)
			{
				if (MBSceneUtilities.IsPointInsideBoundaries(in position, collection.ToMBList<Vec2>(), 0.05f))
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x0006A7E8 File Offset: 0x000689E8
		public float GetSpawnPathOffset(Team team)
		{
			return this.GetTeamPlan(team).GetSpawnPathOffset(false);
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x0006A7F7 File Offset: 0x000689F7
		public float GetTargetOffset(Team team)
		{
			return this.GetTeamPlan(team).GetTargetOffset(false);
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x0006A806 File Offset: 0x00068A06
		public int GetTroopCount(Team team, bool isReinforcement = false)
		{
			return this.GetTeamPlan(team).GetTroopCount(isReinforcement);
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x0006A815 File Offset: 0x00068A15
		public IFormationDeploymentPlan GetFormationPlan(Team team, FormationClass fClass, bool isReinforcement)
		{
			return this.GetTeamPlan(team).GetFormationPlan(fClass, isReinforcement);
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x0006A828 File Offset: 0x00068A28
		public bool IsPlanMade(Team team)
		{
			DefaultTeamDeploymentPlan teamPlanAux = this.GetTeamPlanAux(team);
			return teamPlanAux != null && teamPlanAux.IsPlanMade(false);
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x0006A84C File Offset: 0x00068A4C
		public bool IsPlanMade(Team team, out bool isFirstPlan)
		{
			DefaultTeamDeploymentPlan teamPlanAux = this.GetTeamPlanAux(team);
			isFirstPlan = false;
			if (teamPlanAux != null && teamPlanAux.IsPlanMade(false))
			{
				isFirstPlan = teamPlanAux.IsFirstPlan(false);
				return true;
			}
			return false;
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x0006A87C File Offset: 0x00068A7C
		public bool IsReinforcementPlanMade(Team team)
		{
			DefaultTeamDeploymentPlan teamPlanAux = this.GetTeamPlanAux(team);
			return teamPlanAux != null && teamPlanAux.IsPlanMade(true);
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0006A89D File Offset: 0x00068A9D
		public bool IsInitialPlanSuitableForFormations(Team team, [TupleElementNames(new string[] { "footTroopCount", "mountedTroopCount" })] ValueTuple<int, int>[] troopDataPerFormationClass)
		{
			return this.GetTeamPlan(team).IsInitialPlanSuitableForFormations(troopDataPerFormationClass);
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0006A8AC File Offset: 0x00068AAC
		public bool HasDeploymentBoundaries(Team team)
		{
			DefaultTeamDeploymentPlan teamPlanAux = this.GetTeamPlanAux(team);
			return teamPlanAux != null && teamPlanAux.HasDeploymentBoundaries();
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0006A8CC File Offset: 0x00068ACC
		public MatrixFrame GetDeploymentFrame(Team team)
		{
			return this.GetTeamPlan(team).GetDeploymentFrame();
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0006A8DC File Offset: 0x00068ADC
		public void ProjectPositionToDeploymentBoundaries(Team team, ref WorldPosition endPosition)
		{
			if (this.HasDeploymentBoundaries(team))
			{
				Vec2 asVec = endPosition.AsVec2;
				if (!this.IsPositionInsideDeploymentBoundaries(team, in asVec))
				{
					MatrixFrame deploymentFrame = this.GetDeploymentFrame(team);
					WorldPosition worldPosition = new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, deploymentFrame.origin, false);
					WorldPosition worldPosition2;
					if (this.GetPathDeploymentBoundaryIntersection(team, in worldPosition, in endPosition, out worldPosition2))
					{
						endPosition = worldPosition2;
					}
				}
			}
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0006A93E File Offset: 0x00068B3E
		[return: TupleElementNames(new string[] { "id", "points" })]
		public MBReadOnlyList<ValueTuple<string, MBList<Vec2>>> GetDeploymentBoundaries(Team team)
		{
			return this.GetTeamPlan(team).GetDeploymentBoundaries();
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0006A94C File Offset: 0x00068B4C
		public Vec3 GetMeanPosition(Team team, bool isReinforcement = false)
		{
			return this.GetTeamPlan(team).GetMeanPosition(isReinforcement);
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0006A95B File Offset: 0x00068B5B
		public void UpdateReinforcementPlan(Team team)
		{
			this.GetTeamPlan(team).UpdateReinforcementPlans();
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0006A969 File Offset: 0x00068B69
		public MatrixFrame GetZoomFocusFrame(Team team)
		{
			return this.GetDeploymentFrame(team);
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x0006A972 File Offset: 0x00068B72
		public float GetZoomOffset(Team team, float fovAngle)
		{
			return 0.2f * (float)this.GetTroopCount(team, false);
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x0006A984 File Offset: 0x00068B84
		private void MakeDeploymentPlanAux(Team team, bool isReinforcement = false, float spawnOffset = 0f, float targetOffset = 0f)
		{
			DefaultTeamDeploymentPlan teamPlan = this.GetTeamPlan(team);
			if (teamPlan.IsPlanMade(isReinforcement))
			{
				teamPlan.ClearPlan(isReinforcement);
			}
			if (!this._mission.HasSpawnPath && this._formationSceneSpawnEntries == null)
			{
				this.ReadSpawnEntitiesFromScene(this._mission.IsFieldBattle);
			}
			teamPlan.MakeDeploymentPlan(spawnOffset, targetOffset, this._formationSceneSpawnEntries, isReinforcement);
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x0006A9E0 File Offset: 0x00068BE0
		private void ReadSpawnEntitiesFromScene(bool isFieldBattle)
		{
			for (int i = 0; i < 2; i++)
			{
				this._playerSpawnFrames[i] = null;
			}
			this._formationSceneSpawnEntries = new FormationSceneSpawnEntry[2, 11];
			Scene scene = this._mission.Scene;
			if (isFieldBattle)
			{
				for (int j = 0; j < 2; j++)
				{
					string text = ((j == 1) ? "attacker_" : "defender_");
					for (int k = 0; k < 11; k++)
					{
						FormationClass formationClass = (FormationClass)k;
						WeakGameEntity weakGameEntity = scene.FindWeakEntityWithTag(text + formationClass.GetName().ToLower());
						if (weakGameEntity == null)
						{
							FormationClass formationClass2 = formationClass.FallbackClass();
							int num = (int)formationClass2;
							GameEntity spawnEntity = this._formationSceneSpawnEntries[j, num].SpawnEntity;
							weakGameEntity = ((spawnEntity != null) ? spawnEntity.WeakEntity : scene.FindWeakEntityWithTag(text + formationClass2.GetName().ToLower()));
							formationClass = ((weakGameEntity != null) ? formationClass2 : FormationClass.NumberOfAllFormations);
						}
						GameEntity gameEntity = null;
						if (weakGameEntity.IsValid)
						{
							gameEntity = GameEntity.CreateFromWeakEntity(weakGameEntity);
						}
						this._formationSceneSpawnEntries[j, k] = new FormationSceneSpawnEntry(formationClass, gameEntity, gameEntity);
					}
				}
				return;
			}
			GameEntity gameEntity2 = null;
			if (this._mission.IsSallyOutBattle)
			{
				gameEntity2 = scene.FindEntityWithTag("sally_out_ambush_battle_set");
			}
			if (gameEntity2 != null)
			{
				this.ReadSallyOutEntitiesFromScene(gameEntity2);
			}
			else
			{
				this.ReadSiegeBattleEntitiesFromScene(scene, BattleSideEnum.Defender);
			}
			this.ReadSiegeBattleEntitiesFromScene(scene, BattleSideEnum.Attacker);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x0006AB60 File Offset: 0x00068D60
		private void ReadSallyOutEntitiesFromScene(GameEntity sallyOutSetEntity)
		{
			int num = 0;
			MatrixFrame globalFrame = sallyOutSetEntity.GetFirstChildEntityWithTag("sally_out_ambush_player").GetGlobalFrame();
			WorldPosition worldPosition = new WorldPosition(this._mission.Scene, UIntPtr.Zero, globalFrame.origin, false);
			this._playerSpawnFrames[num] = new WorldFrame?(new WorldFrame(globalFrame.rotation, worldPosition));
			GameEntity firstChildEntityWithTag = sallyOutSetEntity.GetFirstChildEntityWithTag("sally_out_ambush_infantry");
			GameEntity firstChildEntityWithTag2 = sallyOutSetEntity.GetFirstChildEntityWithTag("sally_out_ambush_archer");
			GameEntity firstChildEntityWithTag3 = sallyOutSetEntity.GetFirstChildEntityWithTag("sally_out_ambush_cavalry");
			for (int i = 0; i < 11; i++)
			{
				FormationClass formationClass = (FormationClass)i;
				FormationClass formationClass2 = formationClass.FallbackClass();
				GameEntity gameEntity = null;
				switch (formationClass2)
				{
				case FormationClass.Infantry:
					gameEntity = firstChildEntityWithTag;
					break;
				case FormationClass.Ranged:
					gameEntity = firstChildEntityWithTag2;
					break;
				case FormationClass.Cavalry:
				case FormationClass.HorseArcher:
					gameEntity = firstChildEntityWithTag3;
					break;
				}
				this._formationSceneSpawnEntries[num, i] = new FormationSceneSpawnEntry(formationClass, gameEntity, gameEntity);
			}
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x0006AC44 File Offset: 0x00068E44
		private void ReadSiegeBattleEntitiesFromScene(Scene missionScene, BattleSideEnum battleSide)
		{
			int num = (int)battleSide;
			string text = battleSide.ToString().ToLower() + "_";
			for (int i = 0; i < 11; i++)
			{
				FormationClass formationClass = (FormationClass)i;
				string text2 = text + formationClass.GetName().ToLower();
				string text3 = text2 + "_reinforcement";
				GameEntity gameEntity = missionScene.FindEntityWithTag(text2);
				GameEntity gameEntity2;
				if (gameEntity == null)
				{
					FormationClass formationClass2 = formationClass.FallbackClass();
					int num2 = (int)formationClass2;
					FormationSceneSpawnEntry formationSceneSpawnEntry = this._formationSceneSpawnEntries[num, num2];
					if (formationSceneSpawnEntry.SpawnEntity != null)
					{
						gameEntity = formationSceneSpawnEntry.SpawnEntity;
						gameEntity2 = formationSceneSpawnEntry.ReinforcementSpawnEntity;
					}
					else
					{
						text2 = text + formationClass2.GetName().ToLower();
						text3 = text2 + "_reinforcement";
						gameEntity = missionScene.FindEntityWithTag(text2);
						gameEntity2 = missionScene.FindEntityWithTag(text3);
					}
					formationClass = ((gameEntity != null) ? formationClass2 : FormationClass.NumberOfAllFormations);
				}
				else
				{
					gameEntity2 = missionScene.FindEntityWithTag(text3);
				}
				if (gameEntity2 == null)
				{
					gameEntity2 = gameEntity;
				}
				this._formationSceneSpawnEntries[num, i] = new FormationSceneSpawnEntry(formationClass, gameEntity, gameEntity2);
			}
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x0006AD77 File Offset: 0x00068F77
		private DefaultTeamDeploymentPlan GetTeamPlan(Team team)
		{
			return this.GetTeamPlanAux(team);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x0006AD80 File Offset: 0x00068F80
		private DefaultTeamDeploymentPlan GetTeamPlanAux(Team team)
		{
			return this._teamDeploymentPlans.FirstOrDefault<ValueTuple<Team, DefaultTeamDeploymentPlan>>(([TupleElementNames(new string[] { "team", "plan" })] ValueTuple<Team, DefaultTeamDeploymentPlan> t) => t.Item1 == team).Item2;
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x0006ADB6 File Offset: 0x00068FB6
		bool IMissionDeploymentPlan.IsPositionInsideDeploymentBoundaries(Team team, in Vec2 position)
		{
			return this.IsPositionInsideDeploymentBoundaries(team, in position);
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x0006ADC0 File Offset: 0x00068FC0
		Vec2 IMissionDeploymentPlan.GetClosestDeploymentBoundaryPosition(Team team, in Vec2 position)
		{
			return this.GetClosestDeploymentBoundaryPosition(team, in position);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x0006ADCA File Offset: 0x00068FCA
		bool IMissionDeploymentPlan.GetPathDeploymentBoundaryIntersection(Team team, in WorldPosition startPosition, in WorldPosition endPosition, out WorldPosition intersection)
		{
			return this.GetPathDeploymentBoundaryIntersection(team, in startPosition, in endPosition, out intersection);
		}

		// Token: 0x04000A82 RID: 2690
		private readonly Mission _mission;

		// Token: 0x04000A83 RID: 2691
		[TupleElementNames(new string[] { "team", "plan" })]
		private readonly MBList<ValueTuple<Team, DefaultTeamDeploymentPlan>> _teamDeploymentPlans = new MBList<ValueTuple<Team, DefaultTeamDeploymentPlan>>();

		// Token: 0x04000A84 RID: 2692
		private readonly WorldFrame?[] _playerSpawnFrames = new WorldFrame?[2];

		// Token: 0x04000A85 RID: 2693
		private FormationSceneSpawnEntry[,] _formationSceneSpawnEntries;
	}
}
