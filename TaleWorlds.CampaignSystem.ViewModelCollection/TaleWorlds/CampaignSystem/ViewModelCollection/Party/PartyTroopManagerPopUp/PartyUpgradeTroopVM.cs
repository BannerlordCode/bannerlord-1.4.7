using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party.PartyTroopManagerPopUp
{
	// Token: 0x02000035 RID: 53
	public class PartyUpgradeTroopVM : PartyTroopManagerVM
	{
		// Token: 0x06000550 RID: 1360 RVA: 0x0001CF5C File Offset: 0x0001B15C
		public PartyUpgradeTroopVM(PartyVM partyVM)
			: base(partyVM)
		{
			this.RefreshValues();
			base.IsUpgradePopUp = true;
			this._openButtonEnabledHint = new TextObject("{=hRSezxnT}Some of your troops are ready to upgrade.", null);
			this._openButtonNoTroopsHint = new TextObject("{=fpE7BQ7f}You don't have any upgradable troops.", null);
			this._openButtonIrrelevantScreenHint = new TextObject("{=mdvnjI72}Troops are not upgradable in this screen.", null);
			this._openButtonUpgradesDisabledHint = new TextObject("{=R4rTlKMU}Troop upgrades are currently disabled.", null);
			base.UsedHorsesHint = new BasicTooltipViewModel(() => this.GetUsedHorsesTooltip());
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001CFE0 File Offset: 0x0001B1E0
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.TitleText = new TextObject("{=IgoxNz2H}Upgrade Troops", null).ToString();
			this.UpgradeCostText = new TextObject("{=SK8G9QpE}Upgrd. Cost", null).ToString();
			GameTexts.SetVariable("LEFT", new TextObject("{=6bx9IhpD}Upgrades", null).ToString());
			GameTexts.SetVariable("RIGHT", new TextObject("{=guxNZZWh}Requirements", null).ToString());
			this.UpgradesAndRequirementsText = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001D06C File Offset: 0x0001B26C
		public void OnRanOutTroop(PartyCharacterVM troop)
		{
			if (!base.IsOpen)
			{
				return;
			}
			PartyTroopManagerItemVM partyTroopManagerItemVM = base.Troops.FirstOrDefault<PartyTroopManagerItemVM>((PartyTroopManagerItemVM x) => x.PartyCharacter == troop);
			base.Troops.Remove(partyTroopManagerItemVM);
			this._disabledTroopsStartIndex--;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001D0C4 File Offset: 0x0001B2C4
		public void OnTroopUpgraded()
		{
			if (!base.IsOpen)
			{
				return;
			}
			this._hasMadeChanges = true;
			for (int i = 0; i < this._disabledTroopsStartIndex; i++)
			{
				if (base.Troops[i].PartyCharacter.NumOfReadyToUpgradeTroops <= 0)
				{
					this._disabledTroopsStartIndex--;
					base.Troops.RemoveAt(i);
					i--;
				}
				else if (base.Troops[i].PartyCharacter.NumOfUpgradeableTroops <= 0)
				{
					this._disabledTroopsStartIndex--;
					PartyTroopManagerItemVM partyTroopManagerItemVM = base.Troops[i];
					base.Troops.RemoveAt(i);
					base.Troops.Insert(this._disabledTroopsStartIndex, partyTroopManagerItemVM);
					i--;
				}
			}
			base.UpdateLabels();
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001D18D File Offset: 0x0001B38D
		public override void OpenPopUp()
		{
			base.OpenPopUp();
			this.PopulateTroops();
			this.UpdateUpgradesOfAllTroops();
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001D1A1 File Offset: 0x0001B3A1
		public override void ExecuteDone()
		{
			base.ExecuteDone();
			this._partyVM.OnUpgradePopUpClosed(false);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001D1B5 File Offset: 0x0001B3B5
		public override void ExecuteCancel()
		{
			base.ShowCancelInquiry(new Action(this.ConfirmCancel));
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001D1CA File Offset: 0x0001B3CA
		protected override void ConfirmCancel()
		{
			base.ConfirmCancel();
			this._partyVM.OnUpgradePopUpClosed(true);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001D1E0 File Offset: 0x0001B3E0
		private void UpdateUpgradesOfAllTroops()
		{
			foreach (PartyTroopManagerItemVM partyTroopManagerItemVM in base.Troops)
			{
				partyTroopManagerItemVM.PartyCharacter.InitializeUpgrades();
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001D230 File Offset: 0x0001B430
		private void PopulateTroops()
		{
			base.Troops = new MBBindingList<PartyTroopManagerItemVM>();
			this._disabledTroopsStartIndex = 0;
			foreach (PartyCharacterVM partyCharacterVM in this._partyVM.MainPartyTroops)
			{
				if (partyCharacterVM.IsTroopUpgradable)
				{
					base.Troops.Insert(this._disabledTroopsStartIndex, new PartyTroopManagerItemVM(partyCharacterVM, new Action<PartyTroopManagerItemVM>(base.SetFocusedCharacter)));
					this._disabledTroopsStartIndex++;
				}
				else if (partyCharacterVM.NumOfReadyToUpgradeTroops > 0)
				{
					base.Troops.Add(new PartyTroopManagerItemVM(partyCharacterVM, new Action<PartyTroopManagerItemVM>(base.SetFocusedCharacter)));
				}
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001D2F0 File Offset: 0x0001B4F0
		private List<TooltipProperty> GetUsedHorsesTooltip()
		{
			List<Tuple<EquipmentElement, int>> list = this._partyVM.PartyScreenLogic.CurrentData.UsedUpgradeHorsesHistory.ToList<Tuple<EquipmentElement, int>>();
			using (List<Tuple<EquipmentElement, int>>.Enumerator enumerator = this._initialUsedUpgradeHorsesHistory.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tuple<EquipmentElement, int> item = enumerator.Current;
					int num = list.FindIndex((Tuple<EquipmentElement, int> x) => x.Item1.IsEqualTo(item.Item1));
					if (num != -1)
					{
						if (list[num].Item2 > item.Item2)
						{
							list[num] = new Tuple<EquipmentElement, int>(list[num].Item1, list[num].Item2 - item.Item2);
						}
						else
						{
							list.RemoveAt(num);
						}
					}
				}
			}
			return CampaignUIHelper.GetUsedHorsesTooltip(list);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001D3D4 File Offset: 0x0001B5D4
		public override void ExecuteItemPrimaryAction()
		{
			this.UpgradeTroopAtIndex(0);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001D3DD File Offset: 0x0001B5DD
		public override void ExecuteItemSecondaryAction()
		{
			this.UpgradeTroopAtIndex(1);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001D3E6 File Offset: 0x0001B5E6
		public override void ExecuteItemTertiaryAction()
		{
			this.UpgradeTroopAtIndex(2);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001D3F0 File Offset: 0x0001B5F0
		private void UpgradeTroopAtIndex(int upgradeIndex)
		{
			PartyTroopManagerItemVM focusedTroop = base.FocusedTroop;
			PartyCharacterVM partyCharacterVM = ((focusedTroop != null) ? focusedTroop.PartyCharacter : null);
			if (partyCharacterVM != null && partyCharacterVM.Upgrades.Count > upgradeIndex && partyCharacterVM.Upgrades[upgradeIndex].IsAvailable)
			{
				partyCharacterVM.Upgrades[upgradeIndex].ExecuteUpgrade();
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0001D448 File Offset: 0x0001B648
		// (set) Token: 0x06000560 RID: 1376 RVA: 0x0001D450 File Offset: 0x0001B650
		[DataSourceProperty]
		public string UpgradeCostText
		{
			get
			{
				return this._upgradeCostText;
			}
			set
			{
				if (value != this._upgradeCostText)
				{
					this._upgradeCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "UpgradeCostText");
				}
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x0001D473 File Offset: 0x0001B673
		// (set) Token: 0x06000562 RID: 1378 RVA: 0x0001D47B File Offset: 0x0001B67B
		[DataSourceProperty]
		public string UpgradesAndRequirementsText
		{
			get
			{
				return this._upgradesAndRequirementsText;
			}
			set
			{
				if (value != this._upgradesAndRequirementsText)
				{
					this._upgradesAndRequirementsText = value;
					base.OnPropertyChangedWithValue<string>(value, "UpgradesAndRequirementsText");
				}
			}
		}

		// Token: 0x0400024F RID: 591
		private int _disabledTroopsStartIndex = -1;

		// Token: 0x04000250 RID: 592
		private string _upgradeCostText;

		// Token: 0x04000251 RID: 593
		private string _upgradesAndRequirementsText;
	}
}
