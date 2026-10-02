using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Conversation
{
	// Token: 0x02000117 RID: 279
	public class ConversationItemVM : ViewModel
	{
		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06001982 RID: 6530 RVA: 0x00060EF4 File Offset: 0x0005F0F4
		private ConversationSentenceOption _option
		{
			get
			{
				List<ConversationSentenceOption> curOptions = Campaign.Current.ConversationManager.CurOptions;
				if (curOptions == null || curOptions.Count <= 0)
				{
					return default(ConversationSentenceOption);
				}
				return Campaign.Current.ConversationManager.CurOptions[this.Index];
			}
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x00060F48 File Offset: 0x0005F148
		public ConversationItemVM(Action<int> action, Action onReadyToContinue, Action<ConversationItemVM> setCurrentAnswer, int index)
		{
			this.ActionWihIntIndex = action;
			this.Index = index;
			this._onReadyToContinue = onReadyToContinue;
			this.IsEnabled = this._option.IsClickable;
			this.HasPersuasion = this._option.HasPersuasion;
			this._setCurrentAnswer = setCurrentAnswer;
			this.PersuasionItem = new PersuasionOptionVM(Campaign.Current.ConversationManager, index, new Action(this.OnReadyToContinue));
			this.IsSpecial = this._option.IsSpecial;
			this.RefreshValues();
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x00060FD4 File Offset: 0x0005F1D4
		private void OnReadyToContinue()
		{
			this._onReadyToContinue.DynamicInvokeWithLog(Array.Empty<object>());
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x00060FE7 File Offset: 0x0005F1E7
		public ConversationItemVM()
		{
			this.Index = 0;
			this.ItemText = "";
			this.IsEnabled = false;
			this.OptionHint = new HintViewModel();
			this.HasPersuasion = false;
			this._setCurrentAnswer = null;
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x00061024 File Offset: 0x0005F224
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject text = this._option.Text;
			string text2 = ((text != null) ? text.ToString() : null) ?? "";
			this.OptionHint = new HintViewModel((this._option.HintText != null) ? this._option.HintText : TextObject.GetEmpty(), null);
			PersuasionOptionVM persuasionItem = this.PersuasionItem;
			if (persuasionItem != null)
			{
				persuasionItem.RefreshValues();
			}
			if (this.PersuasionItem != null)
			{
				string persuasionAdditionalText = this.PersuasionItem.GetPersuasionAdditionalText();
				if (!string.IsNullOrEmpty(persuasionAdditionalText))
				{
					GameTexts.SetVariable("STR1", text2);
					GameTexts.SetVariable("STR2", persuasionAdditionalText);
					text2 = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
				}
			}
			this.ItemText = text2;
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x000610E4 File Offset: 0x0005F2E4
		public void ExecuteAction()
		{
			Action<int> actionWihIntIndex = this.ActionWihIntIndex;
			if (actionWihIntIndex == null)
			{
				return;
			}
			actionWihIntIndex(this.Index);
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x000610FC File Offset: 0x0005F2FC
		public void SetCurrentAnswer()
		{
			Action<ConversationItemVM> setCurrentAnswer = this._setCurrentAnswer;
			if (setCurrentAnswer == null)
			{
				return;
			}
			setCurrentAnswer(this);
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0006110F File Offset: 0x0005F30F
		public void ResetCurrentAnswer()
		{
			this._setCurrentAnswer(null);
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x0006111D File Offset: 0x0005F31D
		internal void OnPersuasionProgress(Tuple<PersuasionOptionArgs, PersuasionOptionResult> result)
		{
			PersuasionOptionVM persuasionItem = this.PersuasionItem;
			if (persuasionItem == null)
			{
				return;
			}
			persuasionItem.OnPersuasionProgress(result);
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x0600198B RID: 6539 RVA: 0x00061130 File Offset: 0x0005F330
		// (set) Token: 0x0600198C RID: 6540 RVA: 0x00061138 File Offset: 0x0005F338
		[DataSourceProperty]
		public PersuasionOptionVM PersuasionItem
		{
			get
			{
				return this._persuasionItem;
			}
			set
			{
				if (this._persuasionItem != value)
				{
					this._persuasionItem = value;
					base.OnPropertyChangedWithValue<PersuasionOptionVM>(value, "PersuasionItem");
				}
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x0600198D RID: 6541 RVA: 0x00061156 File Offset: 0x0005F356
		// (set) Token: 0x0600198E RID: 6542 RVA: 0x0006115E File Offset: 0x0005F35E
		[DataSourceProperty]
		public bool HasPersuasion
		{
			get
			{
				return this._hasPersuasion;
			}
			set
			{
				if (this._hasPersuasion != value)
				{
					this._hasPersuasion = value;
					base.OnPropertyChangedWithValue(value, "HasPersuasion");
				}
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x0600198F RID: 6543 RVA: 0x0006117C File Offset: 0x0005F37C
		// (set) Token: 0x06001990 RID: 6544 RVA: 0x00061184 File Offset: 0x0005F384
		[DataSourceProperty]
		public int IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (this._iconType != value)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue(value, "IconType");
				}
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06001991 RID: 6545 RVA: 0x000611A2 File Offset: 0x0005F3A2
		// (set) Token: 0x06001992 RID: 6546 RVA: 0x000611AA File Offset: 0x0005F3AA
		[DataSourceProperty]
		public HintViewModel OptionHint
		{
			get
			{
				return this._optionHint;
			}
			set
			{
				if (this._optionHint != value)
				{
					this._optionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "OptionHint");
				}
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06001993 RID: 6547 RVA: 0x000611C8 File Offset: 0x0005F3C8
		// (set) Token: 0x06001994 RID: 6548 RVA: 0x000611D0 File Offset: 0x0005F3D0
		[DataSourceProperty]
		public string ItemText
		{
			get
			{
				return this._itemText;
			}
			set
			{
				if (this._itemText != value)
				{
					this._itemText = value;
					base.OnPropertyChangedWithValue<string>(value, "ItemText");
				}
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06001995 RID: 6549 RVA: 0x000611F3 File Offset: 0x0005F3F3
		// (set) Token: 0x06001996 RID: 6550 RVA: 0x000611FB File Offset: 0x0005F3FB
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (this._isEnabled != value)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06001997 RID: 6551 RVA: 0x00061219 File Offset: 0x0005F419
		// (set) Token: 0x06001998 RID: 6552 RVA: 0x00061221 File Offset: 0x0005F421
		[DataSourceProperty]
		public bool IsSpecial
		{
			get
			{
				return this._isSpecial;
			}
			set
			{
				if (this._isSpecial != value)
				{
					this._isSpecial = value;
					base.OnPropertyChangedWithValue(value, "IsSpecial");
				}
			}
		}

		// Token: 0x04000BB7 RID: 2999
		public Action<int> ActionWihIntIndex;

		// Token: 0x04000BB8 RID: 3000
		public Action<ConversationItemVM> _setCurrentAnswer;

		// Token: 0x04000BB9 RID: 3001
		public int Index;

		// Token: 0x04000BBA RID: 3002
		private Action _onReadyToContinue;

		// Token: 0x04000BBB RID: 3003
		private bool _hasPersuasion;

		// Token: 0x04000BBC RID: 3004
		private bool _isSpecial;

		// Token: 0x04000BBD RID: 3005
		private string _itemText;

		// Token: 0x04000BBE RID: 3006
		private int _iconType;

		// Token: 0x04000BBF RID: 3007
		private bool _isEnabled;

		// Token: 0x04000BC0 RID: 3008
		private PersuasionOptionVM _persuasionItem;

		// Token: 0x04000BC1 RID: 3009
		private HintViewModel _optionHint;
	}
}
