using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000AE RID: 174
	public abstract class MultiplayerAdminPanelOptionBaseVM : ViewModel
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600106E RID: 4206 RVA: 0x000333E8 File Offset: 0x000315E8
		// (remove) Token: 0x0600106F RID: 4207 RVA: 0x0003341C File Offset: 0x0003161C
		public static event Action<MultiplayerAdminPanelOptionBaseVM> OnOptionRefreshed;

		// Token: 0x06001070 RID: 4208 RVA: 0x00033450 File Offset: 0x00031650
		protected MultiplayerAdminPanelOptionBaseVM(IAdminPanelOption option)
		{
			this._option = option;
			IAdminPanelOption option2 = this._option;
			if (option2 != null)
			{
				option2.SetOnRefreshCallback(new Action(this.OnOptionRefreshedAux));
			}
			this.RequiresRestart = option != null && option.RequiresMissionRestart;
			this.RefreshValues();
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x000334A0 File Offset: 0x000316A0
		public override void RefreshValues()
		{
			base.RefreshValues();
			IAdminPanelOption option = this._option;
			string text;
			if (option == null)
			{
				text = null;
			}
			else
			{
				string name = option.Name;
				text = ((name != null) ? name.ToString() : null);
			}
			this.OptionTitle = text;
			IAdminPanelOption option2 = this._option;
			string text2;
			if (option2 == null)
			{
				text2 = null;
			}
			else
			{
				string description = option2.Description;
				text2 = ((description != null) ? description.ToString() : null);
			}
			this.OptionDescription = text2;
			IAdminPanelOption option3 = this._option;
			if (!string.IsNullOrEmpty((option3 != null) ? option3.Description : null))
			{
				this.DescriptionHint = new HintViewModel(new TextObject("{=!}" + this._option.Description, null), null);
			}
			else
			{
				this.DescriptionHint = null;
			}
			this.RequiresRestartHint = new HintViewModel(new TextObject("{=MxRJ4CWL}This option won't take effect until next mission.", null), null);
			this.IsDirtyHint = new HintViewModel(new TextObject("{=ftM2TjQ5}Revert changes", null), null);
			this.RestoreToDefaultsHint = new HintViewModel(new TextObject("{=36ll5uSI}Restore to defaults", null), null);
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00033589 File Offset: 0x00031789
		public override void OnFinalize()
		{
			base.OnFinalize();
			IAdminPanelOption option = this._option;
			if (option == null)
			{
				return;
			}
			option.SetOnRefreshCallback(null);
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x000335A2 File Offset: 0x000317A2
		private void OnOptionRefreshedAux()
		{
			Action<MultiplayerAdminPanelOptionBaseVM> onOptionRefreshed = MultiplayerAdminPanelOptionBaseVM.OnOptionRefreshed;
			if (onOptionRefreshed == null)
			{
				return;
			}
			onOptionRefreshed(this);
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x000335B4 File Offset: 0x000317B4
		public virtual void UpdateValues()
		{
			IAdminPanelOption option = this._option;
			this.IsFilteredOut = option != null && !option.GetIsAvailable();
			IAdminPanelOption option2 = this._option;
			this.IsDirty = option2 != null && option2.IsDirty;
			IAdminPanelOption option3 = this._option;
			this.CanResetToDefault = option3 != null && option3.CanRevertToDefaultValue;
			string empty = string.Empty;
			IAdminPanelOption option4 = this._option;
			this.IsDisabled = option4 != null && option4.GetIsDisabled(out empty);
			IAdminPanelOption option5 = this._option;
			this.IsRequired = option5 != null && option5.IsRequired;
			if (!string.IsNullOrEmpty(empty))
			{
				this.DisabledHint = new HintViewModel(new TextObject("{=!}" + empty, null), null);
				return;
			}
			this.DisabledHint = null;
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00033671 File Offset: 0x00031871
		public virtual void ExecuteRevertChanges()
		{
			IAdminPanelOption option = this._option;
			if (option == null)
			{
				return;
			}
			option.RevertChanges();
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00033683 File Offset: 0x00031883
		public virtual void ExecuteRestoreDefaults()
		{
			IAdminPanelOption option = this._option;
			if (option == null)
			{
				return;
			}
			option.RestoreDefaults();
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x00033695 File Offset: 0x00031895
		// (set) Token: 0x06001078 RID: 4216 RVA: 0x0003369D File Offset: 0x0003189D
		[DataSourceProperty]
		public bool IsRequired
		{
			get
			{
				return this._isRequired;
			}
			set
			{
				if (value != this._isRequired)
				{
					this._isRequired = value;
					base.OnPropertyChangedWithValue(value, "IsRequired");
				}
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x000336BB File Offset: 0x000318BB
		// (set) Token: 0x0600107A RID: 4218 RVA: 0x000336C3 File Offset: 0x000318C3
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x000336E1 File Offset: 0x000318E1
		// (set) Token: 0x0600107C RID: 4220 RVA: 0x000336E9 File Offset: 0x000318E9
		[DataSourceProperty]
		public bool IsDirty
		{
			get
			{
				return this._isDirty;
			}
			set
			{
				if (value != this._isDirty)
				{
					this._isDirty = value;
					base.OnPropertyChangedWithValue(value, "IsDirty");
				}
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x00033707 File Offset: 0x00031907
		// (set) Token: 0x0600107E RID: 4222 RVA: 0x0003370F File Offset: 0x0003190F
		[DataSourceProperty]
		public bool CanResetToDefault
		{
			get
			{
				return this._canResetToDefault;
			}
			set
			{
				if (value != this._canResetToDefault)
				{
					this._canResetToDefault = value;
					base.OnPropertyChangedWithValue(value, "CanResetToDefault");
				}
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0003372D File Offset: 0x0003192D
		// (set) Token: 0x06001080 RID: 4224 RVA: 0x00033735 File Offset: 0x00031935
		[DataSourceProperty]
		public bool IsFilteredOut
		{
			get
			{
				return this._isFilteredOut;
			}
			set
			{
				if (value != this._isFilteredOut)
				{
					this._isFilteredOut = value;
					base.OnPropertyChangedWithValue(value, "IsFilteredOut");
				}
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x00033753 File Offset: 0x00031953
		// (set) Token: 0x06001082 RID: 4226 RVA: 0x0003375B File Offset: 0x0003195B
		[DataSourceProperty]
		public bool RequiresRestart
		{
			get
			{
				return this._requiresRestart;
			}
			set
			{
				if (value != this._requiresRestart)
				{
					this._requiresRestart = value;
					base.OnPropertyChangedWithValue(value, "RequiresRestart");
				}
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001083 RID: 4227 RVA: 0x00033779 File Offset: 0x00031979
		// (set) Token: 0x06001084 RID: 4228 RVA: 0x00033781 File Offset: 0x00031981
		[DataSourceProperty]
		public string OptionTitle
		{
			get
			{
				return this._optionTitle;
			}
			set
			{
				if (value != this._optionTitle)
				{
					this._optionTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionTitle");
				}
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x000337A4 File Offset: 0x000319A4
		// (set) Token: 0x06001086 RID: 4230 RVA: 0x000337AC File Offset: 0x000319AC
		[DataSourceProperty]
		public string OptionDescription
		{
			get
			{
				return this._optionDescription;
			}
			set
			{
				if (value != this._optionDescription)
				{
					this._optionDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionDescription");
				}
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x000337CF File Offset: 0x000319CF
		// (set) Token: 0x06001088 RID: 4232 RVA: 0x000337D7 File Offset: 0x000319D7
		[DataSourceProperty]
		public HintViewModel DisabledHint
		{
			get
			{
				return this._disabledHint;
			}
			set
			{
				if (value != this._disabledHint)
				{
					this._disabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DisabledHint");
				}
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x000337F5 File Offset: 0x000319F5
		// (set) Token: 0x0600108A RID: 4234 RVA: 0x000337FD File Offset: 0x000319FD
		[DataSourceProperty]
		public HintViewModel DescriptionHint
		{
			get
			{
				return this._descriptionHint;
			}
			set
			{
				if (value != this._descriptionHint)
				{
					this._descriptionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DescriptionHint");
				}
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x0003381B File Offset: 0x00031A1B
		// (set) Token: 0x0600108C RID: 4236 RVA: 0x00033823 File Offset: 0x00031A23
		[DataSourceProperty]
		public HintViewModel RequiresRestartHint
		{
			get
			{
				return this._requiresRestartHint;
			}
			set
			{
				if (value != this._requiresRestartHint)
				{
					this._requiresRestartHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RequiresRestartHint");
				}
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x00033841 File Offset: 0x00031A41
		// (set) Token: 0x0600108E RID: 4238 RVA: 0x00033849 File Offset: 0x00031A49
		[DataSourceProperty]
		public HintViewModel IsDirtyHint
		{
			get
			{
				return this._isDirtyHint;
			}
			set
			{
				if (value != this._isDirtyHint)
				{
					this._isDirtyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "IsDirtyHint");
				}
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x0600108F RID: 4239 RVA: 0x00033867 File Offset: 0x00031A67
		// (set) Token: 0x06001090 RID: 4240 RVA: 0x0003386F File Offset: 0x00031A6F
		[DataSourceProperty]
		public HintViewModel RestoreToDefaultsHint
		{
			get
			{
				return this._restoreToDefaultsHint;
			}
			set
			{
				if (value != this._restoreToDefaultsHint)
				{
					this._restoreToDefaultsHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RestoreToDefaultsHint");
				}
			}
		}

		// Token: 0x040007A8 RID: 1960
		protected readonly IAdminPanelOption _option;

		// Token: 0x040007A9 RID: 1961
		private bool _isRequired;

		// Token: 0x040007AA RID: 1962
		private bool _isDisabled;

		// Token: 0x040007AB RID: 1963
		private bool _isDirty;

		// Token: 0x040007AC RID: 1964
		private bool _canResetToDefault;

		// Token: 0x040007AD RID: 1965
		private bool _isFilteredOut;

		// Token: 0x040007AE RID: 1966
		private bool _requiresRestart;

		// Token: 0x040007AF RID: 1967
		private string _optionTitle;

		// Token: 0x040007B0 RID: 1968
		private string _optionDescription;

		// Token: 0x040007B1 RID: 1969
		private HintViewModel _disabledHint;

		// Token: 0x040007B2 RID: 1970
		private HintViewModel _descriptionHint;

		// Token: 0x040007B3 RID: 1971
		private HintViewModel _requiresRestartHint;

		// Token: 0x040007B4 RID: 1972
		private HintViewModel _isDirtyHint;

		// Token: 0x040007B5 RID: 1973
		private HintViewModel _restoreToDefaultsHint;
	}
}
