using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.KillFeed
{
	// Token: 0x020000BE RID: 190
	public class MultiplayerDuelKillFeedItemWidget : MultiplayerGeneralKillFeedItemWidget
	{
		// Token: 0x060009E3 RID: 2531 RVA: 0x0001BABC File Offset: 0x00019CBC
		public MultiplayerDuelKillFeedItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x0001BAC5 File Offset: 0x00019CC5
		// (set) Token: 0x060009E5 RID: 2533 RVA: 0x0001BAD0 File Offset: 0x00019CD0
		[Editor(false)]
		public bool IsEndOfDuel
		{
			get
			{
				return this._isEndOfDuel;
			}
			set
			{
				if (value != this._isEndOfDuel)
				{
					this._isEndOfDuel = value;
					base.OnPropertyChanged(value, "IsEndOfDuel");
					if (value)
					{
						BrushWidget background = this.Background;
						if (background != null)
						{
							background.SetState("EndOfDuel");
						}
						BrushWidget victimCompassBackground = this.VictimCompassBackground;
						if (victimCompassBackground != null)
						{
							victimCompassBackground.SetState("EndOfDuel");
						}
						BrushWidget murdererCompassBackground = this.MurdererCompassBackground;
						if (murdererCompassBackground != null)
						{
							murdererCompassBackground.SetState("EndOfDuel");
						}
						ScrollingRichTextWidget victimNameText = this.VictimNameText;
						if (victimNameText != null)
						{
							victimNameText.SetState("EndOfDuel");
						}
						ScrollingRichTextWidget murdererNameText = this.MurdererNameText;
						if (murdererNameText != null)
						{
							murdererNameText.SetState("EndOfDuel");
						}
						TextWidget victimScoreText = this.VictimScoreText;
						if (victimScoreText != null)
						{
							victimScoreText.SetState("EndOfDuel");
						}
						TextWidget murdererScoreText = this.MurdererScoreText;
						if (murdererScoreText == null)
						{
							return;
						}
						murdererScoreText.SetState("EndOfDuel");
					}
				}
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0001BB9B File Offset: 0x00019D9B
		// (set) Token: 0x060009E7 RID: 2535 RVA: 0x0001BBA3 File Offset: 0x00019DA3
		[Editor(false)]
		public BrushWidget Background
		{
			get
			{
				return this._background;
			}
			set
			{
				if (value != this._background)
				{
					this._background = value;
					base.OnPropertyChanged<BrushWidget>(value, "Background");
				}
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0001BBC1 File Offset: 0x00019DC1
		// (set) Token: 0x060009E9 RID: 2537 RVA: 0x0001BBC9 File Offset: 0x00019DC9
		[Editor(false)]
		public BrushWidget VictimCompassBackground
		{
			get
			{
				return this._victimCompassBackground;
			}
			set
			{
				if (value != this._victimCompassBackground)
				{
					this._victimCompassBackground = value;
					base.OnPropertyChanged<BrushWidget>(value, "VictimCompassBackground");
				}
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0001BBE7 File Offset: 0x00019DE7
		// (set) Token: 0x060009EB RID: 2539 RVA: 0x0001BBEF File Offset: 0x00019DEF
		[Editor(false)]
		public BrushWidget MurdererCompassBackground
		{
			get
			{
				return this._murdererCompassBackground;
			}
			set
			{
				if (value != this._murdererCompassBackground)
				{
					this._murdererCompassBackground = value;
					base.OnPropertyChanged<BrushWidget>(value, "MurdererCompassBackground");
				}
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x0001BC0D File Offset: 0x00019E0D
		// (set) Token: 0x060009ED RID: 2541 RVA: 0x0001BC15 File Offset: 0x00019E15
		[Editor(false)]
		public ScrollingRichTextWidget VictimNameText
		{
			get
			{
				return this._victimNameText;
			}
			set
			{
				if (value != this._victimNameText)
				{
					this._victimNameText = value;
					base.OnPropertyChanged<ScrollingRichTextWidget>(value, "VictimNameText");
				}
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x0001BC33 File Offset: 0x00019E33
		// (set) Token: 0x060009EF RID: 2543 RVA: 0x0001BC3B File Offset: 0x00019E3B
		[Editor(false)]
		public ScrollingRichTextWidget MurdererNameText
		{
			get
			{
				return this._murdererNameText;
			}
			set
			{
				if (value != this._murdererNameText)
				{
					this._murdererNameText = value;
					base.OnPropertyChanged<ScrollingRichTextWidget>(value, "MurdererNameText");
				}
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0001BC59 File Offset: 0x00019E59
		// (set) Token: 0x060009F1 RID: 2545 RVA: 0x0001BC61 File Offset: 0x00019E61
		[Editor(false)]
		public TextWidget VictimScoreText
		{
			get
			{
				return this._victimScoreText;
			}
			set
			{
				if (value != this._victimScoreText)
				{
					this._victimScoreText = value;
					base.OnPropertyChanged<TextWidget>(value, "VictimScoreText");
				}
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0001BC7F File Offset: 0x00019E7F
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x0001BC87 File Offset: 0x00019E87
		[Editor(false)]
		public TextWidget MurdererScoreText
		{
			get
			{
				return this._murdererScoreText;
			}
			set
			{
				if (value != this._murdererScoreText)
				{
					this._murdererScoreText = value;
					base.OnPropertyChanged<TextWidget>(value, "MurdererScoreText");
				}
			}
		}

		// Token: 0x04000477 RID: 1143
		private const string EndOfDuelState = "EndOfDuel";

		// Token: 0x04000478 RID: 1144
		private bool _isEndOfDuel;

		// Token: 0x04000479 RID: 1145
		private BrushWidget _background;

		// Token: 0x0400047A RID: 1146
		private BrushWidget _victimCompassBackground;

		// Token: 0x0400047B RID: 1147
		private BrushWidget _murdererCompassBackground;

		// Token: 0x0400047C RID: 1148
		private ScrollingRichTextWidget _victimNameText;

		// Token: 0x0400047D RID: 1149
		private ScrollingRichTextWidget _murdererNameText;

		// Token: 0x0400047E RID: 1150
		private TextWidget _victimScoreText;

		// Token: 0x0400047F RID: 1151
		private TextWidget _murdererScoreText;
	}
}
