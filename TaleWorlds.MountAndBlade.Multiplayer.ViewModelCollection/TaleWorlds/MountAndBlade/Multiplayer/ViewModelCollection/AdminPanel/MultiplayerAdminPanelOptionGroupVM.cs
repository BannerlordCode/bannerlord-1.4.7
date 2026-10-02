using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000AF RID: 175
	public class MultiplayerAdminPanelOptionGroupVM : ViewModel
	{
		// Token: 0x06001091 RID: 4241 RVA: 0x00033890 File Offset: 0x00031A90
		public MultiplayerAdminPanelOptionGroupVM(IAdminPanelOptionGroup optionGroup, Func<IAdminPanelOption, MultiplayerAdminPanelOptionBaseVM> onCreateOptionVm, Func<IAdminPanelAction, MultiplayerAdminPanelOptionBaseVM> onCreateActionVm)
		{
			this._optionGroup = optionGroup;
			this._onCreateOptionVM = onCreateOptionVm;
			this._onCreateActionVM = onCreateActionVm;
			this.Options = new MBBindingList<MultiplayerAdminPanelOptionBaseVM>();
			for (int i = 0; i < this._optionGroup.Options.Count; i++)
			{
				Func<IAdminPanelOption, MultiplayerAdminPanelOptionBaseVM> onCreateOptionVM = this._onCreateOptionVM;
				MultiplayerAdminPanelOptionBaseVM multiplayerAdminPanelOptionBaseVM = ((onCreateOptionVM != null) ? onCreateOptionVM(optionGroup.Options[i]) : null);
				if (multiplayerAdminPanelOptionBaseVM != null)
				{
					this.Options.Add(multiplayerAdminPanelOptionBaseVM);
				}
				else
				{
					Debug.FailedAssert("Failed to create view model for option type: " + optionGroup.Options[i].GetType().Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\AdminPanel\\MultiplayerAdminPanelOptionGroupVM.cs", ".ctor", 34);
				}
			}
			for (int j = 0; j < this._optionGroup.Actions.Count; j++)
			{
				Func<IAdminPanelAction, MultiplayerAdminPanelOptionBaseVM> onCreateActionVM = this._onCreateActionVM;
				MultiplayerAdminPanelOptionBaseVM multiplayerAdminPanelOptionBaseVM2 = ((onCreateActionVM != null) ? onCreateActionVM(optionGroup.Actions[j]) : null);
				if (multiplayerAdminPanelOptionBaseVM2 != null)
				{
					this.Options.Add(multiplayerAdminPanelOptionBaseVM2);
				}
				else
				{
					Debug.FailedAssert("Failed to create view model for option type: " + optionGroup.Options[j].GetType().Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\AdminPanel\\MultiplayerAdminPanelOptionGroupVM.cs", ".ctor", 48);
				}
			}
			this.RequiresRestart = this._optionGroup.RequiresRestart;
			this.RequiresRestartHint = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x000339E0 File Offset: 0x00031BE0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RequiresRestartHint.HintText = (this.RequiresRestart ? new TextObject("{=sTVcpXkf}All options under this category requires restart.", null) : TextObject.GetEmpty());
			this.GroupName = this._optionGroup.Name.ToString();
			this.Options.ApplyActionOnAllItems(delegate(MultiplayerAdminPanelOptionBaseVM o)
			{
				o.RefreshValues();
			});
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00033A58 File Offset: 0x00031C58
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Options.ApplyActionOnAllItems(delegate(MultiplayerAdminPanelOptionBaseVM o)
			{
				o.OnFinalize();
			});
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x00033A8A File Offset: 0x00031C8A
		// (set) Token: 0x06001095 RID: 4245 RVA: 0x00033A92 File Offset: 0x00031C92
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

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001096 RID: 4246 RVA: 0x00033AB0 File Offset: 0x00031CB0
		// (set) Token: 0x06001097 RID: 4247 RVA: 0x00033AB8 File Offset: 0x00031CB8
		[DataSourceProperty]
		public string GroupName
		{
			get
			{
				return this._groupName;
			}
			set
			{
				if (value != this._groupName)
				{
					this._groupName = value;
					base.OnPropertyChangedWithValue<string>(value, "GroupName");
				}
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x00033ADB File Offset: 0x00031CDB
		// (set) Token: 0x06001099 RID: 4249 RVA: 0x00033AE3 File Offset: 0x00031CE3
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

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x00033B01 File Offset: 0x00031D01
		// (set) Token: 0x0600109B RID: 4251 RVA: 0x00033B09 File Offset: 0x00031D09
		[DataSourceProperty]
		public MBBindingList<MultiplayerAdminPanelOptionBaseVM> Options
		{
			get
			{
				return this._options;
			}
			set
			{
				if (value != this._options)
				{
					this._options = value;
					base.OnPropertyChangedWithValue<MBBindingList<MultiplayerAdminPanelOptionBaseVM>>(value, "Options");
				}
			}
		}

		// Token: 0x040007B6 RID: 1974
		private readonly IAdminPanelOptionGroup _optionGroup;

		// Token: 0x040007B7 RID: 1975
		private readonly Func<IAdminPanelOption, MultiplayerAdminPanelOptionBaseVM> _onCreateOptionVM;

		// Token: 0x040007B8 RID: 1976
		private readonly Func<IAdminPanelAction, MultiplayerAdminPanelOptionBaseVM> _onCreateActionVM;

		// Token: 0x040007B9 RID: 1977
		private bool _requiresRestart;

		// Token: 0x040007BA RID: 1978
		private string _groupName;

		// Token: 0x040007BB RID: 1979
		private HintViewModel _requiresRestartHint;

		// Token: 0x040007BC RID: 1980
		private MBBindingList<MultiplayerAdminPanelOptionBaseVM> _options;
	}
}
