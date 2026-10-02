using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Options;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x0200006E RID: 110
	public class GroupedOptionCategoryVM : ViewModel
	{
		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x0001D296 File Offset: 0x0001B496
		public IEnumerable<GenericOptionDataVM> AllOptions
		{
			get
			{
				return this.BaseOptions.Concat<GenericOptionDataVM>(this.Groups.SelectMany<OptionGroupVM, GenericOptionDataVM>((OptionGroupVM g) => g.Options));
			}
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001D2CD File Offset: 0x0001B4CD
		public GroupedOptionCategoryVM(OptionsVM options, TextObject name, OptionCategory category, bool isEnabled, bool isResetSupported = false)
		{
			this._category = category;
			this._nameTextObject = name;
			this._options = options;
			this.IsEnabled = isEnabled;
			this.IsResetSupported = isResetSupported;
			this.InitializeOptions();
			this.RefreshValues();
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0001D308 File Offset: 0x0001B508
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameTextObject.ToString();
			this.BaseOptions.ApplyActionOnAllItems(delegate(GenericOptionDataVM b)
			{
				b.RefreshValues();
			});
			this.Groups.ApplyActionOnAllItems(delegate(OptionGroupVM g)
			{
				g.RefreshValues();
			});
			this.ResetText = new TextObject("{=RVIKFCno}Reset to Defaults", null).ToString();
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001D398 File Offset: 0x0001B598
		private void InitializeOptions()
		{
			this.BaseOptions = new MBBindingList<GenericOptionDataVM>();
			this.Groups = new MBBindingList<OptionGroupVM>();
			if (this._category == null)
			{
				return;
			}
			if (this._category.Groups != null)
			{
				foreach (OptionGroup optionGroup in this._category.Groups)
				{
					this.Groups.Add(new OptionGroupVM(optionGroup.GroupName, this._options, optionGroup.Options));
				}
			}
			if (this._category.BaseOptions != null)
			{
				foreach (IOptionData optionData in this._category.BaseOptions)
				{
					this.BaseOptions.Add(this._options.GetOptionItem(optionData));
				}
			}
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0001D490 File Offset: 0x0001B690
		internal IEnumerable<IOptionData> GetManagedOptions()
		{
			List<IOptionData> managedOptions = new List<IOptionData>();
			this.Groups.ApplyActionOnAllItems(delegate(OptionGroupVM g)
			{
				managedOptions.AppendList<IOptionData>(g.GetManagedOptions());
			});
			return managedOptions.AsReadOnly();
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001D4D0 File Offset: 0x0001B6D0
		internal void InitializeDependentConfigs(Action<IOptionData, float> updateDependentConfigs)
		{
			this.Groups.ApplyActionOnAllItems(delegate(OptionGroupVM g)
			{
				g.InitializeDependentConfigs(updateDependentConfigs);
			});
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001D504 File Offset: 0x0001B704
		internal bool IsChanged()
		{
			if (!this.BaseOptions.Any<GenericOptionDataVM>((GenericOptionDataVM b) => b.IsChanged()))
			{
				return this.Groups.Any<OptionGroupVM>((OptionGroupVM g) => g.IsChanged());
			}
			return true;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001D56C File Offset: 0x0001B76C
		internal void Cancel()
		{
			this.BaseOptions.ApplyActionOnAllItems(delegate(GenericOptionDataVM b)
			{
				b.Cancel();
			});
			this.Groups.ApplyActionOnAllItems(delegate(OptionGroupVM g)
			{
				g.Cancel();
			});
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0001D5D0 File Offset: 0x0001B7D0
		public void ResetData()
		{
			this.BaseOptions.ApplyActionOnAllItems(delegate(GenericOptionDataVM b)
			{
				b.ResetData();
			});
			foreach (OptionGroupVM optionGroupVM in this.Groups)
			{
				optionGroupVM.Options.ApplyActionOnAllItems(delegate(GenericOptionDataVM o)
				{
					o.ResetData();
				});
			}
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0001D668 File Offset: 0x0001B868
		public void ExecuteResetToDefault()
		{
			InformationManager.ShowInquiry(new InquiryData(new TextObject("{=oZc8oEAP}Reset this category to default", null).ToString(), new TextObject("{=CCBcdzGa}This will reset ALL options of this category to their default states. You won't be able to undo this action. {newline} {newline}Are you sure?", null).ToString(), true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), new Action(this.ResetToDefault), null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0001D6E0 File Offset: 0x0001B8E0
		private void ResetToDefault()
		{
			this.BaseOptions.ApplyActionOnAllItems(delegate(GenericOptionDataVM b)
			{
				b.ResetToDefault();
			});
			this.Groups.ApplyActionOnAllItems(delegate(OptionGroupVM g)
			{
				g.Options.ApplyActionOnAllItems(delegate(GenericOptionDataVM o)
				{
					o.ResetToDefault();
				});
			});
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001D744 File Offset: 0x0001B944
		public GenericOptionDataVM GetOption(ManagedOptions.ManagedOptionsType optionType)
		{
			return this.AllOptions.FirstOrDefault<GenericOptionDataVM>((GenericOptionDataVM o) => !o.IsNative && (ManagedOptions.ManagedOptionsType)o.GetOptionType() == optionType);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001D778 File Offset: 0x0001B978
		public GenericOptionDataVM GetOption(NativeOptions.NativeOptionsType optionType)
		{
			return this.AllOptions.FirstOrDefault<GenericOptionDataVM>((GenericOptionDataVM o) => o.IsNative && (NativeOptions.NativeOptionsType)o.GetOptionType() == optionType);
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0001D7A9 File Offset: 0x0001B9A9
		// (set) Token: 0x060008A6 RID: 2214 RVA: 0x0001D7B1 File Offset: 0x0001B9B1
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

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0001D7CF File Offset: 0x0001B9CF
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x0001D7D7 File Offset: 0x0001B9D7
		[DataSourceProperty]
		public bool IsResetSupported
		{
			get
			{
				return this._isResetSupported;
			}
			set
			{
				if (value != this._isResetSupported)
				{
					this._isResetSupported = value;
					base.OnPropertyChangedWithValue(value, "IsResetSupported");
				}
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0001D7F5 File Offset: 0x0001B9F5
		// (set) Token: 0x060008AA RID: 2218 RVA: 0x0001D7FD File Offset: 0x0001B9FD
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

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0001D820 File Offset: 0x0001BA20
		// (set) Token: 0x060008AC RID: 2220 RVA: 0x0001D828 File Offset: 0x0001BA28
		[DataSourceProperty]
		public string ResetText
		{
			get
			{
				return this._resetText;
			}
			set
			{
				if (value != this._resetText)
				{
					this._resetText = value;
					base.OnPropertyChangedWithValue<string>(value, "ResetText");
				}
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x0001D84B File Offset: 0x0001BA4B
		// (set) Token: 0x060008AE RID: 2222 RVA: 0x0001D853 File Offset: 0x0001BA53
		[DataSourceProperty]
		public MBBindingList<OptionGroupVM> Groups
		{
			get
			{
				return this._groups;
			}
			set
			{
				if (value != this._groups)
				{
					this._groups = value;
					base.OnPropertyChangedWithValue<MBBindingList<OptionGroupVM>>(value, "Groups");
				}
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x0001D871 File Offset: 0x0001BA71
		// (set) Token: 0x060008B0 RID: 2224 RVA: 0x0001D879 File Offset: 0x0001BA79
		[DataSourceProperty]
		public MBBindingList<GenericOptionDataVM> BaseOptions
		{
			get
			{
				return this._baseOptions;
			}
			set
			{
				if (value != this._baseOptions)
				{
					this._baseOptions = value;
					base.OnPropertyChangedWithValue<MBBindingList<GenericOptionDataVM>>(value, "BaseOptions");
				}
			}
		}

		// Token: 0x040003D0 RID: 976
		private readonly OptionCategory _category;

		// Token: 0x040003D1 RID: 977
		private readonly TextObject _nameTextObject;

		// Token: 0x040003D2 RID: 978
		protected readonly OptionsVM _options;

		// Token: 0x040003D3 RID: 979
		private bool _isEnabled;

		// Token: 0x040003D4 RID: 980
		private bool _isResetSupported;

		// Token: 0x040003D5 RID: 981
		private string _name;

		// Token: 0x040003D6 RID: 982
		private string _resetText;

		// Token: 0x040003D7 RID: 983
		private MBBindingList<GenericOptionDataVM> _baseOptions;

		// Token: 0x040003D8 RID: 984
		private MBBindingList<OptionGroupVM> _groups;
	}
}
