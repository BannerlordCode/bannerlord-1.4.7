using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x02000031 RID: 49
	public class UpgradeRequirementsVM : ViewModel
	{
		// Token: 0x060004D8 RID: 1240 RVA: 0x0001BF36 File Offset: 0x0001A136
		public UpgradeRequirementsVM()
		{
			this.IsItemRequirementMet = true;
			this.IsPerkRequirementMet = true;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001BF62 File Offset: 0x0001A162
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.UpdateItemRequirementHint();
			this.UpdatePerkRequirementHint();
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001BF76 File Offset: 0x0001A176
		public void SetItemRequirement(ItemCategory category)
		{
			if (category != null)
			{
				this.HasItemRequirement = true;
				this._category = category;
				this.ItemRequirement = category.StringId.ToLower();
				this.UpdateItemRequirementHint();
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001BFA0 File Offset: 0x0001A1A0
		public void SetPerkRequirement(PerkObject perk)
		{
			if (perk != null)
			{
				this.HasPerkRequirement = true;
				this._perk = perk;
				this.PerkRequirement = perk.Skill.StringId.ToLower();
				this.UpdatePerkRequirementHint();
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001BFCF File Offset: 0x0001A1CF
		public void SetRequirementsMet(bool isItemRequirementMet, bool isPerkRequirementMet)
		{
			this.IsItemRequirementMet = !this.HasItemRequirement || isItemRequirementMet;
			this.IsPerkRequirementMet = !this.HasPerkRequirement || isPerkRequirementMet;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001BFF4 File Offset: 0x0001A1F4
		private void UpdateItemRequirementHint()
		{
			if (this._category == null)
			{
				return;
			}
			TextObject textObject = new TextObject("{=Q0j1umAt}Requirement: {REQUIREMENT_NAME}", null);
			textObject.SetTextVariable("REQUIREMENT_NAME", this._category.GetName().ToString());
			this.ItemRequirementHint = new HintViewModel(textObject, null);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001C040 File Offset: 0x0001A240
		private void UpdatePerkRequirementHint()
		{
			if (this._perk == null)
			{
				return;
			}
			TextObject textObject = new TextObject("{=Q0j1umAt}Requirement: {REQUIREMENT_NAME}", null);
			textObject.SetTextVariable("REQUIREMENT_NAME", this._perk.Name.ToString());
			this.PerkRequirementHint = new HintViewModel(textObject, null);
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x0001C08B File Offset: 0x0001A28B
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x0001C093 File Offset: 0x0001A293
		[DataSourceProperty]
		public bool IsItemRequirementMet
		{
			get
			{
				return this._isItemRequirementMet;
			}
			set
			{
				if (value != this._isItemRequirementMet)
				{
					this._isItemRequirementMet = value;
					base.OnPropertyChangedWithValue(value, "IsItemRequirementMet");
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x0001C0B1 File Offset: 0x0001A2B1
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x0001C0B9 File Offset: 0x0001A2B9
		[DataSourceProperty]
		public bool IsPerkRequirementMet
		{
			get
			{
				return this._isPerkRequirementMet;
			}
			set
			{
				if (value != this._isPerkRequirementMet)
				{
					this._isPerkRequirementMet = value;
					base.OnPropertyChangedWithValue(value, "IsPerkRequirementMet");
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0001C0D7 File Offset: 0x0001A2D7
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x0001C0DF File Offset: 0x0001A2DF
		[DataSourceProperty]
		public bool HasItemRequirement
		{
			get
			{
				return this._hasItemRequirement;
			}
			set
			{
				if (value != this._hasItemRequirement)
				{
					this._hasItemRequirement = value;
					base.OnPropertyChangedWithValue(value, "HasItemRequirement");
				}
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0001C0FD File Offset: 0x0001A2FD
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0001C105 File Offset: 0x0001A305
		[DataSourceProperty]
		public bool HasPerkRequirement
		{
			get
			{
				return this._hasPerkRequirement;
			}
			set
			{
				if (value != this._hasPerkRequirement)
				{
					this._hasPerkRequirement = value;
					base.OnPropertyChangedWithValue(value, "HasPerkRequirement");
				}
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0001C123 File Offset: 0x0001A323
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x0001C12B File Offset: 0x0001A32B
		[DataSourceProperty]
		public string PerkRequirement
		{
			get
			{
				return this._perkRequirement;
			}
			set
			{
				if (value != this._perkRequirement)
				{
					this._perkRequirement = value;
					base.OnPropertyChangedWithValue<string>(value, "PerkRequirement");
				}
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0001C14E File Offset: 0x0001A34E
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x0001C156 File Offset: 0x0001A356
		[DataSourceProperty]
		public string ItemRequirement
		{
			get
			{
				return this._itemRequirement;
			}
			set
			{
				if (value != this._itemRequirement)
				{
					this._itemRequirement = value;
					base.OnPropertyChangedWithValue<string>(value, "ItemRequirement");
				}
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0001C179 File Offset: 0x0001A379
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x0001C181 File Offset: 0x0001A381
		[DataSourceProperty]
		public HintViewModel ItemRequirementHint
		{
			get
			{
				return this._itemRequirementHint;
			}
			set
			{
				if (value != this._itemRequirementHint)
				{
					this._itemRequirementHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ItemRequirementHint");
				}
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0001C19F File Offset: 0x0001A39F
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x0001C1A7 File Offset: 0x0001A3A7
		[DataSourceProperty]
		public HintViewModel PerkRequirementHint
		{
			get
			{
				return this._perkRequirementHint;
			}
			set
			{
				if (value != this._perkRequirementHint)
				{
					this._perkRequirementHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PerkRequirementHint");
				}
			}
		}

		// Token: 0x0400021D RID: 541
		private ItemCategory _category;

		// Token: 0x0400021E RID: 542
		private PerkObject _perk;

		// Token: 0x0400021F RID: 543
		private bool _isItemRequirementMet;

		// Token: 0x04000220 RID: 544
		private bool _isPerkRequirementMet;

		// Token: 0x04000221 RID: 545
		private bool _hasItemRequirement;

		// Token: 0x04000222 RID: 546
		private bool _hasPerkRequirement;

		// Token: 0x04000223 RID: 547
		private string _perkRequirement = "";

		// Token: 0x04000224 RID: 548
		private string _itemRequirement = "";

		// Token: 0x04000225 RID: 549
		private HintViewModel _itemRequirementHint;

		// Token: 0x04000226 RID: 550
		private HintViewModel _perkRequirementHint;
	}
}
