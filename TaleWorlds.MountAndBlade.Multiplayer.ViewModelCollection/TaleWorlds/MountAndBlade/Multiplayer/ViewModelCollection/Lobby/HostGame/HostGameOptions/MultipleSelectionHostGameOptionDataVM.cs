using System;
using System.Collections.Generic;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions
{
	// Token: 0x0200004A RID: 74
	public class MultipleSelectionHostGameOptionDataVM : GenericHostGameOptionDataVM
	{
		// Token: 0x06000695 RID: 1685 RVA: 0x00015564 File Offset: 0x00013764
		public MultipleSelectionHostGameOptionDataVM(MultiplayerOptions.OptionType optionType, int preferredIndex)
			: base(OptionsVM.OptionsDataType.MultipleSelectionOption, optionType, preferredIndex)
		{
			List<string> multiplayerOptionsList = MultiplayerOptions.Instance.GetMultiplayerOptionsList(base.OptionType);
			List<string> multiplayerOptionsTextList = MultiplayerOptions.Instance.GetMultiplayerOptionsTextList(base.OptionType);
			List<string> list = new List<string>();
			foreach (string text in multiplayerOptionsTextList)
			{
				list.Add(text);
			}
			this.Selector = new SelectorVM<SelectorItemVM>(list, multiplayerOptionsList.IndexOf(MultiplayerOptions.Instance.GetValueTextForOptionWithMultipleSelection(base.OptionType)), null);
			this.Selector.SetOnChangeAction(new Action<SelectorVM<SelectorItemVM>>(this.OnChangeSelected));
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001561C File Offset: 0x0001381C
		public override void RefreshData()
		{
			this.Selector.SetOnChangeAction(null);
			List<string> multiplayerOptionsList = MultiplayerOptions.Instance.GetMultiplayerOptionsList(base.OptionType);
			List<string> multiplayerOptionsTextList = MultiplayerOptions.Instance.GetMultiplayerOptionsTextList(base.OptionType);
			List<string> list = new List<string>();
			foreach (string text in multiplayerOptionsTextList)
			{
				list.Add(text);
			}
			int num = multiplayerOptionsList.IndexOf(MultiplayerOptions.Instance.GetValueTextForOptionWithMultipleSelection(base.OptionType));
			if (num != this.Selector.SelectedIndex)
			{
				this.Selector.SelectedIndex = num;
			}
			this.Selector.SetOnChangeAction(new Action<SelectorVM<SelectorItemVM>>(this.OnChangeSelected));
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000156E8 File Offset: 0x000138E8
		public void RefreshList()
		{
			List<string> multiplayerOptionsList = MultiplayerOptions.Instance.GetMultiplayerOptionsList(base.OptionType);
			List<string> multiplayerOptionsTextList = MultiplayerOptions.Instance.GetMultiplayerOptionsTextList(base.OptionType);
			List<string> list = new List<string>();
			foreach (string text in multiplayerOptionsTextList)
			{
				list.Add(text);
			}
			this.Selector.Refresh(list, multiplayerOptionsList.IndexOf(MultiplayerOptions.Instance.GetValueTextForOptionWithMultipleSelection(base.OptionType)), new Action<SelectorVM<SelectorItemVM>>(this.OnChangeSelected));
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001578C File Offset: 0x0001398C
		private void OnChangeSelected(SelectorVM<SelectorItemVM> selector)
		{
			if (selector.SelectedIndex < 0 || selector.SelectedIndex >= selector.ItemList.Count)
			{
				return;
			}
			string text = MultiplayerOptions.Instance.GetMultiplayerOptionsList(base.OptionType)[selector.SelectedIndex];
			MultiplayerOptions.Instance.SetValueForOptionWithMultipleSelectionFromText(base.OptionType, text);
			Action<MultipleSelectionHostGameOptionDataVM> onChangedSelection = this.OnChangedSelection;
			if (onChangedSelection == null)
			{
				return;
			}
			onChangedSelection(this);
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x000157F4 File Offset: 0x000139F4
		// (set) Token: 0x0600069A RID: 1690 RVA: 0x000157FC File Offset: 0x000139FC
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> Selector
		{
			get
			{
				return this._selector;
			}
			set
			{
				if (value != this._selector)
				{
					this._selector = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "Selector");
				}
			}
		}

		// Token: 0x0400031A RID: 794
		public Action<MultipleSelectionHostGameOptionDataVM> OnChangedSelection;

		// Token: 0x0400031B RID: 795
		private SelectorVM<SelectorItemVM> _selector;
	}
}
