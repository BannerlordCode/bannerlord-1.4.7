using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000098 RID: 152
	public struct LinearFrictionTerm
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0001D06D File Offset: 0x0001B26D
		public static LinearFrictionTerm Invalid
		{
			get
			{
				return new LinearFrictionTerm(0f, 0f, 0f, 0f, 0f, 0f);
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x0001D092 File Offset: 0x0001B292
		public static LinearFrictionTerm One
		{
			get
			{
				return new LinearFrictionTerm(1f, 1f, 1f, 1f, 1f, 1f);
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x0001D0B8 File Offset: 0x0001B2B8
		public bool IsValid
		{
			get
			{
				return this.Right > 0f && this.Left > 0f && this.Forward > 0f && this.Backward > 0f && this.Up > 0f && this.Down > 0f;
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0001D115 File Offset: 0x0001B315
		public LinearFrictionTerm(float right, float left, float forward, float backward, float up, float down)
		{
			this.Right = right;
			this.Left = left;
			this.Forward = forward;
			this.Backward = backward;
			this.Up = up;
			this.Down = down;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0001D144 File Offset: 0x0001B344
		public static LinearFrictionTerm operator /(LinearFrictionTerm o, float f)
		{
			return o * (1f / f);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0001D153 File Offset: 0x0001B353
		public static LinearFrictionTerm operator *(LinearFrictionTerm o, float f)
		{
			return new LinearFrictionTerm(o.Right * f, o.Left * f, o.Forward * f, o.Backward * f, o.Up * f, o.Down * f);
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0001D18C File Offset: 0x0001B38C
		public LinearFrictionTerm ElementWiseProduct(LinearFrictionTerm o)
		{
			return new LinearFrictionTerm(this.Right * o.Right, this.Left * o.Left, this.Forward * o.Forward, this.Backward * o.Backward, this.Up * o.Up, this.Down * o.Down);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0001D1EC File Offset: 0x0001B3EC
		public bool NearlyEquals(in LinearFrictionTerm o, float epsilon = 1E-05f)
		{
			return this.Right.ApproximatelyEqualsTo(o.Right, epsilon) && this.Left.ApproximatelyEqualsTo(o.Left, epsilon) && this.Forward.ApproximatelyEqualsTo(o.Forward, epsilon) && this.Backward.ApproximatelyEqualsTo(o.Backward, epsilon) && this.Up.ApproximatelyEqualsTo(o.Up, epsilon) && this.Down.ApproximatelyEqualsTo(o.Down, epsilon);
		}

		// Token: 0x040004AC RID: 1196
		public readonly float Right;

		// Token: 0x040004AD RID: 1197
		public readonly float Left;

		// Token: 0x040004AE RID: 1198
		public readonly float Forward;

		// Token: 0x040004AF RID: 1199
		public readonly float Backward;

		// Token: 0x040004B0 RID: 1200
		public readonly float Up;

		// Token: 0x040004B1 RID: 1201
		public readonly float Down;
	}
}
