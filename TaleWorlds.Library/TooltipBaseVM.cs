using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200003F RID: 63
	public abstract class TooltipBaseVM : ViewModel
	{
		// Token: 0x06000204 RID: 516 RVA: 0x00007A91 File Offset: 0x00005C91
		public TooltipBaseVM(Type invokedType, object[] invokedArgs)
		{
			this._invokedType = invokedType;
			this._invokedArgs = invokedArgs;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00007AA7 File Offset: 0x00005CA7
		public override void OnFinalize()
		{
			this.OnFinalizeInternal();
			this._invokedArgs = null;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00007AB6 File Offset: 0x00005CB6
		protected virtual void OnFinalizeInternal()
		{
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00007AB8 File Offset: 0x00005CB8
		public virtual void Tick(float dt)
		{
			if (this.IsActive && this._isPeriodicRefreshEnabled)
			{
				this._periodicRefreshTimer -= dt;
				if (this._periodicRefreshTimer < 0f)
				{
					this.OnPeriodicRefresh();
					this._periodicRefreshTimer = this._periodicRefreshDelay;
					return;
				}
			}
			else
			{
				this._periodicRefreshTimer = this._periodicRefreshDelay;
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00007B10 File Offset: 0x00005D10
		protected void InvokeRefreshData<T>(T tooltip) where T : TooltipBaseVM
		{
			InformationManager.TooltipRegistry tooltipRegistry;
			Action<T, object[]> action;
			if (InformationManager.RegisteredTypes.TryGetValue(this._invokedType, out tooltipRegistry) && (action = tooltipRegistry.OnRefreshData as Action<T, object[]>) != null)
			{
				action(tooltip, this._invokedArgs);
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00007B4D File Offset: 0x00005D4D
		protected virtual void OnPeriodicRefresh()
		{
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00007B4F File Offset: 0x00005D4F
		protected virtual void OnIsExtendedChanged()
		{
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00007B51 File Offset: 0x00005D51
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00007B59 File Offset: 0x00005D59
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00007B77 File Offset: 0x00005D77
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00007B7F File Offset: 0x00005D7F
		[DataSourceProperty]
		public bool IsExtended
		{
			get
			{
				return this._isExtended;
			}
			set
			{
				if (this._isExtended != value)
				{
					this._isExtended = value;
					base.OnPropertyChangedWithValue(value, "IsExtended");
					this.OnIsExtendedChanged();
				}
			}
		}

		// Token: 0x040000D6 RID: 214
		protected readonly Type _invokedType;

		// Token: 0x040000D7 RID: 215
		protected object[] _invokedArgs;

		// Token: 0x040000D8 RID: 216
		protected bool _isPeriodicRefreshEnabled;

		// Token: 0x040000D9 RID: 217
		protected float _periodicRefreshDelay;

		// Token: 0x040000DA RID: 218
		private float _periodicRefreshTimer;

		// Token: 0x040000DB RID: 219
		private bool _isActive;

		// Token: 0x040000DC RID: 220
		private bool _isExtended;
	}
}
