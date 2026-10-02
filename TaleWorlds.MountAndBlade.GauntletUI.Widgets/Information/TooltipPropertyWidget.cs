using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information
{
	// Token: 0x02000148 RID: 328
	public class TooltipPropertyWidget : Widget
	{
		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x0002FB5D File Offset: 0x0002DD5D
		// (set) Token: 0x06001158 RID: 4440 RVA: 0x0002FB65 File Offset: 0x0002DD65
		public bool IsTwoColumn { get; private set; }

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001159 RID: 4441 RVA: 0x0002FB6E File Offset: 0x0002DD6E
		// (set) Token: 0x0600115A RID: 4442 RVA: 0x0002FB76 File Offset: 0x0002DD76
		public TooltipPropertyWidget.TooltipPropertyFlags PropertyModifierAsFlag { get; private set; }

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x0002FB7F File Offset: 0x0002DD7F
		private bool _allBrushesInitialized
		{
			get
			{
				return this.SubtextBrush != null && this.ValueTextBrush != null && this.DescriptionTextBrush != null && this.ValueNameTextBrush != null && this.RundownSeperatorSprite != null && this.DefaultSeperatorSprite != null && this.TitleBackgroundSprite != null;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x0002FBBC File Offset: 0x0002DDBC
		public bool IsMultiLine
		{
			get
			{
				return this._isMultiLine;
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x0002FBC4 File Offset: 0x0002DDC4
		public bool IsBattleMode
		{
			get
			{
				return this._isBattleMode;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x0002FBCC File Offset: 0x0002DDCC
		public bool IsBattleModeOver
		{
			get
			{
				return this._isBattleModeOver;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x0600115F RID: 4447 RVA: 0x0002FBD4 File Offset: 0x0002DDD4
		public bool IsCost
		{
			get
			{
				return this._isCost;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x0002FBDC File Offset: 0x0002DDDC
		public bool IsRelation
		{
			get
			{
				return this._isRelation;
			}
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0002FBE4 File Offset: 0x0002DDE4
		public TooltipPropertyWidget(UIContext context)
			: base(context)
		{
			this._isMultiLine = false;
			this._isBattleMode = false;
			this._isBattleModeOver = false;
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0002FC10 File Offset: 0x0002DE10
		public void SetBattleScope(bool battleScope)
		{
			if (battleScope)
			{
				this.DefinitionLabel.HorizontalAlignment = HorizontalAlignment.Center;
				this.ValueLabel.HorizontalAlignment = HorizontalAlignment.Center;
				return;
			}
			this.DefinitionLabel.HorizontalAlignment = HorizontalAlignment.Right;
			this.ValueLabel.HorizontalAlignment = HorizontalAlignment.Left;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0002FC48 File Offset: 0x0002DE48
		public void RefreshSize(bool inBattleScope, float battleScopeSize, float maxValueLabelSizeX, float maxDefinitionLabelSizeX, Brush definitionRelationBrush = null, Brush valueRelationBrush = null)
		{
			if (this._isMultiLine || this._isSubtext)
			{
				this.DefinitionLabelContainer.IsVisible = false;
				this.DefinitionLabelContainer.ScaledSuggestedWidth = 0f;
				this.ValueLabel.WidthSizePolicy = SizePolicy.Fixed;
				this.ValueLabelContainer.WidthSizePolicy = SizePolicy.Fixed;
				this.ValueLabel.ScaledSuggestedWidth = base.ParentWidget.Size.X - (base.ScaledMarginLeft + base.ScaledMarginRight);
				this.ValueLabelContainer.ScaledSuggestedWidth = base.ParentWidget.Size.X - (base.ScaledMarginLeft + base.ScaledMarginRight);
			}
			else if (inBattleScope)
			{
				this.DefinitionLabelContainer.ScaledSuggestedWidth = battleScopeSize;
				this.DefinitionLabel.Brush = definitionRelationBrush;
				this.ValueLabelContainer.ScaledSuggestedWidth = battleScopeSize;
				this.ValueLabel.Brush = valueRelationBrush;
				this.ValueLabelContainer.HorizontalAlignment = HorizontalAlignment.Left;
				this.ValueLabel.HorizontalAlignment = HorizontalAlignment.Left;
				this.ValueLabel.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Left;
			}
			else if (!this.IsTwoColumn)
			{
				if (!string.IsNullOrEmpty(this.DefinitionLabel.Text))
				{
					float num = ((this.DefinitionLabel.Size.X > this.ValueLabel.Size.X) ? this.DefinitionLabel.Size.X : this.ValueLabel.Size.X);
					this.DefinitionLabelContainer.ScaledSuggestedWidth = num;
					this.ValueLabelContainer.ScaledSuggestedWidth = num;
				}
				else
				{
					this.DefinitionLabelContainer.ScaledSuggestedWidth = 0f;
					this.DefinitionLabelContainer.IsVisible = false;
					this.ValueLabelContainer.ScaledSuggestedWidth = this.ValueLabel.Size.X;
				}
			}
			if (this.IsTwoColumn && !this._isMultiLine && (!this._isTitle || (this._isTitle && this.IsTwoColumn)))
			{
				this.ValueLabelContainer.WidthSizePolicy = SizePolicy.Fixed;
				this.ValueLabelContainer.ScaledSuggestedWidth = MathF.Max(53f * base._scaleToUse, maxValueLabelSizeX);
				if (this.ItemModifierLabel != null && this.ItemModifierLabel.IsVisible)
				{
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabel.MaxWidth = MathF.Max(53f, maxValueLabelSizeX * base._inverseScaleToUse) - this.ItemModifierLabel.Size.X * base._inverseScaleToUse - (this.ItemModifierLabel.MarginLeft + this.ItemModifierLabel.MarginRight);
				}
				else
				{
					this.ValueLabel.WidthSizePolicy = SizePolicy.Fixed;
					this.ValueLabel.ScaledSuggestedWidth = MathF.Max(53f * base._scaleToUse, maxValueLabelSizeX);
				}
			}
			if (this.IsTwoColumn && !this._isMultiLine && this._isTitle)
			{
				this.DefinitionLabel.WidthSizePolicy = SizePolicy.Fixed;
				this.DefinitionLabel.ScaledSuggestedWidth = MathF.Max(53f * base._scaleToUse, maxDefinitionLabelSizeX);
				this.DefinitionLabelContainer.WidthSizePolicy = SizePolicy.Fixed;
				this.DefinitionLabelContainer.ScaledSuggestedWidth = MathF.Max(53f * base._scaleToUse, maxDefinitionLabelSizeX);
			}
			Widget parentWidget = base.ParentWidget;
			this.SetGlobalAlphaRecursively((parentWidget != null) ? parentWidget.AlphaFactor : 0f);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0002FF87 File Offset: 0x0002E187
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._firstFrame)
			{
				this.RefreshText();
				this._firstFrame = false;
			}
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0002FFA8 File Offset: 0x0002E1A8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.ValueBackgroundSpriteWidget.HeightSizePolicy = SizePolicy.CoverChildren;
			if (this._currentSprite != null)
			{
				if (this.DefinitionLabelContainer.Size.X + this.ValueLabelContainer.Size.X > base.ParentWidget.Size.X)
				{
					this.ValueBackgroundSpriteWidget.WidthSizePolicy = SizePolicy.Fixed;
					this.ValueBackgroundSpriteWidget.ScaledSuggestedWidth = this.DefinitionLabelContainer.Size.X + this.ValueLabelContainer.Size.X;
					base.MarginLeft = 0f;
					base.MarginRight = 0f;
				}
				else
				{
					this.ValueBackgroundSpriteWidget.WidthSizePolicy = SizePolicy.Fixed;
					this.ValueBackgroundSpriteWidget.ScaledSuggestedWidth = base.ParentWidget.Size.X - (base.MarginLeft + base.MarginRight) * base._scaleToUse;
				}
				this.ValueBackgroundSpriteWidget.MinHeight = (float)this._currentSprite.Height;
				if (this._isTitle)
				{
					base.PositionXOffset = -base.MarginLeft;
					this.ValueLabelContainer.PositionYOffset = 0f;
					if (!this.IsTwoColumn)
					{
						this.ValueLabelContainer.MarginLeft = base.MarginLeft;
						this.ValueBackgroundSpriteWidget.ScaledSuggestedHeight = this.ValueLabel.Size.Y;
						return;
					}
					this.DefinitionLabelContainer.MarginLeft = base.MarginLeft;
					this.DefinitionLabel.HorizontalAlignment = HorizontalAlignment.Left;
					return;
				}
			}
			else
			{
				this.ValueBackgroundSpriteWidget.SuggestedWidth = 0f;
			}
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00030134 File Offset: 0x0002E334
		private void RefreshText()
		{
			this.DefinitionLabel.Text = this._definitionText;
			this.ValueLabel.Text = this._valueText;
			this.DetermineTypeOfTooltipProperty();
			this.ValueLabelContainer.IsVisible = true;
			this.DefinitionLabelContainer.IsVisible = true;
			this.DefinitionLabel.IsVisible = true;
			this.ValueLabel.IsVisible = true;
			this._currentSprite = null;
			if (this._allBrushesInitialized)
			{
				if (this._isRelation)
				{
					this.DefinitionLabel.Text = "";
					this.ValueLabel.Text = "";
				}
				else if (this._isBattleMode)
				{
					this.DefinitionLabel.Text = "";
					this.ValueLabel.Text = "";
				}
				else if (this._isBattleModeOver)
				{
					this.DefinitionLabel.Text = "";
					this.ValueLabel.Text = "";
				}
				else if (this._isMultiLine)
				{
					this.DefinitionLabelContainer.IsVisible = false;
					this.ValueLabel.Text = this._valueText;
					this.ValueLabel.Brush = ((this.TextHeight < 0) ? this.SubtextBrush : this.DescriptionTextBrush);
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.Fixed;
					this.ValueLabelContainer.SuggestedWidth = 0f;
				}
				else if (this._isCost)
				{
					this.DefinitionLabel.Text = "";
					this.ValueLabel.Text = this._valueText;
					base.HorizontalAlignment = HorizontalAlignment.Center;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
				}
				else if (this._isRundownSeperator)
				{
					this.ValueLabel.IsVisible = false;
					this.DefinitionLabelContainer.IsVisible = false;
					this.ValueBackgroundSpriteWidget.IsVisible = true;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					this._currentSprite = this.RundownSeperatorSprite;
					this.ValueBackgroundSpriteWidget.HorizontalAlignment = HorizontalAlignment.Right;
					this.ValueBackgroundSpriteWidget.PositionXOffset = base.Right * base._inverseScaleToUse;
					this.ValueBackgroundSpriteWidget.Sprite = this._currentSprite;
					this.ValueBackgroundSpriteWidget.HeightSizePolicy = SizePolicy.Fixed;
					this.ValueBackgroundSpriteWidget.WidthSizePolicy = SizePolicy.Fixed;
				}
				else if (this._isDefaultSeperator)
				{
					this.ValueLabel.IsVisible = false;
					this.DefinitionLabelContainer.IsVisible = false;
					this.ValueBackgroundSpriteWidget.IsVisible = true;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					this._currentSprite = this.DefaultSeperatorSprite;
					this.ValueBackgroundSpriteWidget.HorizontalAlignment = HorizontalAlignment.Right;
					this.ValueBackgroundSpriteWidget.PositionXOffset = base.Right * base._inverseScaleToUse;
					this.ValueBackgroundSpriteWidget.Sprite = this._currentSprite;
					this.ValueBackgroundSpriteWidget.AlphaFactor = 0.5f;
					this.ValueBackgroundSpriteWidget.HeightSizePolicy = SizePolicy.Fixed;
					this.ValueBackgroundSpriteWidget.WidthSizePolicy = SizePolicy.Fixed;
				}
				else if (this._isTitle)
				{
					this.DefinitionLabel.Brush = this.TitleTextBrush;
					this.ValueLabel.Brush = this.TitleTextBrush;
					this.DefinitionLabel.HeightSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabel.HeightSizePolicy = SizePolicy.CoverChildren;
					this.DefinitionLabelContainer.HeightSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabelContainer.HeightSizePolicy = SizePolicy.CoverChildren;
					if (this.IsTwoColumn)
					{
						this.DefinitionLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
						this.DefinitionLabelContainer.HorizontalAlignment = HorizontalAlignment.Left;
						this.DefinitionLabel.WidthSizePolicy = SizePolicy.CoverChildren;
						this.DefinitionLabel.HorizontalAlignment = HorizontalAlignment.Left;
						this.DefinitionLabel.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Left;
						this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
						this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
						this.ValueLabel.MarginLeft = base.MarginLeft;
					}
					else
					{
						this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
						this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					}
					this._currentSprite = this.TitleBackgroundSprite;
					this.ValueBackgroundSpriteWidget.HeightSizePolicy = SizePolicy.CoverChildren;
					this.ValueBackgroundSpriteWidget.Sprite = this._currentSprite;
					this.ValueBackgroundSpriteWidget.IsVisible = true;
				}
				else if (this.IsTwoColumn)
				{
					this.DefinitionLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					this.DefinitionLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.DefinitionLabelContainer.HorizontalAlignment = HorizontalAlignment.Right;
					this.DefinitionLabel.HorizontalAlignment = HorizontalAlignment.Right;
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					base.HorizontalAlignment = HorizontalAlignment.Right;
					this.ValueLabel.MarginLeft = base.MarginLeft;
					this.DefinitionLabel.Brush = this.ValueNameTextBrush;
					this.ValueLabel.Brush = this.ValueTextBrush;
				}
				else if (this._isSubtext)
				{
					this.DefinitionLabelContainer.IsVisible = false;
					this.ValueLabel.Brush = this.SubtextBrush;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
				}
				else if (this._isEmptySpace)
				{
					this.DefinitionLabel.IsVisible = false;
					this.ValueLabel.Text = " ";
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					if (this.TextHeight > 0)
					{
						this.ValueLabel.Brush.FontSize = 30;
					}
					else if (this.TextHeight < 0)
					{
						this.ValueLabel.Brush.FontSize = 10;
					}
					else
					{
						this.ValueLabel.Brush.FontSize = 15;
					}
				}
				else if (this.DefinitionLabel.Text == string.Empty && this.ValueLabel.Text != string.Empty)
				{
					this.DefinitionLabelContainer.IsVisible = false;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabel.Brush = this.DescriptionTextBrush;
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
				}
				else
				{
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.DefinitionLabel.WidthSizePolicy = SizePolicy.CoverChildren;
				}
				if (this._useCustomColor)
				{
					this.ValueLabel.Brush.FontColor = this.TextColor;
					this.ValueLabel.Brush.TextAlphaFactor = this.TextColor.Alpha;
				}
				if (this._isRundownResult)
				{
					this.ValueLabel.Brush.FontSize = (int)((float)this.ValueLabel.ReadOnlyBrush.FontSize * 1.3f);
					this.DefinitionLabel.Brush.FontSize = (int)((float)this.DefinitionLabel.ReadOnlyBrush.FontSize * 1.3f);
				}
			}
			Widget parentWidget = base.ParentWidget;
			this.SetGlobalAlphaRecursively((parentWidget != null) ? parentWidget.AlphaFactor : 0f);
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00030804 File Offset: 0x0002EA04
		private void DetermineTypeOfTooltipProperty()
		{
			this.PropertyModifierAsFlag = (TooltipPropertyWidget.TooltipPropertyFlags)this.PropertyModifier;
			this._isMultiLine = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.MultiLine) == TooltipPropertyWidget.TooltipPropertyFlags.MultiLine;
			this._isBattleMode = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.BattleMode) == TooltipPropertyWidget.TooltipPropertyFlags.BattleMode;
			this._isBattleModeOver = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.BattleModeOver) == TooltipPropertyWidget.TooltipPropertyFlags.BattleModeOver;
			this._isCost = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.Cost) == TooltipPropertyWidget.TooltipPropertyFlags.Cost;
			this._isTitle = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.Title) == TooltipPropertyWidget.TooltipPropertyFlags.Title;
			this._isRelation = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarFirstEnemy) == TooltipPropertyWidget.TooltipPropertyFlags.WarFirstEnemy || (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarFirstAlly) == TooltipPropertyWidget.TooltipPropertyFlags.WarFirstAlly || (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarFirstNeutral) == TooltipPropertyWidget.TooltipPropertyFlags.WarFirstNeutral || (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarSecondEnemy) == TooltipPropertyWidget.TooltipPropertyFlags.WarSecondEnemy || (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarSecondAlly) == TooltipPropertyWidget.TooltipPropertyFlags.WarSecondAlly || (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarSecondNeutral) == TooltipPropertyWidget.TooltipPropertyFlags.WarSecondNeutral;
			this._isRundownSeperator = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.RundownSeperator) == TooltipPropertyWidget.TooltipPropertyFlags.RundownSeperator;
			this._isDefaultSeperator = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.DefaultSeperator) == TooltipPropertyWidget.TooltipPropertyFlags.DefaultSeperator;
			this._isRundownResult = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.RundownResult) == TooltipPropertyWidget.TooltipPropertyFlags.RundownResult;
			this.IsTwoColumn = false;
			this._isSubtext = false;
			this._isEmptySpace = false;
			if (!this._isMultiLine && !this._isBattleMode && !this._isBattleModeOver && !this._isCost && !this._isRundownSeperator && !this._isDefaultSeperator)
			{
				this._isEmptySpace = this.DefinitionText == string.Empty && this.ValueText == string.Empty;
				this.IsTwoColumn = this.DefinitionText != string.Empty && this.ValueText != string.Empty && this.TextHeight == 0;
				this._isSubtext = this.DefinitionText == string.Empty && this.ValueText != string.Empty && this.TextHeight < 0;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001168 RID: 4456 RVA: 0x00030A1C File Offset: 0x0002EC1C
		// (set) Token: 0x06001169 RID: 4457 RVA: 0x00030A24 File Offset: 0x0002EC24
		[Editor(false)]
		public string RundownSeperatorSpriteName
		{
			get
			{
				return this._rundownSeperatorSpriteName;
			}
			set
			{
				if (this._rundownSeperatorSpriteName != value)
				{
					this._rundownSeperatorSpriteName = value;
					base.OnPropertyChanged<string>(value, "RundownSeperatorSpriteName");
					this.RundownSeperatorSprite = base.Context.SpriteData.GetSprite(value);
				}
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x0600116A RID: 4458 RVA: 0x00030A5E File Offset: 0x0002EC5E
		// (set) Token: 0x0600116B RID: 4459 RVA: 0x00030A66 File Offset: 0x0002EC66
		[Editor(false)]
		public string DefaultSeperatorSpriteName
		{
			get
			{
				return this._defaultSeperatorSpriteName;
			}
			set
			{
				if (this._defaultSeperatorSpriteName != value)
				{
					this._defaultSeperatorSpriteName = value;
					base.OnPropertyChanged<string>(value, "DefaultSeperatorSpriteName");
					this.DefaultSeperatorSprite = base.Context.SpriteData.GetSprite(value);
				}
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x00030AA0 File Offset: 0x0002ECA0
		// (set) Token: 0x0600116D RID: 4461 RVA: 0x00030AA8 File Offset: 0x0002ECA8
		[Editor(false)]
		public string TitleBackgroundSpriteName
		{
			get
			{
				return this._titleBackgroundSpriteName;
			}
			set
			{
				if (this._titleBackgroundSpriteName != value)
				{
					this._titleBackgroundSpriteName = value;
					base.OnPropertyChanged<string>(value, "TitleBackgroundSpriteName");
					this.TitleBackgroundSprite = base.Context.SpriteData.GetSprite(value);
				}
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x00030AE2 File Offset: 0x0002ECE2
		// (set) Token: 0x0600116F RID: 4463 RVA: 0x00030AEA File Offset: 0x0002ECEA
		[Editor(false)]
		public Brush ValueNameTextBrush
		{
			get
			{
				return this._valueNameTextBrush;
			}
			set
			{
				if (this._valueNameTextBrush != value)
				{
					this._valueNameTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "ValueNameTextBrush");
				}
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001170 RID: 4464 RVA: 0x00030B08 File Offset: 0x0002ED08
		// (set) Token: 0x06001171 RID: 4465 RVA: 0x00030B10 File Offset: 0x0002ED10
		[Editor(false)]
		public Brush TitleTextBrush
		{
			get
			{
				return this._titleTextBrush;
			}
			set
			{
				if (this._titleTextBrush != value)
				{
					this._titleTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "TitleTextBrush");
				}
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001172 RID: 4466 RVA: 0x00030B2E File Offset: 0x0002ED2E
		// (set) Token: 0x06001173 RID: 4467 RVA: 0x00030B36 File Offset: 0x0002ED36
		[Editor(false)]
		public Brush SubtextBrush
		{
			get
			{
				return this._subtextBrush;
			}
			set
			{
				if (this._subtextBrush != value)
				{
					this._subtextBrush = value;
					base.OnPropertyChanged<Brush>(value, "SubtextBrush");
				}
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x00030B54 File Offset: 0x0002ED54
		// (set) Token: 0x06001175 RID: 4469 RVA: 0x00030B5C File Offset: 0x0002ED5C
		[Editor(false)]
		public Brush ValueTextBrush
		{
			get
			{
				return this._valueTextBrush;
			}
			set
			{
				if (this._valueTextBrush != value)
				{
					this._valueTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "ValueTextBrush");
				}
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x00030B7A File Offset: 0x0002ED7A
		// (set) Token: 0x06001177 RID: 4471 RVA: 0x00030B82 File Offset: 0x0002ED82
		[Editor(false)]
		public Brush DescriptionTextBrush
		{
			get
			{
				return this._descriptionTextBrush;
			}
			set
			{
				if (this._descriptionTextBrush != value)
				{
					this._descriptionTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "DescriptionTextBrush");
				}
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x00030BA0 File Offset: 0x0002EDA0
		// (set) Token: 0x06001179 RID: 4473 RVA: 0x00030BA8 File Offset: 0x0002EDA8
		[Editor(false)]
		public bool ModifyDefinitionColor
		{
			get
			{
				return this._modifyDefinitionColor;
			}
			set
			{
				if (this._modifyDefinitionColor != value)
				{
					this._modifyDefinitionColor = value;
					base.OnPropertyChanged(value, "ModifyDefinitionColor");
				}
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x0600117A RID: 4474 RVA: 0x00030BC6 File Offset: 0x0002EDC6
		// (set) Token: 0x0600117B RID: 4475 RVA: 0x00030BCE File Offset: 0x0002EDCE
		[Editor(false)]
		public RichTextWidget DefinitionLabel
		{
			get
			{
				return this._definitionLabel;
			}
			set
			{
				if (this._definitionLabel != value)
				{
					this._definitionLabel = value;
					base.OnPropertyChanged<RichTextWidget>(value, "DefinitionLabel");
				}
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x00030BEC File Offset: 0x0002EDEC
		// (set) Token: 0x0600117D RID: 4477 RVA: 0x00030BF4 File Offset: 0x0002EDF4
		[Editor(false)]
		public RichTextWidget ValueLabel
		{
			get
			{
				return this._valueLabel;
			}
			set
			{
				if (this._valueLabel != value)
				{
					this._valueLabel = value;
					base.OnPropertyChanged<RichTextWidget>(value, "ValueLabel");
				}
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x00030C12 File Offset: 0x0002EE12
		// (set) Token: 0x0600117F RID: 4479 RVA: 0x00030C1A File Offset: 0x0002EE1A
		[Editor(false)]
		public TextWidget ItemModifierLabel
		{
			get
			{
				return this._itemModifierLabel;
			}
			set
			{
				if (this._itemModifierLabel != value)
				{
					this._itemModifierLabel = value;
					base.OnPropertyChanged<TextWidget>(value, "ItemModifierLabel");
				}
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x00030C38 File Offset: 0x0002EE38
		// (set) Token: 0x06001181 RID: 4481 RVA: 0x00030C40 File Offset: 0x0002EE40
		[Editor(false)]
		public ListPanel ValueBackgroundSpriteWidget
		{
			get
			{
				return this._valueBackgroundSpriteWidget;
			}
			set
			{
				if (this._valueBackgroundSpriteWidget != value)
				{
					this._valueBackgroundSpriteWidget = value;
					base.OnPropertyChanged<ListPanel>(value, "ValueBackgroundSpriteWidget");
				}
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x00030C5E File Offset: 0x0002EE5E
		// (set) Token: 0x06001183 RID: 4483 RVA: 0x00030C66 File Offset: 0x0002EE66
		[Editor(false)]
		public Widget DefinitionLabelContainer
		{
			get
			{
				return this._definitionLabelContainer;
			}
			set
			{
				if (this._definitionLabelContainer != value)
				{
					this._definitionLabelContainer = value;
					base.OnPropertyChanged<Widget>(value, "DefinitionLabelContainer");
				}
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x00030C84 File Offset: 0x0002EE84
		// (set) Token: 0x06001185 RID: 4485 RVA: 0x00030C8C File Offset: 0x0002EE8C
		[Editor(false)]
		public Widget ValueLabelContainer
		{
			get
			{
				return this._valueLabelContainer;
			}
			set
			{
				if (this._valueLabelContainer != value)
				{
					this._valueLabelContainer = value;
					base.OnPropertyChanged<Widget>(value, "ValueLabelContainer");
				}
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x00030CAA File Offset: 0x0002EEAA
		// (set) Token: 0x06001187 RID: 4487 RVA: 0x00030CB2 File Offset: 0x0002EEB2
		[Editor(false)]
		public Color TextColor
		{
			get
			{
				return this._textColor;
			}
			set
			{
				if (this._textColor != value)
				{
					this._textColor = value;
					base.OnPropertyChanged(value, "TextColor");
					this._useCustomColor = true;
				}
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001188 RID: 4488 RVA: 0x00030CDC File Offset: 0x0002EEDC
		// (set) Token: 0x06001189 RID: 4489 RVA: 0x00030CE4 File Offset: 0x0002EEE4
		[Editor(false)]
		public int TextHeight
		{
			get
			{
				return this._textHeight;
			}
			set
			{
				if (this._textHeight != value)
				{
					this._textHeight = value;
					base.OnPropertyChanged(value, "TextHeight");
				}
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x0600118A RID: 4490 RVA: 0x00030D02 File Offset: 0x0002EF02
		// (set) Token: 0x0600118B RID: 4491 RVA: 0x00030D0A File Offset: 0x0002EF0A
		[Editor(false)]
		public string DefinitionText
		{
			get
			{
				return this._definitionText;
			}
			set
			{
				if (this._definitionText != value)
				{
					this._definitionText = value;
					base.OnPropertyChanged<string>(value, "DefinitionText");
					this._firstFrame = true;
				}
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x0600118C RID: 4492 RVA: 0x00030D34 File Offset: 0x0002EF34
		// (set) Token: 0x0600118D RID: 4493 RVA: 0x00030D3C File Offset: 0x0002EF3C
		[Editor(false)]
		public string ValueText
		{
			get
			{
				return this._valueText;
			}
			set
			{
				if (this._valueText != value)
				{
					this._valueText = value;
					base.OnPropertyChanged<string>(value, "ValueText");
					this._firstFrame = true;
				}
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x0600118E RID: 4494 RVA: 0x00030D66 File Offset: 0x0002EF66
		// (set) Token: 0x0600118F RID: 4495 RVA: 0x00030D6E File Offset: 0x0002EF6E
		[Editor(false)]
		public int PropertyModifier
		{
			get
			{
				return this._propertyModifier;
			}
			set
			{
				if (this._propertyModifier != value)
				{
					this._propertyModifier = value;
					base.OnPropertyChanged(value, "PropertyModifier");
				}
			}
		}

		// Token: 0x040007DD RID: 2013
		private const int HeaderSize = 30;

		// Token: 0x040007DE RID: 2014
		private const int DefaultSize = 15;

		// Token: 0x040007DF RID: 2015
		private const int SubTextSize = 10;

		// Token: 0x040007E1 RID: 2017
		private bool _isMultiLine;

		// Token: 0x040007E2 RID: 2018
		private bool _isBattleMode;

		// Token: 0x040007E3 RID: 2019
		private bool _isBattleModeOver;

		// Token: 0x040007E4 RID: 2020
		private bool _isCost;

		// Token: 0x040007E5 RID: 2021
		private bool _isRundownSeperator;

		// Token: 0x040007E6 RID: 2022
		private bool _isDefaultSeperator;

		// Token: 0x040007E7 RID: 2023
		private bool _isRundownResult;

		// Token: 0x040007E8 RID: 2024
		private bool _isTitle;

		// Token: 0x040007E9 RID: 2025
		private bool _isSubtext;

		// Token: 0x040007EA RID: 2026
		private bool _isEmptySpace;

		// Token: 0x040007EB RID: 2027
		private bool _isRelation;

		// Token: 0x040007ED RID: 2029
		private bool _useCustomColor;

		// Token: 0x040007EE RID: 2030
		private Sprite RundownSeperatorSprite;

		// Token: 0x040007EF RID: 2031
		private Sprite DefaultSeperatorSprite;

		// Token: 0x040007F0 RID: 2032
		private Sprite TitleBackgroundSprite;

		// Token: 0x040007F1 RID: 2033
		private Sprite _currentSprite;

		// Token: 0x040007F2 RID: 2034
		private bool _firstFrame = true;

		// Token: 0x040007F3 RID: 2035
		private bool _modifyDefinitionColor = true;

		// Token: 0x040007F4 RID: 2036
		private Color _textColor;

		// Token: 0x040007F5 RID: 2037
		private RichTextWidget _definitionLabel;

		// Token: 0x040007F6 RID: 2038
		private RichTextWidget _valueLabel;

		// Token: 0x040007F7 RID: 2039
		private TextWidget _itemModifierLabel;

		// Token: 0x040007F8 RID: 2040
		private Widget _definitionLabelContainer;

		// Token: 0x040007F9 RID: 2041
		private Widget _valueLabelContainer;

		// Token: 0x040007FA RID: 2042
		private ListPanel _valueBackgroundSpriteWidget;

		// Token: 0x040007FB RID: 2043
		private int _textHeight;

		// Token: 0x040007FC RID: 2044
		private Brush _titleTextBrush;

		// Token: 0x040007FD RID: 2045
		private Brush _subtextBrush;

		// Token: 0x040007FE RID: 2046
		private Brush _valueTextBrush;

		// Token: 0x040007FF RID: 2047
		private Brush _descriptionTextBrush;

		// Token: 0x04000800 RID: 2048
		private Brush _valueNameTextBrush;

		// Token: 0x04000801 RID: 2049
		private string _rundownSeperatorSpriteName;

		// Token: 0x04000802 RID: 2050
		private string _defaultSeperatorSpriteName;

		// Token: 0x04000803 RID: 2051
		private string _titleBackgroundSpriteName;

		// Token: 0x04000804 RID: 2052
		private string _definitionText;

		// Token: 0x04000805 RID: 2053
		private string _valueText;

		// Token: 0x04000806 RID: 2054
		private int _propertyModifier;

		// Token: 0x020001D2 RID: 466
		[Flags]
		public enum TooltipPropertyFlags
		{
			// Token: 0x04000A41 RID: 2625
			None = 0,
			// Token: 0x04000A42 RID: 2626
			MultiLine = 1,
			// Token: 0x04000A43 RID: 2627
			BattleMode = 2,
			// Token: 0x04000A44 RID: 2628
			BattleModeOver = 4,
			// Token: 0x04000A45 RID: 2629
			WarFirstEnemy = 8,
			// Token: 0x04000A46 RID: 2630
			WarFirstAlly = 16,
			// Token: 0x04000A47 RID: 2631
			WarFirstNeutral = 32,
			// Token: 0x04000A48 RID: 2632
			WarSecondEnemy = 64,
			// Token: 0x04000A49 RID: 2633
			WarSecondAlly = 128,
			// Token: 0x04000A4A RID: 2634
			WarSecondNeutral = 256,
			// Token: 0x04000A4B RID: 2635
			RundownSeperator = 512,
			// Token: 0x04000A4C RID: 2636
			DefaultSeperator = 1024,
			// Token: 0x04000A4D RID: 2637
			Cost = 2048,
			// Token: 0x04000A4E RID: 2638
			Title = 4096,
			// Token: 0x04000A4F RID: 2639
			RundownResult = 8192
		}
	}
}
