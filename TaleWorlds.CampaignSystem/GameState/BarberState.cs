using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000389 RID: 905
	public class BarberState : GameState
	{
		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x060034CF RID: 13519 RVA: 0x000D951F File Offset: 0x000D771F
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x060034D0 RID: 13520 RVA: 0x000D9522 File Offset: 0x000D7722
		// (set) Token: 0x060034D1 RID: 13521 RVA: 0x000D952A File Offset: 0x000D772A
		public IFaceGeneratorCustomFilter Filter { get; private set; }

		// Token: 0x060034D2 RID: 13522 RVA: 0x000D9533 File Offset: 0x000D7733
		public BarberState()
		{
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x000D953B File Offset: 0x000D773B
		public BarberState(BasicCharacterObject character, IFaceGeneratorCustomFilter filter)
		{
			this.Character = character;
			this.Filter = filter;
		}

		// Token: 0x04000F19 RID: 3865
		public BasicCharacterObject Character;
	}
}
