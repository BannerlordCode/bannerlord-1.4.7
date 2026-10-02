using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200028A RID: 650
	public class MissionBattleSideSpawnContext
	{
		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x00082699 File Offset: 0x00080899
		// (set) Token: 0x06002411 RID: 9233 RVA: 0x000826A1 File Offset: 0x000808A1
		public bool TroopSpawnActive { get; private set; }

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06002412 RID: 9234 RVA: 0x000826AA File Offset: 0x000808AA
		public bool IsPlayerSide { get; }

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06002413 RID: 9235 RVA: 0x000826B2 File Offset: 0x000808B2
		// (set) Token: 0x06002414 RID: 9236 RVA: 0x000826BA File Offset: 0x000808BA
		public bool ReinforcementSpawnActive { get; private set; }

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002415 RID: 9237 RVA: 0x000826C3 File Offset: 0x000808C3
		public bool SpawnWithHorses
		{
			get
			{
				return this._spawnWithHorses;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002416 RID: 9238 RVA: 0x000826CB File Offset: 0x000808CB
		// (set) Token: 0x06002417 RID: 9239 RVA: 0x000826D3 File Offset: 0x000808D3
		public bool ReinforcementsNotifiedOnLastBatch { get; private set; }

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x000826DC File Offset: 0x000808DC
		public int NumberOfActiveTroops
		{
			get
			{
				return this._numSpawnedTroops - this._troopSupplier.NumRemovedTroops;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x000826F0 File Offset: 0x000808F0
		public int ReinforcementQuotaRequirement
		{
			get
			{
				return this._reinforcementQuotaRequirement;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x000826F8 File Offset: 0x000808F8
		public int ReinforcementsSpawnedInLastBatch
		{
			get
			{
				return this._reinforcementsSpawnedInLastBatch;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x00082700 File Offset: 0x00080900
		public float ReinforcementBatchSize
		{
			get
			{
				return (float)this._reinforcementBatchSize;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x00082709 File Offset: 0x00080909
		public bool HasReservedTroops
		{
			get
			{
				return this._reservedTroops.Count > 0;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x0600241D RID: 9245 RVA: 0x00082719 File Offset: 0x00080919
		public bool HasSpawnableReinforcements
		{
			get
			{
				return this.ReinforcementSpawnActive && this.HasReservedTroops && this.ReinforcementBatchSize > 0f;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x0008273A File Offset: 0x0008093A
		// (set) Token: 0x0600241F RID: 9247 RVA: 0x00082742 File Offset: 0x00080942
		public bool ForceSpawnPlayerMounted { get; private set; }

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x0008274B File Offset: 0x0008094B
		public float ReinforcementBatchPriority
		{
			get
			{
				return this._reinforcementBatchPriority;
			}
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x00082753 File Offset: 0x00080953
		public int GetNumberOfPlayerControllableTroops()
		{
			return this._troopSupplier.GetNumberOfPlayerControllableTroops();
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x00082760 File Offset: 0x00080960
		public int ReservedTroopsCount
		{
			get
			{
				return this._reservedTroops.Count;
			}
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x00082770 File Offset: 0x00080970
		public MissionBattleSideSpawnContext(IBattleMissionAgentSpawnLogic spawnLogic, BattleSideEnum side, IMissionTroopSupplier troopSupplier, bool isPlayerSide, bool forceSpawnPlayerMounted = true)
		{
			this._spawnLogic = spawnLogic;
			this._side = side;
			this._spawnWithHorses = true;
			this._spawnedFormations = new MBArrayList<Formation>();
			this._troopSupplier = troopSupplier;
			this._reinforcementQuotaRequirement = 0;
			this._reinforcementBatchSize = 0;
			this._reinforcementSpawnedUnitCountPerFormation = new ValueTuple<int, int>[8];
			this._reinforcementTroopFormationAssignments = new Dictionary<IAgentOriginBase, int>();
			this.IsPlayerSide = isPlayerSide;
			this.ReinforcementsNotifiedOnLastBatch = false;
			this.ForceSpawnPlayerMounted = forceSpawnPlayerMounted;
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x000827F4 File Offset: 0x000809F4
		public int TryReinforcementSpawn()
		{
			int num = 0;
			if (this.ReinforcementSpawnActive && this.TroopSpawnActive && this._reservedTroops.Count > 0)
			{
				int num2 = DefaultBattleMissionAgentSpawnLogic.MaxNumberOfAgentsForMission - this._spawnLogic.NumberOfAgents;
				int reservedTroopQuota = this.GetReservedTroopQuota(0);
				if (num2 >= reservedTroopQuota)
				{
					num = this.SpawnTroops(1, true);
					if (num > 0)
					{
						this._reinforcementQuotaRequirement -= reservedTroopQuota;
						if (this._reservedTroops.Count >= this._reinforcementBatchSize)
						{
							this._reinforcementQuotaRequirement += this.GetReservedTroopQuota(this._reinforcementBatchSize - 1);
						}
						this._reinforcementBatchPriority /= 2f;
					}
				}
			}
			this._reinforcementsSpawnedInLastBatch += num;
			return num;
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000828B0 File Offset: 0x00080AB0
		public void GetTeamFormationsSpawnData([TupleElementNames(new string[] { "team", "formationSpawnData" })] out MBList<ValueTuple<Team, MissionFormationSpawnData[]>> teamFormationsSpawnData)
		{
			Mission mission = Mission.Current;
			teamFormationsSpawnData = new MBList<ValueTuple<Team, MissionFormationSpawnData[]>>();
			foreach (Team team in mission.Teams.Where<Team>((Team t) => t.Side == this._side && t == mission.PlayerTeam).Concat<Team>(mission.Teams.Where<Team>((Team t) => t.Side == this._side && t != mission.PlayerTeam)))
			{
				if (team.Side == this._side)
				{
					MissionFormationSpawnData[] array = new MissionFormationSpawnData[11];
					for (int i = 0; i < array.Length; i++)
					{
						array[i].FootTroopCount = 0;
						array[i].MountedTroopCount = 0;
					}
					teamFormationsSpawnData.Add(new ValueTuple<Team, MissionFormationSpawnData[]>(team, array));
				}
			}
			foreach (IAgentOriginBase agentOriginBase in this._reservedTroops)
			{
				FormationClass agentTroopClass = Mission.Current.GetAgentTroopClass(this._side, agentOriginBase.Troop);
				bool flag = this._side == Mission.Current.PlayerTeam.Side;
				Team troopTeam = Mission.GetAgentTeam(agentOriginBase, flag);
				MissionFormationSpawnData[] item = teamFormationsSpawnData.FirstOrDefault<ValueTuple<Team, MissionFormationSpawnData[]>>(([TupleElementNames(new string[] { "team", "formationSpawnData" })] ValueTuple<Team, MissionFormationSpawnData[]> tf) => tf.Item1 == troopTeam).Item2;
				if (agentOriginBase.Troop.HasMount() && this.SpawnWithHorses)
				{
					MissionFormationSpawnData[] array2 = item;
					FormationClass formationClass = agentTroopClass;
					array2[(int)formationClass].MountedTroopCount = array2[(int)formationClass].MountedTroopCount + 1;
				}
				else
				{
					MissionFormationSpawnData[] array3 = item;
					FormationClass formationClass2 = agentTroopClass;
					array3[(int)formationClass2].FootTroopCount = array3[(int)formationClass2].FootTroopCount + 1;
				}
			}
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x00082A84 File Offset: 0x00080C84
		public void ReserveTroops(int number)
		{
			if (number > 0 && this._troopSupplier.AnyTroopRemainsToBeSupplied)
			{
				this._reservedTroops.AddRange(this._troopSupplier.SupplyTroops(number));
			}
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x00082AAE File Offset: 0x00080CAE
		public BasicCharacterObject GetGeneralCharacter()
		{
			return this._troopSupplier.GetGeneralCharacter();
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x00082ABC File Offset: 0x00080CBC
		public unsafe bool CheckReinforcementBatch()
		{
			MissionSpawnPhase missionSpawnPhase;
			if (this._side == BattleSideEnum.Defender)
			{
				missionSpawnPhase = this._spawnLogic.DefenderActivePhase;
			}
			else
			{
				missionSpawnPhase = this._spawnLogic.AttackerActivePhase;
			}
			this._reinforcementsSpawnedInLastBatch = 0;
			this.ReinforcementsNotifiedOnLastBatch = false;
			int num = 0;
			MissionSpawnSettings missionSpawnSettings = *this._spawnLogic.SpawnSettings;
			switch (missionSpawnSettings.ReinforcementTroopsSpawnMethod)
			{
			case MissionSpawnSettings.ReinforcementSpawnMethod.Balanced:
				num = this.ComputeBalancedBatch(missionSpawnPhase);
				break;
			case MissionSpawnSettings.ReinforcementSpawnMethod.Wave:
				num = this.ComputeWaveBatch(missionSpawnPhase);
				break;
			case MissionSpawnSettings.ReinforcementSpawnMethod.Fixed:
				num = this.ComputeFixedBatch(missionSpawnPhase);
				break;
			}
			num = Math.Min(num, missionSpawnPhase.RemainingSpawnNumber);
			num -= this._reservedTroops.Count;
			if (num > 0)
			{
				int count = this._reservedTroops.Count;
				this.ReserveTroops(num);
				if (count < this._reinforcementBatchSize)
				{
					int num2 = Math.Min(this._reservedTroops.Count, this._reinforcementBatchSize);
					for (int i = count; i < num2; i++)
					{
						this._reinforcementQuotaRequirement += this.GetReservedTroopQuota(i);
					}
				}
			}
			this._reinforcementBatchPriority = (float)this._reservedTroops.Count;
			bool flag;
			if (missionSpawnSettings.ReinforcementTroopsSpawnMethod == MissionSpawnSettings.ReinforcementSpawnMethod.Wave)
			{
				flag = this._reservedTroops.Count > 0;
			}
			else
			{
				flag = this._reservedTroops.Count > 0 && (this._reservedTroops.Count >= this._reinforcementBatchSize || missionSpawnPhase.RemainingSpawnNumber <= this._reinforcementBatchSize);
			}
			this.ReinforcementSpawnActive = flag;
			if (this.ReinforcementSpawnActive)
			{
				this.ResetReinforcementSpawnedUnitCountsPerFormation();
				foreach (Team team in Mission.Current.Teams)
				{
					if (team.Side == this._side)
					{
						this._spawnLogic.DeploymentPlan.UpdateReinforcementPlan(team);
					}
				}
			}
			return this.ReinforcementSpawnActive;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x00082CB0 File Offset: 0x00080EB0
		public IEnumerable<IAgentOriginBase> GetAllTroops()
		{
			return this._troopSupplier.GetAllTroops();
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x00082CC0 File Offset: 0x00080EC0
		public int SpawnTroops(int number, bool isReinforcement)
		{
			if (number <= 0)
			{
				return 0;
			}
			List<IAgentOriginBase> list = new List<IAgentOriginBase>();
			int num = MathF.Min(this._reservedTroops.Count, number);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					IAgentOriginBase agentOriginBase = this._reservedTroops[i];
					list.Add(agentOriginBase);
				}
				this._reservedTroops.RemoveRange(0, num);
			}
			int num2 = number - num;
			list.AddRange(this._troopSupplier.SupplyTroops(num2));
			Mission mission = Mission.Current;
			if (this._troopOriginsToSpawnPerTeam == null)
			{
				this._troopOriginsToSpawnPerTeam = new List<ValueTuple<Team, List<IAgentOriginBase>>>();
				using (List<Team>.Enumerator enumerator = mission.Teams.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Team team = enumerator.Current;
						bool flag = team.Side == mission.PlayerTeam.Side;
						if ((this.IsPlayerSide && flag) || (!this.IsPlayerSide && !flag))
						{
							this._troopOriginsToSpawnPerTeam.Add(new ValueTuple<Team, List<IAgentOriginBase>>(team, new List<IAgentOriginBase>()));
						}
					}
					goto IL_0136;
				}
			}
			foreach (ValueTuple<Team, List<IAgentOriginBase>> valueTuple in this._troopOriginsToSpawnPerTeam)
			{
				valueTuple.Item2.Clear();
			}
			IL_0136:
			int num3 = 0;
			foreach (IAgentOriginBase agentOriginBase2 in list)
			{
				Team agentTeam = Mission.GetAgentTeam(agentOriginBase2, this.IsPlayerSide);
				foreach (ValueTuple<Team, List<IAgentOriginBase>> valueTuple2 in this._troopOriginsToSpawnPerTeam)
				{
					if (agentTeam == valueTuple2.Item1)
					{
						num3++;
						valueTuple2.Item2.Add(agentOriginBase2);
					}
				}
			}
			int num4 = 0;
			List<IAgentOriginBase> list2 = new List<IAgentOriginBase>();
			foreach (ValueTuple<Team, List<IAgentOriginBase>> valueTuple3 in this._troopOriginsToSpawnPerTeam)
			{
				if (!valueTuple3.Item2.IsEmpty<IAgentOriginBase>())
				{
					int num5 = 0;
					List<ValueTuple<IAgentOriginBase, int>> list3 = null;
					if (isReinforcement)
					{
						list3 = new List<ValueTuple<IAgentOriginBase, int>>();
						using (List<IAgentOriginBase>.Enumerator enumerator3 = valueTuple3.Item2.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								IAgentOriginBase agentOriginBase3 = enumerator3.Current;
								int num6;
								this._reinforcementTroopFormationAssignments.TryGetValue(agentOriginBase3, out num6);
								list3.Add(new ValueTuple<IAgentOriginBase, int>(agentOriginBase3, num6));
							}
							goto IL_027A;
						}
						goto IL_025C;
					}
					goto IL_025C;
					IL_027A:
					for (int j = 0; j < 8; j++)
					{
						int num7 = 0;
						int num8 = 0;
						list2.Clear();
						IAgentOriginBase agentOriginBase4 = null;
						foreach (ValueTuple<IAgentOriginBase, int> valueTuple4 in list3)
						{
							IAgentOriginBase item = valueTuple4.Item1;
							int item2 = valueTuple4.Item2;
							if (j == item2)
							{
								if (item.Troop == Game.Current.PlayerTroop)
								{
									agentOriginBase4 = item;
								}
								else
								{
									if (item.Troop.HasMount())
									{
										num7++;
									}
									else
									{
										num8++;
									}
									list2.Add(item);
								}
							}
						}
						if (agentOriginBase4 != null)
						{
							if (agentOriginBase4.Troop.HasMount())
							{
								num7++;
							}
							else
							{
								num8++;
							}
							list2.Add(agentOriginBase4);
						}
						int count = list2.Count;
						if (count > 0)
						{
							bool flag2 = this._spawnWithHorses && DefaultMissionDeploymentPlan.HasSignificantMountedTroops(num8, num7);
							int num9 = 0;
							int num10 = count;
							if (this.ReinforcementSpawnActive)
							{
								num9 = this._reinforcementSpawnedUnitCountPerFormation[j].Item1;
								num10 = this._reinforcementSpawnedUnitCountPerFormation[j].Item2;
							}
							Formation formation = valueTuple3.Item1.GetFormation((FormationClass)j);
							if (!formation.HasBeenPositioned)
							{
								formation.BeginSpawn(num10, flag2);
								mission.SetFormationPositioningFromDeploymentPlan(formation);
								this._spawnedFormations.Add(formation);
							}
							foreach (IAgentOriginBase agentOriginBase5 in list2)
							{
								if (!agentOriginBase5.Troop.IsHero && this._bannerBearerLogic != null && mission.Mode != MissionMode.Deployment && this._bannerBearerLogic.GetMissingBannerCount(formation) > 0)
								{
									this._bannerBearerLogic.SpawnBannerBearer(agentOriginBase5, this.IsPlayerSide, formation, this._spawnWithHorses, isReinforcement, num10, num9, true, true, null, null, null, mission.IsSallyOutBattle);
								}
								else
								{
									bool flag3 = (agentOriginBase5.Troop.IsPlayerCharacter && this.ForceSpawnPlayerMounted) || this._spawnWithHorses;
									mission.SpawnTroop(agentOriginBase5, this.IsPlayerSide, true, flag3, isReinforcement, num10, num9, true, true, null, null, null, null, formation.FormationIndex, mission.IsSallyOutBattle);
								}
								this._numSpawnedTroops++;
								num9++;
								num5++;
							}
							if (this.ReinforcementSpawnActive)
							{
								this._reinforcementSpawnedUnitCountPerFormation[j].Item1 = num9;
							}
						}
					}
					if (num5 > 0)
					{
						valueTuple3.Item1.QuerySystem.Expire();
					}
					num4 += num5;
					foreach (Formation formation2 in valueTuple3.Item1.FormationsIncludingEmpty)
					{
						if (formation2.CountOfUnits > 0 && formation2.IsSpawning)
						{
							formation2.EndSpawn();
						}
					}
					continue;
					IL_025C:
					list3 = MissionGameModels.Current.BattleSpawnModel.GetInitialSpawnAssignments(this._side, valueTuple3.Item2);
					goto IL_027A;
				}
			}
			return num4;
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x00083348 File Offset: 0x00081548
		public void SetSpawnWithHorses(bool spawnWithHorses)
		{
			this._spawnWithHorses = spawnWithHorses;
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x00083354 File Offset: 0x00081554
		private unsafe int ComputeBalancedBatch(MissionSpawnPhase activePhase)
		{
			int num = 0;
			if (activePhase != null && activePhase.RemainingSpawnNumber > 0)
			{
				MissionSpawnSettings missionSpawnSettings = *this._spawnLogic.SpawnSettings;
				int reinforcementBatchSize = this._reinforcementBatchSize;
				this._reinforcementBatchSize = (int)((float)this._spawnLogic.BattleSize * missionSpawnSettings.ReinforcementBatchPercentage);
				if (reinforcementBatchSize != this._reinforcementBatchSize)
				{
					this.UpdateReinforcementQuotaRequirement(reinforcementBatchSize);
				}
				int num2 = activePhase.TotalSpawnNumber - activePhase.InitialSpawnedNumber;
				num = MathF.Max(1, this._reservedTroops.Count + (int)((float)num2 * missionSpawnSettings.DesiredReinforcementPercentage));
				num = MathF.Min(num, activePhase.InitialSpawnedNumber - this.NumberOfActiveTroops);
			}
			return num;
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x000833FC File Offset: 0x000815FC
		private unsafe int ComputeFixedBatch(MissionSpawnPhase activePhase)
		{
			int num = 0;
			if (activePhase != null && activePhase.RemainingSpawnNumber > 0)
			{
				MissionSpawnSettings missionSpawnSettings = *this._spawnLogic.SpawnSettings;
				float num2 = ((this._side == BattleSideEnum.Defender) ? missionSpawnSettings.DefenderReinforcementBatchPercentage : missionSpawnSettings.AttackerReinforcementBatchPercentage);
				int reinforcementBatchSize = this._reinforcementBatchSize;
				this._reinforcementBatchSize = (int)((float)this._spawnLogic.TotalSpawnNumber * num2);
				if (reinforcementBatchSize != this._reinforcementBatchSize)
				{
					this.UpdateReinforcementQuotaRequirement(reinforcementBatchSize);
				}
				num = MathF.Max(1, this._reinforcementBatchSize);
			}
			return num;
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0008347C File Offset: 0x0008167C
		private unsafe int ComputeWaveBatch(MissionSpawnPhase activePhase)
		{
			int num = 0;
			if (activePhase != null && activePhase.RemainingSpawnNumber > 0 && this._reservedTroops.IsEmpty<IAgentOriginBase>())
			{
				MissionSpawnSettings missionSpawnSettings = *this._spawnLogic.SpawnSettings;
				int reinforcementBatchSize = this._reinforcementBatchSize;
				int num2 = (int)Math.Max(1f, (float)activePhase.InitialSpawnedNumber * missionSpawnSettings.ReinforcementWavePercentage);
				this._reinforcementBatchSize = num2;
				if (reinforcementBatchSize != this._reinforcementBatchSize)
				{
					this.UpdateReinforcementQuotaRequirement(reinforcementBatchSize);
				}
				if (activePhase.InitialSpawnedNumber - activePhase.NumberActiveTroops >= num2)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x00083501 File Offset: 0x00081701
		public void SetBannerBearerLogic(BannerBearerLogic bannerBearerLogic)
		{
			this._bannerBearerLogic = bannerBearerLogic;
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x0008350C File Offset: 0x0008170C
		private void UpdateReinforcementQuotaRequirement(int previousBatchSize)
		{
			if (this._reinforcementBatchSize < previousBatchSize)
			{
				for (int i = MathF.Min(this._reservedTroops.Count - 1, previousBatchSize - 1); i >= this._reinforcementBatchSize; i--)
				{
					this._reinforcementQuotaRequirement -= this.GetReservedTroopQuota(i);
				}
				return;
			}
			if (this._reinforcementBatchSize > previousBatchSize)
			{
				int num = MathF.Min(this._reservedTroops.Count - 1, this._reinforcementBatchSize - 1);
				for (int j = previousBatchSize; j <= num; j++)
				{
					this._reinforcementQuotaRequirement += this.GetReservedTroopQuota(j);
				}
			}
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x000835A0 File Offset: 0x000817A0
		public void SetReinforcementsNotifiedOnLastBatch(bool value)
		{
			this.ReinforcementsNotifiedOnLastBatch = value;
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000835AC File Offset: 0x000817AC
		private void ResetReinforcementSpawnedUnitCountsPerFormation()
		{
			for (int i = 0; i < 8; i++)
			{
				this._reinforcementSpawnedUnitCountPerFormation[i].Item1 = 0;
				this._reinforcementSpawnedUnitCountPerFormation[i].Item2 = 0;
			}
			this._reinforcementTroopFormationAssignments.Clear();
			foreach (ValueTuple<IAgentOriginBase, int> valueTuple in MissionGameModels.Current.BattleSpawnModel.GetReinforcementAssignments(this._side, this._reservedTroops))
			{
				int item = valueTuple.Item2;
				this._reinforcementTroopFormationAssignments.Add(valueTuple.Item1, valueTuple.Item2);
				ValueTuple<int, int>[] reinforcementSpawnedUnitCountPerFormation = this._reinforcementSpawnedUnitCountPerFormation;
				int num = item;
				reinforcementSpawnedUnitCountPerFormation[num].Item2 = reinforcementSpawnedUnitCountPerFormation[num].Item2 + 1;
			}
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0008367C File Offset: 0x0008187C
		public void SetSpawnTroops(bool spawnTroops)
		{
			this.TroopSpawnActive = spawnTroops;
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x00083685 File Offset: 0x00081885
		private int GetReservedTroopQuota(int index)
		{
			if (!this._spawnWithHorses || !this._reservedTroops[index].Troop.IsMounted)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000836AC File Offset: 0x000818AC
		public void OnInitialSpawnOver()
		{
			foreach (Formation formation in this._spawnedFormations)
			{
				formation.EndSpawn();
			}
		}

		// Token: 0x04000DEA RID: 3562
		private readonly IBattleMissionAgentSpawnLogic _spawnLogic;

		// Token: 0x04000DEB RID: 3563
		private readonly BattleSideEnum _side;

		// Token: 0x04000DEC RID: 3564
		private readonly IMissionTroopSupplier _troopSupplier;

		// Token: 0x04000DED RID: 3565
		private BannerBearerLogic _bannerBearerLogic;

		// Token: 0x04000DEE RID: 3566
		private readonly MBArrayList<Formation> _spawnedFormations;

		// Token: 0x04000DEF RID: 3567
		private bool _spawnWithHorses;

		// Token: 0x04000DF0 RID: 3568
		private float _reinforcementBatchPriority;

		// Token: 0x04000DF1 RID: 3569
		private int _reinforcementQuotaRequirement;

		// Token: 0x04000DF2 RID: 3570
		private int _reinforcementBatchSize;

		// Token: 0x04000DF3 RID: 3571
		private int _reinforcementsSpawnedInLastBatch;

		// Token: 0x04000DF4 RID: 3572
		private int _numSpawnedTroops;

		// Token: 0x04000DF5 RID: 3573
		private readonly List<IAgentOriginBase> _reservedTroops = new List<IAgentOriginBase>();

		// Token: 0x04000DF6 RID: 3574
		[TupleElementNames(new string[] { "team", "origins" })]
		private List<ValueTuple<Team, List<IAgentOriginBase>>> _troopOriginsToSpawnPerTeam;

		// Token: 0x04000DFC RID: 3580
		[TupleElementNames(new string[] { "currentTroopIndex", "troopCount" })]
		private readonly ValueTuple<int, int>[] _reinforcementSpawnedUnitCountPerFormation;

		// Token: 0x04000DFD RID: 3581
		private readonly Dictionary<IAgentOriginBase, int> _reinforcementTroopFormationAssignments;
	}
}
