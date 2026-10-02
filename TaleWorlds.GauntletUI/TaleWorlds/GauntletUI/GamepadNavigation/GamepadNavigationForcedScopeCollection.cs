using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.GamepadNavigation
{
	// Token: 0x0200004B RID: 75
	public class GamepadNavigationForcedScopeCollection
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x00011E85 File Offset: 0x00010085
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x00011E8D File Offset: 0x0001008D
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					Action<GamepadNavigationForcedScopeCollection> onAvailabilityChanged = this.OnAvailabilityChanged;
					if (onAvailabilityChanged == null)
					{
						return;
					}
					onAvailabilityChanged(this);
				}
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00011EB0 File Offset: 0x000100B0
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x00011EBB File Offset: 0x000100BB
		public bool IsDisabled
		{
			get
			{
				return !this.IsEnabled;
			}
			set
			{
				if (value == this.IsEnabled)
				{
					this.IsEnabled = !value;
				}
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00011ED0 File Offset: 0x000100D0
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x00011ED8 File Offset: 0x000100D8
		public string CollectionID { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x00011EE1 File Offset: 0x000100E1
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x00011EE9 File Offset: 0x000100E9
		public int CollectionOrder { get; set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x00011EF2 File Offset: 0x000100F2
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x00011EFC File Offset: 0x000100FC
		public Widget ParentWidget
		{
			get
			{
				return this._parentWidget;
			}
			set
			{
				if (value != this._parentWidget)
				{
					if (this._parentWidget != null)
					{
						this._invisibleParents.Clear();
						for (Widget widget = this._parentWidget; widget != null; widget = widget.ParentWidget)
						{
							widget.OnVisibilityChanged -= this.OnParentVisibilityChanged;
						}
					}
					this._parentWidget = value;
					for (Widget widget2 = this._parentWidget; widget2 != null; widget2 = widget2.ParentWidget)
					{
						if (!widget2.IsVisible)
						{
							this._invisibleParents.Add(widget2);
						}
						widget2.OnVisibilityChanged += this.OnParentVisibilityChanged;
					}
				}
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x00011F8A File Offset: 0x0001018A
		// (set) Token: 0x0600046E RID: 1134 RVA: 0x00011F92 File Offset: 0x00010192
		public List<GamepadNavigationScope> Scopes { get; private set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x00011F9B File Offset: 0x0001019B
		// (set) Token: 0x06000470 RID: 1136 RVA: 0x00011FA3 File Offset: 0x000101A3
		public GamepadNavigationScope ActiveScope { get; set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x00011FAC File Offset: 0x000101AC
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x00011FB4 File Offset: 0x000101B4
		public GamepadNavigationScope PreviousScope { get; set; }

		// Token: 0x06000473 RID: 1139 RVA: 0x00011FBD File Offset: 0x000101BD
		public GamepadNavigationForcedScopeCollection()
		{
			this.Scopes = new List<GamepadNavigationScope>();
			this._invisibleParents = new List<Widget>();
			this.IsEnabled = true;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00011FE4 File Offset: 0x000101E4
		private void OnParentVisibilityChanged(Widget parent)
		{
			bool flag = this._invisibleParents.Count == 0;
			if (!parent.IsVisible)
			{
				this._invisibleParents.Add(parent);
			}
			else
			{
				this._invisibleParents.Remove(parent);
			}
			bool flag2 = this._invisibleParents.Count == 0;
			if (flag != flag2)
			{
				Action<GamepadNavigationForcedScopeCollection> onAvailabilityChanged = this.OnAvailabilityChanged;
				if (onAvailabilityChanged == null)
				{
					return;
				}
				onAvailabilityChanged(this);
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00012048 File Offset: 0x00010248
		public bool IsAvailable()
		{
			if (this.IsEnabled && this._invisibleParents.Count == 0)
			{
				if (this.Scopes.Any<GamepadNavigationScope>((GamepadNavigationScope x) => x.IsAvailable()))
				{
					return this.ParentWidget.Context.GamepadNavigation.IsAvailableForNavigation();
				}
			}
			return false;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000120AD File Offset: 0x000102AD
		public void AddScope(GamepadNavigationScope scope)
		{
			if (!this.Scopes.Contains(scope))
			{
				this.Scopes.Add(scope);
			}
			Action<GamepadNavigationForcedScopeCollection> onAvailabilityChanged = this.OnAvailabilityChanged;
			if (onAvailabilityChanged == null)
			{
				return;
			}
			onAvailabilityChanged(this);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x000120DA File Offset: 0x000102DA
		public void RemoveScope(GamepadNavigationScope scope)
		{
			if (this.Scopes.Contains(scope))
			{
				this.Scopes.Remove(scope);
			}
			Action<GamepadNavigationForcedScopeCollection> onAvailabilityChanged = this.OnAvailabilityChanged;
			if (onAvailabilityChanged == null)
			{
				return;
			}
			onAvailabilityChanged(this);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00012108 File Offset: 0x00010308
		public void ClearScopes()
		{
			this.Scopes.Clear();
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00012115 File Offset: 0x00010315
		public override string ToString()
		{
			return string.Format("ID:{0} C.C.:{1}", this.CollectionID, this.Scopes.Count);
		}

		// Token: 0x0400022B RID: 555
		public Action<GamepadNavigationForcedScopeCollection> OnAvailabilityChanged;

		// Token: 0x0400022C RID: 556
		private List<Widget> _invisibleParents;

		// Token: 0x0400022D RID: 557
		private bool _isEnabled;

		// Token: 0x04000230 RID: 560
		private Widget _parentWidget;
	}
}
