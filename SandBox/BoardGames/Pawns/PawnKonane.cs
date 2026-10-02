using System;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Pawns
{
	// Token: 0x020000FA RID: 250
	public class PawnKonane : PawnBase
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x0005DC77 File Offset: 0x0005BE77
		public override bool IsPlaced
		{
			get
			{
				return this.X >= 0 && this.X < BoardGameKonane.BoardWidth && this.Y >= 0 && this.Y < BoardGameKonane.BoardHeight;
			}
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0005DCA7 File Offset: 0x0005BEA7
		public PawnKonane(GameEntity entity, bool playerOne)
			: base(entity, playerOne)
		{
			this.X = -1;
			this.Y = -1;
			this.PrevX = -1;
			this.PrevY = -1;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0005DCCD File Offset: 0x0005BECD
		public override void Reset()
		{
			base.Reset();
			this.X = -1;
			this.Y = -1;
			this.PrevX = -1;
			this.PrevY = -1;
		}

		// Token: 0x04000580 RID: 1408
		public int X;

		// Token: 0x04000581 RID: 1409
		public int Y;

		// Token: 0x04000582 RID: 1410
		public int PrevX;

		// Token: 0x04000583 RID: 1411
		public int PrevY;
	}
}
