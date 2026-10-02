using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200038A RID: 906
	public class CharacterDeveloperState : GameState
	{
		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x060034D4 RID: 13524 RVA: 0x000D9551 File Offset: 0x000D7751
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x060034D5 RID: 13525 RVA: 0x000D9554 File Offset: 0x000D7754
		// (set) Token: 0x060034D6 RID: 13526 RVA: 0x000D955C File Offset: 0x000D775C
		public Hero InitialSelectedHero { get; private set; }

		// Token: 0x060034D7 RID: 13527 RVA: 0x000D9565 File Offset: 0x000D7765
		public CharacterDeveloperState()
		{
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x000D956D File Offset: 0x000D776D
		public CharacterDeveloperState(Hero initialSelectedHero)
		{
			this.InitialSelectedHero = initialSelectedHero;
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x060034D9 RID: 13529 RVA: 0x000D957C File Offset: 0x000D777C
		// (set) Token: 0x060034DA RID: 13530 RVA: 0x000D9584 File Offset: 0x000D7784
		public ICharacterDeveloperStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x04000F1C RID: 3868
		private ICharacterDeveloperStateHandler _handler;
	}
}
