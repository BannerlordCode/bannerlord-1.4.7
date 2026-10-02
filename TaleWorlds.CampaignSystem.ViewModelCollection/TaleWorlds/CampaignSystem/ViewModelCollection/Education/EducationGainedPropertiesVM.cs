using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000F0 RID: 240
	public class EducationGainedPropertiesVM : ViewModel
	{
		// Token: 0x060015F6 RID: 5622 RVA: 0x00055FD0 File Offset: 0x000541D0
		public EducationGainedPropertiesVM(Hero child, int pageCount)
		{
			this._child = child;
			this._pageCount = pageCount;
			this._educationBehavior = Campaign.Current.GetCampaignBehavior<IEducationLogic>();
			this._affectedSkillFocusMap = new Dictionary<SkillObject, Tuple<int, int>>();
			this._affectedSkillValueMap = new Dictionary<SkillObject, Tuple<int, int>>();
			this._affectedAttributesMap = new Dictionary<CharacterAttribute, Tuple<int, int>>();
			this.GainGroups = new MBBindingList<EducationGainGroupItemVM>();
			this.OtherSkills = new MBBindingList<EducationGainedSkillItemVM>();
			List<CharacterAttribute> list = Attributes.All.ToList<CharacterAttribute>();
			list.Sort(CampaignUIHelper.CharacterAttributeComparerInstance);
			foreach (CharacterAttribute characterAttribute in list)
			{
				this.GainGroups.Add(new EducationGainGroupItemVM(characterAttribute));
			}
			List<SkillObject> list2 = Skills.All.ToList<SkillObject>();
			list2.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			using (List<SkillObject>.Enumerator enumerator2 = list2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					SkillObject skill = enumerator2.Current;
					Func<EducationGainedSkillItemVM, bool> <>9__1;
					if (!this.GainGroups.Any<EducationGainGroupItemVM>(delegate(EducationGainGroupItemVM attribute)
					{
						IEnumerable<EducationGainedSkillItemVM> skills = attribute.Skills;
						Func<EducationGainedSkillItemVM, bool> func;
						if ((func = <>9__1) == null)
						{
							func = (<>9__1 = (EducationGainedSkillItemVM attributeSkill) => attributeSkill.SkillId == skill.StringId);
						}
						return skills.Any<EducationGainedSkillItemVM>(func);
					}))
					{
						this.OtherSkills.Add(new EducationGainedSkillItemVM(skill));
					}
				}
			}
			this.UpdateWithSelections(new List<string>(), -1);
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x00056128 File Offset: 0x00054328
		internal void UpdateWithSelections(List<string> selectedOptions, int currentPageIndex)
		{
			this._affectedAttributesMap.Clear();
			this._affectedSkillFocusMap.Clear();
			this._affectedSkillValueMap.Clear();
			this.GainGroups.ApplyActionOnAllItems(delegate(EducationGainGroupItemVM g)
			{
				g.ResetValues();
			});
			this.OtherSkills.ApplyActionOnAllItems(delegate(EducationGainedSkillItemVM s)
			{
				s.ResetValues();
			});
			this.PopulateInitialValues();
			this.PopulateGainedAttributeValues(selectedOptions, currentPageIndex);
			foreach (KeyValuePair<CharacterAttribute, Tuple<int, int>> keyValuePair in this._affectedAttributesMap)
			{
				this.GetItemFromAttribute(keyValuePair.Key).SetValue(keyValuePair.Value.Item1, keyValuePair.Value.Item2);
			}
			foreach (KeyValuePair<SkillObject, Tuple<int, int>> keyValuePair2 in this._affectedSkillFocusMap)
			{
				this.GetItemFromSkill(keyValuePair2.Key).SetFocusValue(keyValuePair2.Value.Item1, keyValuePair2.Value.Item2);
			}
			foreach (KeyValuePair<SkillObject, Tuple<int, int>> keyValuePair3 in this._affectedSkillValueMap)
			{
				this.GetItemFromSkill(keyValuePair3.Key).SetSkillValue(keyValuePair3.Value.Item1, keyValuePair3.Value.Item2);
			}
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x000562EC File Offset: 0x000544EC
		private void PopulateInitialValues()
		{
			foreach (SkillObject skillObject in Skills.All)
			{
				int focus = this._child.HeroDeveloper.GetFocus(skillObject);
				if (this._affectedSkillFocusMap.ContainsKey(skillObject))
				{
					Tuple<int, int> tuple = this._affectedSkillFocusMap[skillObject];
					this._affectedSkillFocusMap[skillObject] = new Tuple<int, int>(tuple.Item1 + focus, 0);
				}
				else
				{
					this._affectedSkillFocusMap.Add(skillObject, new Tuple<int, int>(focus, 0));
				}
				int skillValue = this._child.GetSkillValue(skillObject);
				if (this._affectedSkillValueMap.ContainsKey(skillObject))
				{
					Tuple<int, int> tuple2 = this._affectedSkillValueMap[skillObject];
					this._affectedSkillValueMap[skillObject] = new Tuple<int, int>(tuple2.Item1 + skillValue, 0);
				}
				else
				{
					this._affectedSkillValueMap.Add(skillObject, new Tuple<int, int>(skillValue, 0));
				}
			}
			foreach (CharacterAttribute characterAttribute in Attributes.All)
			{
				int attributeValue = this._child.GetAttributeValue(characterAttribute);
				if (this._affectedAttributesMap.ContainsKey(characterAttribute))
				{
					Tuple<int, int> tuple3 = this._affectedAttributesMap[characterAttribute];
					this._affectedAttributesMap[characterAttribute] = new Tuple<int, int>(tuple3.Item1 + attributeValue, 0);
				}
				else
				{
					this._affectedAttributesMap.Add(characterAttribute, new Tuple<int, int>(attributeValue, 0));
				}
			}
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x00056494 File Offset: 0x00054694
		private void PopulateGainedAttributeValues(List<string> selectedOptions, int currentPageIndex)
		{
			bool flag = currentPageIndex == this._pageCount - 1;
			for (int i = 0; i < selectedOptions.Count; i++)
			{
				string text = selectedOptions[i];
				TextObject textObject;
				TextObject textObject2;
				TextObject textObject3;
				ValueTuple<CharacterAttribute, int>[] array;
				ValueTuple<SkillObject, int>[] array2;
				ValueTuple<SkillObject, int>[] array3;
				EducationCampaignBehavior.EducationCharacterProperties[] array4;
				this._educationBehavior.GetOptionProperties(this._child, text, selectedOptions, out textObject, out textObject2, out textObject3, out array, out array2, out array3, out array4);
				bool flag2 = i == currentPageIndex;
				if (array != null)
				{
					foreach (ValueTuple<CharacterAttribute, int> valueTuple in array)
					{
						Tuple<int, int> tuple = this._affectedAttributesMap[valueTuple.Item1];
						int num = (flag2 ? valueTuple.Item2 : (flag ? (tuple.Item2 + valueTuple.Item2) : 0));
						int num2 = (flag2 ? tuple.Item1 : (flag ? tuple.Item1 : (tuple.Item1 + valueTuple.Item2)));
						this._affectedAttributesMap[valueTuple.Item1] = new Tuple<int, int>(num2, num);
					}
				}
				if (array2 != null)
				{
					foreach (ValueTuple<SkillObject, int> valueTuple2 in array2)
					{
						Tuple<int, int> tuple2 = this._affectedSkillValueMap[valueTuple2.Item1];
						int num3 = (flag2 ? valueTuple2.Item2 : (flag ? (tuple2.Item2 + valueTuple2.Item2) : 0));
						int num4 = (flag2 ? tuple2.Item1 : (flag ? tuple2.Item1 : (tuple2.Item1 + valueTuple2.Item2)));
						this._affectedSkillValueMap[valueTuple2.Item1] = new Tuple<int, int>(num4, num3);
					}
				}
				if (array3 != null)
				{
					foreach (ValueTuple<SkillObject, int> valueTuple3 in array3)
					{
						Tuple<int, int> tuple3 = this._affectedSkillFocusMap[valueTuple3.Item1];
						int num5 = (flag2 ? valueTuple3.Item2 : (flag ? (tuple3.Item2 + valueTuple3.Item2) : 0));
						int num6 = (flag2 ? tuple3.Item1 : (flag ? tuple3.Item1 : (tuple3.Item1 + valueTuple3.Item2)));
						num6 = Math.Min(num6, 5);
						num5 = Math.Min(num5, 5 - num6);
						this._affectedSkillFocusMap[valueTuple3.Item1] = new Tuple<int, int>(num6, num5);
					}
				}
			}
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x0005670C File Offset: 0x0005490C
		private EducationGainedAttributeItemVM GetItemFromAttribute(CharacterAttribute attribute)
		{
			EducationGainGroupItemVM educationGainGroupItemVM = this.GainGroups.SingleOrDefault<EducationGainGroupItemVM>((EducationGainGroupItemVM g) => g.AttributeObj == attribute);
			if (educationGainGroupItemVM == null)
			{
				return null;
			}
			return educationGainGroupItemVM.Attribute;
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x00056748 File Offset: 0x00054948
		private EducationGainedSkillItemVM GetItemFromSkill(SkillObject skill)
		{
			foreach (EducationGainGroupItemVM educationGainGroupItemVM in this.GainGroups)
			{
				foreach (EducationGainedSkillItemVM educationGainedSkillItemVM in educationGainGroupItemVM.Skills)
				{
					if (educationGainedSkillItemVM.SkillObj == skill)
					{
						return educationGainedSkillItemVM;
					}
				}
			}
			foreach (EducationGainedSkillItemVM educationGainedSkillItemVM2 in this.OtherSkills)
			{
				if (educationGainedSkillItemVM2.SkillObj == skill)
				{
					return educationGainedSkillItemVM2;
				}
			}
			return null;
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060015FC RID: 5628 RVA: 0x00056818 File Offset: 0x00054A18
		// (set) Token: 0x060015FD RID: 5629 RVA: 0x00056820 File Offset: 0x00054A20
		[DataSourceProperty]
		public MBBindingList<EducationGainGroupItemVM> GainGroups
		{
			get
			{
				return this._gainGroups;
			}
			set
			{
				if (value != this._gainGroups)
				{
					this._gainGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<EducationGainGroupItemVM>>(value, "GainGroups");
				}
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060015FE RID: 5630 RVA: 0x0005683E File Offset: 0x00054A3E
		// (set) Token: 0x060015FF RID: 5631 RVA: 0x00056846 File Offset: 0x00054A46
		[DataSourceProperty]
		public MBBindingList<EducationGainedSkillItemVM> OtherSkills
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
					base.OnPropertyChangedWithValue<MBBindingList<EducationGainedSkillItemVM>>(value, "OtherSkills");
				}
			}
		}

		// Token: 0x040009FA RID: 2554
		private readonly Hero _child;

		// Token: 0x040009FB RID: 2555
		private readonly int _pageCount;

		// Token: 0x040009FC RID: 2556
		private readonly IEducationLogic _educationBehavior;

		// Token: 0x040009FD RID: 2557
		private readonly Dictionary<CharacterAttribute, Tuple<int, int>> _affectedAttributesMap;

		// Token: 0x040009FE RID: 2558
		private readonly Dictionary<SkillObject, Tuple<int, int>> _affectedSkillFocusMap;

		// Token: 0x040009FF RID: 2559
		private readonly Dictionary<SkillObject, Tuple<int, int>> _affectedSkillValueMap;

		// Token: 0x04000A00 RID: 2560
		private MBBindingList<EducationGainGroupItemVM> _gainGroups;

		// Token: 0x04000A01 RID: 2561
		private MBBindingList<EducationGainedSkillItemVM> _otherSkills;
	}
}
