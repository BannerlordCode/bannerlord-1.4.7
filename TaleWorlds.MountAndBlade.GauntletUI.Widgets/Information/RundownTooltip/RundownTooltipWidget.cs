using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.GauntletUI.Layout;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information.RundownTooltip
{
	// Token: 0x0200014B RID: 331
	public class RundownTooltipWidget : TooltipWidget
	{
		// Token: 0x060011A3 RID: 4515 RVA: 0x00030FB4 File Offset: 0x0002F1B4
		public RundownTooltipWidget(UIContext context)
			: base(context)
		{
			this.RefreshOnNextLateUpdate();
			this._animationDelayInFrames = 2;
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x00031040 File Offset: 0x0002F240
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.LineContainerWidget != null)
			{
				GridLayout gridLayout = this.LineContainerWidget.GridLayout;
				bool flag = this._lastCheckedColumnWidths.Count != gridLayout.ColumnWidths.Count;
				bool flag2 = false;
				for (int i = 0; i < this._lastCheckedColumnWidths.Count; i++)
				{
					float num = this._lastCheckedColumnWidths[i];
					float num2 = ((i < gridLayout.ColumnWidths.Count) ? gridLayout.ColumnWidths[i] : (-1f));
					if (MathF.Abs(num - num2) > 1E-05f)
					{
						flag2 = true;
						break;
					}
				}
				if (flag || flag2)
				{
					this._lastCheckedColumnWidths = gridLayout.ColumnWidths;
					RundownColumnDividerCollectionWidget dividerCollectionWidget = this.DividerCollectionWidget;
					if (dividerCollectionWidget == null)
					{
						return;
					}
					dividerCollectionWidget.Refresh(gridLayout.ColumnWidths);
				}
			}
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x00031108 File Offset: 0x0002F308
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			GridLayout gridLayout = this.LineContainerWidget.GridLayout;
			for (int i = 0; i < this.LineContainerWidget.ChildCount; i++)
			{
				RundownLineWidget rundownLineWidget = this.LineContainerWidget.GetChild(i) as RundownLineWidget;
				int num = i / this.LineContainerWidget.RowCount;
				rundownLineWidget.RefreshValueOffset((num < gridLayout.ColumnWidths.Count) ? gridLayout.ColumnWidths[num] : (-1f));
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x00031184 File Offset: 0x0002F384
		private void Refresh()
		{
			RundownTooltipWidget.ValueCategorization valueCategorizationAsInt = (RundownTooltipWidget.ValueCategorization)this.ValueCategorizationAsInt;
			if (this.LineContainerWidget != null)
			{
				List<RundownLineWidget> list = new List<RundownLineWidget>();
				float num = 0f;
				float num2 = 0f;
				using (List<Widget>.Enumerator enumerator = this.LineContainerWidget.Children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RundownLineWidget rundownLineWidget;
						if ((rundownLineWidget = enumerator.Current as RundownLineWidget) != null)
						{
							list.Add(rundownLineWidget);
							float value = rundownLineWidget.Value;
							if (value < num)
							{
								num = value;
							}
							if (value > num2)
							{
								num2 = value;
							}
						}
					}
				}
				foreach (RundownLineWidget rundownLineWidget2 in list)
				{
					float value2 = rundownLineWidget2.Value;
					Brush brush = rundownLineWidget2.ValueTextWidget.Brush;
					Color color = this._defaultValueColor;
					if (valueCategorizationAsInt != RundownTooltipWidget.ValueCategorization.None)
					{
						float num3 = ((value2 < 0f) ? num : num2);
						float num4 = MathF.Abs(value2 / num3);
						float num5 = (float)((valueCategorizationAsInt == RundownTooltipWidget.ValueCategorization.LargeIsBetter) ? 1 : (-1)) * value2;
						color = Color.Lerp(this._defaultValueColor, (num5 < 0f) ? this._negativeValueColor : this._positiveValueColor, num4);
					}
					brush.FontColor = color;
				}
			}
			this._willRefreshThisFrame = false;
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x000312D8 File Offset: 0x0002F4D8
		private void RefreshOnNextLateUpdate()
		{
			if (!this._willRefreshThisFrame)
			{
				this._willRefreshThisFrame = true;
				base.EventManager.AddLateUpdateAction(this, delegate(float _)
				{
					this.Refresh();
				}, 1);
			}
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00031302 File Offset: 0x0002F502
		private void OnLineContainerEventFire(Widget widget, string eventName, object[] args)
		{
			if (eventName == "ItemAdd" || eventName == "ItemRemove")
			{
				this.RefreshOnNextLateUpdate();
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060011A9 RID: 4521 RVA: 0x00031324 File Offset: 0x0002F524
		// (set) Token: 0x060011AA RID: 4522 RVA: 0x0003132C File Offset: 0x0002F52C
		[Editor(false)]
		public GridWidget LineContainerWidget
		{
			get
			{
				return this._lineContainerWidget;
			}
			set
			{
				if (value != this._lineContainerWidget)
				{
					if (this._lineContainerWidget != null)
					{
						this._lineContainerWidget.EventFire -= this.OnLineContainerEventFire;
					}
					this._lineContainerWidget = value;
					base.OnPropertyChanged<GridWidget>(value, "LineContainerWidget");
					this.RefreshOnNextLateUpdate();
					if (this._lineContainerWidget != null)
					{
						this._lineContainerWidget.EventFire += this.OnLineContainerEventFire;
					}
				}
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060011AB RID: 4523 RVA: 0x00031399 File Offset: 0x0002F599
		// (set) Token: 0x060011AC RID: 4524 RVA: 0x000313A1 File Offset: 0x0002F5A1
		[Editor(false)]
		public RundownColumnDividerCollectionWidget DividerCollectionWidget
		{
			get
			{
				return this._dividerCollectionWidget;
			}
			set
			{
				if (value != this._dividerCollectionWidget)
				{
					this._dividerCollectionWidget = value;
					base.OnPropertyChanged<RundownColumnDividerCollectionWidget>(value, "DividerCollectionWidget");
					this.RefreshOnNextLateUpdate();
				}
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060011AD RID: 4525 RVA: 0x000313C5 File Offset: 0x0002F5C5
		// (set) Token: 0x060011AE RID: 4526 RVA: 0x000313CD File Offset: 0x0002F5CD
		[Editor(false)]
		public int ValueCategorizationAsInt
		{
			get
			{
				return this._valueCategorizationAsInt;
			}
			set
			{
				if (value != this._valueCategorizationAsInt)
				{
					this._valueCategorizationAsInt = value;
					base.OnPropertyChanged(value, "ValueCategorizationAsInt");
					this.RefreshOnNextLateUpdate();
				}
			}
		}

		// Token: 0x0400080D RID: 2061
		private readonly Color _defaultValueColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x0400080E RID: 2062
		private readonly Color _negativeValueColor = new Color(0.8352941f, 0.12941177f, 0.12941177f, 1f);

		// Token: 0x0400080F RID: 2063
		private readonly Color _positiveValueColor = new Color(0.38039216f, 0.7490196f, 0.33333334f, 1f);

		// Token: 0x04000810 RID: 2064
		private bool _willRefreshThisFrame;

		// Token: 0x04000811 RID: 2065
		private IReadOnlyList<float> _lastCheckedColumnWidths = new List<float>();

		// Token: 0x04000812 RID: 2066
		private GridWidget _lineContainerWidget;

		// Token: 0x04000813 RID: 2067
		private RundownColumnDividerCollectionWidget _dividerCollectionWidget;

		// Token: 0x04000814 RID: 2068
		private int _valueCategorizationAsInt;

		// Token: 0x020001D3 RID: 467
		private enum ValueCategorization
		{
			// Token: 0x04000A51 RID: 2641
			None,
			// Token: 0x04000A52 RID: 2642
			LargeIsBetter,
			// Token: 0x04000A53 RID: 2643
			SmallIsBetter
		}
	}
}
