using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin.Internal
{
	// Token: 0x02000078 RID: 120
	internal class AdminPanelMultiSelectionOption : AdminPanelOption<IAdminPanelMultiSelectionItem>, IAdminPanelMultiSelectionOption, IAdminPanelOption<IAdminPanelMultiSelectionItem>, IAdminPanelOption
	{
		// Token: 0x060003B7 RID: 951 RVA: 0x00010099 File Offset: 0x0000E299
		public AdminPanelMultiSelectionOption(string uniqueId)
			: base(uniqueId)
		{
			this._availableOptions = new MBList<IAdminPanelMultiSelectionItem>();
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x000100AD File Offset: 0x0000E2AD
		protected override bool AreEqualValues(IAdminPanelMultiSelectionItem first, IAdminPanelMultiSelectionItem second)
		{
			return first == second;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x000100B4 File Offset: 0x0000E2B4
		protected override IAdminPanelMultiSelectionItem GetOptionValue(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode = MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions)
		{
			string strValue = optionType.GetStrValue(accessMode);
			for (int i = 0; i < this._availableOptions.Count; i++)
			{
				if (this._availableOptions[i].Value == strValue)
				{
					return this._availableOptions[i];
				}
			}
			return null;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00010106 File Offset: 0x0000E306
		protected override void OnValueChanged(IAdminPanelMultiSelectionItem previousValue, IAdminPanelMultiSelectionItem newValue)
		{
			this._selectedOption = newValue;
			base.OnValueChanged(previousValue, newValue);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00010117 File Offset: 0x0000E317
		protected override bool OnGetCanRevertToDefaultValue()
		{
			return this._availableOptions.Contains(base.DefaultValue);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0001012C File Offset: 0x0000E32C
		public virtual AdminPanelMultiSelectionOption BuildAvailableOptions(MBReadOnlyList<IAdminPanelMultiSelectionItem> options)
		{
			this._availableOptions.Clear();
			if (options != null && options.Count > 0)
			{
				for (int i = 0; i < options.Count; i++)
				{
					this._availableOptions.Add(options[i]);
				}
			}
			this.OnRefresh();
			return this;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0001017C File Offset: 0x0000E37C
		public virtual AdminPanelMultiSelectionOption BuildAvailableOptions(MultiplayerOptions.OptionType optionType, bool buildDefaultValue = true)
		{
			this._availableOptions.Clear();
			string strValue = optionType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions);
			List<string> multiplayerOptionsList = MultiplayerOptions.Instance.GetMultiplayerOptionsList(optionType);
			if (multiplayerOptionsList != null && multiplayerOptionsList.Count > 0)
			{
				for (int i = 0; i < multiplayerOptionsList.Count; i++)
				{
					AdminPanelMultiSelectionItem adminPanelMultiSelectionItem = new AdminPanelMultiSelectionItem(multiplayerOptionsList[i], null, false, false, true);
					this._availableOptions.Add(adminPanelMultiSelectionItem);
					if (buildDefaultValue && adminPanelMultiSelectionItem.Value == strValue)
					{
						base.BuildDefaultValue(adminPanelMultiSelectionItem);
						base.BuildInitialValue(adminPanelMultiSelectionItem);
					}
				}
			}
			this.OnRefresh();
			return this;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001020A File Offset: 0x0000E40A
		public MBReadOnlyList<IAdminPanelMultiSelectionItem> GetAvailableOptions()
		{
			return this._availableOptions;
		}

		// Token: 0x0400011B RID: 283
		protected IAdminPanelMultiSelectionItem _selectedOption;

		// Token: 0x0400011C RID: 284
		protected MBList<IAdminPanelMultiSelectionItem> _availableOptions;
	}
}
