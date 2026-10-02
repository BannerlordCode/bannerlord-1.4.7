using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000150 RID: 336
	public class CharacterCreationGainGroupItemVM : ViewModel
	{
		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06001FEB RID: 8171 RVA: 0x00075326 File Offset: 0x00073526
		// (set) Token: 0x06001FEC RID: 8172 RVA: 0x0007532E File Offset: 0x0007352E
		public CharacterAttribute AttributeObj { get; private set; }

		// Token: 0x06001FED RID: 8173 RVA: 0x00075338 File Offset: 0x00073538
		public CharacterCreationGainGroupItemVM(CharacterAttribute attributeObj)
		{
			this.AttributeObj = attributeObj;
			this.Skills = new MBBindingList<CharacterCreationGainedSkillItemVM>();
			this.Attribute = new CharacterCreationGainedAttributeItemVM(this.AttributeObj);
			List<SkillObject> list = TaleWorlds.CampaignSystem.Extensions.Skills.All.ToList<SkillObject>();
			list.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			using (List<SkillObject>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SkillObject skill = enumerator.Current;
					if (!CampaignUIHelper.GetIsNavalSkill(skill) && skill.Attributes.FirstOrDefault<CharacterAttribute>() == attributeObj && !this.Skills.Any<CharacterCreationGainedSkillItemVM>((CharacterCreationGainedSkillItemVM s) => s.SkillObj == skill))
					{
						this.Skills.Add(new CharacterCreationGainedSkillItemVM(skill));
					}
				}
			}
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x00075418 File Offset: 0x00073618
		public void ResetValues()
		{
			this.Attribute.ResetValues();
			this.Skills.ApplyActionOnAllItems(delegate(CharacterCreationGainedSkillItemVM s)
			{
				s.ResetValues();
			});
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06001FEF RID: 8175 RVA: 0x0007544F File Offset: 0x0007364F
		// (set) Token: 0x06001FF0 RID: 8176 RVA: 0x00075457 File Offset: 0x00073657
		[DataSourceProperty]
		public MBBindingList<CharacterCreationGainedSkillItemVM> Skills
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
					base.OnPropertyChangedWithValue<MBBindingList<CharacterCreationGainedSkillItemVM>>(value, "Skills");
				}
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x00075475 File Offset: 0x00073675
		// (set) Token: 0x06001FF2 RID: 8178 RVA: 0x0007547D File Offset: 0x0007367D
		[DataSourceProperty]
		public CharacterCreationGainedAttributeItemVM Attribute
		{
			get
			{
				return this._attribute;
			}
			set
			{
				if (value != this._attribute)
				{
					this._attribute = value;
					base.OnPropertyChangedWithValue<CharacterCreationGainedAttributeItemVM>(value, "Attribute");
				}
			}
		}

		// Token: 0x04000EDF RID: 3807
		private MBBindingList<CharacterCreationGainedSkillItemVM> _skills;

		// Token: 0x04000EE0 RID: 3808
		private CharacterCreationGainedAttributeItemVM _attribute;
	}
}
