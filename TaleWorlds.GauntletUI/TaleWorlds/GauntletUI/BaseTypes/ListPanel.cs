using System;
using System.Numerics;
using TaleWorlds.GauntletUI.Layout;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200005E RID: 94
	public class ListPanel : Container
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0001B740 File Offset: 0x00019940
		// (set) Token: 0x06000659 RID: 1625 RVA: 0x0001B748 File Offset: 0x00019948
		public StackLayout StackLayout { get; private set; }

		// Token: 0x0600065A RID: 1626 RVA: 0x0001B751 File Offset: 0x00019951
		public ListPanel(UIContext context)
			: base(context)
		{
			this.StackLayout = new StackLayout();
			base.LayoutImp = this.StackLayout;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001B771 File Offset: 0x00019971
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.UpdateListPanel();
			if (this.ResetSelectedOnLosingFocus && !base.CheckIsMyChildRecursive(base.EventManager.LatestMouseDownWidget))
			{
				base.IntValue = -1;
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001B7A2 File Offset: 0x000199A2
		private void UpdateListPanel()
		{
			if (base.AcceptDrop && this.IsDragHovering)
			{
				base.DragHoverInsertionIndex = this.GetIndexForDrop(base.EventManager.MousePosition);
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001B7CB File Offset: 0x000199CB
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x0001B7D3 File Offset: 0x000199D3
		public override Predicate<Widget> AcceptDropPredicate { get; set; }

		// Token: 0x0600065F RID: 1631 RVA: 0x0001B7DC File Offset: 0x000199DC
		public override int GetIndexForDrop(Vector2 mousePosition)
		{
			return this.StackLayout.GetIndexForDrop(this, mousePosition);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001B7EB File Offset: 0x000199EB
		public override Vector2 GetDropGizmoPosition(Vector2 mousePosition)
		{
			return this.StackLayout.GetDropGizmoPosition(this, mousePosition);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001B7FC File Offset: 0x000199FC
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

		// Token: 0x06000662 RID: 1634 RVA: 0x0001B82F File Offset: 0x00019A2F
		protected internal override void OnDragHoverBegin()
		{
			this._dragHovering = true;
			base.SetMeasureAndLayoutDirty();
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001B83E File Offset: 0x00019A3E
		protected internal override void OnDragHoverEnd()
		{
			this._dragHovering = false;
			base.SetMeasureAndLayoutDirty();
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001B84D File Offset: 0x00019A4D
		protected override bool OnPreviewDragHover()
		{
			return base.AcceptDrop;
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x0001B855 File Offset: 0x00019A55
		public override bool IsDragHovering
		{
			get
			{
				return this._dragHovering;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0001B85D File Offset: 0x00019A5D
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x0001B865 File Offset: 0x00019A65
		[Editor(false)]
		public bool ResetSelectedOnLosingFocus
		{
			get
			{
				return this._resetSelectedOnLosingFocus;
			}
			set
			{
				if (this._resetSelectedOnLosingFocus != value)
				{
					this._resetSelectedOnLosingFocus = value;
					base.OnPropertyChanged(value, "ResetSelectedOnLosingFocus");
				}
			}
		}

		// Token: 0x040002FC RID: 764
		private bool _dragHovering;

		// Token: 0x040002FD RID: 765
		private bool _resetSelectedOnLosingFocus;
	}
}
