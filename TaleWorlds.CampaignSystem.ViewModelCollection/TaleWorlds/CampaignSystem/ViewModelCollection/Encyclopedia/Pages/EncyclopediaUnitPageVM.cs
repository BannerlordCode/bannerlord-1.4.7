using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000DA RID: 218
	[EncyclopediaViewModel(typeof(CharacterObject))]
	public class EncyclopediaUnitPageVM : EncyclopediaContentPageVM
	{
		// Token: 0x06001503 RID: 5379 RVA: 0x0005382C File Offset: 0x00051A2C
		public EncyclopediaUnitPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this._character = base.Obj as CharacterObject;
			this.UnitCharacter = new CharacterViewModel(CharacterViewModel.StanceTypes.OnMount);
			this.UnitCharacter.FillFrom(this._character, -1, null);
			this.HasErrors = this.DoesCharacterHaveCircularUpgradePaths(this._character, null);
			if (!this.HasErrors)
			{
				CharacterObject characterObject = CharacterHelper.FindUpgradeRootOf(this._character);
				this.Tree = new EncyclopediaTroopTreeNodeVM(characterObject, this._character, false, null);
			}
			this.PropertiesList = new MBBindingList<StringItemWithHintVM>();
			this.EquipmentSetSelector = new SelectorVM<EncyclopediaUnitEquipmentSetSelectorItemVM>(0, new Action<SelectorVM<EncyclopediaUnitEquipmentSetSelectorItemVM>>(this.OnEquipmentSetChange));
			base.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(this._character);
			this.RefreshValues();
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x000538F4 File Offset: 0x00051AF4
		private bool DoesCharacterHaveCircularUpgradePaths(CharacterObject baseCharacter, CharacterObject character = null)
		{
			bool flag = false;
			if (character == null)
			{
				character = baseCharacter;
			}
			for (int i = 0; i < character.UpgradeTargets.Length; i++)
			{
				if (character.UpgradeTargets[i] == baseCharacter)
				{
					Debug.FailedAssert(string.Format("Circular dependency on troop upgrade paths: {0} --> {1}", character.Name, baseCharacter.Name), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Encyclopedia\\Pages\\EncyclopediaUnitPageVM.cs", "DoesCharacterHaveCircularUpgradePaths", 56);
					flag = true;
					break;
				}
				flag = this.DoesCharacterHaveCircularUpgradePaths(baseCharacter, character.UpgradeTargets[i]);
			}
			return flag;
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x00053964 File Offset: 0x00051B64
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._equipmentSetTextObj = new TextObject("{=vggt7exj}Set {CURINDEX}/{COUNT}", null);
			this.PropertiesList.Clear();
			this.PropertiesList.Add(CampaignUIHelper.GetCharacterTierData(this._character, true));
			this.PropertiesList.Add(CampaignUIHelper.GetCharacterTypeData(this._character, true));
			this.EquipmentSetSelector.ItemList.Clear();
			using (IEnumerator<Equipment> enumerator = this._character.BattleEquipments.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Equipment equipment = enumerator.Current;
					if (!this.EquipmentSetSelector.ItemList.Any<EncyclopediaUnitEquipmentSetSelectorItemVM>((EncyclopediaUnitEquipmentSetSelectorItemVM x) => x.EquipmentSet.IsEquipmentEqualTo(equipment)))
					{
						this.EquipmentSetSelector.AddItem(new EncyclopediaUnitEquipmentSetSelectorItemVM(equipment, ""));
					}
				}
			}
			if (this.EquipmentSetSelector.ItemList.Count > 0)
			{
				this.EquipmentSetSelector.SelectedIndex = 0;
			}
			this._equipmentSetTextObj.SetTextVariable("CURINDEX", this.EquipmentSetSelector.SelectedIndex + 1);
			this._equipmentSetTextObj.SetTextVariable("COUNT", this.EquipmentSetSelector.ItemList.Count);
			this.EquipmentSetText = this._equipmentSetTextObj.ToString();
			this.TreeDisplayErrorText = new TextObject("{=BkDycbdq}Error while displaying the troop tree", null).ToString();
			this.Skills = new MBBindingList<EncyclopediaSkillVM>();
			List<SkillObject> list = TaleWorlds.CampaignSystem.Extensions.Skills.All.ToList<SkillObject>();
			list.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			foreach (SkillObject skillObject in list)
			{
				if (this._character.GetSkillValue(skillObject) > 0)
				{
					this.Skills.Add(new EncyclopediaSkillVM(skillObject, this._character.GetSkillValue(skillObject)));
				}
			}
			this.DescriptionText = GameTexts.FindText("str_encyclopedia_unit_description", this._character.StringId).ToString();
			this.NameText = this._character.Name.ToString();
			EncyclopediaTroopTreeNodeVM tree = this.Tree;
			if (tree != null)
			{
				tree.RefreshValues();
			}
			CharacterViewModel unitCharacter = this.UnitCharacter;
			if (unitCharacter != null)
			{
				unitCharacter.RefreshValues();
			}
			base.UpdateBookmarkHintText();
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x00053BB8 File Offset: 0x00051DB8
		private void OnEquipmentSetChange(SelectorVM<EncyclopediaUnitEquipmentSetSelectorItemVM> selector)
		{
			this.CurrentSelectedEquipmentSet = selector.SelectedItem;
			this.UnitCharacter.SetEquipment(this.CurrentSelectedEquipmentSet.EquipmentSet);
			this._equipmentSetTextObj.SetTextVariable("CURINDEX", selector.SelectedIndex + 1);
			this._equipmentSetTextObj.SetTextVariable("COUNT", selector.ItemList.Count);
			this.EquipmentSetText = this._equipmentSetTextObj.ToString();
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x00053C2D File Offset: 0x00051E2D
		public override string GetName()
		{
			return this._character.Name.ToString();
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x00053C40 File Offset: 0x00051E40
		public override string GetNavigationBarURL()
		{
			return HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ " + HyperlinkTexts.GetGenericHyperlinkText("ListPage-Units", GameTexts.FindText("str_encyclopedia_troops", null).ToString()) + " \\ " + this.GetName();
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x00053CA8 File Offset: 0x00051EA8
		public override void ExecuteSwitchBookmarkedState()
		{
			base.ExecuteSwitchBookmarkedState();
			if (base.IsBookmarked)
			{
				Campaign.Current.EncyclopediaManager.ViewDataTracker.AddEncyclopediaBookmarkToItem(this._character);
				return;
			}
			Campaign.Current.EncyclopediaManager.ViewDataTracker.RemoveEncyclopediaBookmarkFromItem(this._character);
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x00053CF8 File Offset: 0x00051EF8
		// (set) Token: 0x0600150B RID: 5387 RVA: 0x00053D00 File Offset: 0x00051F00
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSkillVM> Skills
		{
			get
			{
				return this._skills;
			}
			set
			{
				if (value != this._skills)
				{
					this._skills = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSkillVM>>(value, "Skills");
				}
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x0600150C RID: 5388 RVA: 0x00053D1E File Offset: 0x00051F1E
		// (set) Token: 0x0600150D RID: 5389 RVA: 0x00053D26 File Offset: 0x00051F26
		[DataSourceProperty]
		public MBBindingList<StringItemWithHintVM> PropertiesList
		{
			get
			{
				return this._propertiesList;
			}
			set
			{
				if (value != this._propertiesList)
				{
					this._propertiesList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringItemWithHintVM>>(value, "PropertiesList");
				}
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x00053D44 File Offset: 0x00051F44
		// (set) Token: 0x0600150F RID: 5391 RVA: 0x00053D4C File Offset: 0x00051F4C
		[DataSourceProperty]
		public SelectorVM<EncyclopediaUnitEquipmentSetSelectorItemVM> EquipmentSetSelector
		{
			get
			{
				return this._equipmentSetSelector;
			}
			set
			{
				if (value != this._equipmentSetSelector)
				{
					this._equipmentSetSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<EncyclopediaUnitEquipmentSetSelectorItemVM>>(value, "EquipmentSetSelector");
				}
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06001510 RID: 5392 RVA: 0x00053D6A File Offset: 0x00051F6A
		// (set) Token: 0x06001511 RID: 5393 RVA: 0x00053D72 File Offset: 0x00051F72
		[DataSourceProperty]
		public EncyclopediaUnitEquipmentSetSelectorItemVM CurrentSelectedEquipmentSet
		{
			get
			{
				return this._currentSelectedEquipmentSet;
			}
			set
			{
				if (value != this._currentSelectedEquipmentSet)
				{
					this._currentSelectedEquipmentSet = value;
					base.OnPropertyChangedWithValue<EncyclopediaUnitEquipmentSetSelectorItemVM>(value, "CurrentSelectedEquipmentSet");
				}
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x00053D90 File Offset: 0x00051F90
		// (set) Token: 0x06001513 RID: 5395 RVA: 0x00053D98 File Offset: 0x00051F98
		[DataSourceProperty]
		public CharacterViewModel UnitCharacter
		{
			get
			{
				return this._unitCharacter;
			}
			set
			{
				if (value != this._unitCharacter)
				{
					this._unitCharacter = value;
					base.OnPropertyChangedWithValue<CharacterViewModel>(value, "UnitCharacter");
				}
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x00053DB6 File Offset: 0x00051FB6
		// (set) Token: 0x06001515 RID: 5397 RVA: 0x00053DBE File Offset: 0x00051FBE
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x00053DE1 File Offset: 0x00051FE1
		// (set) Token: 0x06001517 RID: 5399 RVA: 0x00053DE9 File Offset: 0x00051FE9
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x00053E0C File Offset: 0x0005200C
		// (set) Token: 0x06001519 RID: 5401 RVA: 0x00053E14 File Offset: 0x00052014
		[DataSourceProperty]
		public EncyclopediaTroopTreeNodeVM Tree
		{
			get
			{
				return this._tree;
			}
			set
			{
				if (value != this._tree)
				{
					this._tree = value;
					base.OnPropertyChangedWithValue<EncyclopediaTroopTreeNodeVM>(value, "Tree");
				}
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x0600151A RID: 5402 RVA: 0x00053E32 File Offset: 0x00052032
		// (set) Token: 0x0600151B RID: 5403 RVA: 0x00053E3A File Offset: 0x0005203A
		[DataSourceProperty]
		public string TreeDisplayErrorText
		{
			get
			{
				return this._treeDisplayErrorText;
			}
			set
			{
				if (value != this._treeDisplayErrorText)
				{
					this._treeDisplayErrorText = value;
					base.OnPropertyChangedWithValue<string>(value, "TreeDisplayErrorText");
				}
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x0600151C RID: 5404 RVA: 0x00053E5D File Offset: 0x0005205D
		// (set) Token: 0x0600151D RID: 5405 RVA: 0x00053E65 File Offset: 0x00052065
		[DataSourceProperty]
		public string EquipmentSetText
		{
			get
			{
				return this._equipmentSetText;
			}
			set
			{
				if (value != this._equipmentSetText)
				{
					this._equipmentSetText = value;
					base.OnPropertyChangedWithValue<string>(value, "EquipmentSetText");
				}
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x0600151E RID: 5406 RVA: 0x00053E88 File Offset: 0x00052088
		// (set) Token: 0x0600151F RID: 5407 RVA: 0x00053E90 File Offset: 0x00052090
		[DataSourceProperty]
		public bool HasErrors
		{
			get
			{
				return this._hasErrors;
			}
			set
			{
				if (value != this._hasErrors)
				{
					this._hasErrors = value;
					base.OnPropertyChangedWithValue(value, "HasErrors");
				}
			}
		}

		// Token: 0x04000994 RID: 2452
		private readonly CharacterObject _character;

		// Token: 0x04000995 RID: 2453
		private TextObject _equipmentSetTextObj;

		// Token: 0x04000996 RID: 2454
		private MBBindingList<EncyclopediaSkillVM> _skills;

		// Token: 0x04000997 RID: 2455
		private MBBindingList<StringItemWithHintVM> _propertiesList;

		// Token: 0x04000998 RID: 2456
		private SelectorVM<EncyclopediaUnitEquipmentSetSelectorItemVM> _equipmentSetSelector;

		// Token: 0x04000999 RID: 2457
		private EncyclopediaUnitEquipmentSetSelectorItemVM _currentSelectedEquipmentSet;

		// Token: 0x0400099A RID: 2458
		private EncyclopediaTroopTreeNodeVM _tree;

		// Token: 0x0400099B RID: 2459
		private string _descriptionText;

		// Token: 0x0400099C RID: 2460
		private CharacterViewModel _unitCharacter;

		// Token: 0x0400099D RID: 2461
		private string _nameText;

		// Token: 0x0400099E RID: 2462
		private string _treeDisplayErrorText;

		// Token: 0x0400099F RID: 2463
		private string _equipmentSetText;

		// Token: 0x040009A0 RID: 2464
		private bool _hasErrors;
	}
}
