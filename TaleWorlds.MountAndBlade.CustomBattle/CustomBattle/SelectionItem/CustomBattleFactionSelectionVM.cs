using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x0200001E RID: 30
	public class CustomBattleFactionSelectionVM : ViewModel
	{
		// Token: 0x060001AA RID: 426 RVA: 0x0000A440 File Offset: 0x00008640
		public CustomBattleFactionSelectionVM(Action<BasicCultureObject> onSelectionChanged)
		{
			this._onSelectionChanged = onSelectionChanged;
			this.Factions = new MBBindingList<FactionItemVM>();
			foreach (BasicCultureObject basicCultureObject in CustomBattleData.Factions)
			{
				this.Factions.Add(new FactionItemVM(basicCultureObject, new Action<FactionItemVM>(this.OnFactionSelected)));
			}
			this.SelectFaction(0);
			this.RefreshValues();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000A4C8 File Offset: 0x000086C8
		public override void RefreshValues()
		{
			base.RefreshValues();
			FactionItemVM selectedItem = this.SelectedItem;
			this.SelectedFactionName = ((selectedItem != null) ? selectedItem.Faction.Name.ToString() : null);
			this.Factions.ApplyActionOnAllItems(delegate(FactionItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000A527 File Offset: 0x00008727
		public void SelectFaction(int index)
		{
			if (index >= 0 && index < this.Factions.Count)
			{
				this.SelectedItem = this.Factions[index];
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000A554 File Offset: 0x00008754
		public void ExecuteRandomize()
		{
			int num = MBRandom.RandomInt(this.Factions.Count);
			this.SelectFaction(num);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000A579 File Offset: 0x00008779
		private void OnFactionSelected(FactionItemVM faction)
		{
			this.SelectedItem = faction;
			this._onSelectionChanged(faction.Faction);
			this.SelectedFactionName = this.SelectedItem.Faction.Name.ToString();
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000A5AE File Offset: 0x000087AE
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x0000A5B6 File Offset: 0x000087B6
		[DataSourceProperty]
		public MBBindingList<FactionItemVM> Factions
		{
			get
			{
				return this._factions;
			}
			set
			{
				if (value != this._factions)
				{
					this._factions = value;
					base.OnPropertyChangedWithValue<MBBindingList<FactionItemVM>>(value, "Factions");
				}
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000A5D4 File Offset: 0x000087D4
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x0000A5DC File Offset: 0x000087DC
		[DataSourceProperty]
		public string SelectedFactionName
		{
			get
			{
				return this._selectedFactionName;
			}
			set
			{
				if (value != this._selectedFactionName)
				{
					this._selectedFactionName = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectedFactionName");
				}
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000A5FF File Offset: 0x000087FF
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x0000A608 File Offset: 0x00008808
		[DataSourceProperty]
		public FactionItemVM SelectedItem
		{
			get
			{
				return this._selectedItem;
			}
			set
			{
				if (value != this._selectedItem)
				{
					if (this._selectedItem != null)
					{
						this._selectedItem.IsSelected = false;
					}
					this._selectedItem = value;
					base.OnPropertyChangedWithValue<FactionItemVM>(value, "SelectedItem");
					if (this._selectedItem != null)
					{
						this._selectedItem.IsSelected = true;
					}
				}
			}
		}

		// Token: 0x04000104 RID: 260
		private Action<BasicCultureObject> _onSelectionChanged;

		// Token: 0x04000105 RID: 261
		private MBBindingList<FactionItemVM> _factions;

		// Token: 0x04000106 RID: 262
		private string _selectedFactionName;

		// Token: 0x04000107 RID: 263
		private FactionItemVM _selectedItem;
	}
}
