using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x0200012D RID: 301
	public class ClanRoleMemberItemVM : ViewModel
	{
		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06001C44 RID: 7236 RVA: 0x000685C6 File Offset: 0x000667C6
		// (set) Token: 0x06001C45 RID: 7237 RVA: 0x000685CE File Offset: 0x000667CE
		public PartyRole Role { get; private set; }

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x000685D7 File Offset: 0x000667D7
		// (set) Token: 0x06001C47 RID: 7239 RVA: 0x000685DF File Offset: 0x000667DF
		public SkillObject RelevantSkill { get; private set; }

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x000685E8 File Offset: 0x000667E8
		// (set) Token: 0x06001C49 RID: 7241 RVA: 0x000685F0 File Offset: 0x000667F0
		public int RelevantSkillValue { get; private set; }

		// Token: 0x06001C4A RID: 7242 RVA: 0x000685FC File Offset: 0x000667FC
		public ClanRoleMemberItemVM(MobileParty party, PartyRole role, ClanPartyMemberItemVM member, Action onRoleAssigned)
		{
			this.Role = role;
			this.Member = member;
			this._party = party;
			this._onRoleAssigned = onRoleAssigned;
			this.RelevantSkill = Campaign.Current.Models.ClanMemberPartyRoleModel.GetRelevantSkillForPartyRole(role);
			ClanPartyMemberItemVM member2 = this.Member;
			int? num;
			if (member2 == null)
			{
				num = null;
			}
			else
			{
				Hero heroObject = member2.HeroObject;
				num = ((heroObject != null) ? new int?(heroObject.GetSkillValue(this.RelevantSkill)) : null);
			}
			this.RelevantSkillValue = num ?? (-1);
			this._skillEffects = SkillEffect.All.Where<SkillEffect>((SkillEffect x) => x.Role != PartyRole.Personal);
			this._perks = PerkObject.All.Where<PerkObject>((PerkObject x) => this.Member.HeroObject.GetPerkValue(x));
			this.IsRemoveAssigneeOption = this.Member == null;
			this.Hint = new HintViewModel(this.IsRemoveAssigneeOption ? new TextObject("{=bfWlTVjs}Remove assignee", null) : this.GetRoleHint(this.Role), null);
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x00068721 File Offset: 0x00066921
		public override void RefreshValues()
		{
			base.RefreshValues();
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x00068729 File Offset: 0x00066929
		public override void OnFinalize()
		{
			base.OnFinalize();
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x00068734 File Offset: 0x00066934
		public void ExecuteAssignHeroToRole()
		{
			if (this.Member == null)
			{
				switch (this.Role)
				{
				case PartyRole.Surgeon:
					this._party.SetPartySurgeon(null);
					break;
				case PartyRole.Engineer:
					this._party.SetPartyEngineer(null);
					break;
				case PartyRole.Scout:
					this._party.SetPartyScout(null);
					break;
				case PartyRole.Quartermaster:
					this._party.SetPartyQuartermaster(null);
					break;
				case PartyRole.FirstMate:
					this._party.SetPartyFirstMate(null);
					break;
				case PartyRole.Navigator:
					this._party.SetPartyNavigator(null);
					break;
				}
			}
			else
			{
				this.OnSetMemberAsRole(this.Role);
			}
			Action onRoleAssigned = this._onRoleAssigned;
			if (onRoleAssigned == null)
			{
				return;
			}
			onRoleAssigned();
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x000687F4 File Offset: 0x000669F4
		private void OnSetMemberAsRole(PartyRole role)
		{
			if (role != PartyRole.None)
			{
				if (!this._party.GetHeroPartyRoles(this.Member.HeroObject).Contains(role))
				{
					if (role == PartyRole.Engineer)
					{
						this._party.SetPartyEngineer(this.Member.HeroObject);
					}
					else if (role == PartyRole.Quartermaster)
					{
						this._party.SetPartyQuartermaster(this.Member.HeroObject);
					}
					else if (role == PartyRole.Scout)
					{
						this._party.SetPartyScout(this.Member.HeroObject);
					}
					else if (role == PartyRole.Surgeon)
					{
						this._party.SetPartySurgeon(this.Member.HeroObject);
					}
					else if (role == PartyRole.FirstMate)
					{
						this._party.SetPartyFirstMate(this.Member.HeroObject);
					}
					else if (role == PartyRole.Navigator)
					{
						this._party.SetPartyNavigator(this.Member.HeroObject);
					}
					Game game = Game.Current;
					if (game != null)
					{
						game.EventManager.TriggerEvent<ClanRoleAssignedThroughClanScreenEvent>(new ClanRoleAssignedThroughClanScreenEvent(role, this.Member.HeroObject));
					}
				}
			}
			else if (role == PartyRole.None)
			{
				this._party.RemoveOnePartyRoleOfHero(this.Member.HeroObject);
			}
			Action onRoleAssigned = this._onRoleAssigned;
			if (onRoleAssigned == null)
			{
				return;
			}
			onRoleAssigned();
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x00068928 File Offset: 0x00066B28
		private TextObject GetRoleHint(PartyRole role)
		{
			string text = "";
			if (this.RelevantSkillValue <= 0)
			{
				GameTexts.SetVariable("LEFT", this.RelevantSkill.Name.ToString());
				GameTexts.SetVariable("RIGHT", this.RelevantSkillValue.ToString());
				GameTexts.SetVariable("STR2", GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString());
				GameTexts.SetVariable("STR1", this.Member.Name.ToString());
				text = GameTexts.FindText("str_string_newline_string", null).ToString();
			}
			else if (!Campaign.Current.Models.ClanMemberPartyRoleModel.DoesHeroHaveEnoughSkillForPartyRole(this.Member.HeroObject, role, this._party))
			{
				GameTexts.SetVariable("SKILL_NAME", this.RelevantSkill.Name.ToString());
				GameTexts.SetVariable("MIN_SKILL_AMOUNT", 0);
				text = GameTexts.FindText("str_character_role_disabled_tooltip", null).ToString();
			}
			else
			{
				if (!role.Equals(PartyRole.None))
				{
					IEnumerable<SkillEffect> enumerable = this._skillEffects.Where<SkillEffect>((SkillEffect x) => x.Role == role);
					IEnumerable<PerkObject> enumerable2 = this._perks.Where<PerkObject>((PerkObject x) => x.PrimaryRole == role || x.SecondaryRole == role);
					GameTexts.SetVariable("LEFT", this.RelevantSkill.Name.ToString());
					GameTexts.SetVariable("RIGHT", this.RelevantSkillValue.ToString());
					GameTexts.SetVariable("STR2", GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString());
					GameTexts.SetVariable("STR1", this.Member.Name.ToString());
					text = GameTexts.FindText("str_string_newline_string", null).ToString();
					int num = 0;
					TextObject textObject = GameTexts.FindText("str_LEFT_colon_RIGHT", null).CopyTextObject();
					textObject.SetTextVariable("LEFT", new TextObject("{=Avy8Gua1}Perks", null));
					textObject.SetTextVariable("RIGHT", new TextObject("{=Gp2vmZGZ}{PERKS}", null));
					foreach (PerkObject perkObject in enumerable2)
					{
						if (num == 0)
						{
							GameTexts.SetVariable("PERKS", perkObject.Name.ToString());
						}
						else
						{
							GameTexts.SetVariable("RIGHT", perkObject.Name.ToString());
							GameTexts.SetVariable("LEFT", new TextObject("{=Gp2vmZGZ}{PERKS}", null).ToString());
							GameTexts.SetVariable("PERKS", GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString());
						}
						num++;
					}
					GameTexts.SetVariable("newline", "\n \n");
					if (num > 0)
					{
						GameTexts.SetVariable("STR1", text);
						GameTexts.SetVariable("STR2", textObject.ToString());
						text = GameTexts.FindText("str_string_newline_string", null).ToString();
					}
					if (!enumerable.Any<SkillEffect>())
					{
						goto IL_03AF;
					}
					GameTexts.SetVariable("LEFT", new TextObject("{=DKJIp6xG}Effects", null).ToString());
					string text2 = GameTexts.FindText("str_LEFT_colon", null).ToString();
					GameTexts.SetVariable("STR1", text);
					GameTexts.SetVariable("STR2", text2);
					text = GameTexts.FindText("str_string_newline_string", null).ToString();
					GameTexts.SetVariable("newline", "\n");
					using (IEnumerator<SkillEffect> enumerator2 = enumerable.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							SkillEffect skillEffect = enumerator2.Current;
							GameTexts.SetVariable("STR1", text);
							GameTexts.SetVariable("STR2", SkillHelper.GetEffectDescriptionForSkillLevel(skillEffect, this.RelevantSkillValue).ToString());
							text = GameTexts.FindText("str_string_newline_string", null).ToString();
						}
						goto IL_03AF;
					}
				}
				text = null;
			}
			IL_03AF:
			if (!string.IsNullOrEmpty(text))
			{
				return new TextObject("{=!}" + text, null);
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x00068D20 File Offset: 0x00066F20
		public string GetEffectsList(PartyRole role)
		{
			string text = "";
			IEnumerable<SkillEffect> enumerable = this._skillEffects.Where<SkillEffect>((SkillEffect x) => x.Role == role);
			int num = 0;
			if (this.RelevantSkillValue > 0)
			{
				foreach (SkillEffect skillEffect in enumerable)
				{
					if (num == 0)
					{
						text = SkillHelper.GetEffectDescriptionForSkillLevel(skillEffect, this.RelevantSkillValue).ToString();
					}
					else
					{
						GameTexts.SetVariable("STR1", text);
						GameTexts.SetVariable("STR2", SkillHelper.GetEffectDescriptionForSkillLevel(skillEffect, this.RelevantSkillValue).ToString());
						text = GameTexts.FindText("str_string_newline_string", null).ToString();
					}
					num++;
				}
			}
			return text;
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06001C51 RID: 7249 RVA: 0x00068DF4 File Offset: 0x00066FF4
		// (set) Token: 0x06001C52 RID: 7250 RVA: 0x00068DFC File Offset: 0x00066FFC
		[DataSourceProperty]
		public ClanPartyMemberItemVM Member
		{
			get
			{
				return this._member;
			}
			set
			{
				if (value != this._member)
				{
					this._member = value;
					base.OnPropertyChangedWithValue<ClanPartyMemberItemVM>(value, "Member");
				}
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06001C53 RID: 7251 RVA: 0x00068E1A File Offset: 0x0006701A
		// (set) Token: 0x06001C54 RID: 7252 RVA: 0x00068E22 File Offset: 0x00067022
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06001C55 RID: 7253 RVA: 0x00068E40 File Offset: 0x00067040
		// (set) Token: 0x06001C56 RID: 7254 RVA: 0x00068E48 File Offset: 0x00067048
		[DataSourceProperty]
		public bool IsRemoveAssigneeOption
		{
			get
			{
				return this._isRemoveAssigneeOption;
			}
			set
			{
				if (value != this._isRemoveAssigneeOption)
				{
					this._isRemoveAssigneeOption = value;
					base.OnPropertyChangedWithValue(value, "IsRemoveAssigneeOption");
				}
			}
		}

		// Token: 0x04000D31 RID: 3377
		private Action _onRoleAssigned;

		// Token: 0x04000D32 RID: 3378
		private MobileParty _party;

		// Token: 0x04000D33 RID: 3379
		private readonly IEnumerable<SkillEffect> _skillEffects;

		// Token: 0x04000D34 RID: 3380
		private readonly IEnumerable<PerkObject> _perks;

		// Token: 0x04000D35 RID: 3381
		private ClanPartyMemberItemVM _member;

		// Token: 0x04000D36 RID: 3382
		private HintViewModel _hint;

		// Token: 0x04000D37 RID: 3383
		private bool _isRemoveAssigneeOption;
	}
}
