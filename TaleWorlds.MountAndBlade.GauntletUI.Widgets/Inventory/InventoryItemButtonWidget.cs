using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x0200013D RID: 317
	public abstract class InventoryItemButtonWidget : ButtonWidget
	{
		// Token: 0x06001069 RID: 4201 RVA: 0x0002CDD8 File Offset: 0x0002AFD8
		protected InventoryItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0002CDE1 File Offset: 0x0002AFE1
		protected override void OnDragBegin()
		{
			InventoryScreenWidget screenWidget = this.ScreenWidget;
			if (screenWidget != null)
			{
				screenWidget.ItemWidgetDragBegin(this);
			}
			base.OnDragBegin();
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0002CDFB File Offset: 0x0002AFFB
		protected override bool OnDrop()
		{
			InventoryScreenWidget screenWidget = this.ScreenWidget;
			if (screenWidget != null)
			{
				screenWidget.ItemWidgetDrop(this);
			}
			return base.OnDrop();
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0002CE18 File Offset: 0x0002B018
		private void AssignScreenWidget()
		{
			Widget widget = this;
			while (widget != base.EventManager.Root && this._screenWidget == null)
			{
				if (widget is InventoryScreenWidget)
				{
					this._screenWidget = (InventoryScreenWidget)widget;
				}
				else
				{
					widget = widget.ParentWidget;
				}
			}
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0002CE5C File Offset: 0x0002B05C
		private void ItemTypeUpdated()
		{
			AudioProperty audioProperty = base.Brush.SoundProperties.GetEventAudioProperty("DragEnd");
			if (audioProperty == null)
			{
				audioProperty = new AudioProperty();
				base.Brush.SoundProperties.AddEventSound("DragEnd", audioProperty);
			}
			audioProperty.AudioName = this.GetSound(this.ItemType);
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0002CEB0 File Offset: 0x0002B0B0
		private string GetSound(string typeID)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(typeID);
			if (num <= 1387635315U)
			{
				if (num <= 778761250U)
				{
					if (num <= 498656566U)
					{
						if (num != 302839205U)
						{
							if (num != 368918302U)
							{
								if (num != 498656566U)
								{
									goto IL_042D;
								}
								if (!(typeID == "Sling"))
								{
									goto IL_042D;
								}
								return "inventory/bow";
							}
							else
							{
								if (!(typeID == "LegArmor"))
								{
									goto IL_042D;
								}
								goto IL_0409;
							}
						}
						else
						{
							if (!(typeID == "ChestArmor"))
							{
								goto IL_042D;
							}
							goto IL_0403;
						}
					}
					else if (num != 678699352U)
					{
						if (num != 731742070U)
						{
							if (num != 778761250U)
							{
								goto IL_042D;
							}
							if (!(typeID == "HeadArmor"))
							{
								goto IL_042D;
							}
							return "inventory/helmet";
						}
						else
						{
							if (!(typeID == "Pistol"))
							{
								goto IL_042D;
							}
							goto IL_0427;
						}
					}
					else
					{
						if (!(typeID == "Book"))
						{
							goto IL_042D;
						}
						return "inventory/book";
					}
				}
				else if (num <= 995063962U)
				{
					if (num != 784896431U)
					{
						if (num != 881552253U)
						{
							if (num != 995063962U)
							{
								goto IL_042D;
							}
							if (!(typeID == "Cape"))
							{
								goto IL_042D;
							}
							goto IL_0403;
						}
						else
						{
							if (!(typeID == "Animal"))
							{
								goto IL_042D;
							}
							return "inventory/animal";
						}
					}
					else
					{
						if (!(typeID == "Banner"))
						{
							goto IL_042D;
						}
						return "inventory/perk";
					}
				}
				else if (num <= 1061154663U)
				{
					if (num != 1048100111U)
					{
						if (num != 1061154663U)
						{
							goto IL_042D;
						}
						if (!(typeID == "OneHandedWeapon"))
						{
							goto IL_042D;
						}
						return "inventory/onehanded";
					}
					else if (!(typeID == "Bolts"))
					{
						goto IL_042D;
					}
				}
				else if (num != 1095128646U)
				{
					if (num != 1387635315U)
					{
						goto IL_042D;
					}
					if (!(typeID == "Thrown"))
					{
						goto IL_042D;
					}
					return "inventory/throwing";
				}
				else
				{
					if (!(typeID == "Horse"))
					{
						goto IL_042D;
					}
					return "inventory/horse";
				}
			}
			else if (num <= 2996768862U)
			{
				if (num <= 1982439889U)
				{
					if (num != 1486204743U)
					{
						if (num != 1721772824U)
						{
							if (num != 1982439889U)
							{
								goto IL_042D;
							}
							if (!(typeID == "Goods"))
							{
								goto IL_042D;
							}
							return "inventory/sack";
						}
						else if (!(typeID == "SlingStones"))
						{
							goto IL_042D;
						}
					}
					else
					{
						if (!(typeID == "HandArmor"))
						{
							goto IL_042D;
						}
						goto IL_0409;
					}
				}
				else if (num != 2039097040U)
				{
					if (num != 2161253412U)
					{
						if (num != 2996768862U)
						{
							goto IL_042D;
						}
						if (!(typeID == "Bullets"))
						{
							goto IL_042D;
						}
						goto IL_0427;
					}
					else
					{
						if (!(typeID == "BodyArmor"))
						{
							goto IL_042D;
						}
						goto IL_0403;
					}
				}
				else
				{
					if (!(typeID == "Shield"))
					{
						goto IL_042D;
					}
					return "inventory/shield";
				}
			}
			else if (num <= 3618788796U)
			{
				if (num != 3083591375U)
				{
					if (num != 3565557811U)
					{
						if (num != 3618788796U)
						{
							goto IL_042D;
						}
						if (!(typeID == "Musket"))
						{
							goto IL_042D;
						}
						goto IL_0427;
					}
					else
					{
						if (!(typeID == "Polearm"))
						{
							goto IL_042D;
						}
						return "inventory/polearm";
					}
				}
				else if (!(typeID == "Arrows"))
				{
					goto IL_042D;
				}
			}
			else if (num <= 3656874833U)
			{
				if (num != 3637216139U)
				{
					if (num != 3656874833U)
					{
						goto IL_042D;
					}
					if (!(typeID == "TwoHandedWeapon"))
					{
						goto IL_042D;
					}
					return "inventory/twohanded";
				}
				else
				{
					if (!(typeID == "Bow"))
					{
						goto IL_042D;
					}
					return "inventory/bow";
				}
			}
			else if (num != 3918828990U)
			{
				if (num != 4282369777U)
				{
					goto IL_042D;
				}
				if (!(typeID == "Crossbow"))
				{
					goto IL_042D;
				}
				return "inventory/crossbow";
			}
			else
			{
				if (!(typeID == "HorseHarness"))
				{
					goto IL_042D;
				}
				return "inventory/horsearmor";
			}
			return "inventory/quiver";
			IL_0403:
			return "inventory/leather";
			IL_0409:
			return "inventory/leather_lite";
			IL_0427:
			return "inventory/leather";
			IL_042D:
			return "inventory/leather";
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x0002D2EF File Offset: 0x0002B4EF
		// (set) Token: 0x06001070 RID: 4208 RVA: 0x0002D2F7 File Offset: 0x0002B4F7
		[Editor(false)]
		public bool IsRightSide
		{
			get
			{
				return this._isRightSide;
			}
			set
			{
				if (this._isRightSide != value)
				{
					this._isRightSide = value;
					base.OnPropertyChanged(value, "IsRightSide");
				}
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x0002D315 File Offset: 0x0002B515
		// (set) Token: 0x06001072 RID: 4210 RVA: 0x0002D31D File Offset: 0x0002B51D
		[Editor(false)]
		public string ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (this._itemType != value)
				{
					this._itemType = value;
					base.OnPropertyChanged<string>(value, "ItemType");
					this.ItemTypeUpdated();
				}
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x0002D346 File Offset: 0x0002B546
		// (set) Token: 0x06001074 RID: 4212 RVA: 0x0002D34E File Offset: 0x0002B54E
		[Editor(false)]
		public int EquipmentIndex
		{
			get
			{
				return this._equipmentIndex;
			}
			set
			{
				if (this._equipmentIndex != value)
				{
					this._equipmentIndex = value;
					base.OnPropertyChanged(value, "EquipmentIndex");
				}
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x0002D36C File Offset: 0x0002B56C
		public InventoryScreenWidget ScreenWidget
		{
			get
			{
				if (this._screenWidget == null)
				{
					this.AssignScreenWidget();
				}
				return this._screenWidget;
			}
		}

		// Token: 0x0400076B RID: 1899
		private bool _isRightSide;

		// Token: 0x0400076C RID: 1900
		private string _itemType;

		// Token: 0x0400076D RID: 1901
		private int _equipmentIndex;

		// Token: 0x0400076E RID: 1902
		private InventoryScreenWidget _screenWidget;
	}
}
