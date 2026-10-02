using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000183 RID: 387
	public abstract class TeamAIComponent
	{
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060014B4 RID: 5300 RVA: 0x0004CA80 File Offset: 0x0004AC80
		public MBReadOnlyList<StrategicArea> StrategicAreas
		{
			get
			{
				return this._strategicAreas;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x0004CA88 File Offset: 0x0004AC88
		public bool HasStrategicAreas
		{
			get
			{
				return !this._strategicAreas.IsEmpty<StrategicArea>();
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060014B6 RID: 5302 RVA: 0x0004CA98 File Offset: 0x0004AC98
		// (set) Token: 0x060014B7 RID: 5303 RVA: 0x0004CAA0 File Offset: 0x0004ACA0
		public bool IsDefenseApplicable { get; private set; }

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060014B8 RID: 5304 RVA: 0x0004CAA9 File Offset: 0x0004ACA9
		// (set) Token: 0x060014B9 RID: 5305 RVA: 0x0004CAB1 File Offset: 0x0004ACB1
		public bool GetIsFirstTacticChosen { get; private set; }

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060014BA RID: 5306 RVA: 0x0004CABA File Offset: 0x0004ACBA
		// (set) Token: 0x060014BB RID: 5307 RVA: 0x0004CAC2 File Offset: 0x0004ACC2
		private protected TacticComponent CurrentTactic
		{
			protected get
			{
				return this._currentTactic;
			}
			private set
			{
				TacticComponent currentTactic = this._currentTactic;
				if (currentTactic != null)
				{
					currentTactic.OnCancel();
				}
				this._currentTactic = value;
				if (this._currentTactic != null)
				{
					this._currentTactic.OnApply();
					this._currentTactic.TickOccasionally();
				}
			}
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0004CAFC File Offset: 0x0004ACFC
		protected TeamAIComponent(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
		{
			this.Mission = currentMission;
			this.Team = currentTeam;
			this._thinkTimer = new Timer(this.Mission.CurrentTime, thinkTimerTime, true);
			this._applyTimer = new Timer(this.Mission.CurrentTime, applyTimerTime, true);
			this._occasionalTickTime = applyTimerTime;
			this._availableTactics = new List<TacticComponent>();
			this.TacticalPositions = currentMission.ActiveMissionObjects.FindAllWithType<TacticalPosition>().ToList<TacticalPosition>();
			this.TacticalRegions = currentMission.ActiveMissionObjects.FindAllWithType<TacticalRegion>().ToList<TacticalRegion>();
			this._strategicAreas = (from amo in currentMission.ActiveMissionObjects.Where<MissionObject>(delegate(MissionObject amo)
				{
					StrategicArea strategicArea;
					return (strategicArea = amo as StrategicArea) != null && strategicArea.IsActive && strategicArea.IsUsableBy(this.Team.Side);
				})
				select amo as StrategicArea).ToMBList<StrategicArea>();
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0004CBD3 File Offset: 0x0004ADD3
		public void AddStrategicArea(StrategicArea strategicArea)
		{
			this._strategicAreas.Add(strategicArea);
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0004CBE1 File Offset: 0x0004ADE1
		public void RemoveStrategicArea(StrategicArea strategicArea)
		{
			if (this.Team.DetachmentManager.ContainsDetachment(strategicArea))
			{
				this.Team.DetachmentManager.DestroyDetachment(strategicArea);
			}
			this._strategicAreas.Remove(strategicArea);
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0004CC14 File Offset: 0x0004AE14
		public void RemoveAllStrategicAreas()
		{
			foreach (StrategicArea strategicArea in this._strategicAreas)
			{
				if (this.Team.DetachmentManager.ContainsDetachment(strategicArea))
				{
					this.Team.DetachmentManager.DestroyDetachment(strategicArea);
				}
			}
			this._strategicAreas.Clear();
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0004CC90 File Offset: 0x0004AE90
		public void AddTacticOption(TacticComponent tacticOption)
		{
			this._availableTactics.Add(tacticOption);
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0004CCA0 File Offset: 0x0004AEA0
		public void RemoveTacticOption(Type tacticType)
		{
			this._availableTactics.RemoveAll((TacticComponent at) => tacticType == at.GetType());
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0004CCD2 File Offset: 0x0004AED2
		public void ClearTacticOptions()
		{
			this._availableTactics.Clear();
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0004CCDF File Offset: 0x0004AEDF
		[Conditional("DEBUG")]
		public void AssertTeam(Team team)
		{
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0004CCE1 File Offset: 0x0004AEE1
		public void NotifyTacticalDecision(in TacticalDecision decision)
		{
			TeamAIComponent.TacticalDecisionDelegate onNotifyTacticalDecision = this.OnNotifyTacticalDecision;
			if (onNotifyTacticalDecision == null)
			{
				return;
			}
			onNotifyTacticalDecision(in decision);
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0004CCF4 File Offset: 0x0004AEF4
		public virtual void OnDeploymentFinished()
		{
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0004CCF6 File Offset: 0x0004AEF6
		public virtual void OnFormationFrameChanged(Agent agent, bool isFrameEnabled, WorldPosition frame)
		{
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0004CCF8 File Offset: 0x0004AEF8
		public virtual void OnMissionEnded()
		{
			MBDebug.Print("Mission end received by teamAI", 0, Debug.DebugColor.White, 17592186044416UL);
			foreach (Formation formation in this.Team.FormationsIncludingSpecialAndEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					foreach (UsableMachine usableMachine in formation.GetUsedMachines().ToList<UsableMachine>())
					{
						formation.StopUsingMachine(usableMachine, false);
					}
				}
			}
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0004CDB0 File Offset: 0x0004AFB0
		public void ResetTacticalPositions()
		{
			this.TacticalPositions = this.Mission.ActiveMissionObjects.FindAllWithType<TacticalPosition>().ToList<TacticalPosition>();
			this.TacticalRegions = this.Mission.ActiveMissionObjects.FindAllWithType<TacticalRegion>().ToList<TacticalRegion>();
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0004CDE8 File Offset: 0x0004AFE8
		public void ResetTactic(bool keepCurrentTactic = true)
		{
			if (!keepCurrentTactic)
			{
				this.CurrentTactic = null;
			}
			this._thinkTimer.Reset(this.Mission.CurrentTime);
			this._applyTimer.Reset(this.Mission.CurrentTime);
			this.MakeDecision();
			this.TickOccasionally();
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0004CE38 File Offset: 0x0004B038
		protected internal virtual void Tick(float dt)
		{
			if (this.Team.BodyGuardFormation != null && this.Team.BodyGuardFormation.CountOfUnits > 0 && (this.Team.GeneralsFormation == null || this.Team.GeneralsFormation.CountOfUnits == 0))
			{
				this.Team.BodyGuardFormation.AI.ResetBehaviorWeights();
				this.Team.BodyGuardFormation.AI.SetBehaviorWeight<BehaviorCharge>(1f);
			}
			if (this._nextTacticChooseTime.IsPast)
			{
				this.MakeDecision();
				this._nextTacticChooseTime = MissionTime.SecondsFromNow(5f);
			}
			if (this._nextOccasionalTickTime.IsPast)
			{
				this.TickOccasionally();
				this._nextOccasionalTickTime = MissionTime.SecondsFromNow(this._occasionalTickTime);
			}
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0004CEFC File Offset: 0x0004B0FC
		public void CheckIsDefenseApplicable()
		{
			if (this.Team.Side != BattleSideEnum.Defender)
			{
				this.IsDefenseApplicable = false;
				return;
			}
			int memberCount = this.Team.QuerySystem.MemberCount;
			float maxUnderRangedAttackRatio = this.Team.QuerySystem.MaxUnderRangedAttackRatio;
			float num = (float)memberCount * maxUnderRangedAttackRatio;
			int deathByRangedCount = this.Team.QuerySystem.DeathByRangedCount;
			int deathCount = this.Team.QuerySystem.DeathCount;
			float num2 = MBMath.ClampFloat((num + (float)deathByRangedCount) / (float)(memberCount + deathCount), 0.05f, 1f);
			int enemyUnitCount = this.Team.QuerySystem.EnemyUnitCount;
			float num3 = 0f;
			int num4 = 0;
			int num5 = 0;
			foreach (Team team in this.Mission.Teams)
			{
				if (this.Team.IsEnemyOf(team))
				{
					TeamQuerySystem querySystem = team.QuerySystem;
					num4 += querySystem.DeathByRangedCount;
					num5 += querySystem.DeathCount;
					num3 += ((enemyUnitCount == 0) ? 0f : (querySystem.MaxUnderRangedAttackRatio * ((float)querySystem.MemberCount / (float)((enemyUnitCount > 0) ? enemyUnitCount : 1))));
				}
			}
			float num6 = (float)enemyUnitCount * num3;
			int num7 = enemyUnitCount + num5;
			float num8 = MBMath.ClampFloat((num6 + (float)num4) / (float)((num7 > 0) ? num7 : 1), 0.05f, 1f);
			float num9 = MathF.Pow(num2 / num8, 3f * (this.Team.QuerySystem.EnemyRangedRatio + this.Team.QuerySystem.EnemyRangedCavalryRatio));
			this.IsDefenseApplicable = num9 <= 1.5f;
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0004D0B4 File Offset: 0x0004B2B4
		public void OnTacticAppliedForFirstTime()
		{
			this.GetIsFirstTacticChosen = false;
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0004D0C0 File Offset: 0x0004B2C0
		private void MakeDecision()
		{
			List<TacticComponent> availableTactics = this._availableTactics;
			if ((this.Mission.CurrentState != Mission.State.Continuing && availableTactics.Count == 0) || !this.Team.HasAnyFormationsIncludingSpecialThatIsNotEmpty())
			{
				return;
			}
			bool flag = true;
			foreach (Team team in this.Mission.Teams)
			{
				if (team.IsEnemyOf(this.Team) && team.HasAnyFormationsIncludingSpecialThatIsNotEmpty())
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				if (this.Mission.MissionEnded)
				{
					return;
				}
				if (!(this.CurrentTactic is TacticCharge))
				{
					foreach (TacticComponent tacticComponent in availableTactics)
					{
						if (tacticComponent is TacticCharge)
						{
							if (this.CurrentTactic == null)
							{
								this.GetIsFirstTacticChosen = true;
							}
							this.CurrentTactic = tacticComponent;
							break;
						}
					}
					if (!(this.CurrentTactic is TacticCharge))
					{
						if (this.CurrentTactic == null)
						{
							this.GetIsFirstTacticChosen = true;
						}
						this.CurrentTactic = availableTactics.FirstOrDefault<TacticComponent>();
					}
				}
			}
			this.CheckIsDefenseApplicable();
			TacticComponent tacticComponent2 = availableTactics.MaxBy<TacticComponent, float>((TacticComponent to) => to.GetTacticWeight() * ((to == this._currentTactic) ? 1.5f : 1f));
			bool flag2 = false;
			if (this.CurrentTactic == null)
			{
				flag2 = true;
			}
			else if (this.CurrentTactic != tacticComponent2)
			{
				if (!this.CurrentTactic.ResetTacticalPositions())
				{
					flag2 = true;
				}
				else
				{
					float tacticWeight = tacticComponent2.GetTacticWeight();
					float num = this.CurrentTactic.GetTacticWeight() * 1.5f;
					if (tacticWeight > num)
					{
						flag2 = true;
					}
				}
			}
			if (flag2)
			{
				if (this.CurrentTactic == null)
				{
					this.GetIsFirstTacticChosen = true;
				}
				this.CurrentTactic = tacticComponent2;
				if (Mission.Current.MainAgent != null && this.Team.GeneralAgent != null && this.Team.IsPlayerTeam && this.Team.IsPlayerSergeant)
				{
					string name = tacticComponent2.GetType().Name;
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_team_ai_tactic_text", name), 4000, this.Team.GeneralAgent.Character, null, "");
				}
			}
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0004D2E8 File Offset: 0x0004B4E8
		public virtual void TickOccasionally()
		{
			if (Mission.Current.AllowAiTicking && this.Team.HasBots)
			{
				TacticComponent currentTactic = this.CurrentTactic;
				if (currentTactic == null)
				{
					return;
				}
				currentTactic.TickOccasionally();
			}
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0004D313 File Offset: 0x0004B513
		public bool IsCurrentTactic(TacticComponent tactic)
		{
			return tactic == this.CurrentTactic;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0004D320 File Offset: 0x0004B520
		[Conditional("DEBUG")]
		protected virtual void DebugTick(float dt)
		{
			if (!MBDebug.IsDisplayingHighLevelAI)
			{
				return;
			}
			TacticComponent currentTactic = this.CurrentTactic;
			if (Input.DebugInput.IsHotKeyPressed("UsableMachineAiBaseHotkeyRetreatScriptActive"))
			{
				TeamAIComponent._retreatScriptActive = true;
			}
			else if (Input.DebugInput.IsHotKeyPressed("UsableMachineAiBaseHotkeyRetreatScriptPassive"))
			{
				TeamAIComponent._retreatScriptActive = false;
			}
			bool retreatScriptActive = TeamAIComponent._retreatScriptActive;
		}

		// Token: 0x060014D1 RID: 5329
		public abstract void OnUnitAddedToFormationForTheFirstTime(Formation formation);

		// Token: 0x060014D2 RID: 5330 RVA: 0x0004D372 File Offset: 0x0004B572
		protected internal virtual void CreateMissionSpecificBehaviors()
		{
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0004D374 File Offset: 0x0004B574
		protected internal virtual void InitializeDetachments(Mission mission)
		{
			DeploymentHandler missionBehavior = this.Mission.GetMissionBehavior<DeploymentHandler>();
			if (missionBehavior == null)
			{
				return;
			}
			missionBehavior.InitializeDeploymentPoints();
		}

		// Token: 0x04000594 RID: 1428
		public TeamAIComponent.TacticalDecisionDelegate OnNotifyTacticalDecision;

		// Token: 0x04000595 RID: 1429
		public const int BattleTokenForceSize = 10;

		// Token: 0x04000596 RID: 1430
		private readonly List<TacticComponent> _availableTactics;

		// Token: 0x04000597 RID: 1431
		private static bool _retreatScriptActive;

		// Token: 0x04000598 RID: 1432
		protected readonly Mission Mission;

		// Token: 0x04000599 RID: 1433
		protected readonly Team Team;

		// Token: 0x0400059A RID: 1434
		private readonly Timer _thinkTimer;

		// Token: 0x0400059B RID: 1435
		private readonly Timer _applyTimer;

		// Token: 0x0400059C RID: 1436
		private TacticComponent _currentTactic;

		// Token: 0x0400059D RID: 1437
		public List<TacticalPosition> TacticalPositions;

		// Token: 0x0400059E RID: 1438
		public List<TacticalRegion> TacticalRegions;

		// Token: 0x0400059F RID: 1439
		private readonly MBList<StrategicArea> _strategicAreas;

		// Token: 0x040005A0 RID: 1440
		private readonly float _occasionalTickTime;

		// Token: 0x040005A1 RID: 1441
		private MissionTime _nextTacticChooseTime;

		// Token: 0x040005A2 RID: 1442
		private MissionTime _nextOccasionalTickTime;

		// Token: 0x020004D6 RID: 1238
		protected class TacticOption
		{
			// Token: 0x17000A2C RID: 2604
			// (get) Token: 0x06003AE8 RID: 15080 RVA: 0x000ED275 File Offset: 0x000EB475
			// (set) Token: 0x06003AE9 RID: 15081 RVA: 0x000ED27D File Offset: 0x000EB47D
			public string Id { get; private set; }

			// Token: 0x17000A2D RID: 2605
			// (get) Token: 0x06003AEA RID: 15082 RVA: 0x000ED286 File Offset: 0x000EB486
			// (set) Token: 0x06003AEB RID: 15083 RVA: 0x000ED28E File Offset: 0x000EB48E
			public Lazy<TacticComponent> Tactic { get; private set; }

			// Token: 0x17000A2E RID: 2606
			// (get) Token: 0x06003AEC RID: 15084 RVA: 0x000ED297 File Offset: 0x000EB497
			// (set) Token: 0x06003AED RID: 15085 RVA: 0x000ED29F File Offset: 0x000EB49F
			public float Weight { get; set; }

			// Token: 0x06003AEE RID: 15086 RVA: 0x000ED2A8 File Offset: 0x000EB4A8
			public TacticOption(string id, Lazy<TacticComponent> tactic, float weight)
			{
				this.Id = id;
				this.Tactic = tactic;
				this.Weight = weight;
			}
		}

		// Token: 0x020004D7 RID: 1239
		// (Invoke) Token: 0x06003AF0 RID: 15088
		public delegate void TacticalDecisionDelegate(in TacticalDecision decision);
	}
}
