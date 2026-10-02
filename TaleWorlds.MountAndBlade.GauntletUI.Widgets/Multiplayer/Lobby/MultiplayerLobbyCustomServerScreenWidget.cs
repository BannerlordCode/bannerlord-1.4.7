using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A3 RID: 163
	public class MultiplayerLobbyCustomServerScreenWidget : Widget
	{
		// Token: 0x060008BA RID: 2234 RVA: 0x000190A0 File Offset: 0x000172A0
		public MultiplayerLobbyCustomServerScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x000190A9 File Offset: 0x000172A9
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x000190B1 File Offset: 0x000172B1
		[Editor(false)]
		public bool IsPartyLeader
		{
			get
			{
				return this._isPartyLeader;
			}
			set
			{
				if (this._isPartyLeader != value)
				{
					this._isPartyLeader = value;
					base.OnPropertyChanged(value, "IsPartyLeader");
				}
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x000190CF File Offset: 0x000172CF
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x000190D7 File Offset: 0x000172D7
		[Editor(false)]
		public bool IsInParty
		{
			get
			{
				return this._isInParty;
			}
			set
			{
				if (this._isInParty != value)
				{
					this._isInParty = value;
					base.OnPropertyChanged(value, "IsInParty");
				}
			}
		}

		// Token: 0x040003F7 RID: 1015
		private bool _isPartyLeader;

		// Token: 0x040003F8 RID: 1016
		private bool _isInParty;
	}
}
