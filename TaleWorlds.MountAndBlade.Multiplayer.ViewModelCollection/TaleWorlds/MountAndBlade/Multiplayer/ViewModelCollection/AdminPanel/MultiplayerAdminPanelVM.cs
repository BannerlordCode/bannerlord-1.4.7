using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000B2 RID: 178
	public class MultiplayerAdminPanelVM : ViewModel
	{
		// Token: 0x060010A9 RID: 4265 RVA: 0x00033C74 File Offset: 0x00031E74
		public MultiplayerAdminPanelVM(Action<bool> onEscapeMenuToggled, MBReadOnlyList<IAdminPanelOptionProvider> optionProviders, Func<IAdminPanelOption, MultiplayerAdminPanelOptionBaseVM> onGetOptionViewModel, Func<IAdminPanelAction, MultiplayerAdminPanelOptionBaseVM> onGetActionViewModel)
		{
			this._onEscapeMenuToggled = onEscapeMenuToggled;
			this._optionProviders = optionProviders;
			this._onCreateOptionViewModel = onGetOptionViewModel;
			this._onCreateActionViewModel = onGetActionViewModel;
			this.OptionGroups = new MBBindingList<MultiplayerAdminPanelOptionGroupVM>();
			this.InitializeOptions();
			this.InitializeCallbacks();
			this.RefreshValues();
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00033CC4 File Offset: 0x00031EC4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=xILeUbY3}Admin Panel", null).ToString();
			this.CancelText = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
			this.ApplyText = new TextObject("{=WZQnNSwV}Apply Changes", null).ToString();
			this.StartMissionText = new TextObject("{=kwo09aDm}Apply and Start Mission", null).ToString();
			this.ApplyDisabledHint = new HintViewModel(new TextObject("{=TrY4VS1R}Please select valid values for options.", null), null);
			this.OptionGroups.ApplyActionOnAllItems(delegate(MultiplayerAdminPanelOptionGroupVM o)
			{
				o.RefreshValues();
			});
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00033D70 File Offset: 0x00031F70
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._optionProviders != null)
			{
				for (int i = 0; i < this._optionProviders.Count; i++)
				{
					this._optionProviders[i].OnFinalize();
				}
			}
			this.FinalizeCallbacks();
			this.OptionGroups.ApplyActionOnAllItems(delegate(MultiplayerAdminPanelOptionGroupVM o)
			{
				o.OnFinalize();
			});
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00033DE4 File Offset: 0x00031FE4
		public void OnTick(float dt)
		{
			if (this._optionProviders != null)
			{
				for (int i = 0; i < this._optionProviders.Count; i++)
				{
					this._optionProviders[i].OnTick(dt);
				}
			}
			if (this._areOptionValuesDirty)
			{
				this.UpdateOptionValues();
				this._areOptionValuesDirty = false;
			}
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x00033E36 File Offset: 0x00032036
		private void InitializeCallbacks()
		{
			MultiplayerAdminPanelOptionBaseVM.OnOptionRefreshed += this.OnOptionChanged;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x00033E49 File Offset: 0x00032049
		private void FinalizeCallbacks()
		{
			MultiplayerAdminPanelOptionBaseVM.OnOptionRefreshed -= this.OnOptionChanged;
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x00033E5C File Offset: 0x0003205C
		private void InitializeOptions()
		{
			this.OptionGroups.Clear();
			if (this._optionProviders != null)
			{
				foreach (IAdminPanelOptionProvider adminPanelOptionProvider in this._optionProviders)
				{
					foreach (IAdminPanelOptionGroup adminPanelOptionGroup in adminPanelOptionProvider.GetOptionGroups())
					{
						MultiplayerAdminPanelOptionGroupVM multiplayerAdminPanelOptionGroupVM = new MultiplayerAdminPanelOptionGroupVM(adminPanelOptionGroup, this._onCreateOptionViewModel, this._onCreateActionViewModel);
						this.OptionGroups.Add(multiplayerAdminPanelOptionGroupVM);
					}
				}
			}
			this.UpdateOptionValues();
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x00033F18 File Offset: 0x00032118
		private void OnOptionChanged(MultiplayerAdminPanelOptionBaseVM option)
		{
			this._areOptionValuesDirty = true;
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00033F24 File Offset: 0x00032124
		private void UpdateOptionValues()
		{
			bool flag = false;
			foreach (MultiplayerAdminPanelOptionGroupVM multiplayerAdminPanelOptionGroupVM in this.OptionGroups)
			{
				foreach (MultiplayerAdminPanelOptionBaseVM multiplayerAdminPanelOptionBaseVM in multiplayerAdminPanelOptionGroupVM.Options)
				{
					multiplayerAdminPanelOptionBaseVM.UpdateValues();
					if (multiplayerAdminPanelOptionBaseVM.IsRequired && multiplayerAdminPanelOptionBaseVM.IsDisabled)
					{
						flag = true;
					}
				}
			}
			this.IsApplyDisabled = flag;
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00033FC0 File Offset: 0x000321C0
		public void ExecuteApplyChanges()
		{
			if (this._optionProviders == null)
			{
				return;
			}
			foreach (IAdminPanelOptionProvider adminPanelOptionProvider in this._optionProviders)
			{
				adminPanelOptionProvider.ApplyOptions();
			}
			this._areOptionValuesDirty = true;
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00034020 File Offset: 0x00032220
		public void ExecuteCancel()
		{
			this._onEscapeMenuToggled(false);
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x0003402E File Offset: 0x0003222E
		// (set) Token: 0x060010B5 RID: 4277 RVA: 0x00034036 File Offset: 0x00032236
		[DataSourceProperty]
		public bool IsApplyDisabled
		{
			get
			{
				return this._isApplyDisabled;
			}
			set
			{
				if (value != this._isApplyDisabled)
				{
					this._isApplyDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsApplyDisabled");
				}
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x00034054 File Offset: 0x00032254
		// (set) Token: 0x060010B7 RID: 4279 RVA: 0x0003405C File Offset: 0x0003225C
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x0003407F File Offset: 0x0003227F
		// (set) Token: 0x060010B9 RID: 4281 RVA: 0x00034087 File Offset: 0x00032287
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x000340AA File Offset: 0x000322AA
		// (set) Token: 0x060010BB RID: 4283 RVA: 0x000340B2 File Offset: 0x000322B2
		[DataSourceProperty]
		public string ApplyText
		{
			get
			{
				return this._applyText;
			}
			set
			{
				if (value != this._applyText)
				{
					this._applyText = value;
					base.OnPropertyChangedWithValue<string>(value, "ApplyText");
				}
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x000340D5 File Offset: 0x000322D5
		// (set) Token: 0x060010BD RID: 4285 RVA: 0x000340DD File Offset: 0x000322DD
		[DataSourceProperty]
		public string StartMissionText
		{
			get
			{
				return this._startMissionText;
			}
			set
			{
				if (value != this._startMissionText)
				{
					this._startMissionText = value;
					base.OnPropertyChangedWithValue<string>(value, "StartMissionText");
				}
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x00034100 File Offset: 0x00032300
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x00034108 File Offset: 0x00032308
		[DataSourceProperty]
		public HintViewModel ApplyDisabledHint
		{
			get
			{
				return this._applyDisabledHint;
			}
			set
			{
				if (value != this._applyDisabledHint)
				{
					this._applyDisabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ApplyDisabledHint");
				}
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x00034126 File Offset: 0x00032326
		// (set) Token: 0x060010C1 RID: 4289 RVA: 0x0003412E File Offset: 0x0003232E
		[DataSourceProperty]
		public MBBindingList<MultiplayerAdminPanelOptionGroupVM> OptionGroups
		{
			get
			{
				return this._optionGroups;
			}
			set
			{
				if (value != this._optionGroups)
				{
					this._optionGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<MultiplayerAdminPanelOptionGroupVM>>(value, "OptionGroups");
				}
			}
		}

		// Token: 0x040007C3 RID: 1987
		private readonly Action<bool> _onEscapeMenuToggled;

		// Token: 0x040007C4 RID: 1988
		private readonly MBReadOnlyList<IAdminPanelOptionProvider> _optionProviders;

		// Token: 0x040007C5 RID: 1989
		private readonly Func<IAdminPanelOption, MultiplayerAdminPanelOptionBaseVM> _onCreateOptionViewModel;

		// Token: 0x040007C6 RID: 1990
		private readonly Func<IAdminPanelAction, MultiplayerAdminPanelOptionBaseVM> _onCreateActionViewModel;

		// Token: 0x040007C7 RID: 1991
		private bool _areOptionValuesDirty;

		// Token: 0x040007C8 RID: 1992
		private bool _isApplyDisabled;

		// Token: 0x040007C9 RID: 1993
		private string _titleText;

		// Token: 0x040007CA RID: 1994
		private string _cancelText;

		// Token: 0x040007CB RID: 1995
		private string _applyText;

		// Token: 0x040007CC RID: 1996
		private string _startMissionText;

		// Token: 0x040007CD RID: 1997
		private HintViewModel _applyDisabledHint;

		// Token: 0x040007CE RID: 1998
		private MBBindingList<MultiplayerAdminPanelOptionGroupVM> _optionGroups;
	}
}
