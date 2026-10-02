using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000075 RID: 117
	public struct Oriented2DArea
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0000EA90 File Offset: 0x0000CC90
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x0000EA98 File Offset: 0x0000CC98
		public Vec2 GlobalCenter { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x0000EAA1 File Offset: 0x0000CCA1
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x0000EAA9 File Offset: 0x0000CCA9
		public Vec2 GlobalForward { get; private set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x0000EAB2 File Offset: 0x0000CCB2
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x0000EABA File Offset: 0x0000CCBA
		public Vec2 LocalDimensions { get; private set; }

		// Token: 0x06000430 RID: 1072 RVA: 0x0000EAC3 File Offset: 0x0000CCC3
		public Oriented2DArea(in Vec2 globalCenter, in Vec2 globalForward, in Vec2 localDimensions)
		{
			this.GlobalCenter = globalCenter;
			this.GlobalForward = globalForward;
			this.LocalDimensions = localDimensions;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000EAE9 File Offset: 0x0000CCE9
		public void SetGlobalCenter(in Vec2 globalCenter)
		{
			this.GlobalCenter = globalCenter;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000EAF7 File Offset: 0x0000CCF7
		public void SetLocalDimensions(in Vec2 localDimensions)
		{
			this.LocalDimensions = localDimensions;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000EB08 File Offset: 0x0000CD08
		public bool Overlaps(in Oriented2DArea otherArea, float clearanceMargin)
		{
			Oriented2DArea.Corners corners = this.GetCorners();
			Oriented2DArea oriented2DArea = otherArea;
			Oriented2DArea.Corners corners2 = oriented2DArea.GetCorners();
			if (!this.IsProjectionOverlap(in corners, in corners2, this.GlobalForward, clearanceMargin))
			{
				return false;
			}
			Vec2 vec = this.GlobalForward.RightVec();
			if (!this.IsProjectionOverlap(in corners, in corners2, vec, clearanceMargin))
			{
				return false;
			}
			oriented2DArea = otherArea;
			if (!this.IsProjectionOverlap(in corners, in corners2, oriented2DArea.GlobalForward, clearanceMargin))
			{
				return false;
			}
			oriented2DArea = otherArea;
			Vec2 vec2 = oriented2DArea.GlobalForward.RightVec();
			return this.IsProjectionOverlap(in corners, in corners2, vec2, clearanceMargin);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000EBAC File Offset: 0x0000CDAC
		public bool Intersects(in LineSegment2D line, float clearanceMargin)
		{
			Oriented2DArea.Corners corners = this.GetCorners();
			Vec2 vec = this.GlobalForward.RightVec();
			return this.DoesProjectionIntersect(in corners, in line, this.GlobalForward, clearanceMargin) && this.DoesProjectionIntersect(in corners, in line, vec, clearanceMargin) && this.DoesProjectionIntersect(in corners, in line, line.Normal, clearanceMargin);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000EC00 File Offset: 0x0000CE00
		public Oriented2DArea.Corners GetCorners()
		{
			Vec2 vec = this.GlobalForward.RightVec() * (this.LocalDimensions.x * 0.5f);
			Vec2 vec2 = this.GlobalForward * (this.LocalDimensions.y * 0.5f);
			Vec2 vec3 = this.GlobalCenter + vec + vec2;
			Vec2 vec4 = this.GlobalCenter - vec + vec2;
			Vec2 vec5 = this.GlobalCenter - vec - vec2;
			Vec2 vec6 = this.GlobalCenter + vec - vec2;
			return new Oriented2DArea.Corners(in vec4, in vec3, in vec5, in vec6);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000ECAC File Offset: 0x0000CEAC
		private bool IsProjectionOverlap(in Oriented2DArea.Corners cornersA, in Oriented2DArea.Corners cornersB, Vec2 axis, float clearanceMargin)
		{
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float num3 = float.MaxValue;
			float num4 = float.MinValue;
			for (int i = 0; i < 4; i++)
			{
				Oriented2DArea.Corners corners = cornersA;
				float num5 = Vec2.DotProduct(corners[i], axis);
				num = Math.Min(num, num5);
				num2 = Math.Max(num2, num5);
			}
			for (int j = 0; j < 4; j++)
			{
				Oriented2DArea.Corners corners = cornersB;
				float num6 = Vec2.DotProduct(corners[j], axis);
				num3 = Math.Min(num3, num6);
				num4 = Math.Max(num4, num6);
			}
			return num2 + clearanceMargin >= num3 && num4 + clearanceMargin >= num;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000ED5C File Offset: 0x0000CF5C
		private bool DoesProjectionIntersect(in Oriented2DArea.Corners cornersOfArea, in LineSegment2D line, Vec2 axis, float clearanceMargin)
		{
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float num3 = float.MaxValue;
			float num4 = float.MinValue;
			for (int i = 0; i < 4; i++)
			{
				Oriented2DArea.Corners corners = cornersOfArea;
				float num5 = Vec2.DotProduct(corners[i], axis);
				num = MathF.Min(num, num5);
				num2 = MathF.Max(num2, num5);
			}
			for (int j = 0; j < 2; j++)
			{
				LineSegment2D lineSegment2D = line;
				float num6 = Vec2.DotProduct(lineSegment2D[j], axis);
				num3 = MathF.Min(num3, num6);
				num4 = MathF.Max(num4, num6);
			}
			return num2 + clearanceMargin >= num3 && num4 + clearanceMargin >= num;
		}

		// Token: 0x020000DE RID: 222
		public struct Corners
		{
			// Token: 0x170000FE RID: 254
			// (get) Token: 0x0600078B RID: 1931 RVA: 0x00018EE1 File Offset: 0x000170E1
			// (set) Token: 0x0600078C RID: 1932 RVA: 0x00018EE9 File Offset: 0x000170E9
			public Vec2 TopLeft { get; private set; }

			// Token: 0x170000FF RID: 255
			// (get) Token: 0x0600078D RID: 1933 RVA: 0x00018EF2 File Offset: 0x000170F2
			// (set) Token: 0x0600078E RID: 1934 RVA: 0x00018EFA File Offset: 0x000170FA
			public Vec2 TopRight { get; private set; }

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x0600078F RID: 1935 RVA: 0x00018F03 File Offset: 0x00017103
			// (set) Token: 0x06000790 RID: 1936 RVA: 0x00018F0B File Offset: 0x0001710B
			public Vec2 BottomLeft { get; private set; }

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x06000791 RID: 1937 RVA: 0x00018F14 File Offset: 0x00017114
			// (set) Token: 0x06000792 RID: 1938 RVA: 0x00018F1C File Offset: 0x0001711C
			public Vec2 BottomRight { get; private set; }

			// Token: 0x06000793 RID: 1939 RVA: 0x00018F25 File Offset: 0x00017125
			public Corners(in Vec2 topLeft, in Vec2 topRight, in Vec2 bottomLeft, in Vec2 bottomRight)
			{
				this.TopLeft = topLeft;
				this.TopRight = topRight;
				this.BottomLeft = bottomLeft;
				this.BottomRight = bottomRight;
			}

			// Token: 0x17000102 RID: 258
			public Vec2 this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this.TopLeft;
					case 1:
						return this.TopRight;
					case 2:
						return this.BottomLeft;
					case 3:
						return this.BottomRight;
					default:
						Debug.FailedAssert("Invalid index", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Oriented2DArea.cs", "Item", 39);
						return Vec2.Invalid;
					}
				}
			}

			// Token: 0x040002D7 RID: 727
			public const int Count = 4;
		}
	}
}
