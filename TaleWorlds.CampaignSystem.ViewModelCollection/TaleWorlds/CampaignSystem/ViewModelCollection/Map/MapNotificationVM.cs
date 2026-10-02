using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map
{
	// Token: 0x02000036 RID: 54
	public class MapNotificationVM : ViewModel
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000564 RID: 1380 RVA: 0x0001D4A8 File Offset: 0x0001B6A8
		// (remove) Token: 0x06000565 RID: 1381 RVA: 0x0001D4E0 File Offset: 0x0001B6E0
		public event Action<MapNotificationItemBaseVM> ReceiveNewNotification;

		// Token: 0x06000566 RID: 1382 RVA: 0x0001D518 File Offset: 0x0001B718
		public MapNotificationVM(INavigationHandler navigationHandler, Action<CampaignVec2> fastMoveCameraToPosition)
		{
			this._navigationHandler = navigationHandler;
			this._fastMoveCameraToPosition = fastMoveCameraToPosition;
			MBInformationManager.OnAddMapNotice += this.AddMapNotification;
			this.NotificationItems = new MBBindingList<MapNotificationItemBaseVM>();
			this.PopulateTypeDictionary();
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001D566 File Offset: 0x0001B766
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NotificationItems.ApplyActionOnAllItems(delegate(MapNotificationItemBaseVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001D598 File Offset: 0x0001B798
		private void PopulateTypeDictionary()
		{
			this._itemConstructors.Add(typeof(PeaceMapNotification), typeof(PeaceNotificationItemVM));
			this._itemConstructors.Add(typeof(SettlementRebellionMapNotification), typeof(RebellionNotificationItemVM));
			this._itemConstructors.Add(typeof(WarMapNotification), typeof(WarNotificationItemVM));
			this._itemConstructors.Add(typeof(ArmyDispersionMapNotification), typeof(ArmyDispersionItemVM));
			this._itemConstructors.Add(typeof(ChildBornMapNotification), typeof(NewBornNotificationItemVM));
			this._itemConstructors.Add(typeof(DeathMapNotification), typeof(DeathNotificationItemVM));
			this._itemConstructors.Add(typeof(MarriageMapNotification), typeof(MarriageNotificationItemVM));
			this._itemConstructors.Add(typeof(MarriageOfferMapNotification), typeof(MarriageOfferNotificationItemVM));
			this._itemConstructors.Add(typeof(MercenaryOfferMapNotification), typeof(MercenaryOfferMapNotificationItemVM));
			this._itemConstructors.Add(typeof(VassalOfferMapNotification), typeof(VassalOfferMapNotificationItemVM));
			this._itemConstructors.Add(typeof(ArmyCreationMapNotification), typeof(ArmyCreationNotificationItemVM));
			this._itemConstructors.Add(typeof(KingdomDecisionMapNotification), typeof(KingdomVoteNotificationItemVM));
			this._itemConstructors.Add(typeof(SettlementOwnerChangedMapNotification), typeof(SettlementOwnerChangedNotificationItemVM));
			this._itemConstructors.Add(typeof(SettlementUnderSiegeMapNotification), typeof(SettlementUnderSiegeMapNotificationItemVM));
			this._itemConstructors.Add(typeof(AlleyLeaderDiedMapNotification), typeof(AlleyLeaderDiedMapNotificationItemVM));
			this._itemConstructors.Add(typeof(AlleyUnderAttackMapNotification), typeof(AlleyUnderAttackMapNotificationItemVM));
			this._itemConstructors.Add(typeof(EducationMapNotification), typeof(EducationNotificationItemVM));
			this._itemConstructors.Add(typeof(TraitChangedMapNotification), typeof(TraitChangedNotificationItemVM));
			this._itemConstructors.Add(typeof(RansomOfferMapNotification), typeof(RansomNotificationItemVM));
			this._itemConstructors.Add(typeof(PeaceOfferMapNotification), typeof(PeaceOfferNotificationItemVM));
			this._itemConstructors.Add(typeof(PartyLeaderChangeNotification), typeof(PartyLeaderChangeNotificationVM));
			this._itemConstructors.Add(typeof(HeirComeOfAgeMapNotification), typeof(HeirComeOfAgeNotificationItemVM));
			this._itemConstructors.Add(typeof(KingdomDestroyedMapNotification), typeof(KingdomDestroyedNotificationItemVM));
			this._itemConstructors.Add(typeof(AllianceOfferMapNotification), typeof(AllianceOfferNotificationItemVM));
			this._itemConstructors.Add(typeof(AcceptCallToWarOfferMapNotification), typeof(AcceptCallToWarOfferNotificationItemVM));
			this._itemConstructors.Add(typeof(ProposeCallToWarOfferMapNotification), typeof(ProposeCallToWarOfferNotificationItemVM));
			this._itemConstructors.Add(typeof(TributeFinishedMapNotification), typeof(TributeFinishedMapNotificationVM));
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001D8EA File Offset: 0x0001BAEA
		public void RegisterMapNotificationType(Type data, Type item)
		{
			this._itemConstructors[data] = item;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001D8F9 File Offset: 0x0001BAF9
		public override void OnFinalize()
		{
			MBInformationManager.OnAddMapNotice -= this.AddMapNotification;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001D90C File Offset: 0x0001BB0C
		public void OnFrameTick(float dt)
		{
			for (int i = 0; i < this.NotificationItems.Count; i++)
			{
				this.NotificationItems[i].ManualRefreshRelevantStatus();
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001D940 File Offset: 0x0001BB40
		public void OnMenuModeTick(float dt)
		{
			for (int i = 0; i < this.NotificationItems.Count; i++)
			{
				this.NotificationItems[i].ManualRefreshRelevantStatus();
			}
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001D974 File Offset: 0x0001BB74
		private void RemoveNotificationItem(MapNotificationItemBaseVM item)
		{
			item.OnFinalize();
			this.NotificationItems.Remove(item);
			MBInformationManager.MapNoticeRemoved(item.Data);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001D994 File Offset: 0x0001BB94
		private void OnNotificationItemFocus(MapNotificationItemBaseVM item)
		{
			this.FocusedNotificationItem = item;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001D9A0 File Offset: 0x0001BBA0
		public void AddMapNotification(InformationData data)
		{
			MapNotificationItemBaseVM notificationFromData = this.GetNotificationFromData(data);
			if (notificationFromData != null)
			{
				this.NotificationItems.Add(notificationFromData);
				Action<MapNotificationItemBaseVM> receiveNewNotification = this.ReceiveNewNotification;
				if (receiveNewNotification == null)
				{
					return;
				}
				receiveNewNotification(notificationFromData);
			}
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001D9D8 File Offset: 0x0001BBD8
		public void RemoveAllNotifications()
		{
			foreach (MapNotificationItemBaseVM mapNotificationItemBaseVM in this.NotificationItems.ToList<MapNotificationItemBaseVM>())
			{
				this.RemoveNotificationItem(mapNotificationItemBaseVM);
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001DA30 File Offset: 0x0001BC30
		private MapNotificationItemBaseVM GetNotificationFromData(InformationData data)
		{
			Type type = data.GetType();
			MapNotificationItemBaseVM mapNotificationItemBaseVM = null;
			if (this._itemConstructors.ContainsKey(type))
			{
				mapNotificationItemBaseVM = (MapNotificationItemBaseVM)Activator.CreateInstance(this._itemConstructors[type], new object[] { data });
				if (mapNotificationItemBaseVM != null)
				{
					mapNotificationItemBaseVM.OnRemove = new Action<MapNotificationItemBaseVM>(this.RemoveNotificationItem);
					mapNotificationItemBaseVM.OnFocus = new Action<MapNotificationItemBaseVM>(this.OnNotificationItemFocus);
					mapNotificationItemBaseVM.SetNavigationHandler(this._navigationHandler);
					mapNotificationItemBaseVM.SetFastMoveCameraToPosition(this._fastMoveCameraToPosition);
					if (this.RemoveInputKey != null)
					{
						mapNotificationItemBaseVM.RemoveInputKey = this.RemoveInputKey;
					}
				}
			}
			return mapNotificationItemBaseVM;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001DAC9 File Offset: 0x0001BCC9
		public void SetRemoveInputKey(HotKey hotKey)
		{
			this.RemoveInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0001DAD8 File Offset: 0x0001BCD8
		// (set) Token: 0x06000574 RID: 1396 RVA: 0x0001DAE0 File Offset: 0x0001BCE0
		[DataSourceProperty]
		public InputKeyItemVM RemoveInputKey
		{
			get
			{
				return this._removeInputKey;
			}
			set
			{
				if (value != this._removeInputKey)
				{
					this._removeInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RemoveInputKey");
					if (this._removeInputKey != null && this.NotificationItems != null)
					{
						for (int i = 0; i < this.NotificationItems.Count; i++)
						{
							this.NotificationItems[i].RemoveInputKey = this._removeInputKey;
						}
					}
				}
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0001DB46 File Offset: 0x0001BD46
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x0001DB4E File Offset: 0x0001BD4E
		[DataSourceProperty]
		public MapNotificationItemBaseVM FocusedNotificationItem
		{
			get
			{
				return this._focusedNotificationItem;
			}
			set
			{
				if (value != this._focusedNotificationItem)
				{
					this._focusedNotificationItem = value;
					base.OnPropertyChangedWithValue<MapNotificationItemBaseVM>(value, "FocusedNotificationItem");
				}
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0001DB6C File Offset: 0x0001BD6C
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x0001DB74 File Offset: 0x0001BD74
		[DataSourceProperty]
		public MBBindingList<MapNotificationItemBaseVM> NotificationItems
		{
			get
			{
				return this._notificationItems;
			}
			set
			{
				if (value != this._notificationItems)
				{
					this._notificationItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapNotificationItemBaseVM>>(value, "NotificationItems");
				}
			}
		}

		// Token: 0x04000253 RID: 595
		private INavigationHandler _navigationHandler;

		// Token: 0x04000254 RID: 596
		private Action<CampaignVec2> _fastMoveCameraToPosition;

		// Token: 0x04000255 RID: 597
		private Dictionary<Type, Type> _itemConstructors = new Dictionary<Type, Type>();

		// Token: 0x04000256 RID: 598
		private InputKeyItemVM _removeInputKey;

		// Token: 0x04000257 RID: 599
		private MapNotificationItemBaseVM _focusedNotificationItem;

		// Token: 0x04000258 RID: 600
		private MBBindingList<MapNotificationItemBaseVM> _notificationItems;
	}
}
