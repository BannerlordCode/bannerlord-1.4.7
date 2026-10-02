using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Source.Objects.Siege
{
	// Token: 0x020003CD RID: 973
	public class AgentPathNavMeshChecker
	{
		// Token: 0x06003635 RID: 13877 RVA: 0x000DFEC4 File Offset: 0x000DE0C4
		public AgentPathNavMeshChecker(Mission mission, MatrixFrame pathFrameToCheck, float radiusToCheck, int navMeshId, BattleSideEnum teamToCollect, AgentPathNavMeshChecker.Direction directionToCollect, float maxDistanceCheck, float agentMoveTime)
		{
			this._mission = mission;
			this._pathFrameToCheck = pathFrameToCheck;
			this._radiusToCheck = radiusToCheck;
			this._navMeshId = navMeshId;
			this._teamToCollect = teamToCollect;
			this._directionToCollect = directionToCollect;
			this._maxDistanceCheck = maxDistanceCheck;
			this._agentMoveTime = agentMoveTime;
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x000DFF20 File Offset: 0x000DE120
		public void Tick(float dt)
		{
			float currentTime = this._mission.CurrentTime;
			if (this._tickOccasionallyTimer == null || this._tickOccasionallyTimer.Check(currentTime))
			{
				float num = dt;
				if (this._tickOccasionallyTimer != null)
				{
					num = this._tickOccasionallyTimer.ElapsedTime();
				}
				this._tickOccasionallyTimer = new Timer(currentTime, 0.1f + MBRandom.RandomFloat * 0.1f, true);
				this.TickOccasionally(num);
			}
			bool flag = false;
			foreach (Agent agent in this._nearbyAgents)
			{
				Vec3 position = agent.Position;
				if ((this._teamToCollect == BattleSideEnum.None || (agent.Team != null && agent.Team.Side == this._teamToCollect)) && agent.IsAIControlled)
				{
					if (agent.GetCurrentNavigationFaceId() == this._navMeshId)
					{
						flag = true;
						break;
					}
					if (this._isBeingUsed && position.DistanceSquared(this._pathFrameToCheck.origin) < this._radiusToCheck * this._radiusToCheck)
					{
						flag = true;
						break;
					}
					if (agent.MovementVelocity.LengthSquared > 0.01f)
					{
						Vec2 vec;
						if (this._directionToCollect == AgentPathNavMeshChecker.Direction.ForwardOnly)
						{
							vec = this._pathFrameToCheck.rotation.f.AsVec2;
						}
						else if (this._directionToCollect == AgentPathNavMeshChecker.Direction.BackwardOnly)
						{
							vec = -this._pathFrameToCheck.rotation.f.AsVec2;
						}
						else
						{
							vec = Vec2.Zero;
						}
						if (agent.HasPathThroughNavigationFaceIdFromDirection(this._navMeshId, vec))
						{
							float num2 = agent.GetPathDistanceToPoint(ref this._pathFrameToCheck.origin);
							if (num2 >= 100000f)
							{
								num2 = agent.Position.Distance(this._pathFrameToCheck.origin);
							}
							if (num2 < this._radiusToCheck * 2f || num2 / agent.GetMaximumForwardUnlimitedSpeed() < this._agentMoveTime)
							{
								flag = true;
							}
						}
					}
				}
			}
			if (flag)
			{
				this._isBeingUsed = true;
				this._setBeingUsedToFalseTimer = null;
			}
			else if (this._setBeingUsedToFalseTimer == null)
			{
				this._setBeingUsedToFalseTimer = new Timer(currentTime, 1f, true);
			}
			if (this._setBeingUsedToFalseTimer != null && this._setBeingUsedToFalseTimer.Check(currentTime))
			{
				this._setBeingUsedToFalseTimer = null;
				this._isBeingUsed = false;
			}
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x000E0190 File Offset: 0x000DE390
		public void TickOccasionally(float dt)
		{
			this._nearbyAgents = this._mission.GetNearbyAgents(this._pathFrameToCheck.origin.AsVec2, this._maxDistanceCheck, this._nearbyAgents);
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x000E01BF File Offset: 0x000DE3BF
		public bool HasAgentsUsingPath()
		{
			return this._isBeingUsed;
		}

		// Token: 0x04001744 RID: 5956
		private BattleSideEnum _teamToCollect;

		// Token: 0x04001745 RID: 5957
		private AgentPathNavMeshChecker.Direction _directionToCollect;

		// Token: 0x04001746 RID: 5958
		private MatrixFrame _pathFrameToCheck;

		// Token: 0x04001747 RID: 5959
		private float _radiusToCheck;

		// Token: 0x04001748 RID: 5960
		private Mission _mission;

		// Token: 0x04001749 RID: 5961
		private int _navMeshId;

		// Token: 0x0400174A RID: 5962
		private Timer _tickOccasionallyTimer;

		// Token: 0x0400174B RID: 5963
		private MBList<Agent> _nearbyAgents = new MBList<Agent>();

		// Token: 0x0400174C RID: 5964
		private bool _isBeingUsed;

		// Token: 0x0400174D RID: 5965
		private Timer _setBeingUsedToFalseTimer;

		// Token: 0x0400174E RID: 5966
		private float _maxDistanceCheck;

		// Token: 0x0400174F RID: 5967
		private float _agentMoveTime;

		// Token: 0x0200068C RID: 1676
		public enum Direction
		{
			// Token: 0x040022A7 RID: 8871
			ForwardOnly,
			// Token: 0x040022A8 RID: 8872
			BackwardOnly,
			// Token: 0x040022A9 RID: 8873
			BothDirections
		}
	}
}
