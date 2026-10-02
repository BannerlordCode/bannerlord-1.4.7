using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000137 RID: 311
	public class InventoryAlternativeUsageContainer : Container
	{
		// Token: 0x06001035 RID: 4149 RVA: 0x0002C5C6 File Offset: 0x0002A7C6
		public InventoryAlternativeUsageContainer(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0002C5EC File Offset: 0x0002A7EC
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

		// Token: 0x06001037 RID: 4151 RVA: 0x0002C620 File Offset: 0x0002A820
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			foreach (Action<Widget, Widget> action in this.ItemAddEventHandlers)
			{
				action(this, child);
			}
			base.EventFired("ItemAdd", Array.Empty<object>());
			this.SetChildrenLayout();
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0002C690 File Offset: 0x0002A890
		protected override void OnBeforeChildRemoved(Widget child)
		{
			base.OnBeforeChildRemoved(child);
			foreach (Action<Widget, Widget> action in this.ItemRemoveEventHandlers)
			{
				action(this, child);
			}
			base.EventFired("ItemRemove", Array.Empty<object>());
			this.SetChildrenLayout();
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0002C700 File Offset: 0x0002A900
		private void SetChildrenLayout()
		{
			if (base.ChildCount == 0)
			{
				return;
			}
			int num = MathF.Ceiling((float)base.ChildCount / (float)this.ColumnLimit);
			for (int i = 0; i < num; i++)
			{
				int num2 = MathF.Min(this.ColumnLimit, base.ChildCount - (num - 1) * this.ColumnLimit);
				int num3 = i * (int)this.CellHeight;
				for (int j = 0; j < num2; j++)
				{
					int num4 = (int)(((float)j - ((float)num2 - 1f) / 2f) * this.CellWidth);
					int num5 = i * this.ColumnLimit + j;
					Widget child = base.GetChild(num5);
					if (num4 > 0)
					{
						child.MarginLeft = (float)(num4 * 2);
					}
					else if (num4 < 0)
					{
						child.MarginRight = (float)(-(float)num4 * 2);
					}
					child.MarginTop = (float)num3;
				}
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x0002C7D5 File Offset: 0x0002A9D5
		// (set) Token: 0x0600103B RID: 4155 RVA: 0x0002C7DD File Offset: 0x0002A9DD
		[Editor(false)]
		public int ColumnLimit
		{
			get
			{
				return this._columnLimit;
			}
			set
			{
				if (this._columnLimit != value)
				{
					this._columnLimit = value;
					base.OnPropertyChanged(value, "ColumnLimit");
				}
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x0600103C RID: 4156 RVA: 0x0002C7FB File Offset: 0x0002A9FB
		// (set) Token: 0x0600103D RID: 4157 RVA: 0x0002C803 File Offset: 0x0002AA03
		[Editor(false)]
		public float CellWidth
		{
			get
			{
				return this._cellWidth;
			}
			set
			{
				if (this._cellWidth != value)
				{
					this._cellWidth = value;
					base.OnPropertyChanged(value, "CellWidth");
				}
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x0002C821 File Offset: 0x0002AA21
		// (set) Token: 0x0600103F RID: 4159 RVA: 0x0002C829 File Offset: 0x0002AA29
		[Editor(false)]
		public float CellHeight
		{
			get
			{
				return this._cellHeight;
			}
			set
			{
				if (this._cellHeight != value)
				{
					this._cellHeight = value;
					base.OnPropertyChanged(value, "CellHeight");
				}
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x0002C847 File Offset: 0x0002AA47
		// (set) Token: 0x06001041 RID: 4161 RVA: 0x0002C84F File Offset: 0x0002AA4F
		public override Predicate<Widget> AcceptDropPredicate { get; set; }

		// Token: 0x06001042 RID: 4162 RVA: 0x0002C858 File Offset: 0x0002AA58
		public override Vector2 GetDropGizmoPosition(Vector2 draggedWidgetPosition)
		{
			return Vector2.Zero;
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0002C85F File Offset: 0x0002AA5F
		public override int GetIndexForDrop(Vector2 draggedWidgetPosition)
		{
			return -1;
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x0002C862 File Offset: 0x0002AA62
		public override bool IsDragHovering { get; }

		// Token: 0x0400075A RID: 1882
		private int _columnLimit = 2;

		// Token: 0x0400075B RID: 1883
		private float _cellWidth = 100f;

		// Token: 0x0400075C RID: 1884
		private float _cellHeight = 100f;
	}
}
