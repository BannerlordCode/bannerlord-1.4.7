using System;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x0200003A RID: 58
	[OverrideView(typeof(MapNotificationView))]
	public class GauntletMapNotificationView : MapNotificationView
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x000104F8 File Offset: 0x0000E6F8
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._mapNavigationHandler = base.MapScreen.NavigationHandler;
			this._dataSource = new MapNotificationVM(this._mapNavigationHandler, new Action<CampaignVec2>(base.MapScreen.FastMoveCameraToPosition));
			this._dataSource.ReceiveNewNotification += this.OnReceiveNewNotification;
			this._dataSource.SetRemoveInputKey(HotKeyManager.GetCategory("MapNotificationHotKeyCategory").GetHotKey("RemoveNotification"));
			base.Layer = new GauntletLayer("MapNotification", 100, false);
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("MapNotificationHotKeyCategory"));
			base.Layer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.All);
			this._movie = this._layerAsGauntletLayer.LoadMovie("MapNotificationUI", this._dataSource);
			base.MapScreen.AddLayer(base.Layer);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000105F1 File Offset: 0x0000E7F1
		private void OnReceiveNewNotification(MapNotificationItemBaseVM newNotification)
		{
			if (!string.IsNullOrEmpty(newNotification.SoundId))
			{
				SoundEvent.PlaySound2D(newNotification.SoundId);
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0001060C File Offset: 0x0000E80C
		public override void RegisterMapNotificationType(Type data, Type item)
		{
			this._dataSource.RegisterMapNotificationType(data, item);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0001061B File Offset: 0x0000E81B
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._dataSource.OnFinalize();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0001062E File Offset: 0x0000E82E
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this._dataSource.OnFrameTick(dt);
			this.HandleInput();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00010649 File Offset: 0x0000E849
		protected override void OnMenuModeTick(float dt)
		{
			base.OnMenuModeTick(dt);
			this._dataSource.OnMenuModeTick(dt);
			this.HandleInput();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00010664 File Offset: 0x0000E864
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00010680 File Offset: 0x0000E880
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0001069C File Offset: 0x0000E89C
		private void HandleInput()
		{
			if (!this._isHoveringOnNotification && this._dataSource.FocusedNotificationItem != null)
			{
				this._isHoveringOnNotification = true;
				base.Layer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(base.Layer);
			}
			else if (this._isHoveringOnNotification && this._dataSource.FocusedNotificationItem == null)
			{
				this._isHoveringOnNotification = false;
				base.Layer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(base.Layer);
			}
			if (this._isHoveringOnNotification && this._dataSource.FocusedNotificationItem != null && base.Layer.Input.IsHotKeyReleased("RemoveNotification") && !this._dataSource.FocusedNotificationItem.ForceInspection)
			{
				SoundEvent.PlaySound2D("event:/ui/default");
				this._dataSource.FocusedNotificationItem.ExecuteRemove();
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0001076A File Offset: 0x0000E96A
		public override void ResetNotifications()
		{
			base.ResetNotifications();
			MapNotificationVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.RemoveAllNotifications();
		}

		// Token: 0x04000101 RID: 257
		private MapNotificationVM _dataSource;

		// Token: 0x04000102 RID: 258
		private GauntletMovieIdentifier _movie;

		// Token: 0x04000103 RID: 259
		private INavigationHandler _mapNavigationHandler;

		// Token: 0x04000104 RID: 260
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x04000105 RID: 261
		private bool _isHoveringOnNotification;

		// Token: 0x04000106 RID: 262
		private const string _defaultSound = "event:/ui/default";
	}
}
