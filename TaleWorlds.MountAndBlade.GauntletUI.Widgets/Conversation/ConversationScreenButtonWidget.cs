using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Conversation
{
	// Token: 0x02000171 RID: 369
	public class ConversationScreenButtonWidget : ButtonWidget
	{
		// Token: 0x0600134D RID: 4941 RVA: 0x00034702 File Offset: 0x00032902
		public ConversationScreenButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00034718 File Offset: 0x00032918
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.AnswerList != null && this.ContinueButton != null)
			{
				this.ContinueButton.IsVisible = this.AnswerList.ChildCount == 0;
				this.ContinueButton.IsEnabled = this.AnswerList.ChildCount == 0;
			}
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00034770 File Offset: 0x00032970
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			foreach (ConversationOptionListPanel conversationOptionListPanel in this._newlyAddedItems)
			{
				conversationOptionListPanel.OptionButtonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnOptionSelection));
			}
			this._newlyAddedItems.Clear();
			ListPanel answerList = this.AnswerList;
			if (answerList != null && answerList.ChildCount > 0 && this.AnswerList.GetChild(this.AnswerList.ChildCount - 1) != null)
			{
				this.AnswerList.GetChild(this.AnswerList.ChildCount - 1).MarginBottom = 5f;
			}
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0003483C File Offset: 0x00032A3C
		private void OnOptionSelection(Widget obj)
		{
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x00034840 File Offset: 0x00032A40
		private void OnOptionRemoved(Widget obj, Widget child)
		{
			ConversationOptionListPanel conversationOptionListPanel;
			if ((conversationOptionListPanel = obj as ConversationOptionListPanel) != null)
			{
				conversationOptionListPanel.OptionButtonWidget.ClickEventHandlers.Remove(new Action<Widget>(this.OnOptionSelection));
			}
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00034874 File Offset: 0x00032A74
		private void OnNewOptionAdded(Widget parent, Widget child)
		{
			this._newlyAddedItems.Add(child as ConversationOptionListPanel);
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x00034887 File Offset: 0x00032A87
		// (set) Token: 0x06001354 RID: 4948 RVA: 0x00034890 File Offset: 0x00032A90
		[Editor(false)]
		public ListPanel AnswerList
		{
			get
			{
				return this._answerList;
			}
			set
			{
				if (value != this._answerList)
				{
					if (value != null)
					{
						value.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnNewOptionAdded));
						value.ItemRemoveEventHandlers.Add(new Action<Widget, Widget>(this.OnOptionRemoved));
					}
					if (this._answerList != null)
					{
						value.ItemAddEventHandlers.Remove(new Action<Widget, Widget>(this.OnNewOptionAdded));
						value.ItemRemoveEventHandlers.Remove(new Action<Widget, Widget>(this.OnOptionRemoved));
					}
					this._answerList = value;
					base.OnPropertyChanged<ListPanel>(value, "AnswerList");
				}
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001355 RID: 4949 RVA: 0x00034922 File Offset: 0x00032B22
		// (set) Token: 0x06001356 RID: 4950 RVA: 0x0003492A File Offset: 0x00032B2A
		[Editor(false)]
		public ButtonWidget ContinueButton
		{
			get
			{
				return this._continueButton;
			}
			set
			{
				if (value != this._continueButton)
				{
					this._continueButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ContinueButton");
				}
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001357 RID: 4951 RVA: 0x00034948 File Offset: 0x00032B48
		// (set) Token: 0x06001358 RID: 4952 RVA: 0x00034950 File Offset: 0x00032B50
		[Editor(false)]
		public bool IsPersuasionActive
		{
			get
			{
				return this._isPersuasionActive;
			}
			set
			{
				if (value != this._isPersuasionActive)
				{
					this._isPersuasionActive = value;
					base.OnPropertyChanged(value, "IsPersuasionActive");
				}
			}
		}

		// Token: 0x040008C0 RID: 2240
		private List<ConversationOptionListPanel> _newlyAddedItems = new List<ConversationOptionListPanel>();

		// Token: 0x040008C1 RID: 2241
		private ListPanel _answerList;

		// Token: 0x040008C2 RID: 2242
		private ButtonWidget _continueButton;

		// Token: 0x040008C3 RID: 2243
		private bool _isPersuasionActive;
	}
}
