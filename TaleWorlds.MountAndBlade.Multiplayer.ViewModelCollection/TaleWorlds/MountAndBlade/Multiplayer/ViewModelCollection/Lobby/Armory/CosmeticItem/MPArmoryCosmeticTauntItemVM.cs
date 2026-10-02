using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem
{
	// Token: 0x0200007F RID: 127
	public class MPArmoryCosmeticTauntItemVM : MPArmoryCosmeticItemBaseVM
	{
		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x000275BA File Offset: 0x000257BA
		public MPArmoryCosmeticsVM.TauntCategoryFlag TauntCategory { get; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x000275C2 File Offset: 0x000257C2
		public TauntCosmeticElement TauntCosmeticElement { get; }

		// Token: 0x06000CB3 RID: 3251 RVA: 0x000275CC File Offset: 0x000257CC
		public MPArmoryCosmeticTauntItemVM(string tauntId, CosmeticElement cosmetic, string cosmeticID)
			: base(cosmetic, cosmeticID, CosmeticsManager.CosmeticType.Taunt)
		{
			this.TauntID = tauntId;
			this.TauntCosmeticElement = cosmetic as TauntCosmeticElement;
			this.TauntCategory = this.GetCategoryOfTaunt();
			this.TauntUsages = new MBBindingList<StringItemWithEnabledAndHintVM>();
			this.RefreshTauntUsages();
			this.BlocksMovementOnUsageHint = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x00027624 File Offset: 0x00025824
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.BlocksMovementOnUsageHint != null)
			{
				this.BlocksMovementOnUsageHint.HintText = new TextObject("{=BUQsaZMg}Blocks Movement on Usage", null);
			}
			this.SelectSlotText = new TextObject("{=4gfAb1ar}Select a Slot", null).ToString();
			this.CancelEquipText = new TextObject("{=avYRbfHA}Cancel Equip", null).ToString();
			TauntCosmeticElement tauntCosmeticElement = this.TauntCosmeticElement;
			string text;
			if (tauntCosmeticElement == null)
			{
				text = null;
			}
			else
			{
				TextObject name = tauntCosmeticElement.Name;
				text = ((name != null) ? name.ToString() : null);
			}
			base.Name = text;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x000276A8 File Offset: 0x000258A8
		private void RefreshTauntUsages()
		{
			this.TauntUsages.Clear();
			TauntUsageManager.TauntUsageSet usageSet = TauntUsageManager.Instance.GetUsageSet(this.TauntCosmeticElement.Id);
			TauntUsageManager.TauntUsage.TauntUsageFlag? tauntUsageFlag;
			if (usageSet == null)
			{
				tauntUsageFlag = null;
			}
			else
			{
				MBReadOnlyList<TauntUsageManager.TauntUsage> usages = usageSet.GetUsages();
				tauntUsageFlag = ((usages != null) ? new TauntUsageManager.TauntUsage.TauntUsageFlag?(usages.FirstOrDefault<TauntUsageManager.TauntUsage>().UsageFlag) : null);
			}
			TauntUsageManager.TauntUsage.TauntUsageFlag tauntUsageFlag2 = tauntUsageFlag ?? TauntUsageManager.TauntUsage.TauntUsageFlag.None;
			TextObject textObject = new TextObject("{=aeDp7IEK}Usable with {USAGE}", null);
			if ((tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForOneHanded) == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				TextObject textObject2 = new TextObject("{=PiHpR4QL}One Handed", null);
				TextObject textObject3 = textObject.CopyTextObject().SetTextVariable("USAGE", textObject2);
				this.TauntUsages.Add(new StringItemWithEnabledAndHintVM(null, "UsableWithOneHanded", true, null, textObject3));
			}
			if ((tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForTwoHanded) == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				TextObject textObject4 = new TextObject("{=t78atYqH}Two Handed", null);
				TextObject textObject5 = textObject.CopyTextObject().SetTextVariable("USAGE", textObject4);
				this.TauntUsages.Add(new StringItemWithEnabledAndHintVM(null, "UsableWithTwoHanded", true, null, textObject5));
			}
			if ((tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForBow) == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				TextObject textObject6 = new TextObject("{=5rj7xQE4}Bow", null);
				TextObject textObject7 = textObject.CopyTextObject().SetTextVariable("USAGE", textObject6);
				this.TauntUsages.Add(new StringItemWithEnabledAndHintVM(null, "UsableWithBow", true, null, textObject7));
			}
			if ((tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForCrossbow) == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				TextObject textObject8 = new TextObject("{=TTWL7RLe}Crossbow", null);
				TextObject textObject9 = textObject.CopyTextObject().SetTextVariable("USAGE", textObject8);
				this.TauntUsages.Add(new StringItemWithEnabledAndHintVM(null, "UsableWithCrossbow", true, null, textObject9));
			}
			if ((tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForShield) == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				TextObject textObject10 = new TextObject("{=Jd0Kq9lD}Shield", null);
				TextObject textObject11 = textObject.CopyTextObject().SetTextVariable("USAGE", textObject10);
				this.TauntUsages.Add(new StringItemWithEnabledAndHintVM(null, "UsableWithShield", true, null, textObject11));
			}
			if ((tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresOnFoot) == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				TextObject textObject12 = new TextObject("{=uGM8DWrm}Mount", null);
				TextObject textObject13 = textObject.CopyTextObject().SetTextVariable("USAGE", textObject12);
				this.TauntUsages.Add(new StringItemWithEnabledAndHintVM(null, "UsableWithMount", true, null, textObject13));
			}
			this.RequiresOnFoot = (tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresOnFoot) > TauntUsageManager.TauntUsage.TauntUsageFlag.None;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000278C0 File Offset: 0x00025AC0
		private MPArmoryCosmeticsVM.TauntCategoryFlag GetCategoryOfTaunt()
		{
			TauntUsageManager.TauntUsageSet usageSet = TauntUsageManager.Instance.GetUsageSet(this.TauntCosmeticElement.Id);
			MBReadOnlyList<TauntUsageManager.TauntUsage> mbreadOnlyList = ((usageSet != null) ? usageSet.GetUsages() : null);
			MPArmoryCosmeticsVM.TauntCategoryFlag tauntCategoryFlag = MPArmoryCosmeticsVM.TauntCategoryFlag.None;
			if (mbreadOnlyList == null || mbreadOnlyList.Count <= 0)
			{
				return tauntCategoryFlag;
			}
			if (this.AnyUsageNotHaveFlag(mbreadOnlyList, TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresOnFoot))
			{
				tauntCategoryFlag |= MPArmoryCosmeticsVM.TauntCategoryFlag.UsableWithMount;
			}
			if (this.AnyUsageNotHaveFlag(mbreadOnlyList, TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForShield))
			{
				tauntCategoryFlag |= MPArmoryCosmeticsVM.TauntCategoryFlag.UsableWithShield;
			}
			if (this.AnyUsageNotHaveFlag(mbreadOnlyList, TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForOneHanded))
			{
				tauntCategoryFlag |= MPArmoryCosmeticsVM.TauntCategoryFlag.UsableWithOneHanded;
			}
			if (this.AnyUsageNotHaveFlag(mbreadOnlyList, TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForTwoHanded))
			{
				tauntCategoryFlag |= MPArmoryCosmeticsVM.TauntCategoryFlag.UsableWithTwoHanded;
			}
			if (this.AnyUsageNotHaveFlag(mbreadOnlyList, TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForBow))
			{
				tauntCategoryFlag |= MPArmoryCosmeticsVM.TauntCategoryFlag.UsableWithBow;
			}
			if (this.AnyUsageNotHaveFlag(mbreadOnlyList, TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForCrossbow))
			{
				tauntCategoryFlag |= MPArmoryCosmeticsVM.TauntCategoryFlag.UsableWithCrossbow;
			}
			return tauntCategoryFlag;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00027964 File Offset: 0x00025B64
		private bool AllUsagesHaveFlag(MBReadOnlyList<TauntUsageManager.TauntUsage> list, TauntUsageManager.TauntUsage.TauntUsageFlag flag)
		{
			return list.All<TauntUsageManager.TauntUsage>((TauntUsageManager.TauntUsage u) => (u.UsageFlag & flag) > TauntUsageManager.TauntUsage.TauntUsageFlag.None);
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00027990 File Offset: 0x00025B90
		private bool AnyUsageHaveFlag(MBReadOnlyList<TauntUsageManager.TauntUsage> list, TauntUsageManager.TauntUsage.TauntUsageFlag flag)
		{
			return list.Any<TauntUsageManager.TauntUsage>((TauntUsageManager.TauntUsage u) => (u.UsageFlag & flag) > TauntUsageManager.TauntUsage.TauntUsageFlag.None);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x000279BC File Offset: 0x00025BBC
		private bool AnyUsageNotHaveFlag(MBReadOnlyList<TauntUsageManager.TauntUsage> list, TauntUsageManager.TauntUsage.TauntUsageFlag flag)
		{
			return list.Any<TauntUsageManager.TauntUsage>((TauntUsageManager.TauntUsage u) => (u.UsageFlag & flag) == TauntUsageManager.TauntUsage.TauntUsageFlag.None);
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x000279E8 File Offset: 0x00025BE8
		// (set) Token: 0x06000CBB RID: 3259 RVA: 0x000279F0 File Offset: 0x00025BF0
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
					base.UpdatePreviewAndActionTexts();
				}
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x00027A14 File Offset: 0x00025C14
		// (set) Token: 0x06000CBD RID: 3261 RVA: 0x00027A1C File Offset: 0x00025C1C
		[DataSourceProperty]
		public bool RequiresOnFoot
		{
			get
			{
				return this._requiresOnFoot;
			}
			set
			{
				if (value != this._requiresOnFoot)
				{
					this._requiresOnFoot = value;
					base.OnPropertyChangedWithValue(value, "RequiresOnFoot");
				}
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00027A3A File Offset: 0x00025C3A
		// (set) Token: 0x06000CBF RID: 3263 RVA: 0x00027A42 File Offset: 0x00025C42
		[DataSourceProperty]
		public float PreviewAnimationRatio
		{
			get
			{
				return this._previewAnimationRatio;
			}
			set
			{
				if (value != this._previewAnimationRatio)
				{
					this._previewAnimationRatio = value;
					base.OnPropertyChangedWithValue(value, "PreviewAnimationRatio");
				}
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00027A60 File Offset: 0x00025C60
		// (set) Token: 0x06000CC1 RID: 3265 RVA: 0x00027A68 File Offset: 0x00025C68
		[DataSourceProperty]
		public string SelectSlotText
		{
			get
			{
				return this._selectSlotText;
			}
			set
			{
				if (value != this._selectSlotText)
				{
					this._selectSlotText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectSlotText");
					base.UpdatePreviewAndActionTexts();
				}
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x00027A91 File Offset: 0x00025C91
		// (set) Token: 0x06000CC3 RID: 3267 RVA: 0x00027A99 File Offset: 0x00025C99
		[DataSourceProperty]
		public string CancelEquipText
		{
			get
			{
				return this._cancelEquipText;
			}
			set
			{
				if (value != this._cancelEquipText)
				{
					this._cancelEquipText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelEquipText");
					base.UpdatePreviewAndActionTexts();
				}
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x00027AC2 File Offset: 0x00025CC2
		// (set) Token: 0x06000CC5 RID: 3269 RVA: 0x00027ACA File Offset: 0x00025CCA
		[DataSourceProperty]
		public string TauntID
		{
			get
			{
				return this._tauntId;
			}
			set
			{
				if (value != this._tauntId)
				{
					this._tauntId = value;
					base.OnPropertyChangedWithValue<string>(value, "TauntID");
				}
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00027AED File Offset: 0x00025CED
		// (set) Token: 0x06000CC7 RID: 3271 RVA: 0x00027AF5 File Offset: 0x00025CF5
		[DataSourceProperty]
		public MBBindingList<StringItemWithEnabledAndHintVM> TauntUsages
		{
			get
			{
				return this._tauntUsages;
			}
			set
			{
				if (value != this._tauntUsages)
				{
					this._tauntUsages = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringItemWithEnabledAndHintVM>>(value, "TauntUsages");
				}
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x00027B13 File Offset: 0x00025D13
		// (set) Token: 0x06000CC9 RID: 3273 RVA: 0x00027B1B File Offset: 0x00025D1B
		[DataSourceProperty]
		public HintViewModel BlocksMovementOnUsageHint
		{
			get
			{
				return this._blocksMovementOnUsageHint;
			}
			set
			{
				if (value != this._blocksMovementOnUsageHint)
				{
					this._blocksMovementOnUsageHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "BlocksMovementOnUsageHint");
				}
			}
		}

		// Token: 0x040005C6 RID: 1478
		private bool _isSelected;

		// Token: 0x040005C7 RID: 1479
		private bool _requiresOnFoot;

		// Token: 0x040005C8 RID: 1480
		private float _previewAnimationRatio;

		// Token: 0x040005C9 RID: 1481
		private string _selectSlotText;

		// Token: 0x040005CA RID: 1482
		private string _cancelEquipText;

		// Token: 0x040005CB RID: 1483
		private string _tauntId;

		// Token: 0x040005CC RID: 1484
		private HintViewModel _blocksMovementOnUsageHint;

		// Token: 0x040005CD RID: 1485
		private MBBindingList<StringItemWithEnabledAndHintVM> _tauntUsages;
	}
}
