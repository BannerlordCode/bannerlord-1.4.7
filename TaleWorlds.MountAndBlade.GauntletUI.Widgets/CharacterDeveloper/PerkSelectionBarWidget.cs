using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000183 RID: 387
	public class PerkSelectionBarWidget : Widget
	{
		// Token: 0x06001411 RID: 5137 RVA: 0x00036B83 File Offset: 0x00034D83
		public PerkSelectionBarWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x00036BA0 File Offset: 0x00034DA0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.PerksList != null)
			{
				for (int i = 0; i < this.PerksList.ChildCount; i++)
				{
					PerkItemButtonWidget perkItemButtonWidget = this.PerksList.GetChild(i) as PerkItemButtonWidget;
					if (this._perkWidgetWidth != perkItemButtonWidget.Size.X)
					{
						this._perkWidgetWidth = perkItemButtonWidget.Size.X;
					}
					perkItemButtonWidget.PositionXOffset = this.GetXPositionOfLevelOnBar((float)perkItemButtonWidget.Level) - this._perkWidgetWidth / 2f * base._inverseScaleToUse;
					if (perkItemButtonWidget.AlternativeType == 0)
					{
						perkItemButtonWidget.PositionYOffset = 45f;
					}
					else if (perkItemButtonWidget.AlternativeType == 1)
					{
						perkItemButtonWidget.PositionYOffset = 5f;
					}
					else if (perkItemButtonWidget.AlternativeType == 2)
					{
						perkItemButtonWidget.PositionYOffset = (float)((int)Mathf.Round(perkItemButtonWidget.Size.Y * base._inverseScaleToUse));
					}
				}
			}
			if (this.PercentageIndicatorWidget != null)
			{
				float xpositionOfLevelOnBar = this.GetXPositionOfLevelOnBar((float)this.Level);
				float num = xpositionOfLevelOnBar - this.PercentageIndicatorWidget.Size.X / 2f * base._inverseScaleToUse;
				this.PercentageIndicatorWidget.PositionXOffset = num;
				if (this.FullLearningRateClip != null)
				{
					float num2 = this.GetXPositionOfLevelOnBar((float)this.FullLearningRateLevel) - xpositionOfLevelOnBar;
					this.FullLearningRateClip.SuggestedWidth = ((num2 >= 0f) ? num2 : 0f);
					this.FullLearningRateClip.PositionXOffset = this.PercentageIndicatorWidget.PositionXOffset + this.PercentageIndicatorWidget.Size.X / 2f * base._inverseScaleToUse;
					this.FullLearningRateClipInnerContent.PositionXOffset = -this.FullLearningRateClip.PositionXOffset;
					if (this.LearningLimitIndicatorWidget != null)
					{
						this.LearningLimitIndicatorWidget.PositionXOffset = this.FullLearningRateClip.PositionXOffset + this.FullLearningRateClip.SuggestedWidth - this.LearningLimitIndicatorWidget.Size.X * base._inverseScaleToUse / 2f;
					}
				}
				this.ProgressClip.SuggestedWidth = num + this.PercentageIndicatorWidget.Size.X / 2f * base._inverseScaleToUse;
			}
			using (List<Widget>.Enumerator enumerator = this.SeperatorContainer.Children.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharacterDeveloperSkillVerticalSeperatorWidget characterDeveloperSkillVerticalSeperatorWidget;
					if ((characterDeveloperSkillVerticalSeperatorWidget = enumerator.Current as CharacterDeveloperSkillVerticalSeperatorWidget) != null)
					{
						characterDeveloperSkillVerticalSeperatorWidget.PositionXOffset = this.GetXPositionOfLevelOnBar((float)characterDeveloperSkillVerticalSeperatorWidget.SkillValue);
					}
				}
			}
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x00036E2C File Offset: 0x0003502C
		private float GetXPositionOfLevelOnBar(float level)
		{
			return Mathf.Clamp(level / ((float)this.MaxLevel + 25f) * base.Size.X * base._inverseScaleToUse, 0f, base.Size.X * base._inverseScaleToUse);
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x00036E6C File Offset: 0x0003506C
		// (set) Token: 0x06001415 RID: 5141 RVA: 0x00036E74 File Offset: 0x00035074
		public Widget ProgressClip
		{
			get
			{
				return this._progressClip;
			}
			set
			{
				if (this._progressClip != value)
				{
					this._progressClip = value;
					base.OnPropertyChanged<Widget>(value, "ProgressClip");
				}
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x00036E92 File Offset: 0x00035092
		// (set) Token: 0x06001417 RID: 5143 RVA: 0x00036E9A File Offset: 0x0003509A
		public Widget PercentageIndicatorWidget
		{
			get
			{
				return this._percentageIndicatorWidget;
			}
			set
			{
				if (this._percentageIndicatorWidget != value)
				{
					this._percentageIndicatorWidget = value;
					base.OnPropertyChanged<Widget>(value, "PercentageIndicatorWidget");
				}
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x00036EB8 File Offset: 0x000350B8
		// (set) Token: 0x06001419 RID: 5145 RVA: 0x00036EC0 File Offset: 0x000350C0
		public Widget FullLearningRateClip
		{
			get
			{
				return this._fullLearningRateClip;
			}
			set
			{
				if (this._fullLearningRateClip != value)
				{
					this._fullLearningRateClip = value;
					base.OnPropertyChanged<Widget>(value, "FullLearningRateClip");
				}
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x00036EDE File Offset: 0x000350DE
		// (set) Token: 0x0600141B RID: 5147 RVA: 0x00036EE6 File Offset: 0x000350E6
		public Widget SeperatorContainer
		{
			get
			{
				return this._seperatorContainer;
			}
			set
			{
				if (this._seperatorContainer != value)
				{
					this._seperatorContainer = value;
					base.OnPropertyChanged<Widget>(value, "SeperatorContainer");
				}
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x00036F04 File Offset: 0x00035104
		// (set) Token: 0x0600141D RID: 5149 RVA: 0x00036F0C File Offset: 0x0003510C
		public Widget LearningLimitIndicatorWidget
		{
			get
			{
				return this._learningLimitIndicatorWidget;
			}
			set
			{
				if (this._learningLimitIndicatorWidget != value)
				{
					this._learningLimitIndicatorWidget = value;
					base.OnPropertyChanged<Widget>(value, "LearningLimitIndicatorWidget");
				}
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x00036F2A File Offset: 0x0003512A
		// (set) Token: 0x0600141F RID: 5151 RVA: 0x00036F32 File Offset: 0x00035132
		public Widget FullLearningRateClipInnerContent
		{
			get
			{
				return this._fullLearningRateClipInnerContent;
			}
			set
			{
				if (this._fullLearningRateClipInnerContent != value)
				{
					this._fullLearningRateClipInnerContent = value;
					base.OnPropertyChanged<Widget>(value, "FullLearningRateClipInnerContent");
				}
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x00036F50 File Offset: 0x00035150
		// (set) Token: 0x06001421 RID: 5153 RVA: 0x00036F58 File Offset: 0x00035158
		public Widget PerksList
		{
			get
			{
				return this._perksList;
			}
			set
			{
				if (this._perksList != value)
				{
					this._perksList = value;
					base.OnPropertyChanged<Widget>(value, "PerksList");
				}
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x00036F76 File Offset: 0x00035176
		// (set) Token: 0x06001423 RID: 5155 RVA: 0x00036F7E File Offset: 0x0003517E
		public TextWidget PercentageIndicatorTextWidget
		{
			get
			{
				return this._percentageIndicatorTextWidget;
			}
			set
			{
				if (this._percentageIndicatorTextWidget != value)
				{
					this._percentageIndicatorTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "PercentageIndicatorTextWidget");
				}
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x00036F9C File Offset: 0x0003519C
		// (set) Token: 0x06001425 RID: 5157 RVA: 0x00036FA4 File Offset: 0x000351A4
		public int MaxLevel
		{
			get
			{
				return this._maxLevel;
			}
			set
			{
				if (this._maxLevel != value)
				{
					this._maxLevel = value;
					base.OnPropertyChanged(value, "MaxLevel");
				}
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x00036FC2 File Offset: 0x000351C2
		// (set) Token: 0x06001427 RID: 5159 RVA: 0x00036FCA File Offset: 0x000351CA
		public int FullLearningRateLevel
		{
			get
			{
				return this._fullLearningRateLevel;
			}
			set
			{
				if (this._fullLearningRateLevel != value)
				{
					this._fullLearningRateLevel = value;
					base.OnPropertyChanged(value, "FullLearningRateLevel");
				}
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x00036FE8 File Offset: 0x000351E8
		// (set) Token: 0x06001429 RID: 5161 RVA: 0x00036FF0 File Offset: 0x000351F0
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (this._level != value)
				{
					this._level = value;
					base.OnPropertyChanged(value, "Level");
				}
			}
		}

		// Token: 0x0400091A RID: 2330
		private float _perkWidgetWidth = -1f;

		// Token: 0x0400091B RID: 2331
		private Widget _perksList;

		// Token: 0x0400091C RID: 2332
		private Widget _progressClip;

		// Token: 0x0400091D RID: 2333
		private Widget _fullLearningRateClip;

		// Token: 0x0400091E RID: 2334
		private Widget _fullLearningRateClipInnerContent;

		// Token: 0x0400091F RID: 2335
		private Widget _percentageIndicatorWidget;

		// Token: 0x04000920 RID: 2336
		private Widget _seperatorContainer;

		// Token: 0x04000921 RID: 2337
		private Widget _learningLimitIndicatorWidget;

		// Token: 0x04000922 RID: 2338
		private TextWidget _percentageIndicatorTextWidget;

		// Token: 0x04000923 RID: 2339
		private int _maxLevel;

		// Token: 0x04000924 RID: 2340
		private int _fullLearningRateLevel;

		// Token: 0x04000925 RID: 2341
		private int _level = -1;
	}
}
