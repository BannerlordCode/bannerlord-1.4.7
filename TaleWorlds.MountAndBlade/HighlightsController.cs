using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000285 RID: 645
	public class HighlightsController : MissionLogic
	{
		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060023F1 RID: 9201 RVA: 0x00081603 File Offset: 0x0007F803
		// (set) Token: 0x060023F2 RID: 9202 RVA: 0x0008160A File Offset: 0x0007F80A
		private protected static List<HighlightsController.HighlightType> HighlightTypes { protected get; private set; }

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x00081612 File Offset: 0x0007F812
		// (set) Token: 0x060023F4 RID: 9204 RVA: 0x00081619 File Offset: 0x0007F819
		public static bool IsHighlightsInitialized { get; private set; }

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060023F5 RID: 9205 RVA: 0x00081621 File Offset: 0x0007F821
		public bool IsAnyHighlightSaved
		{
			get
			{
				return this._savedHighlightGroups.Count > 0;
			}
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x00081634 File Offset: 0x0007F834
		public static void RemoveHighlights()
		{
			if (HighlightsController.IsHighlightsInitialized)
			{
				foreach (HighlightsController.HighlightType highlightType in HighlightsController.HighlightTypes)
				{
					Highlights.RemoveHighlight(highlightType.Id);
				}
			}
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x00081694 File Offset: 0x0007F894
		public HighlightsController.HighlightType GetHighlightTypeWithId(string highlightId)
		{
			return HighlightsController.HighlightTypes.First<HighlightsController.HighlightType>((HighlightsController.HighlightType h) => h.Id == highlightId);
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x000816C4 File Offset: 0x0007F8C4
		private void SaveVideo(string highlightID, string groupID, int startDelta, int endDelta)
		{
			Highlights.SaveVideo(highlightID, groupID, startDelta, endDelta);
			if (!this._savedHighlightGroups.Contains(groupID))
			{
				this._savedHighlightGroups.Add(groupID);
			}
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000816EC File Offset: 0x0007F8EC
		public override void AfterStart()
		{
			if (!HighlightsController.IsHighlightsInitialized)
			{
				HighlightsController.HighlightTypes = new List<HighlightsController.HighlightType>
				{
					new HighlightsController.HighlightType("hlid_killing_spree", "Killing Spree", "grpid_incidents", -2010, 3000, 0.25f, float.MaxValue, true),
					new HighlightsController.HighlightType("hlid_high_ranged_shot_difficulty", "Sharpshooter", "grpid_incidents", -5000, 3000, 0.25f, float.MaxValue, true),
					new HighlightsController.HighlightType("hlid_archer_salvo_kills", "Death from Above", "grpid_incidents", -5004, 3000, 0.5f, 150f, false),
					new HighlightsController.HighlightType("hlid_couched_lance_against_mounted_opponent", "Lance A Lot", "grpid_incidents", -5000, 3000, 0.25f, float.MaxValue, true),
					new HighlightsController.HighlightType("hlid_cavalry_charge_first_impact", "Cavalry Charge First Impact", "grpid_incidents", -5000, 5000, 0.25f, float.MaxValue, false),
					new HighlightsController.HighlightType("hlid_headshot_kill", "Headshot!", "grpid_incidents", -5000, 3000, 0.25f, 150f, true),
					new HighlightsController.HighlightType("hlid_burning_ammunition_kill", "Burn Baby", "grpid_incidents", -5000, 3000, 0.25f, 100f, true),
					new HighlightsController.HighlightType("hlid_throwing_weapon_kill_against_charging_enemy", "Throwing Weapon Kill Against Charging Enemy", "grpid_incidents", -5000, 3000, 0.25f, 150f, true)
				};
				Highlights.Initialize();
				foreach (HighlightsController.HighlightType highlightType in HighlightsController.HighlightTypes)
				{
					Highlights.AddHighlight(highlightType.Id, highlightType.Description);
				}
				HighlightsController.IsHighlightsInitialized = true;
			}
			foreach (string text in this._highlightGroupIds)
			{
				Highlights.OpenGroup(text);
			}
			this._highlightSaveQueue = new List<HighlightsController.Highlight>();
			this._playerKillTimes = new List<float>();
			this._archerSalvoKillTimes = new List<float>();
			this._cavalryChargeHitTimes = new List<float>();
			this._savedHighlightGroups = new List<string>();
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x00081958 File Offset: 0x0007FB58
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectorAgent != null && affectedAgent != null && affectorAgent.IsHuman && affectedAgent.IsHuman && (agentState == AgentState.Killed || agentState == AgentState.Unconscious))
			{
				bool flag = affectorAgent.Team != null && affectorAgent.Team.IsPlayerTeam;
				bool isMainAgent = affectorAgent.IsMainAgent;
				if ((((isMainAgent || flag) && !affectedAgent.Team.IsPlayerAlly && killingBlow.WeaponClass == 12) || killingBlow.WeaponClass == 13) && this.CanSaveHighlight(this.GetHighlightTypeWithId("hlid_archer_salvo_kills"), affectedAgent.Position))
				{
					if (!this._isArcherSalvoHappening)
					{
						this._archerSalvoKillTimes.RemoveAll((float ht) => ht + 4f < Mission.Current.CurrentTime);
					}
					this._archerSalvoKillTimes.Add(Mission.Current.CurrentTime);
					if (this._archerSalvoKillTimes.Count >= 5)
					{
						this._isArcherSalvoHappening = true;
					}
				}
				if (isMainAgent && this.CanSaveHighlight(this.GetHighlightTypeWithId("hlid_killing_spree"), affectedAgent.Position))
				{
					if (!this._isKillingSpreeHappening)
					{
						this._playerKillTimes.RemoveAll((float ht) => ht + 10f < Mission.Current.CurrentTime);
					}
					this._playerKillTimes.Add(Mission.Current.CurrentTime);
					if (this._playerKillTimes.Count >= 4)
					{
						this._isKillingSpreeHappening = true;
					}
				}
				HighlightsController.Highlight highlight = default(HighlightsController.Highlight);
				highlight.Start = Mission.Current.CurrentTime;
				highlight.End = Mission.Current.CurrentTime;
				bool flag2 = false;
				if (isMainAgent && killingBlow.WeaponRecordWeaponFlags.HasAllFlags(WeaponFlags.Burning))
				{
					highlight.HighlightType = this.GetHighlightTypeWithId("hlid_burning_ammunition_kill");
					flag2 = true;
				}
				if (isMainAgent && killingBlow.IsMissile && killingBlow.IsHeadShot())
				{
					highlight.HighlightType = this.GetHighlightTypeWithId("hlid_headshot_kill");
					flag2 = true;
				}
				if (isMainAgent && killingBlow.IsMissile && affectedAgent.HasMount && affectedAgent.IsDoingPassiveAttack && (killingBlow.WeaponClass == 21 || killingBlow.WeaponClass == 22))
				{
					highlight.HighlightType = this.GetHighlightTypeWithId("hlid_throwing_weapon_kill_against_charging_enemy");
					flag2 = true;
				}
				if (this._isFirstImpact && affectorAgent.Formation != null && affectorAgent.Formation.PhysicalClass.IsMeleeCavalry() && affectorAgent.Formation.GetReadonlyMovementOrderReference() == MovementOrder.MovementOrderCharge && this.CanSaveHighlight(this.GetHighlightTypeWithId("hlid_cavalry_charge_first_impact"), affectedAgent.Position))
				{
					this._cavalryChargeHitTimes.RemoveAll((float ht) => ht + 3f < Mission.Current.CurrentTime);
					this._cavalryChargeHitTimes.Add(Mission.Current.CurrentTime);
					if (this._cavalryChargeHitTimes.Count >= 5)
					{
						highlight.HighlightType = this.GetHighlightTypeWithId("hlid_cavalry_charge_first_impact");
						highlight.Start = this._cavalryChargeHitTimes[0];
						highlight.End = this._cavalryChargeHitTimes[this._cavalryChargeHitTimes.Count - 1];
						flag2 = true;
						this._isFirstImpact = false;
						this._cavalryChargeHitTimes.Clear();
					}
				}
				if (flag2)
				{
					this.SaveHighlight(highlight, affectedAgent.Position);
				}
			}
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x00081CA4 File Offset: 0x0007FEA4
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			if (affectorAgent != null && affectedAgent != null && affectorAgent.IsHuman && affectedAgent.IsHuman)
			{
				bool isMainAgent = affectorAgent.IsMainAgent;
				HighlightsController.Highlight highlight = default(HighlightsController.Highlight);
				highlight.Start = Mission.Current.CurrentTime;
				highlight.End = Mission.Current.CurrentTime;
				bool flag = false;
				if (isMainAgent && shotDifficulty >= 7.5f)
				{
					highlight.HighlightType = this.GetHighlightTypeWithId("hlid_high_ranged_shot_difficulty");
					flag = true;
				}
				if (isMainAgent && affectedAgent.HasMount && blow.AttackType == AgentAttackType.Standard && affectorAgent.HasMount && affectorAgent.IsDoingPassiveAttack)
				{
					highlight.HighlightType = this.GetHighlightTypeWithId("hlid_couched_lance_against_mounted_opponent");
					flag = true;
				}
				if (this._isFirstImpact && affectorAgent.Formation != null && affectorAgent.Formation.PhysicalClass.IsMeleeCavalry() && affectorAgent.Formation.GetReadonlyMovementOrderReference() == MovementOrder.MovementOrderCharge && this.CanSaveHighlight(this.GetHighlightTypeWithId("hlid_cavalry_charge_first_impact"), affectedAgent.Position))
				{
					this._cavalryChargeHitTimes.RemoveAll((float ht) => ht + 3f < Mission.Current.CurrentTime);
					this._cavalryChargeHitTimes.Add(Mission.Current.CurrentTime);
					if (this._cavalryChargeHitTimes.Count >= 5)
					{
						highlight.HighlightType = this.GetHighlightTypeWithId("hlid_cavalry_charge_first_impact");
						highlight.Start = this._cavalryChargeHitTimes[0];
						highlight.End = this._cavalryChargeHitTimes[this._cavalryChargeHitTimes.Count - 1];
						flag = true;
						this._isFirstImpact = false;
						this._cavalryChargeHitTimes.Clear();
					}
				}
				if (flag)
				{
					this.SaveHighlight(highlight, affectedAgent.Position);
				}
			}
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x00081E74 File Offset: 0x00080074
		public override void OnMissionTick(float dt)
		{
			if (this._isArcherSalvoHappening && this._archerSalvoKillTimes[0] + 4f < Mission.Current.CurrentTime)
			{
				HighlightsController.Highlight highlight;
				highlight.HighlightType = this.GetHighlightTypeWithId("hlid_archer_salvo_kills");
				highlight.Start = this._archerSalvoKillTimes[0];
				highlight.End = this._archerSalvoKillTimes[this._archerSalvoKillTimes.Count - 1];
				this.SaveHighlight(highlight);
				this._isArcherSalvoHappening = false;
				this._archerSalvoKillTimes.Clear();
			}
			if (this._isKillingSpreeHappening && this._playerKillTimes[0] + 10f < Mission.Current.CurrentTime)
			{
				HighlightsController.Highlight highlight2;
				highlight2.HighlightType = this.GetHighlightTypeWithId("hlid_killing_spree");
				highlight2.Start = this._playerKillTimes[0];
				highlight2.End = this._playerKillTimes[this._playerKillTimes.Count - 1];
				this.SaveHighlight(highlight2);
				this._isKillingSpreeHappening = false;
				this._playerKillTimes.Clear();
			}
			this.TickHighlightsToBeSaved();
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x00081F90 File Offset: 0x00080190
		protected override void OnEndMission()
		{
			base.OnEndMission();
			foreach (string text in this._highlightGroupIds)
			{
				Highlights.CloseGroup(text, false);
			}
			this._highlightSaveQueue = null;
			this._lastSavedHighlightData = null;
			this._playerKillTimes = null;
			this._archerSalvoKillTimes = null;
			this._cavalryChargeHitTimes = null;
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x0008200C File Offset: 0x0008020C
		public static void AddHighlightType(HighlightsController.HighlightType highlightType)
		{
			if (!HighlightsController.HighlightTypes.Any<HighlightsController.HighlightType>((HighlightsController.HighlightType h) => h.Id == highlightType.Id))
			{
				if (HighlightsController.IsHighlightsInitialized)
				{
					Highlights.AddHighlight(highlightType.Id, highlightType.Description);
				}
				HighlightsController.HighlightTypes.Add(highlightType);
			}
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x00082070 File Offset: 0x00080270
		public void SaveHighlight(HighlightsController.Highlight highlight)
		{
			this._highlightSaveQueue.Add(highlight);
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x0008207E File Offset: 0x0008027E
		public void SaveHighlight(HighlightsController.Highlight highlight, Vec3 position)
		{
			if (this.CanSaveHighlight(highlight.HighlightType, position))
			{
				this._highlightSaveQueue.Add(highlight);
			}
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x0008209C File Offset: 0x0008029C
		public bool CanSaveHighlight(HighlightsController.HighlightType highlightType, Vec3 position)
		{
			return highlightType.MaxHighlightDistance >= Mission.Current.Scene.LastFinalRenderCameraFrame.origin.Distance(position) && highlightType.MinVisibilityScore <= this.GetPlayerIsLookingAtPositionScore(position) && (!highlightType.IsVisibilityRequired || this.CanSeePosition(position));
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000820F4 File Offset: 0x000802F4
		public float GetPlayerIsLookingAtPositionScore(Vec3 position)
		{
			Vec3 vec = -Mission.Current.Scene.LastFinalRenderCameraFrame.rotation.u;
			Vec3 origin = Mission.Current.Scene.LastFinalRenderCameraFrame.origin;
			return MathF.Max(Vec3.DotProduct(vec.NormalizedCopy(), (position - origin).NormalizedCopy()), 0f);
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x0008215C File Offset: 0x0008035C
		public bool CanSeePosition(Vec3 position)
		{
			Vec3 origin = Mission.Current.Scene.LastFinalRenderCameraFrame.origin;
			float num;
			return !Mission.Current.Scene.RayCastForClosestEntityOrTerrain(origin, position, out num, 0.01f, BodyFlags.CameraCollisionRayCastExludeFlags) || MathF.Abs(position.Distance(origin) - num) < 0.1f;
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000821B5 File Offset: 0x000803B5
		public void ShowSummary()
		{
			if (this.IsAnyHighlightSaved)
			{
				Highlights.OpenSummary(this._savedHighlightGroups);
			}
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000821CC File Offset: 0x000803CC
		private void TickHighlightsToBeSaved()
		{
			if (this._highlightSaveQueue != null)
			{
				if (this._lastSavedHighlightData != null && this._highlightSaveQueue.Count > 0)
				{
					float item = this._lastSavedHighlightData.Item1;
					float item2 = this._lastSavedHighlightData.Item2;
					float num = item2 - (item2 - item) * 0.5f;
					for (int i = 0; i < this._highlightSaveQueue.Count; i++)
					{
						float start = this._highlightSaveQueue[i].Start;
						HighlightsController.Highlight highlight = this._highlightSaveQueue[i];
						if (start - (float)highlight.HighlightType.StartDelta < num)
						{
							this._highlightSaveQueue.Remove(this._highlightSaveQueue[i]);
							i--;
						}
					}
				}
				if (this._highlightSaveQueue.Count > 0)
				{
					float start2 = this._highlightSaveQueue[0].Start;
					HighlightsController.Highlight highlight = this._highlightSaveQueue[0];
					float num2 = start2 + (float)(highlight.HighlightType.StartDelta / 1000);
					float end = this._highlightSaveQueue[0].End;
					highlight = this._highlightSaveQueue[0];
					float num3 = end + (float)(highlight.HighlightType.EndDelta / 1000);
					for (int j = 1; j < this._highlightSaveQueue.Count; j++)
					{
						float start3 = this._highlightSaveQueue[j].Start;
						highlight = this._highlightSaveQueue[j];
						float num4 = start3 + (float)(highlight.HighlightType.StartDelta / 1000);
						float end2 = this._highlightSaveQueue[j].End;
						highlight = this._highlightSaveQueue[j];
						float num5 = end2 + (float)(highlight.HighlightType.EndDelta / 1000);
						if (num4 < num2)
						{
							num2 = num4;
						}
						if (num5 > num3)
						{
							num3 = num5;
						}
					}
					highlight = this._highlightSaveQueue[0];
					string id = highlight.HighlightType.Id;
					highlight = this._highlightSaveQueue[0];
					this.SaveVideo(id, highlight.HighlightType.GroupId, (int)(num2 - Mission.Current.CurrentTime) * 1000, (int)(num3 - Mission.Current.CurrentTime) * 1000);
					this._lastSavedHighlightData = new Tuple<float, float>(num2, num3);
					this._highlightSaveQueue.Clear();
				}
			}
		}

		// Token: 0x04000DCC RID: 3532
		private bool _isKillingSpreeHappening;

		// Token: 0x04000DCD RID: 3533
		private List<float> _playerKillTimes;

		// Token: 0x04000DCE RID: 3534
		private const int MinKillingSpreeKills = 4;

		// Token: 0x04000DCF RID: 3535
		private const float MaxKillingSpreeDuration = 10f;

		// Token: 0x04000DD0 RID: 3536
		private const float HighShotDifficultyThreshold = 7.5f;

		// Token: 0x04000DD1 RID: 3537
		private bool _isArcherSalvoHappening;

		// Token: 0x04000DD2 RID: 3538
		private List<float> _archerSalvoKillTimes;

		// Token: 0x04000DD3 RID: 3539
		private const int MinArcherSalvoKills = 5;

		// Token: 0x04000DD4 RID: 3540
		private const float MaxArcherSalvoDuration = 4f;

		// Token: 0x04000DD5 RID: 3541
		private bool _isFirstImpact = true;

		// Token: 0x04000DD6 RID: 3542
		private List<float> _cavalryChargeHitTimes;

		// Token: 0x04000DD7 RID: 3543
		private const float CavalryChargeImpactTimeFrame = 3f;

		// Token: 0x04000DD8 RID: 3544
		private const int MinCavalryChargeHits = 5;

		// Token: 0x04000DD9 RID: 3545
		private Tuple<float, float> _lastSavedHighlightData;

		// Token: 0x04000DDA RID: 3546
		private List<HighlightsController.Highlight> _highlightSaveQueue;

		// Token: 0x04000DDB RID: 3547
		private const float IgnoreIfOverlapsLastVideoPercent = 0.5f;

		// Token: 0x04000DDC RID: 3548
		private List<string> _savedHighlightGroups;

		// Token: 0x04000DDD RID: 3549
		private List<string> _highlightGroupIds = new List<string> { "grpid_incidents", "grpid_achievements" };

		// Token: 0x0200055B RID: 1371
		public struct HighlightType
		{
			// Token: 0x17000A5E RID: 2654
			// (get) Token: 0x06003CE6 RID: 15590 RVA: 0x000F25DB File Offset: 0x000F07DB
			// (set) Token: 0x06003CE7 RID: 15591 RVA: 0x000F25E3 File Offset: 0x000F07E3
			public string Id { get; private set; }

			// Token: 0x17000A5F RID: 2655
			// (get) Token: 0x06003CE8 RID: 15592 RVA: 0x000F25EC File Offset: 0x000F07EC
			// (set) Token: 0x06003CE9 RID: 15593 RVA: 0x000F25F4 File Offset: 0x000F07F4
			public string Description { get; private set; }

			// Token: 0x17000A60 RID: 2656
			// (get) Token: 0x06003CEA RID: 15594 RVA: 0x000F25FD File Offset: 0x000F07FD
			// (set) Token: 0x06003CEB RID: 15595 RVA: 0x000F2605 File Offset: 0x000F0805
			public string GroupId { get; private set; }

			// Token: 0x17000A61 RID: 2657
			// (get) Token: 0x06003CEC RID: 15596 RVA: 0x000F260E File Offset: 0x000F080E
			// (set) Token: 0x06003CED RID: 15597 RVA: 0x000F2616 File Offset: 0x000F0816
			public int StartDelta { get; private set; }

			// Token: 0x17000A62 RID: 2658
			// (get) Token: 0x06003CEE RID: 15598 RVA: 0x000F261F File Offset: 0x000F081F
			// (set) Token: 0x06003CEF RID: 15599 RVA: 0x000F2627 File Offset: 0x000F0827
			public int EndDelta { get; private set; }

			// Token: 0x17000A63 RID: 2659
			// (get) Token: 0x06003CF0 RID: 15600 RVA: 0x000F2630 File Offset: 0x000F0830
			// (set) Token: 0x06003CF1 RID: 15601 RVA: 0x000F2638 File Offset: 0x000F0838
			public float MinVisibilityScore { get; private set; }

			// Token: 0x17000A64 RID: 2660
			// (get) Token: 0x06003CF2 RID: 15602 RVA: 0x000F2641 File Offset: 0x000F0841
			// (set) Token: 0x06003CF3 RID: 15603 RVA: 0x000F2649 File Offset: 0x000F0849
			public float MaxHighlightDistance { get; private set; }

			// Token: 0x17000A65 RID: 2661
			// (get) Token: 0x06003CF4 RID: 15604 RVA: 0x000F2652 File Offset: 0x000F0852
			// (set) Token: 0x06003CF5 RID: 15605 RVA: 0x000F265A File Offset: 0x000F085A
			public bool IsVisibilityRequired { get; private set; }

			// Token: 0x06003CF6 RID: 15606 RVA: 0x000F2663 File Offset: 0x000F0863
			public HighlightType(string id, string description, string groupId, int startDelta, int endDelta, float minVisibilityScore, float maxHighlightDistance, bool isVisibilityRequired)
			{
				this.Id = id;
				this.Description = description;
				this.GroupId = groupId;
				this.StartDelta = startDelta;
				this.EndDelta = endDelta;
				this.MinVisibilityScore = minVisibilityScore;
				this.MaxHighlightDistance = maxHighlightDistance;
				this.IsVisibilityRequired = isVisibilityRequired;
			}
		}

		// Token: 0x0200055C RID: 1372
		public struct Highlight
		{
			// Token: 0x04001DFF RID: 7679
			public HighlightsController.HighlightType HighlightType;

			// Token: 0x04001E00 RID: 7680
			public float Start;

			// Token: 0x04001E01 RID: 7681
			public float End;
		}
	}
}
