using System;

namespace TaleWorlds.Library
{
	// Token: 0x020000A3 RID: 163
	[Serializable]
	public struct Vec3i
	{
		// Token: 0x06000624 RID: 1572 RVA: 0x000156C1 File Offset: 0x000138C1
		public Vec3i(int x = 0, int y = 0, int z = 0)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000156D8 File Offset: 0x000138D8
		public static bool operator ==(Vec3i v1, Vec3i v2)
		{
			return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00015706 File Offset: 0x00013906
		public static bool operator !=(Vec3i v1, Vec3i v2)
		{
			return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00015737 File Offset: 0x00013937
		public Vec3 ToVec3()
		{
			return new Vec3((float)this.X, (float)this.Y, (float)this.Z, -1f);
		}

		// Token: 0x170000AF RID: 175
		public int this[int index]
		{
			get
			{
				if (index == 0)
				{
					return this.X;
				}
				if (index != 1)
				{
					return this.Z;
				}
				return this.Y;
			}
			set
			{
				if (index == 0)
				{
					this.X = value;
					return;
				}
				if (index == 1)
				{
					this.Y = value;
					return;
				}
				this.Z = value;
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00015795 File Offset: 0x00013995
		public static Vec3i operator *(Vec3i v, int mult)
		{
			return new Vec3i(v.X * mult, v.Y * mult, v.Z * mult);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000157B4 File Offset: 0x000139B4
		public static Vec3i operator +(Vec3i v1, Vec3i v2)
		{
			return new Vec3i(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x000157E2 File Offset: 0x000139E2
		public static Vec3i operator -(Vec3i v1, Vec3i v2)
		{
			return new Vec3i(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00015810 File Offset: 0x00013A10
		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType()) && (((Vec3i)obj).X == this.X && ((Vec3i)obj).Y == this.Y) && ((Vec3i)obj).Z == this.Z;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001587A File Offset: 0x00013A7A
		public override int GetHashCode()
		{
			return (((this.X * 397) ^ this.Y) * 397) ^ this.Z;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0001589C File Offset: 0x00013A9C
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}", new object[] { "X", this.X, "Y", this.Y, "Z", this.Z });
		}

		// Token: 0x040001CF RID: 463
		public int X;

		// Token: 0x040001D0 RID: 464
		public int Y;

		// Token: 0x040001D1 RID: 465
		public int Z;

		// Token: 0x040001D2 RID: 466
		public static readonly Vec3i Zero = new Vec3i(0, 0, 0);
	}
}
