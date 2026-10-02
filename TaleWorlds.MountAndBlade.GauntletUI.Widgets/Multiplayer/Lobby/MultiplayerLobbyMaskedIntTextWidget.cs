using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A8 RID: 168
	public class MultiplayerLobbyMaskedIntTextWidget : TextWidget
	{
		// Token: 0x060008D6 RID: 2262 RVA: 0x0001948B File Offset: 0x0001768B
		public MultiplayerLobbyMaskedIntTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00019494 File Offset: 0x00017694
		private void IntValueUpdated()
		{
			if (this.IntValue == this.MaskedIntValue)
			{
				base.Text = this.MaskText;
				return;
			}
			base.IntText = this.IntValue;
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x000194BD File Offset: 0x000176BD
		// (set) Token: 0x060008D9 RID: 2265 RVA: 0x000194C5 File Offset: 0x000176C5
		[Editor(false)]
		public int IntValue
		{
			get
			{
				return this._intValue;
			}
			set
			{
				if (this._intValue != value)
				{
					this._intValue = value;
					base.OnPropertyChanged(value, "IntValue");
					this.IntValueUpdated();
				}
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x000194E9 File Offset: 0x000176E9
		// (set) Token: 0x060008DB RID: 2267 RVA: 0x000194F1 File Offset: 0x000176F1
		[Editor(false)]
		public int MaskedIntValue
		{
			get
			{
				return this._maskedIntValue;
			}
			set
			{
				if (this._maskedIntValue != value)
				{
					this._maskedIntValue = value;
					base.OnPropertyChanged(value, "MaskedIntValue");
				}
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x0001950F File Offset: 0x0001770F
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x00019517 File Offset: 0x00017717
		[Editor(false)]
		public string MaskText
		{
			get
			{
				return this._maskText;
			}
			set
			{
				if (this._maskText != value)
				{
					this._maskText = value;
					base.OnPropertyChanged<string>(value, "MaskText");
				}
			}
		}

		// Token: 0x04000402 RID: 1026
		private int _intValue;

		// Token: 0x04000403 RID: 1027
		private int _maskedIntValue;

		// Token: 0x04000404 RID: 1028
		private string _maskText;
	}
}
