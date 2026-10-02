using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Conversation;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapConversation
{
	// Token: 0x02000058 RID: 88
	public class MapConversationVM : ViewModel
	{
		// Token: 0x06000664 RID: 1636 RVA: 0x00020AE4 File Offset: 0x0001ECE4
		public MapConversationVM(Action onContinue, Func<string> getContinueInputText)
		{
			this._onContinue = onContinue;
			this.DialogController = new MissionConversationVM(getContinueInputText, false);
			this.TableauData = null;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00020B07 File Offset: 0x0001ED07
		public void ExecuteContinue()
		{
			Action onContinue = this._onContinue;
			if (onContinue == null)
			{
				return;
			}
			onContinue();
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00020B19 File Offset: 0x0001ED19
		public override void OnFinalize()
		{
			base.OnFinalize();
			MissionConversationVM dialogController = this.DialogController;
			if (dialogController != null)
			{
				dialogController.OnFinalize();
			}
			this.DialogController = null;
			this.TableauData = null;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00020B40 File Offset: 0x0001ED40
		public void Tick(float dt)
		{
			MissionConversationVM dialogController = this.DialogController;
			if (dialogController == null)
			{
				return;
			}
			dialogController.Tick(dt);
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x00020B53 File Offset: 0x0001ED53
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x00020B5B File Offset: 0x0001ED5B
		[DataSourceProperty]
		public MissionConversationVM DialogController
		{
			get
			{
				return this._dialogController;
			}
			set
			{
				if (value != this._dialogController)
				{
					this._dialogController = value;
					base.OnPropertyChangedWithValue<MissionConversationVM>(value, "DialogController");
				}
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x00020B79 File Offset: 0x0001ED79
		// (set) Token: 0x0600066B RID: 1643 RVA: 0x00020B81 File Offset: 0x0001ED81
		[DataSourceProperty]
		public object TableauData
		{
			get
			{
				return this._tableauData;
			}
			set
			{
				if (value != this._tableauData)
				{
					this._tableauData = value;
					base.OnPropertyChangedWithValue<object>(value, "TableauData");
				}
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00020B9F File Offset: 0x0001ED9F
		// (set) Token: 0x0600066D RID: 1645 RVA: 0x00020BA7 File Offset: 0x0001EDA7
		[DataSourceProperty]
		public bool IsBarterActive
		{
			get
			{
				return this._isBarterActive;
			}
			set
			{
				if (value != this._isBarterActive)
				{
					this._isBarterActive = value;
					base.OnPropertyChangedWithValue(value, "IsBarterActive");
				}
			}
		}

		// Token: 0x040002BC RID: 700
		private readonly Action _onContinue;

		// Token: 0x040002BD RID: 701
		private MissionConversationVM _dialogController;

		// Token: 0x040002BE RID: 702
		private object _tableauData;

		// Token: 0x040002BF RID: 703
		private bool _isBarterActive;
	}
}
