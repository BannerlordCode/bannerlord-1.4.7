using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000F1 RID: 241
	public class EducationGainGroupItemVM : ViewModel
	{
		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001600 RID: 5632 RVA: 0x00056864 File Offset: 0x00054A64
		// (set) Token: 0x06001601 RID: 5633 RVA: 0x0005686C File Offset: 0x00054A6C
		public CharacterAttribute AttributeObj { get; private set; }

		// Token: 0x06001602 RID: 5634 RVA: 0x00056878 File Offset: 0x00054A78
		public EducationGainGroupItemVM(CharacterAttribute attributeObj)
		{
			this.AttributeObj = attributeObj;
			this.Skills = new MBBindingList<EducationGainedSkillItemVM>();
			this.Attribute = new EducationGainedAttributeItemVM(this.AttributeObj);
			List<SkillObject> list = TaleWorlds.CampaignSystem.Extensions.Skills.All.ToList<SkillObject>();
			list.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			using (List<SkillObject>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SkillObject skill = enumerator.Current;
					if (!CampaignUIHelper.GetIsNavalSkill(skill) && skill.Attributes.FirstOrDefault<CharacterAttribute>() == this.AttributeObj && !this.Skills.Any<EducationGainedSkillItemVM>((EducationGainedSkillItemVM s) => s.SkillObj == skill))
					{
						this.Skills.Add(new EducationGainedSkillItemVM(skill));
					}
				}
			}
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x0005695C File Offset: 0x00054B5C
		public void ResetValues()
		{
			this.Attribute.ResetValues();
			this.Skills.ApplyActionOnAllItems(delegate(EducationGainedSkillItemVM s)
			{
				s.ResetValues();
			});
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x00056993 File Offset: 0x00054B93
		// (set) Token: 0x06001605 RID: 5637 RVA: 0x0005699B File Offset: 0x00054B9B
		[DataSourceProperty]
		public MBBindingList<EducationGainedSkillItemVM> Skills
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
					base.OnPropertyChangedWithValue<MBBindingList<EducationGainedSkillItemVM>>(value, "Skills");
				}
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x000569B9 File Offset: 0x00054BB9
		// (set) Token: 0x06001607 RID: 5639 RVA: 0x000569C1 File Offset: 0x00054BC1
		[DataSourceProperty]
		public EducationGainedAttributeItemVM Attribute
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
					base.OnPropertyChangedWithValue<EducationGainedAttributeItemVM>(value, "Attribute");
				}
			}
		}

		// Token: 0x04000A03 RID: 2563
		private MBBindingList<EducationGainedSkillItemVM> _skills;

		// Token: 0x04000A04 RID: 2564
		private EducationGainedAttributeItemVM _attribute;
	}
}
