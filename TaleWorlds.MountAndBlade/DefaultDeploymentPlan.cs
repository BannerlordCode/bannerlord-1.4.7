using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200020E RID: 526
	public class DefaultDeploymentPlan
	{
		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x00069050 File Offset: 0x00067250
		public bool SpawnWithHorses
		{
			get
			{
				return this._spawnWithHorses;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001E63 RID: 7779 RVA: 0x00069058 File Offset: 0x00067258
		public int PlanCount
		{
			get
			{
				return this._planCount;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001E64 RID: 7780 RVA: 0x00069060 File Offset: 0x00067260
		// (set) Token: 0x06001E65 RID: 7781 RVA: 0x00069068 File Offset: 0x00067268
		public bool IsPlanMade { get; private set; }

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001E66 RID: 7782 RVA: 0x00069071 File Offset: 0x00067271
		// (set) Token: 0x06001E67 RID: 7783 RVA: 0x00069079 File Offset: 0x00067279
		public float SpawnPathOffset { get; private set; }

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001E68 RID: 7784 RVA: 0x00069082 File Offset: 0x00067282
		// (set) Token: 0x06001E69 RID: 7785 RVA: 0x0006908A File Offset: 0x0006728A
		public float TargetOffset { get; private set; }

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001E6A RID: 7786 RVA: 0x00069093 File Offset: 0x00067293
		public bool IsSafeToDeploy
		{
			get
			{
				return this.SafetyScore >= 50f;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x000690A5 File Offset: 0x000672A5
		// (set) Token: 0x06001E6C RID: 7788 RVA: 0x000690AD File Offset: 0x000672AD
		public float SafetyScore { get; private set; }

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x000690B8 File Offset: 0x000672B8
		public int FootTroopCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < 11; i++)
				{
					num += this._formationFootTroopCounts[i];
				}
				return num;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001E6E RID: 7790 RVA: 0x000690E0 File Offset: 0x000672E0
		public int MountedTroopCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < 11; i++)
				{
					num += this._formationMountedTroopCounts[i];
				}
				return num;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x00069108 File Offset: 0x00067308
		public int TroopCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < 11; i++)
				{
					num += this._formationFootTroopCounts[i] + this._formationMountedTroopCounts[i];
				}
				return num;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001E70 RID: 7792 RVA: 0x00069139 File Offset: 0x00067339
		public Vec3 MeanPosition
		{
			get
			{
				return this._meanPosition;
			}
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x00069141 File Offset: 0x00067341
		public static DefaultDeploymentPlan CreateInitialPlan(Mission mission, Team team)
		{
			return new DefaultDeploymentPlan(mission, team, false, null);
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x0006914C File Offset: 0x0006734C
		public static DefaultDeploymentPlan CreateReinforcementPlan(Mission mission, Team team)
		{
			return new DefaultDeploymentPlan(mission, team, true, null);
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x00069157 File Offset: 0x00067357
		public static DefaultDeploymentPlan CreateReinforcementPlanWithSpawnPath(Mission mission, Team team, SpawnPathData spawnPathData)
		{
			return new DefaultDeploymentPlan(mission, team, true, spawnPathData);
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x00069164 File Offset: 0x00067364
		private DefaultDeploymentPlan(Mission mission, Team team, bool isReinforcement, SpawnPathData spawnPathData)
		{
			this._mission = mission;
			this._planCount = 0;
			this.Team = team;
			this.IsReinforcement = isReinforcement;
			this.SpawnPathData = spawnPathData;
			int num = 11;
			this._formationPlans = new DefaultFormationDeploymentPlan[num];
			this._formationFootTroopCounts = new int[num];
			this._formationMountedTroopCounts = new int[num];
			this._meanPosition = Vec3.Zero;
			this.IsPlanMade = false;
			this.SpawnPathOffset = 0f;
			this.TargetOffset = 0f;
			this.SafetyScore = 100f;
			for (int i = 0; i < this._formationPlans.Length; i++)
			{
				FormationClass formationClass = (FormationClass)i;
				this._formationPlans[i] = new DefaultFormationDeploymentPlan(formationClass);
			}
			for (int j = 0; j < 4; j++)
			{
				this._deploymentFlanks[j] = new SortedList<FormationDeploymentOrder, DefaultFormationDeploymentPlan>(FormationDeploymentOrder.GetComparer());
			}
			this.ClearAddedTroops();
			this.ClearPlan();
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x0006924E File Offset: 0x0006744E
		public void SetSpawnWithHorses(bool value)
		{
			this._spawnWithHorses = value;
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x00069258 File Offset: 0x00067458
		public void MakeDeploymentPlan(float spawnPathOffset = 0f, float targetOffset = 0f, FormationSceneSpawnEntry[,] formationSceneSpawnEntries = null)
		{
			this.SpawnPathOffset = spawnPathOffset;
			this.TargetOffset = targetOffset;
			this.PlanFormationDimensions();
			if (this._mission.HasSpawnPath)
			{
				this.PlanFieldBattleDeploymentFromSpawnPath(spawnPathOffset, targetOffset);
			}
			else if (this._mission.IsFieldBattle)
			{
				this.PlanFieldBattleDeploymentFromSceneData(formationSceneSpawnEntries);
			}
			else
			{
				this.PlanBattleDeploymentFromSceneData(formationSceneSpawnEntries);
			}
			this.ComputeMeanPosition();
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x000692B4 File Offset: 0x000674B4
		public void ClearPlan()
		{
			DefaultFormationDeploymentPlan[] formationPlans = this._formationPlans;
			for (int i = 0; i < formationPlans.Length; i++)
			{
				formationPlans[i].Clear();
			}
			SortedList<FormationDeploymentOrder, DefaultFormationDeploymentPlan>[] deploymentFlanks = this._deploymentFlanks;
			for (int i = 0; i < deploymentFlanks.Length; i++)
			{
				deploymentFlanks[i].Clear();
			}
			this.IsPlanMade = false;
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x00069304 File Offset: 0x00067504
		public void ClearAddedTroops()
		{
			for (int i = 0; i < 11; i++)
			{
				this._formationFootTroopCounts[i] = 0;
				this._formationMountedTroopCounts[i] = 0;
			}
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x00069330 File Offset: 0x00067530
		public void AddTroops(FormationClass formationClass, int footTroopCount, int mountedTroopCount)
		{
			if (footTroopCount + mountedTroopCount > 0 && formationClass < FormationClass.NumberOfAllFormationsWithUnset)
			{
				this._formationFootTroopCounts[(int)formationClass] += footTroopCount;
				this._formationMountedTroopCounts[(int)formationClass] += mountedTroopCount;
			}
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x0006936C File Offset: 0x0006756C
		public DefaultFormationDeploymentPlan GetFormationPlan(FormationClass fClass)
		{
			return this._formationPlans[(int)fClass];
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x00069378 File Offset: 0x00067578
		public bool GetFormationDeploymentFrame(FormationClass fClass, out MatrixFrame frame)
		{
			DefaultFormationDeploymentPlan formationPlan = this.GetFormationPlan(fClass);
			if (formationPlan.HasFrame())
			{
				frame = formationPlan.GetFrame();
				return true;
			}
			frame = MatrixFrame.Identity;
			return false;
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000693B0 File Offset: 0x000675B0
		public bool GetFirstValidFormationDeploymentFrame(out MatrixFrame frame)
		{
			foreach (DefaultFormationDeploymentPlan defaultFormationDeploymentPlan in this._formationPlans)
			{
				if (defaultFormationDeploymentPlan.HasFrame())
				{
					frame = defaultFormationDeploymentPlan.GetFrame();
					return true;
				}
			}
			frame = MatrixFrame.Identity;
			return false;
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000693F8 File Offset: 0x000675F8
		public bool IsPlanSuitableForFormations(ValueTuple<int, int>[] troopDataPerFormationClass)
		{
			if (troopDataPerFormationClass.Length == 11)
			{
				for (int i = 0; i < 11; i++)
				{
					FormationClass formationClass = (FormationClass)i;
					DefaultFormationDeploymentPlan formationPlan = this.GetFormationPlan(formationClass);
					ValueTuple<int, int> valueTuple = troopDataPerFormationClass[i];
					if (formationPlan.PlannedFootTroopCount != valueTuple.Item1 || formationPlan.PlannedMountedTroopCount != valueTuple.Item2)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x0006944C File Offset: 0x0006764C
		public void UpdateSafetyScore()
		{
			if (this._mission.Teams == null)
			{
				return;
			}
			float num = 100f;
			Team team = ((this.Team.Side == BattleSideEnum.Attacker) ? this._mission.Teams.Defender : ((this.Team.Side == BattleSideEnum.Defender) ? this._mission.Teams.Attacker : null));
			if (team != null)
			{
				foreach (Formation formation in team.FormationsIncludingEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						float num2 = this._meanPosition.AsVec2.Distance(formation.CachedAveragePosition);
						if (num >= num2)
						{
							num = num2;
						}
					}
				}
			}
			team = ((this.Team.Side == BattleSideEnum.Attacker) ? this._mission.Teams.DefenderAlly : ((this.Team.Side == BattleSideEnum.Defender) ? this._mission.Teams.AttackerAlly : null));
			if (team != null)
			{
				foreach (Formation formation2 in team.FormationsIncludingEmpty)
				{
					if (formation2.CountOfUnits > 0)
					{
						float num3 = this._meanPosition.AsVec2.Distance(formation2.CachedAveragePosition);
						if (num >= num3)
						{
							num = num3;
						}
					}
				}
			}
			this.SafetyScore = num;
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000695D4 File Offset: 0x000677D4
		public WorldFrame GetFrameFromFormationSpawnEntity(GameEntity formationSpawnEntity, float depthOffset = 0f)
		{
			MatrixFrame globalFrame = formationSpawnEntity.GetGlobalFrame();
			globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			WorldPosition worldPosition = new WorldPosition(this._mission.Scene, UIntPtr.Zero, globalFrame.origin, false);
			WorldPosition worldPosition2 = worldPosition;
			if (depthOffset != 0f)
			{
				worldPosition2.SetVec2(worldPosition2.AsVec2 - depthOffset * globalFrame.rotation.f.AsVec2);
				if (!worldPosition2.IsValid || worldPosition2.GetNavMesh() == UIntPtr.Zero)
				{
					worldPosition2 = worldPosition;
				}
			}
			return new WorldFrame(globalFrame.rotation, worldPosition2);
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x00069670 File Offset: 0x00067870
		private void PlanFieldBattleDeploymentFromSpawnPath(float pathOffset, float targetOffset)
		{
			bool flag = this.FootTroopCount > 0;
			for (int i = 0; i < this._formationPlans.Length; i++)
			{
				FormationClass formationClass = (FormationClass)i;
				int num = this._formationFootTroopCounts[i] + this._formationMountedTroopCounts[i];
				if (num > 0 || (!this.IsReinforcement && (formationClass == FormationClass.NumberOfRegularFormations || formationClass == FormationClass.Bodyguard)))
				{
					DefaultFormationDeploymentPlan defaultFormationDeploymentPlan = this._formationPlans[i];
					FormationDeploymentFlank defaultFlank = defaultFormationDeploymentPlan.GetDefaultFlank(num, flag, this._spawnWithHorses);
					int num2 = ((num > 0 || formationClass == FormationClass.NumberOfRegularFormations) ? 0 : 1);
					FormationDeploymentOrder flankDeploymentOrder = defaultFormationDeploymentPlan.GetFlankDeploymentOrder(num2);
					this._deploymentFlanks[(int)defaultFlank].Add(flankDeploymentOrder, defaultFormationDeploymentPlan);
				}
			}
			float num3 = this.ComputeHorizontalCenterOffset();
			SpawnPathData initialSpawnPathData = this._mission.GetInitialSpawnPathData(this.Team.Side);
			Vec2 asVec;
			Vec2 vec;
			if (this.IsReinforcement)
			{
				MatrixFrame matrixFrame = this.SpawnPathData.GetSpawnFrame(this.SpawnPathData.GetBaseOffset(), true, SpawnPathData.SearchDirection.Forward);
				asVec = matrixFrame.origin.AsVec2;
				matrixFrame = initialSpawnPathData.GetSpawnFrame(0f, false, SpawnPathData.SearchDirection.Backward);
				vec = (matrixFrame.origin.AsVec2 - asVec).Normalized();
			}
			else
			{
				initialSpawnPathData.GetSpawnPathFrameFacingTarget(pathOffset, targetOffset, false, out asVec, out vec, true, 0.2f);
			}
			this.DeployFlanks(asVec, vec, num3);
			SortedList<FormationDeploymentOrder, DefaultFormationDeploymentPlan>[] deploymentFlanks = this._deploymentFlanks;
			for (int j = 0; j < deploymentFlanks.Length; j++)
			{
				deploymentFlanks[j].Clear();
			}
			this.IsPlanMade = true;
			this._planCount++;
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x000697F4 File Offset: 0x000679F4
		private void PlanFieldBattleDeploymentFromSceneData(FormationSceneSpawnEntry[,] formationSceneSpawnEntries)
		{
			if (formationSceneSpawnEntries == null || formationSceneSpawnEntries.GetLength(0) != 2 || formationSceneSpawnEntries.GetLength(1) != this._formationPlans.Length)
			{
				return;
			}
			int side = (int)this.Team.Side;
			int num = ((this.Team.Side == BattleSideEnum.Attacker) ? 0 : 1);
			Dictionary<GameEntity, float> dictionary = new Dictionary<GameEntity, float>();
			bool flag = !this.IsReinforcement;
			for (int i = 0; i < this._formationPlans.Length; i++)
			{
				DefaultFormationDeploymentPlan defaultFormationDeploymentPlan = this._formationPlans[i];
				FormationSceneSpawnEntry formationSceneSpawnEntry = formationSceneSpawnEntries[side, i];
				FormationSceneSpawnEntry formationSceneSpawnEntry2 = formationSceneSpawnEntries[num, i];
				GameEntity gameEntity = (flag ? formationSceneSpawnEntry.SpawnEntity : formationSceneSpawnEntry.ReinforcementSpawnEntity);
				GameEntity gameEntity2 = (flag ? formationSceneSpawnEntry2.SpawnEntity : formationSceneSpawnEntry2.ReinforcementSpawnEntity);
				if (gameEntity != null && gameEntity2 != null)
				{
					WorldFrame worldFrame = this.ComputeFieldBattleDeploymentFrameForFormation(defaultFormationDeploymentPlan, gameEntity, gameEntity2, ref dictionary);
					defaultFormationDeploymentPlan.SetFrame(in worldFrame);
				}
				else
				{
					defaultFormationDeploymentPlan.SetFrame(in WorldFrame.Invalid);
				}
				defaultFormationDeploymentPlan.SetSpawnClass(formationSceneSpawnEntry.FormationClass);
			}
			this.IsPlanMade = true;
			this._planCount++;
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x00069918 File Offset: 0x00067B18
		private void PlanBattleDeploymentFromSceneData(FormationSceneSpawnEntry[,] formationSceneSpawnEntries)
		{
			if (formationSceneSpawnEntries == null || formationSceneSpawnEntries.GetLength(0) != 2 || formationSceneSpawnEntries.GetLength(1) != this._formationPlans.Length)
			{
				return;
			}
			int side = (int)this.Team.Side;
			Dictionary<GameEntity, float> dictionary = new Dictionary<GameEntity, float>();
			bool flag = !this.IsReinforcement;
			for (int i = 0; i < this._formationPlans.Length; i++)
			{
				DefaultFormationDeploymentPlan defaultFormationDeploymentPlan = this._formationPlans[i];
				FormationSceneSpawnEntry formationSceneSpawnEntry = formationSceneSpawnEntries[side, i];
				GameEntity gameEntity = (flag ? formationSceneSpawnEntry.SpawnEntity : formationSceneSpawnEntry.ReinforcementSpawnEntity);
				if (gameEntity != null)
				{
					float andUpdateSpawnDepth = this.GetAndUpdateSpawnDepth(ref dictionary, gameEntity, defaultFormationDeploymentPlan);
					DefaultFormationDeploymentPlan defaultFormationDeploymentPlan2 = defaultFormationDeploymentPlan;
					WorldFrame frameFromFormationSpawnEntity = this.GetFrameFromFormationSpawnEntity(gameEntity, andUpdateSpawnDepth);
					defaultFormationDeploymentPlan2.SetFrame(in frameFromFormationSpawnEntity);
				}
				else
				{
					defaultFormationDeploymentPlan.SetFrame(in WorldFrame.Invalid);
				}
				defaultFormationDeploymentPlan.SetSpawnClass(formationSceneSpawnEntry.FormationClass);
			}
			this.IsPlanMade = true;
			this._planCount++;
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x00069A00 File Offset: 0x00067C00
		private void PlanFormationDimensions()
		{
			for (int i = 0; i < this._formationPlans.Length; i++)
			{
				int num = this._formationFootTroopCounts[i];
				int num2 = this._formationMountedTroopCounts[i];
				int num3 = num + num2;
				DefaultFormationDeploymentPlan defaultFormationDeploymentPlan = this._formationPlans[i];
				if (num3 > 0)
				{
					bool flag = DefaultMissionDeploymentPlan.HasSignificantMountedTroops(num, num2);
					ValueTuple<float, float> formationSpawnWidthAndDepth = DefaultDeploymentPlan.GetFormationSpawnWidthAndDepth(defaultFormationDeploymentPlan.Class, num3, flag, !this._spawnWithHorses);
					float item = formationSpawnWidthAndDepth.Item1;
					float item2 = formationSpawnWidthAndDepth.Item2;
					defaultFormationDeploymentPlan.SetPlannedDimensions(item, item2);
					defaultFormationDeploymentPlan.SetPlannedTroopCount(num, num2);
				}
			}
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x00069A88 File Offset: 0x00067C88
		private void DeployFlanks(Vec2 deployPosition, Vec2 deployDirection, float horizontalCenterOffset)
		{
			ValueTuple<float, float> valueTuple = this.PlanFlankDeployment(FormationDeploymentFlank.Front, deployPosition, deployDirection, 0f, horizontalCenterOffset);
			float item = valueTuple.Item1;
			float num = valueTuple.Item2;
			num += 3f;
			float item2 = this.PlanFlankDeployment(FormationDeploymentFlank.Rear, deployPosition, deployDirection, num, horizontalCenterOffset).Item1;
			float num2 = MathF.Max(item, item2);
			float num3 = this.ComputeFlankDepth(FormationDeploymentFlank.Front, true);
			num3 += 3f;
			float num4 = this.ComputeFlankWidth(FormationDeploymentFlank.Left);
			float num5 = horizontalCenterOffset + 2f + 0.5f * (num2 + num4);
			this.PlanFlankDeployment(FormationDeploymentFlank.Left, deployPosition, deployDirection, num3, num5);
			float num6 = this.ComputeFlankWidth(FormationDeploymentFlank.Right);
			float num7 = horizontalCenterOffset - (2f + 0.5f * (num2 + num6));
			this.PlanFlankDeployment(FormationDeploymentFlank.Right, deployPosition, deployDirection, num3, num7);
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x00069B44 File Offset: 0x00067D44
		private void ComputeMeanPosition()
		{
			this._meanPosition = Vec3.Zero;
			Vec2 vec = Vec2.Zero;
			int num = 0;
			foreach (DefaultFormationDeploymentPlan defaultFormationDeploymentPlan in this._formationPlans)
			{
				if (defaultFormationDeploymentPlan.HasFrame())
				{
					vec += defaultFormationDeploymentPlan.GetPosition().AsVec2;
					num++;
				}
			}
			if (num > 0)
			{
				vec = new Vec2(vec.X / (float)num, vec.Y / (float)num);
				float num2 = 0f;
				Mission.Current.Scene.GetHeightAtPoint(vec, BodyFlags.None, ref num2);
				this._meanPosition = new Vec3(vec, num2, -1f);
			}
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x00069BF0 File Offset: 0x00067DF0
		[return: TupleElementNames(new string[] { "flankWidth", "flankDepth" })]
		private ValueTuple<float, float> PlanFlankDeployment(FormationDeploymentFlank flankFlank, Vec2 deployPosition, Vec2 deployDirection, float verticalOffset = 0f, float horizontalOffset = 0f)
		{
			Mat3 identity = Mat3.Identity;
			identity.RotateAboutUp(deployDirection.RotationInRadians);
			float num = 0f;
			float num2 = 0f;
			Vec2 vec = deployDirection.LeftVec();
			WorldPosition worldPosition = new WorldPosition(this._mission.Scene, UIntPtr.Zero, deployPosition.ToVec3(0f), false);
			foreach (KeyValuePair<FormationDeploymentOrder, DefaultFormationDeploymentPlan> keyValuePair in this._deploymentFlanks[(int)flankFlank])
			{
				DefaultFormationDeploymentPlan value = keyValuePair.Value;
				Vec2 vec2 = worldPosition.AsVec2 - (num2 + verticalOffset) * deployDirection + horizontalOffset * vec;
				Vec3 lastPointOnNavigationMeshFromWorldPositionToDestination = this._mission.Scene.GetLastPointOnNavigationMeshFromWorldPositionToDestination(ref worldPosition, vec2);
				WorldPosition worldPosition2 = new WorldPosition(this._mission.Scene, UIntPtr.Zero, lastPointOnNavigationMeshFromWorldPositionToDestination, false);
				WorldFrame worldFrame = new WorldFrame(identity, worldPosition2);
				value.SetFrame(in worldFrame);
				float num3 = value.PlannedDepth + 3f;
				num2 += num3;
				num = MathF.Max(num, value.PlannedWidth);
			}
			num2 = MathF.Max(num2 - 3f, 0f);
			return new ValueTuple<float, float>(num, num2);
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x00069D3C File Offset: 0x00067F3C
		private WorldFrame ComputeFieldBattleDeploymentFrameForFormation(DefaultFormationDeploymentPlan formationPlan, GameEntity formationSceneEntity, GameEntity counterSideFormationSceneEntity, ref Dictionary<GameEntity, float> spawnDepths)
		{
			Vec3 globalPosition = formationSceneEntity.GlobalPosition;
			Vec2 asVec = (counterSideFormationSceneEntity.GlobalPosition - globalPosition).AsVec2;
			asVec.Normalize();
			float andUpdateSpawnDepth = this.GetAndUpdateSpawnDepth(ref spawnDepths, formationSceneEntity, formationPlan);
			WorldPosition worldPosition = new WorldPosition(this._mission.Scene, UIntPtr.Zero, globalPosition, false);
			worldPosition.SetVec2(worldPosition.AsVec2 - andUpdateSpawnDepth * asVec);
			Mat3 identity = Mat3.Identity;
			identity.RotateAboutUp(asVec.RotationInRadians);
			return new WorldFrame(identity, worldPosition);
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x00069DCC File Offset: 0x00067FCC
		private float ComputeFlankWidth(FormationDeploymentFlank flank)
		{
			float num = 0f;
			foreach (KeyValuePair<FormationDeploymentOrder, DefaultFormationDeploymentPlan> keyValuePair in this._deploymentFlanks[(int)flank])
			{
				num = MathF.Max(num, keyValuePair.Value.PlannedWidth);
			}
			return num;
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x00069E30 File Offset: 0x00068030
		private float ComputeFlankDepth(FormationDeploymentFlank flank, bool countPositiveNumTroops = false)
		{
			float num = 0f;
			foreach (KeyValuePair<FormationDeploymentOrder, DefaultFormationDeploymentPlan> keyValuePair in this._deploymentFlanks[(int)flank])
			{
				if (!countPositiveNumTroops)
				{
					num += keyValuePair.Value.PlannedDepth + 3f;
				}
				else if (keyValuePair.Value.PlannedTroopCount > 0)
				{
					num += keyValuePair.Value.PlannedDepth + 3f;
				}
			}
			num -= 3f;
			return num;
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x00069EC8 File Offset: 0x000680C8
		private float ComputeHorizontalCenterOffset()
		{
			float num = MathF.Max(this.ComputeFlankWidth(FormationDeploymentFlank.Front), this.ComputeFlankWidth(FormationDeploymentFlank.Rear));
			float num2 = this.ComputeFlankWidth(FormationDeploymentFlank.Left);
			float num3 = this.ComputeFlankWidth(FormationDeploymentFlank.Right);
			float num4 = num / 2f + num2 + 2f;
			return (num / 2f + num3 + 2f - num4) / 2f;
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x00069F20 File Offset: 0x00068120
		private float GetAndUpdateSpawnDepth(ref Dictionary<GameEntity, float> spawnDepths, GameEntity spawnEntity, DefaultFormationDeploymentPlan formationPlan)
		{
			float num;
			bool flag = spawnDepths.TryGetValue(spawnEntity, out num);
			float num2 = (formationPlan.HasDimensions ? (formationPlan.PlannedDepth + 3f) : 0f);
			if (!flag)
			{
				num = 0f;
				spawnDepths[spawnEntity] = num2;
			}
			else if (formationPlan.HasDimensions)
			{
				spawnDepths[spawnEntity] = num + num2;
			}
			return num;
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x00069F7C File Offset: 0x0006817C
		public static ValueTuple<float, float> GetFormationSpawnWidthAndDepth(FormationClass formationNo, int troopCount, bool hasMountedTroops, bool considerCavalryAsInfantry = false)
		{
			bool flag = !considerCavalryAsInfantry && hasMountedTroops;
			float defaultUnitDiameter = Formation.GetDefaultUnitDiameter(flag);
			int unitSpacingOf = ArrangementOrder.GetUnitSpacingOf(ArrangementOrder.ArrangementOrderEnum.Line);
			float num = (flag ? Formation.CavalryInterval(unitSpacingOf) : Formation.InfantryInterval(unitSpacingOf));
			float num2 = (flag ? Formation.CavalryDistance(unitSpacingOf) : Formation.InfantryDistance(unitSpacingOf));
			float num3 = (float)MathF.Max(0, troopCount - 1) * (num + defaultUnitDiameter) + defaultUnitDiameter;
			float num4 = (flag ? 18f : 9f);
			int num5 = (int)(num3 / MathF.Sqrt(num4 * (float)troopCount + 1f));
			num5 = MathF.Max(1, num5);
			float num6 = (float)troopCount / (float)num5;
			float num7 = MathF.Max(0f, num6 - 1f) * (num + defaultUnitDiameter) + defaultUnitDiameter;
			float num8 = (float)(num5 - 1) * (num2 + defaultUnitDiameter) + defaultUnitDiameter;
			return new ValueTuple<float, float>(num7, num8);
		}

		// Token: 0x04000A69 RID: 2665
		public const float VerticalFormationGap = 3f;

		// Token: 0x04000A6A RID: 2666
		public const float HorizontalFormationGap = 2f;

		// Token: 0x04000A6B RID: 2667
		public const float MaxSafetyScore = 100f;

		// Token: 0x04000A6C RID: 2668
		public readonly Team Team;

		// Token: 0x04000A6D RID: 2669
		public readonly bool IsReinforcement;

		// Token: 0x04000A6E RID: 2670
		public readonly SpawnPathData SpawnPathData;

		// Token: 0x04000A73 RID: 2675
		private readonly Mission _mission;

		// Token: 0x04000A74 RID: 2676
		private int _planCount;

		// Token: 0x04000A75 RID: 2677
		private bool _spawnWithHorses;

		// Token: 0x04000A76 RID: 2678
		private readonly int[] _formationMountedTroopCounts;

		// Token: 0x04000A77 RID: 2679
		private readonly int[] _formationFootTroopCounts;

		// Token: 0x04000A78 RID: 2680
		private Vec3 _meanPosition;

		// Token: 0x04000A79 RID: 2681
		private readonly DefaultFormationDeploymentPlan[] _formationPlans;

		// Token: 0x04000A7A RID: 2682
		private readonly SortedList<FormationDeploymentOrder, DefaultFormationDeploymentPlan>[] _deploymentFlanks = new SortedList<FormationDeploymentOrder, DefaultFormationDeploymentPlan>[4];
	}
}
