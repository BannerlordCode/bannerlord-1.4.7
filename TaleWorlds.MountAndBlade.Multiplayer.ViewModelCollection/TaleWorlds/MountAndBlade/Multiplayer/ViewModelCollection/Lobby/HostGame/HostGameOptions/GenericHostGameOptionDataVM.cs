using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions
{
	// Token: 0x02000048 RID: 72
	public abstract class GenericHostGameOptionDataVM : ViewModel
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x000153DA File Offset: 0x000135DA
		public MultiplayerOptions.OptionType OptionType { get; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x000153E2 File Offset: 0x000135E2
		public int PreferredIndex { get; }

		// Token: 0x06000686 RID: 1670 RVA: 0x000153EA File Offset: 0x000135EA
		internal GenericHostGameOptionDataVM(OptionsVM.OptionsDataType type, MultiplayerOptions.OptionType optionType, int preferredIndex)
		{
			this.Category = (int)type;
			this.OptionType = optionType;
			this.PreferredIndex = preferredIndex;
			this.Index = preferredIndex;
			this.IsEnabled = true;
			this.RefreshValues();
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001541C File Offset: 0x0001361C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = GameTexts.FindText("str_multiplayer_option", this.OptionType.ToString()).ToString();
		}

		// Token: 0x06000688 RID: 1672
		public abstract void RefreshData();

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00015458 File Offset: 0x00013658
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x00015460 File Offset: 0x00013660
		[DataSourceProperty]
		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				if (value != this._index)
				{
					this._index = value;
					base.OnPropertyChangedWithValue(value, "Index");
				}
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001547E File Offset: 0x0001367E
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x00015486 File Offset: 0x00013686
		[DataSourceProperty]
		public int Category
		{
			get
			{
				return this._category;
			}
			set
			{
				if (value != this._category)
				{
					this._category = value;
					base.OnPropertyChangedWithValue(value, "Category");
				}
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x000154A4 File Offset: 0x000136A4
		// (set) Token: 0x0600068E RID: 1678 RVA: 0x000154AC File Offset: 0x000136AC
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

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x000154CF File Offset: 0x000136CF
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x000154D7 File Offset: 0x000136D7
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

		// Token: 0x04000315 RID: 789
		private int _index;

		// Token: 0x04000316 RID: 790
		private int _category;

		// Token: 0x04000317 RID: 791
		private string _name;

		// Token: 0x04000318 RID: 792
		private bool _isEnabled;
	}
}
