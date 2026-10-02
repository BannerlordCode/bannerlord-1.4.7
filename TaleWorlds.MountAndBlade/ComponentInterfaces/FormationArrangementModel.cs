using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003FF RID: 1023
	public abstract class FormationArrangementModel : MBGameModel<FormationArrangementModel>
	{
		// Token: 0x060037B4 RID: 14260
		public abstract List<FormationArrangementModel.ArrangementPosition> GetBannerBearerPositions(Formation formation, int maxCount);

		// Token: 0x020006A3 RID: 1699
		public struct ArrangementPosition
		{
			// Token: 0x17000AFD RID: 2813
			// (get) Token: 0x060041C9 RID: 16841 RVA: 0x000FC5AE File Offset: 0x000FA7AE
			public bool IsValid
			{
				get
				{
					return this.FileIndex > -1 && this.RankIndex > -1;
				}
			}

			// Token: 0x17000AFE RID: 2814
			// (get) Token: 0x060041CA RID: 16842 RVA: 0x000FC5C4 File Offset: 0x000FA7C4
			public static FormationArrangementModel.ArrangementPosition Invalid
			{
				get
				{
					return default(FormationArrangementModel.ArrangementPosition);
				}
			}

			// Token: 0x060041CB RID: 16843 RVA: 0x000FC5DA File Offset: 0x000FA7DA
			public ArrangementPosition(int fileIndex = -1, int rankIndex = -1)
			{
				this.FileIndex = fileIndex;
				this.RankIndex = rankIndex;
			}

			// Token: 0x040022E9 RID: 8937
			public readonly int FileIndex;

			// Token: 0x040022EA RID: 8938
			public readonly int RankIndex;
		}
	}
}
