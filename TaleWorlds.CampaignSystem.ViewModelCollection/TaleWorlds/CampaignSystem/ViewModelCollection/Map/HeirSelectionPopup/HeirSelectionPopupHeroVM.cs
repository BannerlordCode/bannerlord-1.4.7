using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MarriageOfferPopup;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.HeirSelectionPopup
{
	// Token: 0x02000060 RID: 96
	public class HeirSelectionPopupHeroVM : ViewModel
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0002291C File Offset: 0x00020B1C
		public Hero Hero { get; }

		// Token: 0x0600071F RID: 1823 RVA: 0x00022924 File Offset: 0x00020B24
		public HeirSelectionPopupHeroVM(Hero hero)
		{
			this.Hero = hero;
			this.FillHeroInformation();
			this.CreateImageIdentifier();
			this.CreateHeroModel();
			this.RefreshValues();
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0002294C File Offset: 0x00020B4C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.Hero.Name.ToString();
			this.Culture = this.Hero.Culture.Name.ToString();
			this.Occupation = CampaignUIHelper.GetHeroOccupationName(this.Hero);
			this.RelationToMainHero = CampaignUIHelper.GetHeroRelationToHeroText(this.Hero, Hero.MainHero, true).ToString();
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x000229C0 File Offset: 0x00020BC0
		public override void OnFinalize()
		{
			HeroViewModel model = this.Model;
			if (model != null)
			{
				model.OnFinalize();
			}
			CharacterImageIdentifierVM imageIdentifier = this.ImageIdentifier;
			if (imageIdentifier != null)
			{
				imageIdentifier.OnFinalize();
			}
			MBBindingList<EncyclopediaTraitItemVM> traits = this.Traits;
			if (traits != null)
			{
				traits.ApplyActionOnAllItems(delegate(EncyclopediaTraitItemVM x)
				{
					x.OnFinalize();
				});
			}
			MBBindingList<EncyclopediaTraitItemVM> traits2 = this.Traits;
			if (traits2 != null)
			{
				traits2.Clear();
			}
			MBBindingList<MarriageOfferPopupHeroAttributeVM> attributes = this.Attributes;
			if (attributes != null)
			{
				attributes.ApplyActionOnAllItems(delegate(MarriageOfferPopupHeroAttributeVM x)
				{
					x.OnFinalize();
				});
			}
			MBBindingList<MarriageOfferPopupHeroAttributeVM> attributes2 = this.Attributes;
			if (attributes2 != null)
			{
				attributes2.Clear();
			}
			MBBindingList<EncyclopediaSkillVM> otherSkills = this.OtherSkills;
			if (otherSkills != null)
			{
				otherSkills.ApplyActionOnAllItems(delegate(EncyclopediaSkillVM x)
				{
					x.OnFinalize();
				});
			}
			MBBindingList<EncyclopediaSkillVM> otherSkills2 = this.OtherSkills;
			if (otherSkills2 != null)
			{
				otherSkills2.Clear();
			}
			base.OnFinalize();
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00022AB8 File Offset: 0x00020CB8
		private void CreateImageIdentifier()
		{
			this.ImageIdentifier = new CharacterImageIdentifierVM(CharacterCode.CreateFrom(this.Hero.CharacterObject));
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00022AD8 File Offset: 0x00020CD8
		private void CreateHeroModel()
		{
			this.Model = new HeroViewModel(CharacterViewModel.StanceTypes.None);
			this.Model.FillFrom(this.Hero, -1, false, true);
			this.Model.SetEquipment(EquipmentIndex.ArmorItemEndSlot, default(EquipmentElement));
			this.Model.SetEquipment(EquipmentIndex.HorseHarness, default(EquipmentElement));
			this.Model.SetEquipment(EquipmentIndex.NumAllWeaponSlots, default(EquipmentElement));
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00022B48 File Offset: 0x00020D48
		private void FillHeroInformation()
		{
			this.Age = (int)this.Hero.Age;
			this.Traits = new MBBindingList<EncyclopediaTraitItemVM>();
			this.Attributes = new MBBindingList<MarriageOfferPopupHeroAttributeVM>();
			this.OtherSkills = new MBBindingList<EncyclopediaSkillVM>();
			List<CharacterAttribute> list = TaleWorlds.CampaignSystem.Extensions.Attributes.All.ToList<CharacterAttribute>();
			list.Sort(CampaignUIHelper.CharacterAttributeComparerInstance);
			foreach (CharacterAttribute characterAttribute in list)
			{
				this.Attributes.Add(new MarriageOfferPopupHeroAttributeVM(this.Hero, characterAttribute));
			}
			List<SkillObject> list2 = Skills.All.ToList<SkillObject>();
			list2.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			using (List<SkillObject>.Enumerator enumerator2 = list2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					SkillObject skill = enumerator2.Current;
					Func<EncyclopediaSkillVM, bool> <>9__1;
					if (!this.Attributes.Any<MarriageOfferPopupHeroAttributeVM>(delegate(MarriageOfferPopupHeroAttributeVM attribute)
					{
						IEnumerable<EncyclopediaSkillVM> attributeSkills = attribute.AttributeSkills;
						Func<EncyclopediaSkillVM, bool> func;
						if ((func = <>9__1) == null)
						{
							func = (<>9__1 = (EncyclopediaSkillVM attributeSkill) => attributeSkill.SkillId == skill.StringId);
						}
						return attributeSkills.Any<EncyclopediaSkillVM>(func);
					}))
					{
						this.OtherSkills.Add(new EncyclopediaSkillVM(skill, this.Hero.GetSkillValue(skill)));
					}
				}
			}
			this.HasOtherSkills = this.OtherSkills.Count > 0;
			foreach (TraitObject traitObject in CampaignUIHelper.GetHeroTraits())
			{
				if (this.Hero.GetTraitLevel(traitObject) != 0)
				{
					this.Traits.Add(new EncyclopediaTraitItemVM(traitObject, this.Hero));
				}
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x00022CF8 File Offset: 0x00020EF8
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x00022D00 File Offset: 0x00020F00
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

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x00022D23 File Offset: 0x00020F23
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x00022D2B File Offset: 0x00020F2B
		[DataSourceProperty]
		public int Age
		{
			get
			{
				return this._age;
			}
			set
			{
				if (value != this._age)
				{
					this._age = value;
					base.OnPropertyChangedWithValue(value, "Age");
				}
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x00022D49 File Offset: 0x00020F49
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x00022D51 File Offset: 0x00020F51
		[DataSourceProperty]
		public string Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				if (value != this._culture)
				{
					this._culture = value;
					base.OnPropertyChangedWithValue<string>(value, "Culture");
				}
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x00022D74 File Offset: 0x00020F74
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x00022D7C File Offset: 0x00020F7C
		[DataSourceProperty]
		public string Occupation
		{
			get
			{
				return this._occupation;
			}
			set
			{
				if (value != this._occupation)
				{
					this._occupation = value;
					base.OnPropertyChangedWithValue<string>(value, "Occupation");
				}
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x00022D9F File Offset: 0x00020F9F
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x00022DA7 File Offset: 0x00020FA7
		[DataSourceProperty]
		public string RelationToMainHero
		{
			get
			{
				return this._relationToMainHero;
			}
			set
			{
				if (value != this._relationToMainHero)
				{
					this._relationToMainHero = value;
					base.OnPropertyChangedWithValue<string>(value, "RelationToMainHero");
				}
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00022DCA File Offset: 0x00020FCA
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x00022DD2 File Offset: 0x00020FD2
		[DataSourceProperty]
		public HeroViewModel Model
		{
			get
			{
				return this._model;
			}
			set
			{
				if (value != this._model)
				{
					this._model = value;
					base.OnPropertyChangedWithValue<HeroViewModel>(value, "Model");
				}
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00022DF0 File Offset: 0x00020FF0
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x00022DF8 File Offset: 0x00020FF8
		[DataSourceProperty]
		public CharacterImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00022E16 File Offset: 0x00021016
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x00022E1E File Offset: 0x0002101E
		[DataSourceProperty]
		public MBBindingList<EncyclopediaTraitItemVM> Traits
		{
			get
			{
				return this._traits;
			}
			set
			{
				if (value != this._traits)
				{
					this._traits = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaTraitItemVM>>(value, "Traits");
				}
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00022E3C File Offset: 0x0002103C
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x00022E44 File Offset: 0x00021044
		[DataSourceProperty]
		public MBBindingList<MarriageOfferPopupHeroAttributeVM> Attributes
		{
			get
			{
				return this._attributes;
			}
			set
			{
				if (value != this._attributes)
				{
					this._attributes = value;
					base.OnPropertyChangedWithValue<MBBindingList<MarriageOfferPopupHeroAttributeVM>>(value, "Attributes");
				}
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x00022E62 File Offset: 0x00021062
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x00022E6A File Offset: 0x0002106A
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00022E88 File Offset: 0x00021088
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x00022E90 File Offset: 0x00021090
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSkillVM> OtherSkills
		{
			get
			{
				return this._otherSkills;
			}
			set
			{
				if (value != this._otherSkills)
				{
					this._otherSkills = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSkillVM>>(value, "OtherSkills");
				}
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x00022EAE File Offset: 0x000210AE
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x00022EB6 File Offset: 0x000210B6
		[DataSourceProperty]
		public bool HasOtherSkills
		{
			get
			{
				return this._hasOtherSkills;
			}
			set
			{
				if (value != this._hasOtherSkills)
				{
					this._hasOtherSkills = value;
					base.OnPropertyChangedWithValue(value, "HasOtherSkills");
				}
			}
		}

		// Token: 0x0400031D RID: 797
		private string _name;

		// Token: 0x0400031E RID: 798
		private int _age;

		// Token: 0x0400031F RID: 799
		private string _culture;

		// Token: 0x04000320 RID: 800
		private string _occupation;

		// Token: 0x04000321 RID: 801
		private string _relationToMainHero;

		// Token: 0x04000322 RID: 802
		private HeroViewModel _model;

		// Token: 0x04000323 RID: 803
		private CharacterImageIdentifierVM _imageIdentifier;

		// Token: 0x04000324 RID: 804
		private MBBindingList<EncyclopediaTraitItemVM> _traits;

		// Token: 0x04000325 RID: 805
		private MBBindingList<MarriageOfferPopupHeroAttributeVM> _attributes;

		// Token: 0x04000326 RID: 806
		private bool _isSelected;

		// Token: 0x04000327 RID: 807
		private MBBindingList<EncyclopediaSkillVM> _otherSkills;

		// Token: 0x04000328 RID: 808
		private bool _hasOtherSkills;
	}
}
