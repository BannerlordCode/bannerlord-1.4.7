using System;
using System.Diagnostics;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200014D RID: 333
	public class MovementPath
	{
		// Token: 0x170003CE RID: 974
		// (get) Token: 0x0600114B RID: 4427 RVA: 0x00032131 File Offset: 0x00030331
		private int LineCount
		{
			get
			{
				return this._navigationData.PointSize - 1;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x00032140 File Offset: 0x00030340
		public Vec2 InitialDirection { get; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x00032148 File Offset: 0x00030348
		public Vec2 FinalDirection { get; }

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x00032150 File Offset: 0x00030350
		public Vec3 Destination
		{
			get
			{
				return this._navigationData.EndPoint;
			}
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0003215D File Offset: 0x0003035D
		public MovementPath(NavigationData navigationData, Vec2 initialDirection, Vec2 finalDirection)
		{
			this._navigationData = navigationData;
			this.InitialDirection = initialDirection;
			this.FinalDirection = finalDirection;
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0003217A File Offset: 0x0003037A
		public MovementPath(Vec3 currentPosition, Vec3 orderPosition, float agentRadius, Vec2 previousDirection, Vec2 finalDirection)
			: this(new NavigationData(currentPosition, orderPosition, agentRadius), previousDirection, finalDirection)
		{
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00032190 File Offset: 0x00030390
		private void UpdateLineLengths()
		{
			if (this._lineLengthAccumulations == null)
			{
				this._lineLengthAccumulations = new float[this.LineCount];
				for (int i = 0; i < this.LineCount; i++)
				{
					this._lineLengthAccumulations[i] = (this._navigationData.Points[i + 1] - this._navigationData.Points[i]).Length;
					if (i > 0)
					{
						this._lineLengthAccumulations[i] += this._lineLengthAccumulations[i - 1];
					}
				}
			}
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00032220 File Offset: 0x00030420
		private float GetPathProggress(Vec2 point, int lineIndex)
		{
			this.UpdateLineLengths();
			float num = this._lineLengthAccumulations[this.LineCount - 1];
			if (num == 0f)
			{
				return 1f;
			}
			return (((lineIndex > 0) ? this._lineLengthAccumulations[lineIndex - 1] : 0f) + (point - this._navigationData.Points[lineIndex]).Length) / num;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00032288 File Offset: 0x00030488
		private void GetClosestPointTo(Vec2 point, out Vec2 closest, out int lineIndex)
		{
			closest = Vec2.Invalid;
			lineIndex = -1;
			float num = float.MaxValue;
			for (int i = 0; i < this.LineCount; i++)
			{
				Vec2 closestPointOnLineSegmentToPoint = MBMath.GetClosestPointOnLineSegmentToPoint(in this._navigationData.Points[i], in this._navigationData.Points[i + 1], in point);
				float num2 = closestPointOnLineSegmentToPoint.DistanceSquared(point);
				if (num2 < num)
				{
					num = num2;
					closest = closestPointOnLineSegmentToPoint;
					lineIndex = i;
				}
			}
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00032300 File Offset: 0x00030500
		[Conditional("DEBUG")]
		public void TickDebug(Vec2 position)
		{
			Vec2 vec;
			int num;
			this.GetClosestPointTo(position, out vec, out num);
			float pathProggress = this.GetPathProggress(vec, num);
			Vec2.Slerp(this.InitialDirection, this.FinalDirection, pathProggress).Normalize();
		}

		// Token: 0x04000400 RID: 1024
		private float[] _lineLengthAccumulations;

		// Token: 0x04000401 RID: 1025
		private NavigationData _navigationData;
	}
}
