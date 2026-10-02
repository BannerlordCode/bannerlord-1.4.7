using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000BA RID: 186
	public class MultiplayerLobbyArmoryCosmeticTierVisualBrushWidget : BrushWidget
	{
		// Token: 0x060009C5 RID: 2501 RVA: 0x0001B5DB File Offset: 0x000197DB
		public MultiplayerLobbyArmoryCosmeticTierVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0001B5EC File Offset: 0x000197EC
		private void UpdateVisual()
		{
			switch (this._rarity)
			{
			case 0:
			case 1:
				this.SetState("Common");
				return;
			case 2:
				this.SetState("Rare");
				return;
			case 3:
				this.SetState("Unique");
				return;
			default:
				return;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0001B63A File Offset: 0x0001983A
		// (set) Token: 0x060009C8 RID: 2504 RVA: 0x0001B642 File Offset: 0x00019842
		[Editor(false)]
		public int Rarity
		{
			get
			{
				return this._rarity;
			}
			set
			{
				if (this._rarity != value)
				{
					this._rarity = value;
					base.OnPropertyChanged(value, "Rarity");
					this.UpdateVisual();
				}
			}
		}

		// Token: 0x0400046B RID: 1131
		private int _rarity = -1;
	}
}
