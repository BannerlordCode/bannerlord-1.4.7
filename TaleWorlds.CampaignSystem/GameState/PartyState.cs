using System;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200039B RID: 923
	public class PartyState : PlayerGameState
	{
		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06003577 RID: 13687 RVA: 0x000D9E33 File Offset: 0x000D8033
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06003578 RID: 13688 RVA: 0x000D9E36 File Offset: 0x000D8036
		// (set) Token: 0x06003579 RID: 13689 RVA: 0x000D9E3E File Offset: 0x000D803E
		public PartyScreenLogic PartyScreenLogic { get; set; }

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x0600357A RID: 13690 RVA: 0x000D9E47 File Offset: 0x000D8047
		// (set) Token: 0x0600357B RID: 13691 RVA: 0x000D9E4F File Offset: 0x000D804F
		public PartyScreenHelper.PartyScreenMode PartyScreenMode { get; set; }

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x0600357C RID: 13692 RVA: 0x000D9E58 File Offset: 0x000D8058
		// (set) Token: 0x0600357D RID: 13693 RVA: 0x000D9E60 File Offset: 0x000D8060
		public bool IsDonating { get; set; }

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x0600357E RID: 13694 RVA: 0x000D9E69 File Offset: 0x000D8069
		// (set) Token: 0x0600357F RID: 13695 RVA: 0x000D9E71 File Offset: 0x000D8071
		public IPartyScreenLogicHandler Handler { get; set; }

		// Token: 0x06003580 RID: 13696 RVA: 0x000D9E7A File Offset: 0x000D807A
		public void RequestUserInput(string text, Action accept, Action cancel)
		{
			if (this.Handler != null)
			{
				this.Handler.RequestUserInput(text, accept, cancel);
			}
		}
	}
}
