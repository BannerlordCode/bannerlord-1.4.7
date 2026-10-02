using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.GauntletUI.SceneNotification;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x0200000F RID: 15
	public class GauntletGameNotification : GlobalLayer
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00004C35 File Offset: 0x00002E35
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00004C3C File Offset: 0x00002E3C
		protected static GauntletGameNotification Current { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00004C44 File Offset: 0x00002E44
		protected virtual string MovieName
		{
			get
			{
				return "GameNotificationUI";
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004C4C File Offset: 0x00002E4C
		protected GauntletGameNotification()
		{
			this._dataSource = new GameNotificationVM();
			this._dataSource.CurrentNotificationChanged += this.OnReceiveNewNotification;
			this._layer = new GauntletLayer("GameNotification", 19007, false);
			this._layer.LoadMovie(this.MovieName, this._dataSource);
			base.Layer = this._layer;
			this._layer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Mouse);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004CCE File Offset: 0x00002ECE
		protected virtual void OnReceiveNewNotification(GameNotificationItemVM notification)
		{
			if (!string.IsNullOrEmpty((notification != null) ? notification.NotificationSoundId : null))
			{
				SoundEvent.PlaySound2D(notification.NotificationSoundId);
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004CEF File Offset: 0x00002EEF
		public static void Initialize()
		{
			GauntletGameNotification gauntletGameNotification = GauntletGameNotification.Current;
			if (gauntletGameNotification != null)
			{
				gauntletGameNotification.OnFinalize();
			}
			GauntletGameNotification.Current = new GauntletGameNotification();
			ScreenManager.AddGlobalLayer(GauntletGameNotification.Current, false);
			GauntletGameNotification.Current.RegisterEvents();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004D20 File Offset: 0x00002F20
		public virtual void OnFinalize()
		{
			GameNotificationVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.ClearNotifications();
			}
			this.UnregisterEvents();
			ScreenManager.RemoveGlobalLayer(this);
			this._dataSource = null;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004D46 File Offset: 0x00002F46
		public virtual void RegisterEvents()
		{
			MBInformationManager.FiringQuickInformation += this._dataSource.AddGameNotification;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004D5E File Offset: 0x00002F5E
		public virtual void UnregisterEvents()
		{
			MBInformationManager.FiringQuickInformation -= this._dataSource.AddGameNotification;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004D78 File Offset: 0x00002F78
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			bool shouldBeSuspended = this.GetShouldBeSuspended();
			if (shouldBeSuspended != this._isSuspended)
			{
				ScreenManager.SetSuspendLayer(GauntletGameNotification.Current._layer, shouldBeSuspended);
				this._isSuspended = shouldBeSuspended;
			}
			this._dataSource.IsPaused = this._isSuspended;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004DC4 File Offset: 0x00002FC4
		protected virtual bool GetShouldBeSuspended()
		{
			return GauntletSceneNotification.Current.IsActive || LoadingWindow.IsLoadingWindowActive;
		}

		// Token: 0x04000055 RID: 85
		protected GameNotificationVM _dataSource;

		// Token: 0x04000056 RID: 86
		private readonly GauntletLayer _layer;

		// Token: 0x04000058 RID: 88
		private bool _isSuspended;
	}
}
