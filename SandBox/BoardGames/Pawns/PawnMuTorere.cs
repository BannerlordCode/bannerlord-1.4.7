using System;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Pawns
{
	// Token: 0x020000FB RID: 251
	public class PawnMuTorere : PawnBase
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x0005DCF1 File Offset: 0x0005BEF1
		// (set) Token: 0x06000CA6 RID: 3238 RVA: 0x0005DCF9 File Offset: 0x0005BEF9
		public int X { get; set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0005DD02 File Offset: 0x0005BF02
		public override bool IsPlaced
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0005DD05 File Offset: 0x0005BF05
		public PawnMuTorere(GameEntity entity, bool playerOne)
			: base(entity, playerOne)
		{
			this.X = -1;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0005DD16 File Offset: 0x0005BF16
		public override void Reset()
		{
			base.Reset();
			this.X = -1;
		}
	}
}
