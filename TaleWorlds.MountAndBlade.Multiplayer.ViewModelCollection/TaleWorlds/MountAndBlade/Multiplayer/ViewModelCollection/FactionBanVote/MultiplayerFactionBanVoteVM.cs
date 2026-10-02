using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FactionBanVote
{
	// Token: 0x0200009E RID: 158
	public class MultiplayerFactionBanVoteVM : ViewModel
	{
		// Token: 0x06000F37 RID: 3895 RVA: 0x0002EF9F File Offset: 0x0002D19F
		public MultiplayerFactionBanVoteVM(BasicCultureObject culture, Action<MultiplayerFactionBanVoteVM> onSelect)
		{
			this.Culture = culture;
			this._onSelect = onSelect;
			this._isEnabled = true;
			this._name = culture.Name.ToString();
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0002EFCD File Offset: 0x0002D1CD
		// (set) Token: 0x06000F39 RID: 3897 RVA: 0x0002EFD5 File Offset: 0x0002D1D5
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
					if (value)
					{
						this._onSelect(this);
					}
				}
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06000F3A RID: 3898 RVA: 0x0002F002 File Offset: 0x0002D202
		// (set) Token: 0x06000F3B RID: 3899 RVA: 0x0002F00A File Offset: 0x0002D20A
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06000F3C RID: 3900 RVA: 0x0002F028 File Offset: 0x0002D228
		// (set) Token: 0x06000F3D RID: 3901 RVA: 0x0002F030 File Offset: 0x0002D230
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x04000703 RID: 1795
		private readonly Action<MultiplayerFactionBanVoteVM> _onSelect;

		// Token: 0x04000704 RID: 1796
		public readonly BasicCultureObject Culture;

		// Token: 0x04000705 RID: 1797
		private string _name;

		// Token: 0x04000706 RID: 1798
		private bool _isEnabled;

		// Token: 0x04000707 RID: 1799
		private bool _isSelected;
	}
}
