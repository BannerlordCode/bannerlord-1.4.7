using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200017B RID: 379
	public class SiegeQuerySystem
	{
		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x00049B71 File Offset: 0x00047D71
		public int LeftRegionMemberCount
		{
			get
			{
				return this._leftRegionMemberCount.Value;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x00049B7E File Offset: 0x00047D7E
		public int LeftCloseAttackerCount
		{
			get
			{
				return this._leftCloseAttackerCount.Value;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x00049B8B File Offset: 0x00047D8B
		public int MiddleRegionMemberCount
		{
			get
			{
				return this._middleRegionMemberCount.Value;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x00049B98 File Offset: 0x00047D98
		public int MiddleCloseAttackerCount
		{
			get
			{
				return this._middleCloseAttackerCount.Value;
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001402 RID: 5122 RVA: 0x00049BA5 File Offset: 0x00047DA5
		public int RightRegionMemberCount
		{
			get
			{
				return this._rightRegionMemberCount.Value;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x00049BB2 File Offset: 0x00047DB2
		public int RightCloseAttackerCount
		{
			get
			{
				return this._rightCloseAttackerCount.Value;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001404 RID: 5124 RVA: 0x00049BBF File Offset: 0x00047DBF
		public int InsideAttackerCount
		{
			get
			{
				return this._insideAttackerCount.Value;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x00049BCC File Offset: 0x00047DCC
		public int LeftDefenderCount
		{
			get
			{
				return this._leftDefenderCount.Value;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x00049BD9 File Offset: 0x00047DD9
		public int MiddleDefenderCount
		{
			get
			{
				return this._middleDefenderCount.Value;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x00049BE6 File Offset: 0x00047DE6
		public int RightDefenderCount
		{
			get
			{
				return this._rightDefenderCount.Value;
			}
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x00049BF4 File Offset: 0x00047DF4
		public SiegeQuerySystem(Team team, IEnumerable<SiegeLane> lanes)
		{
			Mission mission = Mission.Current;
			this._attackerTeam = mission.AttackerTeam;
			Team defenderTeam = mission.DefenderTeam;
			SiegeLane siegeLane = lanes.FirstOrDefault<SiegeLane>((SiegeLane l) => l.LaneSide == FormationAI.BehaviorSide.Left);
			SiegeLane siegeLane2 = lanes.FirstOrDefault<SiegeLane>((SiegeLane l) => l.LaneSide == FormationAI.BehaviorSide.Middle);
			lanes.FirstOrDefault<SiegeLane>((SiegeLane l) => l.LaneSide == FormationAI.BehaviorSide.Right);
			Mission mission2 = Mission.Current;
			WeakGameEntity weakGameEntity = mission2.Scene.FindWeakEntityWithTag("left_defender_origin");
			if (weakGameEntity.IsValid)
			{
				this.LeftDefenderOrigin = weakGameEntity.GlobalPosition;
			}
			else
			{
				this.LeftDefenderOrigin = (siegeLane.DefenderOrigin.AsVec2.IsNonZero() ? siegeLane.DefenderOrigin.GetGroundVec3() : Vec3.Zero);
			}
			WeakGameEntity weakGameEntity2 = mission2.Scene.FindWeakEntityWithTag("left_attacker_origin");
			if (weakGameEntity2.IsValid)
			{
				this.LeftAttackerOrigin = weakGameEntity2.GlobalPosition;
			}
			else
			{
				this.LeftAttackerOrigin = (siegeLane.AttackerOrigin.AsVec2.IsNonZero() ? siegeLane.AttackerOrigin.GetGroundVec3() : Vec3.Zero);
			}
			WeakGameEntity weakGameEntity3 = mission2.Scene.FindWeakEntityWithTag("middle_defender_origin");
			if (weakGameEntity3.IsValid)
			{
				this.MidDefenderOrigin = weakGameEntity3.GlobalPosition;
			}
			else
			{
				this.MidDefenderOrigin = (siegeLane2.DefenderOrigin.AsVec2.IsNonZero() ? siegeLane2.DefenderOrigin.GetGroundVec3() : Vec3.Zero);
			}
			WeakGameEntity weakGameEntity4 = mission2.Scene.FindWeakEntityWithTag("middle_attacker_origin");
			if (weakGameEntity4.IsValid)
			{
				this.MiddleAttackerOrigin = weakGameEntity4.GlobalPosition;
			}
			else
			{
				this.MiddleAttackerOrigin = (siegeLane2.AttackerOrigin.AsVec2.IsNonZero() ? siegeLane2.AttackerOrigin.GetGroundVec3() : Vec3.Zero);
			}
			WeakGameEntity weakGameEntity5 = mission2.Scene.FindWeakEntityWithTag("right_defender_origin");
			if (weakGameEntity5.IsValid)
			{
				this.RightDefenderOrigin = weakGameEntity5.GlobalPosition;
			}
			else
			{
				this.RightDefenderOrigin = (siegeLane2.DefenderOrigin.AsVec2.IsNonZero() ? siegeLane2.DefenderOrigin.GetGroundVec3() : Vec3.Zero);
			}
			WeakGameEntity weakGameEntity6 = mission2.Scene.FindWeakEntityWithTag("right_attacker_origin");
			if (weakGameEntity6.IsValid)
			{
				this.RightAttackerOrigin = weakGameEntity6.GlobalPosition;
			}
			else
			{
				this.RightAttackerOrigin = (siegeLane2.AttackerOrigin.AsVec2.IsNonZero() ? siegeLane2.AttackerOrigin.GetGroundVec3() : Vec3.Zero);
			}
			this.LeftToMidDir = (this.MiddleAttackerOrigin.AsVec2 - this.LeftDefenderOrigin.AsVec2).Normalized();
			this.MidToLeftDir = (this.LeftAttackerOrigin.AsVec2 - this.MidDefenderOrigin.AsVec2).Normalized();
			this.MidToRightDir = (this.RightAttackerOrigin.AsVec2 - this.MidDefenderOrigin.AsVec2).Normalized();
			this.RightToMidDir = (this.MiddleAttackerOrigin.AsVec2 - this.RightDefenderOrigin.AsVec2).Normalized();
			this._leftRegionMemberCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.Left), 5f);
			this._leftCloseAttackerCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.LeftClose), 5f);
			this._middleRegionMemberCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.Middle), 5f);
			this._middleCloseAttackerCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.MiddleClose), 5f);
			this._rightRegionMemberCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.Right), 5f);
			this._rightCloseAttackerCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.RightClose), 5f);
			this._insideAttackerCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.Inside), 5f);
			this._leftDefenderCount = new QueryData<int>(() => mission.GetNearbyAllyAgentsCount(this.LeftDefenderOrigin.AsVec2, 10f, defenderTeam), 5f);
			this._middleDefenderCount = new QueryData<int>(() => mission.GetNearbyAllyAgentsCount(this.MidDefenderOrigin.AsVec2, 10f, defenderTeam), 5f);
			this._rightDefenderCount = new QueryData<int>(() => mission.GetNearbyAllyAgentsCount(this.RightDefenderOrigin.AsVec2, 10f, defenderTeam), 5f);
			this.DefenderLeftToDefenderMidDir = (this.MidDefenderOrigin.AsVec2 - this.LeftDefenderOrigin.AsVec2).Normalized();
			this.DefenderMidToDefenderRightDir = (this.RightDefenderOrigin.AsVec2 - this.MidDefenderOrigin.AsVec2).Normalized();
			this.InitializeTelemetryScopeNames();
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0004A140 File Offset: 0x00048340
		private int LocateAttackers(SiegeQuerySystem.RegionEnum region)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			foreach (Agent agent in this._attackerTeam.ActiveAgents)
			{
				Vec2 vec = agent.Position.AsVec2 - this.LeftDefenderOrigin.AsVec2;
				Vec2 vec2 = agent.Position.AsVec2 - this.MidDefenderOrigin.AsVec2;
				Vec2 vec3 = agent.Position.AsVec2 - this.RightDefenderOrigin.AsVec2;
				if (vec.Normalize() < 15f && Math.Abs(agent.Position.z - this.LeftDefenderOrigin.z) <= 3f)
				{
					num2++;
					num++;
				}
				else
				{
					if (vec.DotProduct(this.LeftToMidDir) >= 0f && vec.DotProduct(this.LeftToMidDir.RightVec()) >= 0f)
					{
						num++;
					}
					else if (vec2.DotProduct(this.MidToLeftDir) >= 0f && vec2.DotProduct(this.MidToLeftDir.RightVec()) >= 0f)
					{
						num++;
					}
					if (vec3.Normalize() < 15f && Math.Abs(agent.Position.z - this.RightDefenderOrigin.z) <= 3f)
					{
						num6++;
						num5++;
					}
					else
					{
						if (vec3.DotProduct(this.RightToMidDir) >= 0f && vec3.DotProduct(this.RightToMidDir.LeftVec()) >= 0f)
						{
							num5++;
						}
						else if (vec2.DotProduct(this.MidToRightDir) >= 0f && vec2.DotProduct(this.MidToRightDir.LeftVec()) >= 0f)
						{
							num5++;
						}
						if (vec2.Normalize() < 15f && Math.Abs(agent.Position.z - this.MidDefenderOrigin.z) <= 3f)
						{
							num4++;
							num3++;
						}
						else
						{
							if ((vec2.DotProduct(this.MidToLeftDir) < 0f || vec2.DotProduct(this.MidToLeftDir.RightVec()) < 0f || vec.DotProduct(this.LeftToMidDir) < 0f || vec.DotProduct(this.LeftToMidDir.RightVec()) < 0f) && (vec2.DotProduct(this.MidToRightDir) < 0f || vec2.DotProduct(this.MidToRightDir.LeftVec()) < 0f || vec3.DotProduct(this.RightToMidDir) < 0f || vec3.DotProduct(this.RightToMidDir.LeftVec()) < 0f))
							{
								num3++;
							}
							if (agent.GetCurrentNavigationFaceId() % 10 == 1)
							{
								num7++;
							}
						}
					}
				}
			}
			float currentTime = Mission.Current.CurrentTime;
			this._leftRegionMemberCount.SetValue(num, currentTime);
			this._leftCloseAttackerCount.SetValue(num2, currentTime);
			this._middleRegionMemberCount.SetValue(num3, currentTime);
			this._middleCloseAttackerCount.SetValue(num4, currentTime);
			this._rightRegionMemberCount.SetValue(num5, currentTime);
			this._rightCloseAttackerCount.SetValue(num6, currentTime);
			this._insideAttackerCount.SetValue(num7, currentTime);
			switch (region)
			{
			case SiegeQuerySystem.RegionEnum.Left:
				return num;
			case SiegeQuerySystem.RegionEnum.LeftClose:
				return num2;
			case SiegeQuerySystem.RegionEnum.Middle:
				return num3;
			case SiegeQuerySystem.RegionEnum.MiddleClose:
				return num4;
			case SiegeQuerySystem.RegionEnum.Right:
				return num5;
			case SiegeQuerySystem.RegionEnum.RightClose:
				return num6;
			case SiegeQuerySystem.RegionEnum.Inside:
				return num7;
			default:
				return 0;
			}
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0004A548 File Offset: 0x00048748
		public void Expire()
		{
			this._leftRegionMemberCount.Expire();
			this._leftCloseAttackerCount.Expire();
			this._middleRegionMemberCount.Expire();
			this._middleCloseAttackerCount.Expire();
			this._rightRegionMemberCount.Expire();
			this._rightCloseAttackerCount.Expire();
			this._insideAttackerCount.Expire();
			this._leftDefenderCount.Expire();
			this._middleDefenderCount.Expire();
			this._rightDefenderCount.Expire();
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0004A5C3 File Offset: 0x000487C3
		private void InitializeTelemetryScopeNames()
		{
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0004A5C8 File Offset: 0x000487C8
		public int DeterminePositionAssociatedSide(Vec3 position)
		{
			float num = position.AsVec2.DistanceSquared(this.LeftDefenderOrigin.AsVec2);
			float num2 = position.AsVec2.DistanceSquared(this.MidDefenderOrigin.AsVec2);
			float num3 = position.AsVec2.DistanceSquared(this.RightDefenderOrigin.AsVec2);
			FormationAI.BehaviorSide behaviorSide;
			if (num < num2 && num < num3)
			{
				behaviorSide = FormationAI.BehaviorSide.Left;
			}
			else if (num3 < num2)
			{
				behaviorSide = FormationAI.BehaviorSide.Right;
			}
			else
			{
				behaviorSide = FormationAI.BehaviorSide.Middle;
			}
			FormationAI.BehaviorSide behaviorSide2 = FormationAI.BehaviorSide.BehaviorSideNotSet;
			switch (behaviorSide)
			{
			case FormationAI.BehaviorSide.Left:
				if ((position.AsVec2 - this.LeftDefenderOrigin.AsVec2).Normalized().DotProduct(this.DefenderLeftToDefenderMidDir) > 0f)
				{
					behaviorSide2 = FormationAI.BehaviorSide.Middle;
				}
				break;
			case FormationAI.BehaviorSide.Middle:
				if ((position.AsVec2 - this.MidDefenderOrigin.AsVec2).Normalized().DotProduct(this.DefenderMidToDefenderRightDir) > 0f)
				{
					behaviorSide2 = FormationAI.BehaviorSide.Right;
				}
				else
				{
					behaviorSide2 = FormationAI.BehaviorSide.Left;
				}
				break;
			case FormationAI.BehaviorSide.Right:
				if ((position.AsVec2 - this.RightDefenderOrigin.AsVec2).Normalized().DotProduct(this.DefenderMidToDefenderRightDir) < 0f)
				{
					behaviorSide2 = FormationAI.BehaviorSide.Middle;
				}
				break;
			}
			int num4 = 1 << (int)behaviorSide;
			if (behaviorSide2 != FormationAI.BehaviorSide.BehaviorSideNotSet)
			{
				num4 |= 1 << (int)behaviorSide2;
			}
			return num4;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0004A746 File Offset: 0x00048946
		public static bool AreSidesRelated(FormationAI.BehaviorSide side, int connectedSides)
		{
			return ((1 << (int)side) & connectedSides) != 0;
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0004A754 File Offset: 0x00048954
		public static int SideDistance(int connectedSides, int side)
		{
			while (connectedSides != 0 && side != 0)
			{
				connectedSides >>= 1;
				side >>= 1;
			}
			int i = ((connectedSides != 0) ? connectedSides : side);
			int num = 0;
			while (i > 0)
			{
				num++;
				if ((i & 1) == 1)
				{
					break;
				}
				i >>= 1;
			}
			return num;
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x0004A792 File Offset: 0x00048992
		public Vec3 LeftDefenderOrigin { get; }

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x0004A79A File Offset: 0x0004899A
		public Vec3 MidDefenderOrigin { get; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001411 RID: 5137 RVA: 0x0004A7A2 File Offset: 0x000489A2
		public Vec3 RightDefenderOrigin { get; }

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x0004A7AA File Offset: 0x000489AA
		public Vec3 LeftAttackerOrigin { get; }

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x0004A7B2 File Offset: 0x000489B2
		public Vec3 MiddleAttackerOrigin { get; }

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x0004A7BA File Offset: 0x000489BA
		public Vec3 RightAttackerOrigin { get; }

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x0004A7C2 File Offset: 0x000489C2
		public Vec2 LeftToMidDir { get; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x0004A7CA File Offset: 0x000489CA
		public Vec2 MidToLeftDir { get; }

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x0004A7D2 File Offset: 0x000489D2
		public Vec2 MidToRightDir { get; }

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x0004A7DA File Offset: 0x000489DA
		public Vec2 RightToMidDir { get; }

		// Token: 0x04000531 RID: 1329
		private const float LaneProximityDistance = 15f;

		// Token: 0x04000532 RID: 1330
		private readonly Team _attackerTeam;

		// Token: 0x04000533 RID: 1331
		public Vec2 DefenderLeftToDefenderMidDir;

		// Token: 0x04000534 RID: 1332
		public Vec2 DefenderMidToDefenderRightDir;

		// Token: 0x04000535 RID: 1333
		private readonly QueryData<int> _leftRegionMemberCount;

		// Token: 0x04000536 RID: 1334
		private readonly QueryData<int> _leftCloseAttackerCount;

		// Token: 0x04000537 RID: 1335
		private readonly QueryData<int> _middleRegionMemberCount;

		// Token: 0x04000538 RID: 1336
		private readonly QueryData<int> _middleCloseAttackerCount;

		// Token: 0x04000539 RID: 1337
		private readonly QueryData<int> _rightRegionMemberCount;

		// Token: 0x0400053A RID: 1338
		private readonly QueryData<int> _rightCloseAttackerCount;

		// Token: 0x0400053B RID: 1339
		private readonly QueryData<int> _insideAttackerCount;

		// Token: 0x0400053C RID: 1340
		private readonly QueryData<int> _leftDefenderCount;

		// Token: 0x0400053D RID: 1341
		private readonly QueryData<int> _middleDefenderCount;

		// Token: 0x0400053E RID: 1342
		private readonly QueryData<int> _rightDefenderCount;

		// Token: 0x020004CC RID: 1228
		private enum RegionEnum
		{
			// Token: 0x04001BEF RID: 7151
			Left,
			// Token: 0x04001BF0 RID: 7152
			LeftClose,
			// Token: 0x04001BF1 RID: 7153
			Middle,
			// Token: 0x04001BF2 RID: 7154
			MiddleClose,
			// Token: 0x04001BF3 RID: 7155
			Right,
			// Token: 0x04001BF4 RID: 7156
			RightClose,
			// Token: 0x04001BF5 RID: 7157
			Inside
		}
	}
}
