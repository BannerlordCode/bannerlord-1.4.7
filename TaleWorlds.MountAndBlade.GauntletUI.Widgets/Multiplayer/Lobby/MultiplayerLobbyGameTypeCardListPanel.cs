using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A5 RID: 165
	public class MultiplayerLobbyGameTypeCardListPanel : ListPanel
	{
		// Token: 0x060008C8 RID: 2248 RVA: 0x000192E3 File Offset: 0x000174E3
		public MultiplayerLobbyGameTypeCardListPanel(UIContext context)
			: base(context)
		{
			this._cardButtons = new List<MultiplayerLobbyGameTypeCardButtonWidget>();
		}

		// Token: 0x040003FC RID: 1020
		private List<MultiplayerLobbyGameTypeCardButtonWidget> _cardButtons;
	}
}
