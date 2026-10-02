using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000BC RID: 188
	public class MultiplayerLobbyClassFilterFactionItemButtonWidget : ButtonWidget
	{
		// Token: 0x060009D6 RID: 2518 RVA: 0x0001B7BE File Offset: 0x000199BE
		public MultiplayerLobbyClassFilterFactionItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0001B7D4 File Offset: 0x000199D4
		private void OnCultureChanged()
		{
			if (this.Culture == null)
			{
				return;
			}
			string text = this.BaseBrushName + "." + this.Culture[0].ToString().ToUpper() + this.Culture.Substring(1).ToLower();
			base.Brush = base.Context.BrushFactory.GetBrush(text);
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0001B83C File Offset: 0x00019A3C
		// (set) Token: 0x060009D9 RID: 2521 RVA: 0x0001B844 File Offset: 0x00019A44
		[Editor(false)]
		public string BaseBrushName
		{
			get
			{
				return this._baseBrushName;
			}
			set
			{
				if (value != this._baseBrushName)
				{
					this._baseBrushName = value;
					base.OnPropertyChanged<string>(value, "BaseBrushName");
					this.OnCultureChanged();
				}
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x0001B86D File Offset: 0x00019A6D
		// (set) Token: 0x060009DB RID: 2523 RVA: 0x0001B875 File Offset: 0x00019A75
		[Editor(false)]
		public string Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				if (this._culture != value)
				{
					this._culture = value;
					base.OnPropertyChanged<string>(value, "Culture");
					this.OnCultureChanged();
				}
			}
		}

		// Token: 0x04000471 RID: 1137
		private string _baseBrushName = "MPLobby.ClassFilter.FactionButton";

		// Token: 0x04000472 RID: 1138
		private string _culture;
	}
}
