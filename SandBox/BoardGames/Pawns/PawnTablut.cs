using System;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Pawns
{
	// Token: 0x020000FE RID: 254
	public class PawnTablut : PawnBase
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0005E21E File Offset: 0x0005C41E
		public override bool IsPlaced
		{
			get
			{
				return this.X >= 0 && this.X < 9 && this.Y >= 0 && this.Y < 9;
			}
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0005E248 File Offset: 0x0005C448
		public PawnTablut(GameEntity entity, bool playerOne)
			: base(entity, playerOne)
		{
			this.X = -1;
			this.Y = -1;
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0005E260 File Offset: 0x0005C460
		public override void Reset()
		{
			base.Reset();
			this.X = -1;
			this.Y = -1;
		}

		// Token: 0x04000592 RID: 1426
		public int X;

		// Token: 0x04000593 RID: 1427
		public int Y;
	}
}
