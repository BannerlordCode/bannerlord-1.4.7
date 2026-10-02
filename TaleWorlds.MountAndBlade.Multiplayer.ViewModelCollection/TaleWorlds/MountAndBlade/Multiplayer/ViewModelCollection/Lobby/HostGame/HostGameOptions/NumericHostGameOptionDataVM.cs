using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions
{
	// Token: 0x0200004B RID: 75
	public class NumericHostGameOptionDataVM : GenericHostGameOptionDataVM
	{
		// Token: 0x0600069B RID: 1691 RVA: 0x0001581A File Offset: 0x00013A1A
		public NumericHostGameOptionDataVM(MultiplayerOptions.OptionType optionType, int preferredIndex)
			: base(OptionsVM.OptionsDataType.NumericOption, optionType, preferredIndex)
		{
			this.RefreshData();
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001582C File Offset: 0x00013A2C
		public override void RefreshData()
		{
			MultiplayerOptionsProperty optionProperty = base.OptionType.GetOptionProperty();
			this.Min = optionProperty.BoundsMin;
			this.Max = optionProperty.BoundsMax;
			this.Value = base.OptionType.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x0001586F File Offset: 0x00013A6F
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x00015877 File Offset: 0x00013A77
		[DataSourceProperty]
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue(value, "Value");
					base.OnPropertyChanged("ValueAsString");
					base.OptionType.SetValue(value, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				}
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x000158AD File Offset: 0x00013AAD
		[DataSourceProperty]
		public string ValueAsString
		{
			get
			{
				return this._value.ToString();
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x000158BA File Offset: 0x00013ABA
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x000158C2 File Offset: 0x00013AC2
		[DataSourceProperty]
		public int Min
		{
			get
			{
				return this._min;
			}
			set
			{
				if (value != this._min)
				{
					this._min = value;
					base.OnPropertyChangedWithValue(value, "Min");
				}
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x000158E0 File Offset: 0x00013AE0
		// (set) Token: 0x060006A3 RID: 1699 RVA: 0x000158E8 File Offset: 0x00013AE8
		[DataSourceProperty]
		public int Max
		{
			get
			{
				return this._max;
			}
			set
			{
				if (value != this._max)
				{
					this._max = value;
					base.OnPropertyChangedWithValue(value, "Max");
				}
			}
		}

		// Token: 0x0400031C RID: 796
		private int _value;

		// Token: 0x0400031D RID: 797
		private int _min;

		// Token: 0x0400031E RID: 798
		private int _max;
	}
}
