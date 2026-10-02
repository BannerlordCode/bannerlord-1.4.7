using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions
{
	// Token: 0x02000047 RID: 71
	public class BooleanHostGameOptionDataVM : GenericHostGameOptionDataVM
	{
		// Token: 0x06000680 RID: 1664 RVA: 0x00015382 File Offset: 0x00013582
		public BooleanHostGameOptionDataVM(MultiplayerOptions.OptionType optionType, int preferredIndex)
			: base(OptionsVM.OptionsDataType.BooleanOption, optionType, preferredIndex)
		{
			this.RefreshData();
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00015393 File Offset: 0x00013593
		public override void RefreshData()
		{
			this.IsSelected = base.OptionType.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x000153A7 File Offset: 0x000135A7
		// (set) Token: 0x06000683 RID: 1667 RVA: 0x000153AF File Offset: 0x000135AF
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
					base.OptionType.SetValue(value, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				}
			}
		}

		// Token: 0x04000312 RID: 786
		private bool _isSelected;
	}
}
