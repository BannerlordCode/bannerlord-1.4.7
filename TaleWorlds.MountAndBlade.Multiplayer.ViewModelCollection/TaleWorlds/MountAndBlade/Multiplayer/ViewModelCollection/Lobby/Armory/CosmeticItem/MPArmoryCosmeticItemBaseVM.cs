using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem
{
	// Token: 0x0200007E RID: 126
	public abstract class MPArmoryCosmeticItemBaseVM : ViewModel
	{
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000C81 RID: 3201 RVA: 0x00026FA0 File Offset: 0x000251A0
		// (remove) Token: 0x06000C82 RID: 3202 RVA: 0x00026FD4 File Offset: 0x000251D4
		public static event Action<MPArmoryCosmeticItemBaseVM> OnEquipped;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000C83 RID: 3203 RVA: 0x00027008 File Offset: 0x00025208
		// (remove) Token: 0x06000C84 RID: 3204 RVA: 0x0002703C File Offset: 0x0002523C
		public static event Action<MPArmoryCosmeticItemBaseVM> OnPurchaseRequested;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000C85 RID: 3205 RVA: 0x00027070 File Offset: 0x00025270
		// (remove) Token: 0x06000C86 RID: 3206 RVA: 0x000270A4 File Offset: 0x000252A4
		public static event Action<MPArmoryCosmeticItemBaseVM> OnPreviewed;

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x000270D7 File Offset: 0x000252D7
		// (set) Token: 0x06000C88 RID: 3208 RVA: 0x000270DF File Offset: 0x000252DF
		public string UnequipText { get; private set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x000270E8 File Offset: 0x000252E8
		public CosmeticsManager.CosmeticType CosmeticType { get; }

		// Token: 0x06000C8A RID: 3210 RVA: 0x000270F0 File Offset: 0x000252F0
		public MPArmoryCosmeticItemBaseVM(CosmeticElement cosmetic, string cosmeticID, CosmeticsManager.CosmeticType cosmeticType)
		{
			this.Cosmetic = cosmetic;
			this.CosmeticID = cosmeticID;
			this.Cost = cosmetic.Cost;
			this.Rarity = (int)cosmetic.Rarity;
			this.CosmeticType = cosmeticType;
			this.IsUnequippable = true;
			this.RefreshValues();
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0002713D File Offset: 0x0002533D
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.OwnedText = new TextObject("{=B5bcj3pC}Owned", null).ToString();
			this.UnequipText = new TextObject("{=QndVFTbx}Unequip", null).ToString();
			this.UpdatePreviewAndActionTexts();
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00027177 File Offset: 0x00025377
		public override void OnFinalize()
		{
			InputKeyItemVM actionKey = this.ActionKey;
			if (actionKey != null)
			{
				actionKey.OnFinalize();
			}
			InputKeyItemVM previewKey = this.PreviewKey;
			if (previewKey == null)
			{
				return;
			}
			previewKey.OnFinalize();
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0002719A File Offset: 0x0002539A
		public void ExecuteAction()
		{
			if (!this.IsUnlocked)
			{
				MPArmoryCosmeticItemBaseVM.OnPurchaseRequested(this);
				return;
			}
			Action<MPArmoryCosmeticItemBaseVM> onEquipped = MPArmoryCosmeticItemBaseVM.OnEquipped;
			if (onEquipped == null)
			{
				return;
			}
			onEquipped(this);
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x000271C0 File Offset: 0x000253C0
		public void ExecutePreview()
		{
			MPArmoryCosmeticItemBaseVM.OnPreviewed(this);
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x000271CD File Offset: 0x000253CD
		public void ExecuteEnableActions()
		{
			this.AreActionsEnabled = true;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x000271D6 File Offset: 0x000253D6
		public void ExecuteDisableActions()
		{
			this.AreActionsEnabled = false;
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x000271E0 File Offset: 0x000253E0
		protected void UpdatePreviewAndActionTexts()
		{
			if (this.IsUnlocked)
			{
				if (this.IsUsed)
				{
					this.ActionText = (this.IsUnequippable ? this.UnequipText : string.Empty);
				}
				else
				{
					this.ActionText = new TextObject("{=DKqLY1aJ}Equip", null).ToString();
				}
			}
			else
			{
				this.ActionText = new TextObject("{=i2mNBaxE}Obtain", null).ToString();
			}
			this.PreviewText = new TextObject("{=un7poy9x}Preview", null).ToString();
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00027260 File Offset: 0x00025460
		public void RefreshKeyBindings(HotKey actionKey, HotKey previewKey)
		{
			if (this.IsUnlocked && this.IsUsed && !this.IsUnequippable)
			{
				this.ActionKey = InputKeyItemVM.CreateFromHotKey(null, false);
			}
			else
			{
				string groupId = actionKey.GroupId;
				InputKeyItemVM actionKey2 = this.ActionKey;
				string text;
				if (actionKey2 == null)
				{
					text = null;
				}
				else
				{
					HotKey hotKey = actionKey2.HotKey;
					text = ((hotKey != null) ? hotKey.GroupId : null);
				}
				if (!(groupId != text))
				{
					string id = actionKey.Id;
					InputKeyItemVM actionKey3 = this.ActionKey;
					string text2;
					if (actionKey3 == null)
					{
						text2 = null;
					}
					else
					{
						HotKey hotKey2 = actionKey3.HotKey;
						text2 = ((hotKey2 != null) ? hotKey2.Id : null);
					}
					if (!(id != text2))
					{
						goto IL_008A;
					}
				}
				this.ActionKey = InputKeyItemVM.CreateFromHotKey(actionKey, false);
			}
			IL_008A:
			string groupId2 = previewKey.GroupId;
			InputKeyItemVM previewKey2 = this.PreviewKey;
			string text3;
			if (previewKey2 == null)
			{
				text3 = null;
			}
			else
			{
				HotKey hotKey3 = previewKey2.HotKey;
				text3 = ((hotKey3 != null) ? hotKey3.GroupId : null);
			}
			if (!(groupId2 != text3))
			{
				string id2 = previewKey.Id;
				InputKeyItemVM previewKey3 = this.PreviewKey;
				string text4;
				if (previewKey3 == null)
				{
					text4 = null;
				}
				else
				{
					HotKey hotKey4 = previewKey3.HotKey;
					text4 = ((hotKey4 != null) ? hotKey4.Id : null);
				}
				if (!(id2 != text4))
				{
					goto IL_00ED;
				}
			}
			this.PreviewKey = InputKeyItemVM.CreateFromHotKey(previewKey, false);
			IL_00ED:
			this.UpdatePreviewAndActionTexts();
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x00027360 File Offset: 0x00025560
		// (set) Token: 0x06000C94 RID: 3220 RVA: 0x00027368 File Offset: 0x00025568
		[DataSourceProperty]
		public bool IsUnlocked
		{
			get
			{
				return this._isUnlocked;
			}
			set
			{
				if (value != this._isUnlocked)
				{
					this._isUnlocked = value;
					base.OnPropertyChangedWithValue(value, "IsUnlocked");
					this.UpdatePreviewAndActionTexts();
				}
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0002738C File Offset: 0x0002558C
		// (set) Token: 0x06000C96 RID: 3222 RVA: 0x00027394 File Offset: 0x00025594
		[DataSourceProperty]
		public bool IsUsed
		{
			get
			{
				return this._isUsed;
			}
			set
			{
				if (value != this._isUsed)
				{
					this._isUsed = value;
					base.OnPropertyChangedWithValue(value, "IsUsed");
					this.UpdatePreviewAndActionTexts();
				}
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x000273B8 File Offset: 0x000255B8
		// (set) Token: 0x06000C98 RID: 3224 RVA: 0x000273C0 File Offset: 0x000255C0
		[DataSourceProperty]
		public bool AreActionsEnabled
		{
			get
			{
				return this._areActionsEnabled;
			}
			set
			{
				if (value != this._areActionsEnabled)
				{
					this._areActionsEnabled = value;
					base.OnPropertyChangedWithValue(value, "AreActionsEnabled");
				}
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x000273DE File Offset: 0x000255DE
		// (set) Token: 0x06000C9A RID: 3226 RVA: 0x000273E6 File Offset: 0x000255E6
		[DataSourceProperty]
		public bool IsSelectable
		{
			get
			{
				return this._isSelectable;
			}
			set
			{
				if (value != this._isSelectable)
				{
					this._isSelectable = value;
					base.OnPropertyChangedWithValue(value, "IsSelectable");
				}
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00027404 File Offset: 0x00025604
		// (set) Token: 0x06000C9C RID: 3228 RVA: 0x0002740C File Offset: 0x0002560C
		[DataSourceProperty]
		public bool IsUnequippable
		{
			get
			{
				return this._isUnequippable;
			}
			set
			{
				if (value != this._isUnequippable)
				{
					this._isUnequippable = value;
					base.OnPropertyChangedWithValue(value, "IsUnequippable");
				}
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x0002742A File Offset: 0x0002562A
		// (set) Token: 0x06000C9E RID: 3230 RVA: 0x00027432 File Offset: 0x00025632
		[DataSourceProperty]
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				if (value != this._cost)
				{
					this._cost = value;
					base.OnPropertyChangedWithValue(value, "Cost");
				}
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x00027450 File Offset: 0x00025650
		// (set) Token: 0x06000CA0 RID: 3232 RVA: 0x00027458 File Offset: 0x00025658
		[DataSourceProperty]
		public int Rarity
		{
			get
			{
				return this._rarity;
			}
			set
			{
				if (value != this._rarity)
				{
					this._rarity = value;
					base.OnPropertyChangedWithValue(value, "Rarity");
				}
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x00027476 File Offset: 0x00025676
		// (set) Token: 0x06000CA2 RID: 3234 RVA: 0x0002747E File Offset: 0x0002567E
		[DataSourceProperty]
		public int ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChangedWithValue(value, "ItemType");
				}
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x0002749C File Offset: 0x0002569C
		// (set) Token: 0x06000CA4 RID: 3236 RVA: 0x000274A4 File Offset: 0x000256A4
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

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x000274C7 File Offset: 0x000256C7
		// (set) Token: 0x06000CA6 RID: 3238 RVA: 0x000274CF File Offset: 0x000256CF
		[DataSourceProperty]
		public string OwnedText
		{
			get
			{
				return this._ownedText;
			}
			set
			{
				if (value != this._ownedText)
				{
					this._ownedText = value;
					base.OnPropertyChangedWithValue<string>(value, "OwnedText");
				}
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x000274F2 File Offset: 0x000256F2
		// (set) Token: 0x06000CA8 RID: 3240 RVA: 0x000274FA File Offset: 0x000256FA
		[DataSourceProperty]
		public string ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (value != this._actionText)
				{
					this._actionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionText");
				}
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x0002751D File Offset: 0x0002571D
		// (set) Token: 0x06000CAA RID: 3242 RVA: 0x00027525 File Offset: 0x00025725
		[DataSourceProperty]
		public string PreviewText
		{
			get
			{
				return this._previewText;
			}
			set
			{
				if (value != this._previewText)
				{
					this._previewText = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviewText");
				}
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x00027548 File Offset: 0x00025748
		// (set) Token: 0x06000CAC RID: 3244 RVA: 0x00027550 File Offset: 0x00025750
		[DataSourceProperty]
		public ItemImageIdentifierVM Icon
		{
			get
			{
				return this._icon;
			}
			set
			{
				if (value != this._icon)
				{
					this._icon = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "Icon");
				}
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x0002756E File Offset: 0x0002576E
		// (set) Token: 0x06000CAE RID: 3246 RVA: 0x00027576 File Offset: 0x00025776
		[DataSourceProperty]
		public InputKeyItemVM ActionKey
		{
			get
			{
				return this._actionKey;
			}
			set
			{
				if (value != this._actionKey)
				{
					this._actionKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ActionKey");
				}
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00027594 File Offset: 0x00025794
		// (set) Token: 0x06000CB0 RID: 3248 RVA: 0x0002759C File Offset: 0x0002579C
		[DataSourceProperty]
		public InputKeyItemVM PreviewKey
		{
			get
			{
				return this._previewKey;
			}
			set
			{
				if (value != this._previewKey)
				{
					this._previewKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviewKey");
				}
			}
		}

		// Token: 0x040005AE RID: 1454
		public readonly CosmeticElement Cosmetic;

		// Token: 0x040005AF RID: 1455
		public readonly string CosmeticID;

		// Token: 0x040005B5 RID: 1461
		private bool _isUnlocked;

		// Token: 0x040005B6 RID: 1462
		private bool _isUsed;

		// Token: 0x040005B7 RID: 1463
		private bool _areActionsEnabled;

		// Token: 0x040005B8 RID: 1464
		private bool _isSelectable;

		// Token: 0x040005B9 RID: 1465
		private bool _isUnequippable;

		// Token: 0x040005BA RID: 1466
		private int _cost;

		// Token: 0x040005BB RID: 1467
		private int _rarity;

		// Token: 0x040005BC RID: 1468
		private int _itemType;

		// Token: 0x040005BD RID: 1469
		private string _name;

		// Token: 0x040005BE RID: 1470
		private string _ownedText;

		// Token: 0x040005BF RID: 1471
		private string _actionText;

		// Token: 0x040005C0 RID: 1472
		private string _previewText;

		// Token: 0x040005C1 RID: 1473
		private ItemImageIdentifierVM _icon;

		// Token: 0x040005C2 RID: 1474
		private InputKeyItemVM _actionKey;

		// Token: 0x040005C3 RID: 1475
		private InputKeyItemVM _previewKey;
	}
}
