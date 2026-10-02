using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection
{
	// Token: 0x02000148 RID: 328
	public class PerkSelectionVM : ViewModel
	{
		// Token: 0x06001F7E RID: 8062 RVA: 0x000739D1 File Offset: 0x00071BD1
		public PerkSelectionVM(HeroDeveloper developer, Action<SkillObject> refreshPerksOf, Action onPerkSelection)
		{
			this._developer = developer;
			this._refreshPerksOf = refreshPerksOf;
			this._onPerkSelection = onPerkSelection;
			this._selectedPerks = new List<PerkObject>();
			this.AvailablePerks = new MBBindingList<PerkSelectionItemVM>();
			this.IsActive = false;
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x00073A0C File Offset: 0x00071C0C
		public void SetCurrentSelectionPerk(PerkVM perk)
		{
			if (this.AvailablePerks.Count > 0 || this.IsActive)
			{
				this.ExecuteDeactivate();
			}
			this.AvailablePerks.Clear();
			this._currentInitialPerk = perk;
			this.AvailablePerks.Add(new PerkSelectionItemVM(perk.Perk, new Action<PerkSelectionItemVM>(this.OnSelectPerk)));
			if (perk.AlternativeType == 2)
			{
				this.AvailablePerks.Insert(0, new PerkSelectionItemVM(perk.Perk.AlternativePerk, new Action<PerkSelectionItemVM>(this.OnSelectPerk)));
			}
			else if (perk.AlternativeType == 1)
			{
				this.AvailablePerks.Add(new PerkSelectionItemVM(perk.Perk.AlternativePerk, new Action<PerkSelectionItemVM>(this.OnSelectPerk)));
			}
			this.IsActive = true;
			this.OnSelectPerk(perk);
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x00073ADC File Offset: 0x00071CDC
		private void OnSelectPerk(PerkVM selectedPerk)
		{
			this._selectedPerks.Add(selectedPerk.Perk);
			this._refreshPerksOf(selectedPerk.Perk.Skill);
			this.IsActive = false;
			Game.Current.EventManager.TriggerEvent<PerkSelectedByPlayerEvent>(new PerkSelectedByPlayerEvent(selectedPerk.Perk));
			Action onPerkSelection = this._onPerkSelection;
			if (onPerkSelection == null)
			{
				return;
			}
			onPerkSelection();
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x00073B44 File Offset: 0x00071D44
		private void OnSelectPerk(PerkSelectionItemVM selectedPerk)
		{
			this._selectedPerks.Add(selectedPerk.Perk);
			this._refreshPerksOf(selectedPerk.Perk.Skill);
			this.IsActive = false;
			Game.Current.EventManager.TriggerEvent<PerkSelectedByPlayerEvent>(new PerkSelectedByPlayerEvent(selectedPerk.Perk));
			Action onPerkSelection = this._onPerkSelection;
			if (onPerkSelection == null)
			{
				return;
			}
			onPerkSelection();
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x00073BAC File Offset: 0x00071DAC
		public void ResetSelectedPerks()
		{
			foreach (PerkObject perkObject in this._selectedPerks)
			{
				this._refreshPerksOf(perkObject.Skill);
			}
			this._selectedPerks.Clear();
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x00073C14 File Offset: 0x00071E14
		public void ApplySelectedPerks()
		{
			foreach (PerkObject perkObject in this._selectedPerks.ToList<PerkObject>())
			{
				this._developer.AddPerk(perkObject);
				this._selectedPerks.Remove(perkObject);
			}
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x00073C80 File Offset: 0x00071E80
		public bool IsPerkSelected(PerkObject perk)
		{
			return this._selectedPerks.Contains(perk);
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x00073C8E File Offset: 0x00071E8E
		public bool IsAnyPerkSelected()
		{
			return this._selectedPerks.Count > 0;
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x00073C9E File Offset: 0x00071E9E
		public void ExecuteDeactivate()
		{
			this.IsActive = false;
			this._refreshPerksOf(this._currentInitialPerk.Perk.Skill);
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x00073CC2 File Offset: 0x00071EC2
		// (set) Token: 0x06001F88 RID: 8072 RVA: 0x00073CCA File Offset: 0x00071ECA
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
					Game.Current.EventManager.TriggerEvent<PerkSelectionToggleEvent>(new PerkSelectionToggleEvent(this.IsActive));
				}
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06001F89 RID: 8073 RVA: 0x00073D02 File Offset: 0x00071F02
		// (set) Token: 0x06001F8A RID: 8074 RVA: 0x00073D0A File Offset: 0x00071F0A
		[DataSourceProperty]
		public MBBindingList<PerkSelectionItemVM> AvailablePerks
		{
			get
			{
				return this._availablePerks;
			}
			set
			{
				if (value != this._availablePerks)
				{
					this._availablePerks = value;
					base.OnPropertyChangedWithValue<MBBindingList<PerkSelectionItemVM>>(value, "AvailablePerks");
				}
			}
		}

		// Token: 0x04000EB3 RID: 3763
		private readonly HeroDeveloper _developer;

		// Token: 0x04000EB4 RID: 3764
		private readonly List<PerkObject> _selectedPerks;

		// Token: 0x04000EB5 RID: 3765
		private readonly Action<SkillObject> _refreshPerksOf;

		// Token: 0x04000EB6 RID: 3766
		private readonly Action _onPerkSelection;

		// Token: 0x04000EB7 RID: 3767
		private PerkVM _currentInitialPerk;

		// Token: 0x04000EB8 RID: 3768
		private bool _isActive;

		// Token: 0x04000EB9 RID: 3769
		private MBBindingList<PerkSelectionItemVM> _availablePerks;
	}
}
