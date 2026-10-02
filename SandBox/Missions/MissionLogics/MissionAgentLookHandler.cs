using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Conversation;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000074 RID: 116
	public class MissionAgentLookHandler : MissionLogic
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x0001DBBC File Offset: 0x0001BDBC
		public MissionAgentLookHandler()
		{
			this._staticPointList = new List<MissionAgentLookHandler.PointOfInterest>();
			this._checklist = new List<MissionAgentLookHandler.LookInfo>();
			this._selectionDelegate = new MissionAgentLookHandler.SelectionDelegate(this.SelectRandomAccordingToScore);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001DBEC File Offset: 0x0001BDEC
		public override void AfterStart()
		{
			this.AddStablePointsOfInterest();
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0001DBF4 File Offset: 0x0001BDF4
		private void AddStablePointsOfInterest()
		{
			foreach (GameEntity gameEntity in base.Mission.Scene.FindEntitiesWithTag("point_of_interest"))
			{
				this._staticPointList.Add(new MissionAgentLookHandler.PointOfInterest(gameEntity.GetGlobalFrame()));
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001DC60 File Offset: 0x0001BE60
		private void DebugTick()
		{
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001DC64 File Offset: 0x0001BE64
		public override void OnMissionTick(float dt)
		{
			if (Game.Current.IsDevelopmentMode)
			{
				this.DebugTick();
			}
			float currentTime = base.Mission.CurrentTime;
			foreach (MissionAgentLookHandler.LookInfo lookInfo in this._checklist)
			{
				if (lookInfo.Agent.IsActive() && !ConversationMission.ConversationAgents.Contains(lookInfo.Agent) && (!ConversationMission.ConversationAgents.Any<Agent>() || !lookInfo.Agent.IsPlayerControlled))
				{
					if (lookInfo.CheckTimer.Check(currentTime))
					{
						MissionAgentLookHandler.PointOfInterest pointOfInterest = this._selectionDelegate(lookInfo.Agent);
						if (pointOfInterest != null)
						{
							lookInfo.Reset(pointOfInterest, 5f);
						}
						else
						{
							lookInfo.Reset(null, 1f + MBRandom.RandomFloat);
						}
					}
					else if (lookInfo.PointOfInterest != null && (!lookInfo.PointOfInterest.IsActive || !lookInfo.PointOfInterest.IsVisibleFor(lookInfo.Agent)))
					{
						MissionAgentLookHandler.PointOfInterest pointOfInterest2 = this._selectionDelegate(lookInfo.Agent);
						if (pointOfInterest2 != null)
						{
							lookInfo.Reset(pointOfInterest2, 5f + MBRandom.RandomFloat);
						}
						else
						{
							lookInfo.Reset(null, MBRandom.RandomFloat * 5f + 5f);
						}
					}
					else if (lookInfo.PointOfInterest != null)
					{
						Vec3 targetPosition = lookInfo.PointOfInterest.GetTargetPosition();
						lookInfo.Agent.SetLookToPointOfInterest(targetPosition);
					}
				}
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001DDFC File Offset: 0x0001BFFC
		private MissionAgentLookHandler.PointOfInterest SelectFirstNonAgent(Agent agent)
		{
			if (agent.IsAIControlled)
			{
				int num = MBRandom.RandomInt(this._staticPointList.Count);
				int num2 = num;
				MissionAgentLookHandler.PointOfInterest pointOfInterest;
				for (;;)
				{
					pointOfInterest = this._staticPointList[num2];
					if (pointOfInterest.GetScore(agent) > 0f)
					{
						break;
					}
					num2 = ((num2 + 1 == this._staticPointList.Count) ? 0 : (num2 + 1));
					if (num2 == num)
					{
						goto IL_0053;
					}
				}
				return pointOfInterest;
			}
			IL_0053:
			return null;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001DE60 File Offset: 0x0001C060
		private MissionAgentLookHandler.PointOfInterest SelectBestOfLimitedNonAgent(Agent agent)
		{
			int num = 3;
			MissionAgentLookHandler.PointOfInterest pointOfInterest = null;
			float num2 = -1f;
			if (agent.IsAIControlled)
			{
				int num3 = MBRandom.RandomInt(this._staticPointList.Count);
				int num4 = num3;
				do
				{
					MissionAgentLookHandler.PointOfInterest pointOfInterest2 = this._staticPointList[num4];
					float score = pointOfInterest2.GetScore(agent);
					if (score > 0f)
					{
						if (score > num2)
						{
							num2 = score;
							pointOfInterest = pointOfInterest2;
						}
						num--;
					}
					num4 = ((num4 + 1 == this._staticPointList.Count) ? 0 : (num4 + 1));
				}
				while (num4 != num3 && num > 0);
			}
			return pointOfInterest;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001DEE8 File Offset: 0x0001C0E8
		private MissionAgentLookHandler.PointOfInterest SelectBest(Agent agent)
		{
			MissionAgentLookHandler.PointOfInterest pointOfInterest = null;
			float num = -1f;
			if (agent.IsAIControlled)
			{
				foreach (MissionAgentLookHandler.PointOfInterest pointOfInterest2 in this._staticPointList)
				{
					float score = pointOfInterest2.GetScore(agent);
					if (score > 0f && score > num)
					{
						num = score;
						pointOfInterest = pointOfInterest2;
					}
				}
				AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(base.Mission, agent.Position.AsVec2, 5f, false);
				while (proximityMapSearchStruct.LastFoundAgent != null)
				{
					MissionAgentLookHandler.PointOfInterest pointOfInterest3 = new MissionAgentLookHandler.PointOfInterest(proximityMapSearchStruct.LastFoundAgent);
					float score2 = pointOfInterest3.GetScore(agent);
					if (score2 > 0f && score2 > num)
					{
						num = score2;
						pointOfInterest = pointOfInterest3;
					}
					AgentProximityMap.FindNext(base.Mission, ref proximityMapSearchStruct);
				}
			}
			return pointOfInterest;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001DFCC File Offset: 0x0001C1CC
		private MissionAgentLookHandler.PointOfInterest SelectRandomAccordingToScore(Agent agent)
		{
			float num = 0f;
			List<KeyValuePair<float, MissionAgentLookHandler.PointOfInterest>> list = new List<KeyValuePair<float, MissionAgentLookHandler.PointOfInterest>>();
			if (agent.IsAIControlled)
			{
				foreach (MissionAgentLookHandler.PointOfInterest pointOfInterest in this._staticPointList)
				{
					float score = pointOfInterest.GetScore(agent);
					if (score > 0f)
					{
						list.Add(new KeyValuePair<float, MissionAgentLookHandler.PointOfInterest>(score, pointOfInterest));
						num += score;
					}
				}
				AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(Mission.Current, agent.Position.AsVec2, 5f, false);
				while (proximityMapSearchStruct.LastFoundAgent != null)
				{
					MissionAgentLookHandler.PointOfInterest pointOfInterest2 = new MissionAgentLookHandler.PointOfInterest(proximityMapSearchStruct.LastFoundAgent);
					float score2 = pointOfInterest2.GetScore(agent);
					if (score2 > 0f)
					{
						list.Add(new KeyValuePair<float, MissionAgentLookHandler.PointOfInterest>(score2, pointOfInterest2));
						num += score2;
					}
					AgentProximityMap.FindNext(Mission.Current, ref proximityMapSearchStruct);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			float num2 = MBRandom.RandomFloat * num;
			MissionAgentLookHandler.PointOfInterest pointOfInterest3 = list[list.Count - 1].Value;
			foreach (KeyValuePair<float, MissionAgentLookHandler.PointOfInterest> keyValuePair in list)
			{
				num2 -= keyValuePair.Key;
				if (num2 <= 0f)
				{
					pointOfInterest3 = keyValuePair.Value;
					break;
				}
			}
			return pointOfInterest3;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001E144 File Offset: 0x0001C344
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (agent.IsHuman)
			{
				this._checklist.Add(new MissionAgentLookHandler.LookInfo(agent, MBRandom.RandomFloat));
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001E164 File Offset: 0x0001C364
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			for (int i = 0; i < this._checklist.Count; i++)
			{
				MissionAgentLookHandler.LookInfo lookInfo = this._checklist[i];
				if (lookInfo.Agent == affectedAgent)
				{
					this._checklist.RemoveAt(i);
					i--;
				}
				else if (lookInfo.PointOfInterest != null && lookInfo.PointOfInterest.IsRelevant(affectedAgent))
				{
					lookInfo.Reset(null, MBRandom.RandomFloat * 2f + 2f);
				}
			}
		}

		// Token: 0x04000279 RID: 633
		private readonly List<MissionAgentLookHandler.PointOfInterest> _staticPointList;

		// Token: 0x0400027A RID: 634
		private readonly List<MissionAgentLookHandler.LookInfo> _checklist;

		// Token: 0x0400027B RID: 635
		private MissionAgentLookHandler.SelectionDelegate _selectionDelegate;

		// Token: 0x0200016B RID: 363
		private class PointOfInterest
		{
			// Token: 0x17000136 RID: 310
			// (get) Token: 0x06000E61 RID: 3681 RVA: 0x0006524B File Offset: 0x0006344B
			public bool IsActive
			{
				get
				{
					return this._agent == null || this._agent.IsActive();
				}
			}

			// Token: 0x06000E62 RID: 3682 RVA: 0x00065264 File Offset: 0x00063464
			public PointOfInterest(Agent agent)
			{
				this._agent = agent;
				this._selectDistance = 5;
				this._releaseDistanceSquare = 36;
				this._ignoreDirection = false;
				CharacterObject characterObject = (CharacterObject)agent.Character;
				if (!agent.IsHuman)
				{
					this._priority = 1;
					return;
				}
				if (characterObject.IsHero)
				{
					this._priority = 5;
					return;
				}
				if (characterObject.Occupation == Occupation.HorseTrader || characterObject.Occupation == Occupation.Weaponsmith || characterObject.Occupation == Occupation.GoodsTrader || characterObject.Occupation == Occupation.Armorer || characterObject.Occupation == Occupation.Blacksmith)
				{
					this._priority = 3;
					return;
				}
				this._priority = 1;
			}

			// Token: 0x06000E63 RID: 3683 RVA: 0x00065300 File Offset: 0x00063500
			public PointOfInterest(MatrixFrame frame)
			{
				this._frame = frame;
				this._selectDistance = 4;
				this._releaseDistanceSquare = 25;
				this._ignoreDirection = true;
				this._priority = 2;
			}

			// Token: 0x06000E64 RID: 3684 RVA: 0x0006532C File Offset: 0x0006352C
			public float GetScore(Agent agent)
			{
				if (agent == this._agent || this.GetBasicPosition().DistanceSquared(agent.Position) > (float)(this._selectDistance * this._selectDistance))
				{
					return -1f;
				}
				Vec3 vec = this.GetTargetPosition() - agent.GetEyeGlobalPosition();
				float num = vec.Normalize();
				if (Vec2.DotProduct(vec.AsVec2, agent.GetMovementDirection()) < 0.7f)
				{
					return -1f;
				}
				float num2 = (float)(this._priority * this._selectDistance) / num;
				if (this.IsMoving())
				{
					num2 *= 5f;
				}
				if (!this._ignoreDirection)
				{
					MatrixFrame matrixFrame = this.GetTargetFrame();
					Vec2 asVec = matrixFrame.rotation.f.AsVec2;
					matrixFrame = agent.Frame;
					float num3 = Vec2.DotProduct(asVec, matrixFrame.rotation.f.AsVec2);
					if (num3 < -0.7f)
					{
						num2 *= 2f;
					}
					else if (MathF.Abs(num3) < 0.1f)
					{
						num2 *= 2f;
					}
				}
				return num2;
			}

			// Token: 0x06000E65 RID: 3685 RVA: 0x00065431 File Offset: 0x00063631
			public Vec3 GetTargetPosition()
			{
				Agent agent = this._agent;
				if (agent == null)
				{
					return this._frame.origin;
				}
				return agent.GetEyeGlobalPosition();
			}

			// Token: 0x06000E66 RID: 3686 RVA: 0x0006544E File Offset: 0x0006364E
			public Vec3 GetBasicPosition()
			{
				if (this._agent == null)
				{
					return this._frame.origin;
				}
				return this._agent.Position;
			}

			// Token: 0x06000E67 RID: 3687 RVA: 0x00065470 File Offset: 0x00063670
			private bool IsMoving()
			{
				return this._agent == null || this._agent.GetCurrentVelocity().LengthSquared > 0.040000003f;
			}

			// Token: 0x06000E68 RID: 3688 RVA: 0x000654A1 File Offset: 0x000636A1
			private MatrixFrame GetTargetFrame()
			{
				if (this._agent == null)
				{
					return this._frame;
				}
				return this._agent.Frame;
			}

			// Token: 0x06000E69 RID: 3689 RVA: 0x000654C0 File Offset: 0x000636C0
			public bool IsVisibleFor(Agent agent)
			{
				Vec3 basicPosition = this.GetBasicPosition();
				Vec3 position = agent.Position;
				if (agent == this._agent || position.DistanceSquared(basicPosition) > (float)this._releaseDistanceSquare)
				{
					return false;
				}
				Vec3 vec = basicPosition - position;
				vec.Normalize();
				return Vec2.DotProduct(vec.AsVec2, agent.GetMovementDirection()) > 0.4f;
			}

			// Token: 0x06000E6A RID: 3690 RVA: 0x00065520 File Offset: 0x00063720
			public bool IsRelevant(Agent agent)
			{
				return agent == this._agent;
			}

			// Token: 0x0400070A RID: 1802
			public const int MaxSelectDistanceForAgent = 5;

			// Token: 0x0400070B RID: 1803
			public const int MaxSelectDistanceForFrame = 4;

			// Token: 0x0400070C RID: 1804
			private readonly int _selectDistance;

			// Token: 0x0400070D RID: 1805
			private readonly int _releaseDistanceSquare;

			// Token: 0x0400070E RID: 1806
			private readonly Agent _agent;

			// Token: 0x0400070F RID: 1807
			private readonly MatrixFrame _frame;

			// Token: 0x04000710 RID: 1808
			private readonly bool _ignoreDirection;

			// Token: 0x04000711 RID: 1809
			private readonly int _priority;
		}

		// Token: 0x0200016C RID: 364
		private class LookInfo
		{
			// Token: 0x06000E6B RID: 3691 RVA: 0x0006552B File Offset: 0x0006372B
			public LookInfo(Agent agent, float checkTime)
			{
				this.Agent = agent;
				this.CheckTimer = new Timer(Mission.Current.CurrentTime, checkTime, true);
			}

			// Token: 0x06000E6C RID: 3692 RVA: 0x00065554 File Offset: 0x00063754
			public void Reset(MissionAgentLookHandler.PointOfInterest pointOfInterest, float duration)
			{
				if (this.PointOfInterest != pointOfInterest)
				{
					this.PointOfInterest = pointOfInterest;
					if (this.PointOfInterest != null)
					{
						this.Agent.SetLookToPointOfInterest(this.PointOfInterest.GetTargetPosition());
					}
					else if (this.Agent.IsActive())
					{
						this.Agent.DisableLookToPointOfInterest();
					}
				}
				this.CheckTimer.Reset(Mission.Current.CurrentTime, duration);
			}

			// Token: 0x04000712 RID: 1810
			public readonly Agent Agent;

			// Token: 0x04000713 RID: 1811
			public MissionAgentLookHandler.PointOfInterest PointOfInterest;

			// Token: 0x04000714 RID: 1812
			public readonly Timer CheckTimer;
		}

		// Token: 0x0200016D RID: 365
		// (Invoke) Token: 0x06000E6E RID: 3694
		private delegate MissionAgentLookHandler.PointOfInterest SelectionDelegate(Agent agent);
	}
}
