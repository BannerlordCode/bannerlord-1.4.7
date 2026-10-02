using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.GamepadNavigation
{
	// Token: 0x0200004E RID: 78
	internal class GamepadNavigationScopeCollection
	{
		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x00013FE5 File Offset: 0x000121E5
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x00013FED File Offset: 0x000121ED
		public IGamepadNavigationContext Source { get; private set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x00013FF6 File Offset: 0x000121F6
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x00013FFE File Offset: 0x000121FE
		public ReadOnlyCollection<GamepadNavigationScope> AllScopes { get; private set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x00014007 File Offset: 0x00012207
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x0001400F File Offset: 0x0001220F
		public ReadOnlyCollection<GamepadNavigationScope> UninitializedScopes { get; private set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x00014018 File Offset: 0x00012218
		// (set) Token: 0x06000514 RID: 1300 RVA: 0x00014020 File Offset: 0x00012220
		public ReadOnlyCollection<GamepadNavigationScope> VisibleScopes { get; private set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00014029 File Offset: 0x00012229
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x00014031 File Offset: 0x00012231
		public ReadOnlyCollection<GamepadNavigationScope> InvisibleScopes { get; private set; }

		// Token: 0x06000517 RID: 1303 RVA: 0x0001403C File Offset: 0x0001223C
		public GamepadNavigationScopeCollection(IGamepadNavigationContext source, Action<GamepadNavigationScope> onScopeNavigatableWidgetsChanged, Action<GamepadNavigationScope, bool> onScopeVisibilityChanged)
		{
			this._onScopeNavigatableWidgetsChanged = onScopeNavigatableWidgetsChanged;
			this._onScopeVisibilityChanged = onScopeVisibilityChanged;
			this.Source = source;
			this._allScopes = new List<GamepadNavigationScope>();
			this.AllScopes = new ReadOnlyCollection<GamepadNavigationScope>(this._allScopes);
			this._uninitializedScopes = new List<GamepadNavigationScope>();
			this.UninitializedScopes = new ReadOnlyCollection<GamepadNavigationScope>(this._uninitializedScopes);
			this._visibleScopes = new List<GamepadNavigationScope>();
			this.VisibleScopes = new ReadOnlyCollection<GamepadNavigationScope>(this._visibleScopes);
			this._invisibleScopes = new List<GamepadNavigationScope>();
			this.InvisibleScopes = new ReadOnlyCollection<GamepadNavigationScope>(this._invisibleScopes);
			this._dirtyScopes = new List<GamepadNavigationScope>();
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000140DF File Offset: 0x000122DF
		internal void OnFinalize()
		{
			this.ClearAllScopes();
			this._onScopeVisibilityChanged = null;
			this._onScopeNavigatableWidgetsChanged = null;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000140F8 File Offset: 0x000122F8
		internal void HandleScopeVisibilities()
		{
			List<GamepadNavigationScope> dirtyScopes = this._dirtyScopes;
			lock (dirtyScopes)
			{
				for (int i = 0; i < this._dirtyScopes.Count; i++)
				{
					if (this._dirtyScopes[i] != null)
					{
						for (int j = i + 1; j < this._dirtyScopes.Count; j++)
						{
							if (this._dirtyScopes[i] == this._dirtyScopes[j])
							{
								this._dirtyScopes[j] = null;
							}
						}
					}
				}
				foreach (GamepadNavigationScope gamepadNavigationScope in this._dirtyScopes)
				{
					if (gamepadNavigationScope != null)
					{
						bool flag2 = gamepadNavigationScope.IsVisible();
						this._visibleScopes.Remove(gamepadNavigationScope);
						this._invisibleScopes.Remove(gamepadNavigationScope);
						if (flag2)
						{
							this._visibleScopes.Add(gamepadNavigationScope);
						}
						else
						{
							this._invisibleScopes.Add(gamepadNavigationScope);
						}
						this._onScopeVisibilityChanged(gamepadNavigationScope, flag2);
					}
				}
				this._dirtyScopes.Clear();
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00014250 File Offset: 0x00012450
		private void OnScopeVisibilityChanged(GamepadNavigationScope scope, bool isVisible)
		{
			List<GamepadNavigationScope> dirtyScopes = this._dirtyScopes;
			lock (dirtyScopes)
			{
				this._dirtyScopes.Add(scope);
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00014298 File Offset: 0x00012498
		private void OnScopeNavigatableWidgetsChanged(GamepadNavigationScope scope)
		{
			this._onScopeNavigatableWidgetsChanged(scope);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000142A6 File Offset: 0x000124A6
		internal int GetTotalNumberOfScopes()
		{
			return this._visibleScopes.Count + this._invisibleScopes.Count + this._uninitializedScopes.Count;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000142CB File Offset: 0x000124CB
		internal void AddScope(GamepadNavigationScope scope)
		{
			this._uninitializedScopes.Add(scope);
			this._allScopes.Add(scope);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000142E8 File Offset: 0x000124E8
		internal void RemoveScope(GamepadNavigationScope scope)
		{
			this._allScopes.Remove(scope);
			this._uninitializedScopes.Remove(scope);
			this._visibleScopes.Remove(scope);
			this._invisibleScopes.Remove(scope);
			scope.OnVisibilityChanged = (Action<GamepadNavigationScope, bool>)Delegate.Remove(scope.OnVisibilityChanged, new Action<GamepadNavigationScope, bool>(this.OnScopeVisibilityChanged));
			scope.OnNavigatableWidgetsChanged = (Action<GamepadNavigationScope>)Delegate.Remove(scope.OnNavigatableWidgetsChanged, new Action<GamepadNavigationScope>(this.OnScopeNavigatableWidgetsChanged));
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001436D File Offset: 0x0001256D
		internal bool HasScopeInAnyList(GamepadNavigationScope scope)
		{
			return this._visibleScopes.Contains(scope) || this._invisibleScopes.Contains(scope) || this._uninitializedScopes.Contains(scope);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001439C File Offset: 0x0001259C
		internal void OnNavigationScopeInitialized(GamepadNavigationScope scope)
		{
			this._uninitializedScopes.Remove(scope);
			if (scope.IsVisible())
			{
				this._visibleScopes.Add(scope);
			}
			else
			{
				this._invisibleScopes.Add(scope);
			}
			scope.OnVisibilityChanged = (Action<GamepadNavigationScope, bool>)Delegate.Combine(scope.OnVisibilityChanged, new Action<GamepadNavigationScope, bool>(this.OnScopeVisibilityChanged));
			scope.OnNavigatableWidgetsChanged = (Action<GamepadNavigationScope>)Delegate.Combine(scope.OnNavigatableWidgetsChanged, new Action<GamepadNavigationScope>(this.OnScopeNavigatableWidgetsChanged));
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001441C File Offset: 0x0001261C
		internal void OnWidgetDisconnectedFromRoot(Widget widget)
		{
			for (int i = 0; i < this._visibleScopes.Count; i++)
			{
				if (this._visibleScopes[i].FindIndexOfWidget(widget) != -1)
				{
					this._visibleScopes[i].RemoveWidget(widget);
					return;
				}
			}
			for (int j = 0; j < this._invisibleScopes.Count; j++)
			{
				if (this._invisibleScopes[j].FindIndexOfWidget(widget) != -1)
				{
					this._invisibleScopes[j].RemoveWidget(widget);
					return;
				}
			}
			for (int k = 0; k < this._uninitializedScopes.Count; k++)
			{
				if (this._uninitializedScopes[k].FindIndexOfWidget(widget) != -1)
				{
					this._uninitializedScopes[k].RemoveWidget(widget);
					return;
				}
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000144E4 File Offset: 0x000126E4
		private void ClearAllScopes()
		{
			for (int i = 0; i < this._allScopes.Count; i++)
			{
				this._allScopes[i].ClearNavigatableWidgets();
				GamepadNavigationScope gamepadNavigationScope = this._allScopes[i];
				gamepadNavigationScope.OnNavigatableWidgetsChanged = (Action<GamepadNavigationScope>)Delegate.Remove(gamepadNavigationScope.OnNavigatableWidgetsChanged, new Action<GamepadNavigationScope>(this.OnScopeNavigatableWidgetsChanged));
				GamepadNavigationScope gamepadNavigationScope2 = this._allScopes[i];
				gamepadNavigationScope2.OnVisibilityChanged = (Action<GamepadNavigationScope, bool>)Delegate.Remove(gamepadNavigationScope2.OnVisibilityChanged, new Action<GamepadNavigationScope, bool>(this.OnScopeVisibilityChanged));
			}
			this._allScopes.Clear();
			this._uninitializedScopes.Clear();
			this._invisibleScopes.Clear();
			this._visibleScopes.Clear();
			this._allScopes = null;
			this._uninitializedScopes = null;
			this._invisibleScopes = null;
			this._visibleScopes = null;
		}

		// Token: 0x04000264 RID: 612
		private Action<GamepadNavigationScope> _onScopeNavigatableWidgetsChanged;

		// Token: 0x04000265 RID: 613
		private Action<GamepadNavigationScope, bool> _onScopeVisibilityChanged;

		// Token: 0x04000266 RID: 614
		private List<GamepadNavigationScope> _allScopes;

		// Token: 0x04000267 RID: 615
		private List<GamepadNavigationScope> _uninitializedScopes;

		// Token: 0x04000268 RID: 616
		private List<GamepadNavigationScope> _visibleScopes;

		// Token: 0x04000269 RID: 617
		private List<GamepadNavigationScope> _invisibleScopes;

		// Token: 0x0400026A RID: 618
		private List<GamepadNavigationScope> _dirtyScopes;
	}
}
