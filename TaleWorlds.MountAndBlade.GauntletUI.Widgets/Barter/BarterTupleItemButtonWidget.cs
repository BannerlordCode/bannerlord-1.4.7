using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Barter
{
	// Token: 0x02000191 RID: 401
	public class BarterTupleItemButtonWidget : ButtonWidget
	{
		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060014A8 RID: 5288 RVA: 0x00038580 File Offset: 0x00036780
		// (set) Token: 0x060014A9 RID: 5289 RVA: 0x00038588 File Offset: 0x00036788
		public ListPanel SliderParentList { get; set; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060014AA RID: 5290 RVA: 0x00038591 File Offset: 0x00036791
		// (set) Token: 0x060014AB RID: 5291 RVA: 0x00038599 File Offset: 0x00036799
		public TextWidget CountText { get; set; }

		// Token: 0x060014AC RID: 5292 RVA: 0x000385A2 File Offset: 0x000367A2
		public BarterTupleItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x000385AB File Offset: 0x000367AB
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.Refresh();
				this._initialized = true;
			}
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x000385CC File Offset: 0x000367CC
		private void Refresh()
		{
			this.SliderParentList.IsVisible = this.IsMultiple && this.IsOffered;
			this.CountText.IsHidden = this.IsMultiple && this.IsOffered;
			base.IsSelected = this.IsOffered;
			base.DoNotAcceptEvents = this.IsOffered;
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x00038629 File Offset: 0x00036829
		// (set) Token: 0x060014B0 RID: 5296 RVA: 0x00038631 File Offset: 0x00036831
		[Editor(false)]
		public bool IsMultiple
		{
			get
			{
				return this._isMultiple;
			}
			set
			{
				if (this._isMultiple != value)
				{
					this._isMultiple = value;
					base.OnPropertyChanged(value, "IsMultiple");
					this.Refresh();
				}
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x00038655 File Offset: 0x00036855
		// (set) Token: 0x060014B2 RID: 5298 RVA: 0x0003865D File Offset: 0x0003685D
		[Editor(false)]
		public bool IsOffered
		{
			get
			{
				return this._isOffered;
			}
			set
			{
				if (this._isOffered != value)
				{
					this._isOffered = value;
					base.OnPropertyChanged(value, "IsOffered");
					this.Refresh();
				}
			}
		}

		// Token: 0x04000960 RID: 2400
		private bool _initialized;

		// Token: 0x04000961 RID: 2401
		private bool _isMultiple;

		// Token: 0x04000962 RID: 2402
		private bool _isOffered;
	}
}
