using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000046 RID: 70
	public class MapNotificationItemBaseVM : ViewModel
	{
		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x0001F6C8 File Offset: 0x0001D8C8
		// (set) Token: 0x060005FC RID: 1532 RVA: 0x0001F6D0 File Offset: 0x0001D8D0
		public INavigationHandler NavigationHandler { get; private set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x0001F6D9 File Offset: 0x0001D8D9
		// (set) Token: 0x060005FE RID: 1534 RVA: 0x0001F6E1 File Offset: 0x0001D8E1
		private protected Action<CampaignVec2> FastMoveCameraToPosition { protected get; private set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x0001F6EA File Offset: 0x0001D8EA
		// (set) Token: 0x06000600 RID: 1536 RVA: 0x0001F6F2 File Offset: 0x0001D8F2
		public InformationData Data { get; private set; }

		// Token: 0x06000601 RID: 1537 RVA: 0x0001F6FC File Offset: 0x0001D8FC
		public MapNotificationItemBaseVM(InformationData data)
		{
			this.Data = data;
			this.ForceInspection = false;
			this.SoundId = data.SoundEventPath;
			this.RefreshValues();
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001F74C File Offset: 0x0001D94C
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject titleText = this.Data.TitleText;
			this.TitleText = ((titleText != null) ? titleText.ToString() : null);
			TextObject descriptionText = this.Data.DescriptionText;
			this.DescriptionText = ((descriptionText != null) ? descriptionText.ToString() : null);
			this._removeHintText = this._removeHintTextObject.ToString();
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001F7AA File Offset: 0x0001D9AA
		public void SetNavigationHandler(INavigationHandler navigationHandler)
		{
			this.NavigationHandler = navigationHandler;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001F7B3 File Offset: 0x0001D9B3
		public void SetFastMoveCameraToPosition(Action<CampaignVec2> fastMoveCameraToPosition)
		{
			this.FastMoveCameraToPosition = fastMoveCameraToPosition;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001F7BC File Offset: 0x0001D9BC
		public void ExecuteAction()
		{
			Action onInspect = this._onInspect;
			if (onInspect == null)
			{
				return;
			}
			onInspect();
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001F7CE File Offset: 0x0001D9CE
		public void ExecuteRemove()
		{
			Action<MapNotificationItemBaseVM> onRemove = this.OnRemove;
			if (onRemove != null)
			{
				onRemove(this);
			}
			Action<MapNotificationItemBaseVM> onFocus = this.OnFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(null);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001F7F3 File Offset: 0x0001D9F3
		public void ExecuteSetFocused()
		{
			this.IsFocused = true;
			Action<MapNotificationItemBaseVM> onFocus = this.OnFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(this);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001F80D File Offset: 0x0001DA0D
		public void ExecuteSetUnfocused()
		{
			this.IsFocused = false;
			Action<MapNotificationItemBaseVM> onFocus = this.OnFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(null);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001F827 File Offset: 0x0001DA27
		public virtual void ManualRefreshRelevantStatus()
		{
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001F829 File Offset: 0x0001DA29
		internal void GoToMapPosition(CampaignVec2 position)
		{
			Action<CampaignVec2> fastMoveCameraToPosition = this.FastMoveCameraToPosition;
			if (fastMoveCameraToPosition == null)
			{
				return;
			}
			fastMoveCameraToPosition(position);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001F83C File Offset: 0x0001DA3C
		public void SetRemoveInputKey(HotKey hotKey)
		{
			this.RemoveInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x0001F84B File Offset: 0x0001DA4B
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x0001F853 File Offset: 0x0001DA53
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
				}
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0001F871 File Offset: 0x0001DA71
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x0001F879 File Offset: 0x0001DA79
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
					Action<MapNotificationItemBaseVM> onFocus = this.OnFocus;
					if (onFocus == null)
					{
						return;
					}
					onFocus(this);
				}
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0001F8A8 File Offset: 0x0001DAA8
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x0001F8B0 File Offset: 0x0001DAB0
		[DataSourceProperty]
		public string NotificationIdentifier
		{
			get
			{
				return this._notificationIdentifier;
			}
			set
			{
				if (value != this._notificationIdentifier)
				{
					this._notificationIdentifier = value;
					base.OnPropertyChangedWithValue<string>(value, "NotificationIdentifier");
				}
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x0001F8D3 File Offset: 0x0001DAD3
		// (set) Token: 0x06000613 RID: 1555 RVA: 0x0001F8DB File Offset: 0x0001DADB
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x0001F8FE File Offset: 0x0001DAFE
		// (set) Token: 0x06000615 RID: 1557 RVA: 0x0001F906 File Offset: 0x0001DB06
		[DataSourceProperty]
		public bool ForceInspection
		{
			get
			{
				return this._forceInspection;
			}
			set
			{
				if (value != this._forceInspection)
				{
					Game game = Game.Current;
					if (game != null && !game.IsDevelopmentMode)
					{
						this._forceInspection = value;
						base.OnPropertyChangedWithValue(value, "ForceInspection");
					}
				}
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0001F93A File Offset: 0x0001DB3A
		// (set) Token: 0x06000617 RID: 1559 RVA: 0x0001F942 File Offset: 0x0001DB42
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x0001F965 File Offset: 0x0001DB65
		// (set) Token: 0x06000619 RID: 1561 RVA: 0x0001F96D File Offset: 0x0001DB6D
		[DataSourceProperty]
		public string SoundId
		{
			get
			{
				return this._soundId;
			}
			set
			{
				if (value != this._soundId)
				{
					this._soundId = value;
					base.OnPropertyChangedWithValue<string>(value, "SoundId");
				}
			}
		}

		// Token: 0x0400028D RID: 653
		internal Action<MapNotificationItemBaseVM> OnRemove;

		// Token: 0x0400028E RID: 654
		internal Action<MapNotificationItemBaseVM> OnFocus;

		// Token: 0x0400028F RID: 655
		protected Action _onInspect;

		// Token: 0x04000291 RID: 657
		private readonly TextObject _removeHintTextObject = new TextObject("{=Bcs9s2tC}Right Click to Remove", null);

		// Token: 0x04000292 RID: 658
		private string _removeHintText;

		// Token: 0x04000293 RID: 659
		private InputKeyItemVM _removeInputKey;

		// Token: 0x04000294 RID: 660
		private bool _isFocused;

		// Token: 0x04000295 RID: 661
		private string _titleText;

		// Token: 0x04000296 RID: 662
		private string _descriptionText;

		// Token: 0x04000297 RID: 663
		private string _soundId;

		// Token: 0x04000298 RID: 664
		private bool _forceInspection;

		// Token: 0x04000299 RID: 665
		private string _notificationIdentifier = "Default";
	}
}
