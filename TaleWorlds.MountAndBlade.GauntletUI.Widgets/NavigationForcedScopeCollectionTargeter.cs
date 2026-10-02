using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000030 RID: 48
	public class NavigationForcedScopeCollectionTargeter : Widget
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600029C RID: 668 RVA: 0x00008DFA File Offset: 0x00006FFA
		// (set) Token: 0x0600029D RID: 669 RVA: 0x00008E02 File Offset: 0x00007002
		public bool UseRootAsTarget
		{
			get
			{
				return this._useRootAsTarget;
			}
			set
			{
				if (this._useRootAsTarget != value)
				{
					this._useRootAsTarget = value;
					if (base.Context.Root != null && this._useRootAsTarget)
					{
						this.CollectionParent = base.Context.Root;
					}
				}
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00008E3A File Offset: 0x0000703A
		public NavigationForcedScopeCollectionTargeter(UIContext context)
			: base(context)
		{
			this._collection = new GamepadNavigationForcedScopeCollection();
			base.WidthSizePolicy = SizePolicy.Fixed;
			base.HeightSizePolicy = SizePolicy.Fixed;
			base.SuggestedHeight = 0f;
			base.SuggestedWidth = 0f;
			base.IsVisible = false;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00008E79 File Offset: 0x00007079
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			if (this.UseRootAsTarget)
			{
				this.CollectionParent = base.Context.Root;
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00008E9A File Offset: 0x0000709A
		protected override void OnDisconnectedFromRoot()
		{
			this.CollectionParent = null;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x00008EA3 File Offset: 0x000070A3
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x00008EB0 File Offset: 0x000070B0
		public bool IsCollectionEnabled
		{
			get
			{
				return this._collection.IsEnabled;
			}
			set
			{
				if (value != this._collection.IsEnabled)
				{
					this._collection.IsEnabled = value;
				}
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x00008ECC File Offset: 0x000070CC
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x00008ED9 File Offset: 0x000070D9
		public bool IsCollectionDisabled
		{
			get
			{
				return this._collection.IsDisabled;
			}
			set
			{
				if (value != this._collection.IsDisabled)
				{
					this._collection.IsDisabled = value;
				}
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00008EF5 File Offset: 0x000070F5
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x00008F02 File Offset: 0x00007102
		public string CollectionID
		{
			get
			{
				return this._collection.CollectionID;
			}
			set
			{
				if (value != this._collection.CollectionID)
				{
					this._collection.CollectionID = value;
				}
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00008F23 File Offset: 0x00007123
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x00008F30 File Offset: 0x00007130
		public int CollectionOrder
		{
			get
			{
				return this._collection.CollectionOrder;
			}
			set
			{
				if (value != this._collection.CollectionOrder)
				{
					this._collection.CollectionOrder = value;
				}
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x00008F4C File Offset: 0x0000714C
		// (set) Token: 0x060002AA RID: 682 RVA: 0x00008F5C File Offset: 0x0000715C
		public Widget CollectionParent
		{
			get
			{
				return this._collection.ParentWidget;
			}
			set
			{
				if (this._collection.ParentWidget != value)
				{
					if (this._collection.ParentWidget != null)
					{
						base.GamepadNavigationContext.RemoveForcedScopeCollection(this._collection);
					}
					if (!this.UseRootAsTarget || value == base.Context.Root)
					{
						this._collection.ParentWidget = value;
					}
					if (this._collection.ParentWidget != null)
					{
						base.GamepadNavigationContext.AddForcedScopeCollection(this._collection);
					}
				}
			}
		}

		// Token: 0x0400012E RID: 302
		private bool _useRootAsTarget;

		// Token: 0x0400012F RID: 303
		private readonly GamepadNavigationForcedScopeCollection _collection;
	}
}
