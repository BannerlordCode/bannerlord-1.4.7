using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper
{
	// Token: 0x02000140 RID: 320
	public class CharacterAttributeItemVM : ViewModel
	{
		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06001E3E RID: 7742 RVA: 0x0006FFAF File Offset: 0x0006E1AF
		// (set) Token: 0x06001E3F RID: 7743 RVA: 0x0006FFB7 File Offset: 0x0006E1B7
		public CharacterAttribute AttributeType { get; private set; }

		// Token: 0x06001E40 RID: 7744 RVA: 0x0006FFC0 File Offset: 0x0006E1C0
		public CharacterAttributeItemVM(Hero hero, CharacterAttribute currAtt, CharacterDeveloperHeroItemVM developerVM, Action<CharacterAttributeItemVM> onInpectAttribute, Action<CharacterAttributeItemVM> onAddAttributePoint)
		{
			this._hero = hero;
			this._developer = this._hero.HeroDeveloper;
			this._characterVM = developerVM;
			this.AttributeType = currAtt;
			this._onInpectAttribute = onInpectAttribute;
			this._onAddAttributePoint = onAddAttributePoint;
			this._initialAttValue = this._characterVM.CharacterAttributes.GetPropertyValue(currAtt);
			this.AttributeValue = this._initialAttValue;
			this.BoundSkills = new MBBindingList<AttributeBoundSkillItemVM>();
			this.RefreshWithCurrentValues();
			this.RefreshValues();
			this.UnspentAttributePoints = this._characterVM.UnspentAttributePoints;
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x00070054 File Offset: 0x0006E254
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.AttributeType.Abbreviation.ToString();
			string text = this.AttributeType.Description.ToString();
			GameTexts.SetVariable("STR1", text);
			GameTexts.SetVariable("ATTRIBUTE_NAME", this.AttributeType.Name);
			TextObject textObject = GameTexts.FindText("str_skill_attribute_bound_skills", null);
			textObject.SetTextVariable("IS_SOCIAL", (this.AttributeType == DefaultCharacterAttributes.Social) ? 1 : 0);
			GameTexts.SetVariable("STR2", textObject);
			this.Description = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			TextObject textObject2 = GameTexts.FindText("str_skill_attribute_increase_description", null);
			textObject2.SetTextVariable("IS_SOCIAL", (this.AttributeType == DefaultCharacterAttributes.Social) ? 1 : 0);
			GameTexts.SetVariable("NUMBER", this.UnspentAttributePoints);
			this.UnspentAttributePointsText = GameTexts.FindText("str_free_attribute_points", null).ToString();
			this.IncreaseHelpText = textObject2.ToString();
			this.BoundSkills.Clear();
			List<SkillObject> list = Skills.All.ToList<SkillObject>();
			list.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			using (List<SkillObject>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SkillObject skill = enumerator.Current;
					if (skill.Attributes.Contains(this.AttributeType) && !this.BoundSkills.Any<AttributeBoundSkillItemVM>((AttributeBoundSkillItemVM s) => s.SkillId == skill.StringId))
					{
						this.BoundSkills.Add(new AttributeBoundSkillItemVM(skill));
					}
				}
			}
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x00070204 File Offset: 0x0006E404
		public void ExecuteInspectAttribute()
		{
			Action<CharacterAttributeItemVM> onInpectAttribute = this._onInpectAttribute;
			if (onInpectAttribute == null)
			{
				return;
			}
			onInpectAttribute(this);
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x00070217 File Offset: 0x0006E417
		public void ExecuteAddAttributePoint()
		{
			Action<CharacterAttributeItemVM> onAddAttributePoint = this._onAddAttributePoint;
			if (onAddAttributePoint != null)
			{
				onAddAttributePoint(this);
			}
			this.UnspentAttributePoints = this._characterVM.UnspentAttributePoints;
			this.RefreshWithCurrentValues();
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x00070242 File Offset: 0x0006E442
		public void Reset()
		{
			this.RefreshWithCurrentValues();
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x0007024C File Offset: 0x0006E44C
		public void RefreshWithCurrentValues()
		{
			this.UnspentAttributePoints = this._characterVM.UnspentAttributePoints;
			this.AttributeValue = this._characterVM.CharacterAttributes.GetPropertyValue(this.AttributeType);
			this.CanAddPoint = this.AttributeValue < Campaign.Current.Models.CharacterDevelopmentModel.MaxAttribute && this._characterVM.UnspentAttributePoints > 0;
			this.IsAttributeAtMax = this.AttributeValue >= Campaign.Current.Models.CharacterDevelopmentModel.MaxAttribute;
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x000702E0 File Offset: 0x0006E4E0
		public void Commit()
		{
			for (int i = 0; i < this.AttributeValue - this._initialAttValue; i++)
			{
				this._developer.AddAttribute(this.AttributeType, 1, true);
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06001E47 RID: 7751 RVA: 0x00070318 File Offset: 0x0006E518
		// (set) Token: 0x06001E48 RID: 7752 RVA: 0x00070320 File Offset: 0x0006E520
		[DataSourceProperty]
		public MBBindingList<AttributeBoundSkillItemVM> BoundSkills
		{
			get
			{
				return this._boundSkills;
			}
			set
			{
				if (value != this._boundSkills)
				{
					this._boundSkills = value;
					base.OnPropertyChangedWithValue<MBBindingList<AttributeBoundSkillItemVM>>(value, "BoundSkills");
				}
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06001E49 RID: 7753 RVA: 0x0007033E File Offset: 0x0006E53E
		// (set) Token: 0x06001E4A RID: 7754 RVA: 0x00070346 File Offset: 0x0006E546
		[DataSourceProperty]
		public int AttributeValue
		{
			get
			{
				return this._atttributeValue;
			}
			set
			{
				if (value != this._atttributeValue)
				{
					this._atttributeValue = value;
					base.OnPropertyChangedWithValue(value, "AttributeValue");
				}
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06001E4B RID: 7755 RVA: 0x00070364 File Offset: 0x0006E564
		// (set) Token: 0x06001E4C RID: 7756 RVA: 0x0007036C File Offset: 0x0006E56C
		[DataSourceProperty]
		public int UnspentAttributePoints
		{
			get
			{
				return this._unspentAttributePoints;
			}
			set
			{
				if (value != this._unspentAttributePoints)
				{
					this._unspentAttributePoints = value;
					base.OnPropertyChangedWithValue(value, "UnspentAttributePoints");
					GameTexts.SetVariable("NUMBER", value);
					this.UnspentAttributePointsText = GameTexts.FindText("str_free_attribute_points", null).ToString();
				}
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06001E4D RID: 7757 RVA: 0x000703AB File Offset: 0x0006E5AB
		// (set) Token: 0x06001E4E RID: 7758 RVA: 0x000703B3 File Offset: 0x0006E5B3
		[DataSourceProperty]
		public string UnspentAttributePointsText
		{
			get
			{
				return this._unspentAttributePointsText;
			}
			set
			{
				if (value != this._unspentAttributePointsText)
				{
					this._unspentAttributePointsText = value;
					base.OnPropertyChangedWithValue<string>(value, "UnspentAttributePointsText");
				}
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06001E4F RID: 7759 RVA: 0x000703D6 File Offset: 0x0006E5D6
		// (set) Token: 0x06001E50 RID: 7760 RVA: 0x000703DE File Offset: 0x0006E5DE
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

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06001E51 RID: 7761 RVA: 0x00070401 File Offset: 0x0006E601
		// (set) Token: 0x06001E52 RID: 7762 RVA: 0x00070409 File Offset: 0x0006E609
		[DataSourceProperty]
		public string NameExtended
		{
			get
			{
				return this._nameExtended;
			}
			set
			{
				if (value != this._nameExtended)
				{
					this._nameExtended = value;
					base.OnPropertyChangedWithValue<string>(value, "NameExtended");
				}
			}
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06001E53 RID: 7763 RVA: 0x0007042C File Offset: 0x0006E62C
		// (set) Token: 0x06001E54 RID: 7764 RVA: 0x00070434 File Offset: 0x0006E634
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06001E55 RID: 7765 RVA: 0x00070457 File Offset: 0x0006E657
		// (set) Token: 0x06001E56 RID: 7766 RVA: 0x0007045F File Offset: 0x0006E65F
		[DataSourceProperty]
		public string IncreaseHelpText
		{
			get
			{
				return this._increaseHelpText;
			}
			set
			{
				if (value != this._increaseHelpText)
				{
					this._increaseHelpText = value;
					base.OnPropertyChangedWithValue<string>(value, "IncreaseHelpText");
				}
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06001E57 RID: 7767 RVA: 0x00070482 File Offset: 0x0006E682
		// (set) Token: 0x06001E58 RID: 7768 RVA: 0x0007048A File Offset: 0x0006E68A
		[DataSourceProperty]
		public bool IsInspecting
		{
			get
			{
				return this._isInspecting;
			}
			set
			{
				if (value != this._isInspecting)
				{
					this._isInspecting = value;
					base.OnPropertyChangedWithValue(value, "IsInspecting");
				}
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06001E59 RID: 7769 RVA: 0x000704A8 File Offset: 0x0006E6A8
		// (set) Token: 0x06001E5A RID: 7770 RVA: 0x000704B0 File Offset: 0x0006E6B0
		[DataSourceProperty]
		public bool IsAttributeAtMax
		{
			get
			{
				return this._isAttributeAtMax;
			}
			set
			{
				if (value != this._isAttributeAtMax)
				{
					this._isAttributeAtMax = value;
					base.OnPropertyChangedWithValue(value, "IsAttributeAtMax");
				}
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06001E5B RID: 7771 RVA: 0x000704CE File Offset: 0x0006E6CE
		// (set) Token: 0x06001E5C RID: 7772 RVA: 0x000704D6 File Offset: 0x0006E6D6
		[DataSourceProperty]
		public bool CanAddPoint
		{
			get
			{
				return this._canAddPoint;
			}
			set
			{
				if (value != this._canAddPoint)
				{
					this._canAddPoint = value;
					base.OnPropertyChangedWithValue(value, "CanAddPoint");
				}
			}
		}

		// Token: 0x04000E20 RID: 3616
		private readonly Hero _hero;

		// Token: 0x04000E22 RID: 3618
		private readonly HeroDeveloper _developer;

		// Token: 0x04000E23 RID: 3619
		private readonly int _initialAttValue;

		// Token: 0x04000E24 RID: 3620
		private readonly Action<CharacterAttributeItemVM> _onInpectAttribute;

		// Token: 0x04000E25 RID: 3621
		private readonly Action<CharacterAttributeItemVM> _onAddAttributePoint;

		// Token: 0x04000E26 RID: 3622
		private readonly CharacterDeveloperHeroItemVM _characterVM;

		// Token: 0x04000E27 RID: 3623
		private int _atttributeValue;

		// Token: 0x04000E28 RID: 3624
		private int _unspentAttributePoints;

		// Token: 0x04000E29 RID: 3625
		private string _unspentAttributePointsText;

		// Token: 0x04000E2A RID: 3626
		private bool _canAddPoint;

		// Token: 0x04000E2B RID: 3627
		private bool _isInspecting;

		// Token: 0x04000E2C RID: 3628
		private bool _isAttributeAtMax;

		// Token: 0x04000E2D RID: 3629
		private string _name;

		// Token: 0x04000E2E RID: 3630
		private string _nameExtended;

		// Token: 0x04000E2F RID: 3631
		private string _description;

		// Token: 0x04000E30 RID: 3632
		private string _increaseHelpText;

		// Token: 0x04000E31 RID: 3633
		private MBBindingList<AttributeBoundSkillItemVM> _boundSkills;
	}
}
