using System;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000064 RID: 100
	public class ScrollablePanelFixedHeaderWidget : Widget
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x0001DC82 File Offset: 0x0001BE82
		// (set) Token: 0x060006CC RID: 1740 RVA: 0x0001DC8A File Offset: 0x0001BE8A
		public Widget FixedHeader { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x0001DC93 File Offset: 0x0001BE93
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x0001DC9B File Offset: 0x0001BE9B
		public float TopOffset { get; set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0001DCA4 File Offset: 0x0001BEA4
		// (set) Token: 0x060006D0 RID: 1744 RVA: 0x0001DCAC File Offset: 0x0001BEAC
		public float BottomOffset { get; set; } = float.MinValue;

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001DCB5 File Offset: 0x0001BEB5
		public ScrollablePanelFixedHeaderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001DCD0 File Offset: 0x0001BED0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isDirty)
			{
				base.EventFired("FixedHeaderPropertyChanged", Array.Empty<object>());
				this._isDirty = false;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0001DCF8 File Offset: 0x0001BEF8
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x0001DD00 File Offset: 0x0001BF00
		public float HeaderHeight
		{
			get
			{
				return this._headerHeight;
			}
			set
			{
				if (value != this._headerHeight)
				{
					this._headerHeight = value;
					base.SuggestedHeight = this._headerHeight;
					this._isDirty = true;
				}
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0001DD25 File Offset: 0x0001BF25
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x0001DD2D File Offset: 0x0001BF2D
		public float AdditionalTopOffset
		{
			get
			{
				return this._additionalTopOffset;
			}
			set
			{
				if (value != this._additionalTopOffset)
				{
					this._additionalTopOffset = value;
					this._isDirty = true;
				}
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0001DD46 File Offset: 0x0001BF46
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x0001DD4E File Offset: 0x0001BF4E
		public float AdditionalBottomOffset
		{
			get
			{
				return this._additionalBottomOffset;
			}
			set
			{
				if (value != this._additionalBottomOffset)
				{
					this._additionalBottomOffset = value;
					this._isDirty = true;
				}
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0001DD67 File Offset: 0x0001BF67
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x0001DD6F File Offset: 0x0001BF6F
		[Editor(false)]
		public bool IsRelevant
		{
			get
			{
				return this._isRelevant;
			}
			set
			{
				if (value != this._isRelevant)
				{
					this._isRelevant = value;
					base.IsVisible = value;
					this._isDirty = true;
					base.OnPropertyChanged(value, "IsRelevant");
				}
			}
		}

		// Token: 0x04000330 RID: 816
		private bool _isDirty;

		// Token: 0x04000334 RID: 820
		private float _headerHeight;

		// Token: 0x04000335 RID: 821
		private float _additionalTopOffset;

		// Token: 0x04000336 RID: 822
		private float _additionalBottomOffset;

		// Token: 0x04000337 RID: 823
		private bool _isRelevant = true;
	}
}
