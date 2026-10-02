using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A4 RID: 164
	public class MultiplayerLobbyGameTypeCardButtonWidget : ButtonWidget
	{
		// Token: 0x060008BF RID: 2239 RVA: 0x000190F5 File Offset: 0x000172F5
		public MultiplayerLobbyGameTypeCardButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00019100 File Offset: 0x00017300
		protected override void RefreshState()
		{
			base.RefreshState();
			if (!base.OverrideDefaultStateSwitchingEnabled)
			{
				if (base.IsDisabled)
				{
					this.SetState(base.IsSelected ? "SelectedDisabled" : "Disabled");
				}
				else if (base.IsSelected)
				{
					this.SetState("Selected");
				}
				else if (base.IsPressed)
				{
					this.SetState("Pressed");
				}
				else if (base.IsHovered)
				{
					this.SetState("Hovered");
				}
				else
				{
					this.SetState("Default");
				}
			}
			if (base.UpdateChildrenStates)
			{
				for (int i = 0; i < base.ChildCount; i++)
				{
					base.GetChild(i).SetState(base.CurrentState);
				}
			}
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x000191B4 File Offset: 0x000173B4
		private void UpdateGameTypeImage()
		{
			if (this.GameTypeImageWidget == null || string.IsNullOrEmpty(this.GameTypeId))
			{
				return;
			}
			Sprite sprite = base.Context.SpriteData.GetSprite("MPLobby\\Matchmaking\\GameTypeCards\\" + this.GameTypeId);
			foreach (Style style in this.GameTypeImageWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Sprite = sprite;
				}
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00019260 File Offset: 0x00017460
		// (set) Token: 0x060008C3 RID: 2243 RVA: 0x00019268 File Offset: 0x00017468
		[Editor(false)]
		public string GameTypeId
		{
			get
			{
				return this._gameTypeId;
			}
			set
			{
				if (this._gameTypeId != value)
				{
					this._gameTypeId = value;
					base.OnPropertyChanged<string>(value, "GameTypeId");
					this.UpdateGameTypeImage();
				}
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x00019291 File Offset: 0x00017491
		// (set) Token: 0x060008C5 RID: 2245 RVA: 0x00019299 File Offset: 0x00017499
		[Editor(false)]
		public BrushWidget GameTypeImageWidget
		{
			get
			{
				return this._gameTypeImageWidget;
			}
			set
			{
				if (this._gameTypeImageWidget != value)
				{
					this._gameTypeImageWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "GameTypeImageWidget");
					this.UpdateGameTypeImage();
				}
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x000192BD File Offset: 0x000174BD
		// (set) Token: 0x060008C7 RID: 2247 RVA: 0x000192C5 File Offset: 0x000174C5
		[Editor(false)]
		public Widget CheckboxWidget
		{
			get
			{
				return this._checkboxWidget;
			}
			set
			{
				if (this._checkboxWidget != value)
				{
					this._checkboxWidget = value;
					base.OnPropertyChanged<Widget>(value, "CheckboxWidget");
				}
			}
		}

		// Token: 0x040003F9 RID: 1017
		private string _gameTypeId;

		// Token: 0x040003FA RID: 1018
		private BrushWidget _gameTypeImageWidget;

		// Token: 0x040003FB RID: 1019
		private Widget _checkboxWidget;
	}
}
