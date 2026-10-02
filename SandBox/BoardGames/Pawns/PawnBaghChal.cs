using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.BoardGames.Pawns
{
	// Token: 0x020000F8 RID: 248
	public class PawnBaghChal : PawnBase
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x0005D431 File Offset: 0x0005B631
		public override bool IsPlaced
		{
			get
			{
				return this.X >= 0 && this.X < BoardGameBaghChal.BoardWidth && this.Y >= 0 && this.Y < BoardGameBaghChal.BoardHeight;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x0005D461 File Offset: 0x0005B661
		public MatrixFrame InitialFrame { get; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x0005D469 File Offset: 0x0005B669
		public bool IsTiger { get; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x0005D471 File Offset: 0x0005B671
		public bool IsGoat
		{
			get
			{
				return !this.IsTiger;
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0005D47C File Offset: 0x0005B67C
		public PawnBaghChal(GameEntity entity, bool playerOne, bool isTiger)
			: base(entity, playerOne)
		{
			this.X = -1;
			this.Y = -1;
			this.PrevX = -1;
			this.PrevY = -1;
			this.IsTiger = isTiger;
			this.InitialFrame = base.Entity.GetFrame();
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0005D4BA File Offset: 0x0005B6BA
		public override void Reset()
		{
			base.Reset();
			this.X = -1;
			this.Y = -1;
			this.PrevX = -1;
			this.PrevY = -1;
		}

		// Token: 0x04000564 RID: 1380
		public int X;

		// Token: 0x04000565 RID: 1381
		public int Y;

		// Token: 0x04000566 RID: 1382
		public int PrevX;

		// Token: 0x04000567 RID: 1383
		public int PrevY;
	}
}
