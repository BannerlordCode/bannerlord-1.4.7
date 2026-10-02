using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000B7 RID: 183
	public class MultiplayerLobbyArmoryCosmeticItemBrushWidget : BrushWidget
	{
		// Token: 0x06000998 RID: 2456 RVA: 0x0001AF89 File Offset: 0x00019189
		public MultiplayerLobbyArmoryCosmeticItemBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0001AF92 File Offset: 0x00019192
		public override void SetState(string stateName)
		{
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0001AF94 File Offset: 0x00019194
		private void OnUsageChanged()
		{
			base.SetState(this.IsUsed ? "Selected" : "Default");
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0001AFB0 File Offset: 0x000191B0
		private void OnRarityChanged()
		{
			switch (this.Rarity)
			{
			case 0:
			case 1:
				base.Brush = base.Context.GetBrush("MPLobby.Armory.CosmeticButton.Common");
				return;
			case 2:
				base.Brush = base.Context.GetBrush("MPLobby.Armory.CosmeticButton.Rare");
				return;
			case 3:
				base.Brush = base.Context.GetBrush("MPLobby.Armory.CosmeticButton.Unique");
				return;
			default:
				return;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x0001B01F File Offset: 0x0001921F
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x0001B027 File Offset: 0x00019227
		[Editor(false)]
		public bool IsUsed
		{
			get
			{
				return this._isUsed;
			}
			set
			{
				if (value != this._isUsed)
				{
					this._isUsed = value;
					base.OnPropertyChanged(value, "IsUsed");
					this.OnUsageChanged();
				}
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x0001B04B File Offset: 0x0001924B
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x0001B053 File Offset: 0x00019253
		[Editor(false)]
		public int Rarity
		{
			get
			{
				return this._rarity;
			}
			set
			{
				if (value != this._rarity)
				{
					this._rarity = value;
					base.OnPropertyChanged(value, "Rarity");
					this.OnRarityChanged();
				}
			}
		}

		// Token: 0x04000457 RID: 1111
		private const string BaseBrushName = "MPLobby.Armory.CosmeticButton";

		// Token: 0x04000458 RID: 1112
		private bool _isUsed;

		// Token: 0x04000459 RID: 1113
		private int _rarity;
	}
}
