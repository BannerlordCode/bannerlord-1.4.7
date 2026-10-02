using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions
{
	// Token: 0x02000049 RID: 73
	public class InputHostGameOptionDataVM : GenericHostGameOptionDataVM
	{
		// Token: 0x06000691 RID: 1681 RVA: 0x000154F5 File Offset: 0x000136F5
		public InputHostGameOptionDataVM(MultiplayerOptions.OptionType optionType, int preferredIndex)
			: base(OptionsVM.OptionsDataType.InputOption, optionType, preferredIndex)
		{
			this.RefreshData();
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00015508 File Offset: 0x00013708
		public override void RefreshData()
		{
			string strValue = base.OptionType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this.Text = strValue;
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x00015529 File Offset: 0x00013729
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x00015531 File Offset: 0x00013731
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
					base.OptionType.SetValue(value, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				}
			}
		}

		// Token: 0x04000319 RID: 793
		private string _text;
	}
}
