using System;
using System.Numerics;
using TaleWorlds.GauntletUI.Layout;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200005A RID: 90
	public class GridWidget : Container
	{
		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0001AAB7 File Offset: 0x00018CB7
		// (set) Token: 0x06000623 RID: 1571 RVA: 0x0001AABF File Offset: 0x00018CBF
		public GridLayout GridLayout { get; private set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x0001AAC8 File Offset: 0x00018CC8
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x0001AAD0 File Offset: 0x00018CD0
		[Editor(false)]
		public float DefaultCellWidth
		{
			get
			{
				return this._defaultCellWidth;
			}
			set
			{
				if (this._defaultCellWidth != value)
				{
					this._defaultCellWidth = value;
					base.OnPropertyChanged(value, "DefaultCellWidth");
				}
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x0001AAEE File Offset: 0x00018CEE
		public float DefaultScaledCellWidth
		{
			get
			{
				return this.DefaultCellWidth * base._scaleToUse;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0001AAFD File Offset: 0x00018CFD
		// (set) Token: 0x06000628 RID: 1576 RVA: 0x0001AB05 File Offset: 0x00018D05
		[Editor(false)]
		public float DefaultCellHeight
		{
			get
			{
				return this._defaultCellHeight;
			}
			set
			{
				if (this._defaultCellHeight != value)
				{
					this._defaultCellHeight = value;
					base.OnPropertyChanged(value, "DefaultCellHeight");
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x0001AB23 File Offset: 0x00018D23
		public float DefaultScaledCellHeight
		{
			get
			{
				return this.DefaultCellHeight * base._scaleToUse;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x0001AB32 File Offset: 0x00018D32
		// (set) Token: 0x0600062B RID: 1579 RVA: 0x0001AB3A File Offset: 0x00018D3A
		[Editor(false)]
		public int RowCount
		{
			get
			{
				return this._rowCount;
			}
			set
			{
				if (this._rowCount != value)
				{
					this._rowCount = value;
					base.OnPropertyChanged(value, "RowCount");
				}
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x0001AB58 File Offset: 0x00018D58
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x0001AB60 File Offset: 0x00018D60
		[Editor(false)]
		public int ColumnCount
		{
			get
			{
				return this._columnCount;
			}
			set
			{
				if (this._columnCount != value)
				{
					this._columnCount = value;
					base.OnPropertyChanged(value, "ColumnCount");
				}
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x0001AB7E File Offset: 0x00018D7E
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x0001AB86 File Offset: 0x00018D86
		[Editor(false)]
		public bool UseDynamicCellWidth
		{
			get
			{
				return this._useDynamicCellWidth;
			}
			set
			{
				if (this._useDynamicCellWidth != value)
				{
					this._useDynamicCellWidth = value;
					base.OnPropertyChanged(value, "UseDynamicCellWidth");
				}
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0001ABA4 File Offset: 0x00018DA4
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x0001ABAC File Offset: 0x00018DAC
		[Editor(false)]
		public bool UseDynamicCellHeight
		{
			get
			{
				return this._useDynamicCellHeight;
			}
			set
			{
				if (this._useDynamicCellHeight != value)
				{
					this._useDynamicCellHeight = value;
					base.OnPropertyChanged(value, "UseDynamicCellHeight");
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x0001ABCA File Offset: 0x00018DCA
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x0001ABD2 File Offset: 0x00018DD2
		public override Predicate<Widget> AcceptDropPredicate { get; set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0001ABDB File Offset: 0x00018DDB
		public override bool IsDragHovering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001ABDE File Offset: 0x00018DDE
		public GridWidget(UIContext context)
			: base(context)
		{
			this.GridLayout = new GridLayout();
			base.LayoutImp = this.GridLayout;
			this.RowCount = -1;
			this.ColumnCount = -1;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001AC0C File Offset: 0x00018E0C
		public override Vector2 GetDropGizmoPosition(Vector2 draggedWidgetPosition)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001AC13 File Offset: 0x00018E13
		public override int GetIndexForDrop(Vector2 draggedWidgetPosition)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001AC1C File Offset: 0x00018E1C
		public override void OnChildSelected(Widget widget)
		{
			int num = -1;
			for (int i = 0; i < base.ChildCount; i++)
			{
				if (widget == base.GetChild(i))
				{
					num = i;
				}
			}
			base.IntValue = num;
		}

		// Token: 0x040002EB RID: 747
		private float _defaultCellWidth;

		// Token: 0x040002EC RID: 748
		private float _defaultCellHeight;

		// Token: 0x040002ED RID: 749
		private int _rowCount;

		// Token: 0x040002EE RID: 750
		private int _columnCount;

		// Token: 0x040002EF RID: 751
		private bool _useDynamicCellWidth;

		// Token: 0x040002F0 RID: 752
		private bool _useDynamicCellHeight;

		// Token: 0x040002F1 RID: 753
		public const int DefaultRowCount = 3;

		// Token: 0x040002F2 RID: 754
		public const int DefaultColumnCount = 3;
	}
}
