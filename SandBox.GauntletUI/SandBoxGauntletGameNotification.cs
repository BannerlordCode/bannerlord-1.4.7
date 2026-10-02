using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI
{
	// Token: 0x02000011 RID: 17
	public class SandBoxGauntletGameNotification : GauntletGameNotification
	{
		// Token: 0x060000CA RID: 202 RVA: 0x00007960 File Offset: 0x00005B60
		public new static void Initialize()
		{
			GauntletGameNotification gauntletGameNotification = GauntletGameNotification.Current;
			if (gauntletGameNotification != null)
			{
				gauntletGameNotification.OnFinalize();
			}
			GauntletGameNotification.Current = new SandBoxGauntletGameNotification();
			ScreenManager.AddGlobalLayer(GauntletGameNotification.Current, false);
			GauntletGameNotification.Current.RegisterEvents();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00007994 File Offset: 0x00005B94
		protected override void OnReceiveNewNotification(GameNotificationItemVM notification)
		{
			base.OnReceiveNewNotification(notification);
			SoundEvent currentNotificationSoundEvent = this._currentNotificationSoundEvent;
			if (currentNotificationSoundEvent != null)
			{
				currentNotificationSoundEvent.Release();
			}
			this._currentNotificationSoundEvent = null;
			if (notification != null && notification.IsDialog)
			{
				this._currentNotificationSoundEvent = SoundEvent.CreateEventFromExternalFile("event:/Extra/voiceover", notification.DialogSoundPath, null, false, false);
				SoundEvent currentNotificationSoundEvent2 = this._currentNotificationSoundEvent;
				if (currentNotificationSoundEvent2 == null)
				{
					return;
				}
				currentNotificationSoundEvent2.Play();
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000079F5 File Offset: 0x00005BF5
		public override void OnFinalize()
		{
			base.OnFinalize();
			SoundEvent currentNotificationSoundEvent = this._currentNotificationSoundEvent;
			if (currentNotificationSoundEvent != null)
			{
				currentNotificationSoundEvent.Release();
			}
			this._currentNotificationSoundEvent = null;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00007A18 File Offset: 0x00005C18
		public override void RegisterEvents()
		{
			base.RegisterEvents();
			CampaignInformationManager.OnDisplayDialog += this._dataSource.AddDialogNotification;
			CampaignInformationManager.OnGetStatusOfDialogNotification += this._dataSource.GetStatusOfDialogNotification;
			CampaignInformationManager.OnClearDialogNotification += this._dataSource.ClearDialogNotification;
			CampaignInformationManager.IsAnyDialogNotificationActiveOrQueued += this._dataSource.GetIsAnyDialogNotificationActiveOrQueued;
			CampaignInformationManager.OnClearAllDialogNotifications += this._dataSource.ClearAllDialogNotifications;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00007A9C File Offset: 0x00005C9C
		public override void UnregisterEvents()
		{
			base.UnregisterEvents();
			CampaignInformationManager.OnDisplayDialog -= this._dataSource.AddDialogNotification;
			CampaignInformationManager.OnGetStatusOfDialogNotification -= this._dataSource.GetStatusOfDialogNotification;
			CampaignInformationManager.OnClearDialogNotification -= this._dataSource.ClearDialogNotification;
			CampaignInformationManager.IsAnyDialogNotificationActiveOrQueued -= this._dataSource.GetIsAnyDialogNotificationActiveOrQueued;
			CampaignInformationManager.OnClearAllDialogNotifications -= this._dataSource.ClearAllDialogNotifications;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00007B1D File Offset: 0x00005D1D
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			this.TickSoundEvent();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00007B2C File Offset: 0x00005D2C
		private void TickSoundEvent()
		{
			if (this._currentNotificationSoundEvent != null)
			{
				if (this._dataSource.GotNotification && this._dataSource.CurrentNotification.IsDialog)
				{
					if (!this._currentNotificationSoundEvent.IsValid || this._currentNotificationSoundEvent.IsStopped())
					{
						this._currentNotificationSoundEvent.Release();
						this._currentNotificationSoundEvent = null;
						this._dataSource.FadeOutCurrentNotification(true);
						return;
					}
					if (this._dataSource.IsPaused && this._currentNotificationSoundEvent.IsPlaying())
					{
						this._currentNotificationSoundEvent.Pause();
						return;
					}
					if (!this._dataSource.IsPaused && this._currentNotificationSoundEvent.IsPaused())
					{
						this._currentNotificationSoundEvent.Resume();
						return;
					}
				}
				else
				{
					this._currentNotificationSoundEvent.Release();
					this._currentNotificationSoundEvent = null;
				}
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00007C00 File Offset: 0x00005E00
		protected override bool GetShouldBeSuspended()
		{
			bool flag = base.GetShouldBeSuspended();
			if (this._dataSource.GotNotification && this._dataSource.CurrentNotification.IsDialog)
			{
				bool flag2;
				if (!flag && !MBCommon.IsPaused)
				{
					GameStateManager gameStateManager = GameStateManager.Current;
					flag2 = gameStateManager != null && gameStateManager.ActiveStateDisabledByUser;
				}
				else
				{
					flag2 = true;
				}
				flag = flag2;
			}
			return flag;
		}

		// Token: 0x04000057 RID: 87
		private SoundEvent _currentNotificationSoundEvent;
	}
}
