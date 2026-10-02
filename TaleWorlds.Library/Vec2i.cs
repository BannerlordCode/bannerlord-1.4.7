using System;

namespace TaleWorlds.Library
{
	// Token: 0x020000A1 RID: 161
	[Serializable]
	public struct Vec2i : IEquatable<Vec2i>
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x000143D5 File Offset: 0x000125D5
		public int Item1
		{
			get
			{
				return this.X;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x000143DD File Offset: 0x000125DD
		public int Item2
		{
			get
			{
				return this.Y;
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000143E5 File Offset: 0x000125E5
		public Vec2i(int x = 0, int y = 0)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000143F5 File Offset: 0x000125F5
		public static bool operator ==(Vec2i a, Vec2i b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00014415 File Offset: 0x00012615
		public static bool operator !=(Vec2i a, Vec2i b)
		{
			return a.X != b.X || a.Y != b.Y;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00014438 File Offset: 0x00012638
		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType()) && ((Vec2i)obj).X == this.X && ((Vec2i)obj).Y == this.Y;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001448F File Offset: 0x0001268F
		public bool Equals(Vec2i value)
		{
			return value.X == this.X && value.Y == this.Y;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x000144AF File Offset: 0x000126AF
		public override int GetHashCode()
		{
			return (23 * 31 + this.X.GetHashCode()) * 31 + this.Y.GetHashCode();
		}

		// Token: 0x040001BF RID: 447
		public int X;

		// Token: 0x040001C0 RID: 448
		public int Y;

		// Token: 0x040001C1 RID: 449
		public static readonly Vec2i Side = new Vec2i(1, 0);

		// Token: 0x040001C2 RID: 450
		public static readonly Vec2i Forward = new Vec2i(0, 1);

		// Token: 0x040001C3 RID: 451
		public static readonly Vec2i One = new Vec2i(1, 1);

		// Token: 0x040001C4 RID: 452
		public static readonly Vec2i Zero = new Vec2i(0, 0);
	}
}
