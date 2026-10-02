using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options
{
	// Token: 0x02000077 RID: 119
	public class OptionsItemWidget : Widget
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x00012B1E File Offset: 0x00010D1E
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x00012B26 File Offset: 0x00010D26
		public Widget BooleanOption { get; set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00012B2F File Offset: 0x00010D2F
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x00012B37 File Offset: 0x00010D37
		public Widget NumericOption { get; set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x00012B40 File Offset: 0x00010D40
		// (set) Token: 0x0600065F RID: 1631 RVA: 0x00012B48 File Offset: 0x00010D48
		public Widget StringOption { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00012B51 File Offset: 0x00010D51
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x00012B59 File Offset: 0x00010D59
		public Widget GameKeyOption { get; set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x00012B62 File Offset: 0x00010D62
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x00012B6A File Offset: 0x00010D6A
		public Widget ActionOption { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x00012B73 File Offset: 0x00010D73
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x00012B7B File Offset: 0x00010D7B
		public Widget InputOption { get; set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x00012B84 File Offset: 0x00010D84
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x00012B8C File Offset: 0x00010D8C
		public AnimatedDropdownWidget DropdownWidget
		{
			get
			{
				return this._dropdownWidget;
			}
			set
			{
				if (value != this._dropdownWidget)
				{
					this._dropdownWidget = value;
				}
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x00012B9E File Offset: 0x00010D9E
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x00012BA6 File Offset: 0x00010DA6
		public ButtonWidget BooleanToggleButtonWidget
		{
			get
			{
				return this._booleanToggleButtonWidget;
			}
			set
			{
				if (value != this._booleanToggleButtonWidget)
				{
					this._booleanToggleButtonWidget = value;
				}
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00012BB8 File Offset: 0x00010DB8
		public OptionsItemWidget(UIContext context)
			: base(context)
		{
			this._optionTypeID = -1;
			this._graphicsSprites = new List<Sprite>();
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00012BDC File Offset: 0x00010DDC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.SetCurrentScreenWidget(this.FindScreenWidget(base.ParentWidget));
				if (this.ImageIDs != null)
				{
					for (int i = 0; i < this.ImageIDs.Length; i++)
					{
						if (this.ImageIDs[i] != string.Empty)
						{
							Sprite sprite = base.Context.SpriteData.GetSprite(this.ImageIDs[i]);
							this._graphicsSprites.Add(sprite);
						}
					}
				}
				this.RefreshVisibilityOfSubItems();
				this.ResetNavigationIndices();
				this._initialized = true;
			}
			if (!this._eventsRegistered)
			{
				this.RegisterHoverEvents();
				this._eventsRegistered = true;
			}
			if (this._isEnabledStateDirty)
			{
				Widget currentOptionWidget = this.GetCurrentOptionWidget();
				if (currentOptionWidget != null)
				{
					currentOptionWidget.ApplyActionToAllChildrenRecursive(delegate(Widget child)
					{
						child.IsEnabled = this.IsOptionEnabled;
					});
				}
				this._isEnabledStateDirty = false;
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00012CB1 File Offset: 0x00010EB1
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			this.SetCurrentOption(false, false, -1);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00012CC2 File Offset: 0x00010EC2
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			this.ResetCurrentOption();
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00012CD0 File Offset: 0x00010ED0
		private OptionsScreenWidget FindScreenWidget(Widget parent)
		{
			OptionsScreenWidget optionsScreenWidget;
			if ((optionsScreenWidget = parent as OptionsScreenWidget) != null)
			{
				return optionsScreenWidget;
			}
			if (parent == null)
			{
				return null;
			}
			return this.FindScreenWidget(parent.ParentWidget);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00012CFC File Offset: 0x00010EFC
		private void SetCurrentOption(bool fromHoverOverDropdown, bool fromBooleanSelection, int hoverDropdownItemIndex = -1)
		{
			if (this._optionTypeID == 3)
			{
				Sprite sprite;
				if (fromHoverOverDropdown)
				{
					sprite = ((this._graphicsSprites.Count > hoverDropdownItemIndex) ? this._graphicsSprites[hoverDropdownItemIndex] : null);
				}
				else
				{
					sprite = ((this._graphicsSprites.Count > this.DropdownWidget.CurrentSelectedIndex && this.DropdownWidget.CurrentSelectedIndex >= 0) ? this._graphicsSprites[this.DropdownWidget.CurrentSelectedIndex] : null);
				}
				OptionsScreenWidget screenWidget = this._screenWidget;
				if (screenWidget == null)
				{
					return;
				}
				screenWidget.SetCurrentOption(this, sprite);
				return;
			}
			else if (this._optionTypeID == 0)
			{
				int num = (this.BooleanToggleButtonWidget.IsSelected ? 0 : 1);
				Sprite sprite2 = ((this._graphicsSprites.Count > num) ? this._graphicsSprites[num] : null);
				OptionsScreenWidget screenWidget2 = this._screenWidget;
				if (screenWidget2 == null)
				{
					return;
				}
				screenWidget2.SetCurrentOption(this, sprite2);
				return;
			}
			else
			{
				OptionsScreenWidget screenWidget3 = this._screenWidget;
				if (screenWidget3 == null)
				{
					return;
				}
				screenWidget3.SetCurrentOption(this, null);
				return;
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00012DE7 File Offset: 0x00010FE7
		public void SetCurrentScreenWidget(OptionsScreenWidget screenWidget)
		{
			this._screenWidget = screenWidget;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00012DF0 File Offset: 0x00010FF0
		private void ResetCurrentOption()
		{
			OptionsScreenWidget screenWidget = this._screenWidget;
			if (screenWidget == null)
			{
				return;
			}
			screenWidget.SetCurrentOption(null, null);
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00012E04 File Offset: 0x00011004
		private void RegisterHoverEvents()
		{
			base.ApplyActionToAllChildrenRecursive(delegate(Widget child)
			{
				child.boolPropertyChanged += this.Child_PropertyChanged;
			});
			if (this.OptionTypeID == 0)
			{
				this.BooleanToggleButtonWidget.boolPropertyChanged += this.BooleanOption_PropertyChanged;
				return;
			}
			if (this.OptionTypeID == 3)
			{
				this._dropdownExtensionParentWidget = this.DropdownWidget.DropdownClipWidget;
				this._dropdownExtensionParentWidget.ApplyActionToAllChildrenRecursive(delegate(Widget child)
				{
					child.boolPropertyChanged += this.DropdownItem_PropertyChanged1;
				});
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00012E74 File Offset: 0x00011074
		private void BooleanOption_PropertyChanged(PropertyOwnerObject childWidget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsSelected")
			{
				this.SetCurrentOption(false, true, -1);
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00012E8C File Offset: 0x0001108C
		private void Child_PropertyChanged(PropertyOwnerObject childWidget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsHovered")
			{
				if (propertyValue)
				{
					this.SetCurrentOption(false, false, -1);
					return;
				}
				this.ResetCurrentOption();
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00012EB0 File Offset: 0x000110B0
		private void DropdownItem_PropertyChanged1(PropertyOwnerObject childWidget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsHovered")
			{
				if (propertyValue)
				{
					Widget widget = childWidget as Widget;
					this.SetCurrentOption(true, false, widget.ParentWidget.GetChildIndex(widget));
					return;
				}
				this.ResetCurrentOption();
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00012EF0 File Offset: 0x000110F0
		private void RefreshVisibilityOfSubItems()
		{
			this.BooleanOption.IsVisible = this.OptionTypeID == 0;
			this.NumericOption.IsVisible = this.OptionTypeID == 1;
			this.StringOption.IsVisible = this.OptionTypeID == 3;
			this.GameKeyOption.IsVisible = this.OptionTypeID == 2;
			this.InputOption.IsVisible = this.OptionTypeID == 4;
			if (this.ActionOption != null)
			{
				this.ActionOption.IsVisible = this.OptionTypeID == 5;
			}
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00012F80 File Offset: 0x00011180
		private Widget GetCurrentOptionWidget()
		{
			switch (this.OptionTypeID)
			{
			case 0:
				return this.BooleanOption;
			case 1:
				return this.NumericOption;
			case 2:
				return this.StringOption;
			case 3:
				return this.GameKeyOption;
			case 4:
				return this.InputOption;
			case 5:
				return this.ActionOption;
			default:
				return null;
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00012FE0 File Offset: 0x000111E0
		private void ResetNavigationIndices()
		{
			if (base.GamepadNavigationIndex == -1)
			{
				return;
			}
			bool flag = false;
			Widget booleanOption = this.BooleanOption;
			if (booleanOption != null && booleanOption.IsVisible)
			{
				this.BooleanOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
				flag = true;
			}
			else
			{
				Widget numericOption = this.NumericOption;
				if (numericOption != null && numericOption.IsVisible)
				{
					this.NumericOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
					flag = true;
				}
				else
				{
					Widget stringOption = this.StringOption;
					if (stringOption != null && stringOption.IsVisible)
					{
						this.StringOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
						flag = true;
					}
					else
					{
						Widget gameKeyOption = this.GameKeyOption;
						if (gameKeyOption != null && gameKeyOption.IsVisible)
						{
							this.GameKeyOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
							flag = true;
						}
						else
						{
							Widget inputOption = this.InputOption;
							if (inputOption != null && inputOption.IsVisible)
							{
								this.InputOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
								flag = true;
							}
							else
							{
								Widget actionOption = this.ActionOption;
								if (actionOption != null && actionOption.IsVisible)
								{
									this.ActionOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
									flag = true;
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				base.GamepadNavigationIndex = -1;
				return;
			}
			Debug.FailedAssert("No option type is visible for: " + base.GetType().Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Options\\OptionsItemWidget.cs", "ResetNavigationIndices", 310);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00013127 File Offset: 0x00011327
		protected override void OnGamepadNavigationIndexUpdated(int newIndex)
		{
			if (this._initialized)
			{
				this.ResetNavigationIndices();
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x00013137 File Offset: 0x00011337
		// (set) Token: 0x0600067B RID: 1659 RVA: 0x0001313F File Offset: 0x0001133F
		public int OptionTypeID
		{
			get
			{
				return this._optionTypeID;
			}
			set
			{
				if (this._optionTypeID != value)
				{
					this._optionTypeID = value;
					base.OnPropertyChanged(value, "OptionTypeID");
				}
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x0001315D File Offset: 0x0001135D
		// (set) Token: 0x0600067D RID: 1661 RVA: 0x00013165 File Offset: 0x00011365
		public bool IsOptionEnabled
		{
			get
			{
				return this._isOptionEnabled;
			}
			set
			{
				if (this._isOptionEnabled != value)
				{
					this._isOptionEnabled = value;
					base.OnPropertyChanged(value, "IsOptionEnabled");
					this._isEnabledStateDirty = true;
				}
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001318A File Offset: 0x0001138A
		// (set) Token: 0x0600067F RID: 1663 RVA: 0x00013192 File Offset: 0x00011392
		public string OptionTitle
		{
			get
			{
				return this._optionTitle;
			}
			set
			{
				if (this._optionTitle != value)
				{
					this._optionTitle = value;
				}
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x000131A9 File Offset: 0x000113A9
		// (set) Token: 0x06000681 RID: 1665 RVA: 0x000131B1 File Offset: 0x000113B1
		public string[] ImageIDs
		{
			get
			{
				return this._imageIDs;
			}
			set
			{
				if (this._imageIDs != value)
				{
					this._imageIDs = value;
				}
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x000131C3 File Offset: 0x000113C3
		// (set) Token: 0x06000683 RID: 1667 RVA: 0x000131CB File Offset: 0x000113CB
		public string OptionDescription
		{
			get
			{
				return this._optionDescription;
			}
			set
			{
				if (this._optionDescription != value)
				{
					this._optionDescription = value;
				}
			}
		}

		// Token: 0x040002BF RID: 703
		private ButtonWidget _booleanToggleButtonWidget;

		// Token: 0x040002C0 RID: 704
		private AnimatedDropdownWidget _dropdownWidget;

		// Token: 0x040002C1 RID: 705
		private OptionsScreenWidget _screenWidget;

		// Token: 0x040002C2 RID: 706
		private Widget _dropdownExtensionParentWidget;

		// Token: 0x040002C3 RID: 707
		private bool _eventsRegistered;

		// Token: 0x040002C4 RID: 708
		private bool _initialized;

		// Token: 0x040002C5 RID: 709
		private List<Sprite> _graphicsSprites;

		// Token: 0x040002C6 RID: 710
		private bool _isEnabledStateDirty = true;

		// Token: 0x040002C7 RID: 711
		private int _optionTypeID;

		// Token: 0x040002C8 RID: 712
		private string _optionDescription;

		// Token: 0x040002C9 RID: 713
		private string _optionTitle;

		// Token: 0x040002CA RID: 714
		private string[] _imageIDs;

		// Token: 0x040002CB RID: 715
		private bool _isOptionEnabled;
	}
}
