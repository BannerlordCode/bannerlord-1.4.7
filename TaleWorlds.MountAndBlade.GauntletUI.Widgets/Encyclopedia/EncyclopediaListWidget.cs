using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x0200015B RID: 347
	public class EncyclopediaListWidget : Widget
	{
		// Token: 0x06001263 RID: 4707 RVA: 0x0003298A File Offset: 0x00030B8A
		public EncyclopediaListWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00032994 File Offset: 0x00030B94
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isListSizeInitialized && this.ItemListScroll != null && this.ItemListScroll.Size.Y != 0f)
			{
				this._isListSizeInitialized = true;
				this._isDirty = true;
			}
			if (this._isDirty)
			{
				this._isDirty = false;
				this.UpdateScrollPosition();
			}
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x000329F4 File Offset: 0x00030BF4
		private void UpdateScrollPosition()
		{
			if (!string.IsNullOrEmpty(this.LastSelectedItemId) && this.ItemList != null && this.ItemListScroll != null)
			{
				Widget firstInChildrenRecursive = this.ItemList.GetFirstInChildrenRecursive(delegate(Widget x)
				{
					EncyclopediaListItemButtonWidget encyclopediaListItemButtonWidget;
					return (encyclopediaListItemButtonWidget = x as EncyclopediaListItemButtonWidget) != null && encyclopediaListItemButtonWidget.ListItemId == this.LastSelectedItemId;
				});
				if (firstInChildrenRecursive != null && firstInChildrenRecursive.IsVisible)
				{
					float num = firstInChildrenRecursive.ScaledSuggestedHeight + firstInChildrenRecursive.ScaledMarginTop + firstInChildrenRecursive.ScaledMarginBottom - 2f * base._scaleToUse;
					int visibleSiblingIndex = firstInChildrenRecursive.GetVisibleSiblingIndex();
					float num2 = num * (float)visibleSiblingIndex - this.ItemListScroll.Size.Y / 2f;
					this.ItemListScroll.SetValueForced(num2);
				}
			}
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00032A93 File Offset: 0x00030C93
		private void OnListItemAdded(Widget widget, string eventName, object[] eventArgs)
		{
			if (eventName == "ItemAdd" && eventArgs.Length != 0 && eventArgs[0] is EncyclopediaListItemButtonWidget)
			{
				this._isDirty = true;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x00032AB7 File Offset: 0x00030CB7
		// (set) Token: 0x06001268 RID: 4712 RVA: 0x00032ABF File Offset: 0x00030CBF
		[Editor(false)]
		public string LastSelectedItemId
		{
			get
			{
				return this._lastSelectedItemId;
			}
			set
			{
				if (this._lastSelectedItemId != value)
				{
					this._lastSelectedItemId = value;
					base.OnPropertyChanged<string>(value, "LastSelectedItemId");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x00032AE9 File Offset: 0x00030CE9
		// (set) Token: 0x0600126A RID: 4714 RVA: 0x00032AF1 File Offset: 0x00030CF1
		public ListPanel ItemList
		{
			get
			{
				return this._itemList;
			}
			set
			{
				if (this._itemList != value)
				{
					this._itemList = value;
					this._isDirty = true;
					this._itemList.EventFire += this.OnListItemAdded;
				}
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x0600126B RID: 4715 RVA: 0x00032B21 File Offset: 0x00030D21
		// (set) Token: 0x0600126C RID: 4716 RVA: 0x00032B29 File Offset: 0x00030D29
		public ScrollbarWidget ItemListScroll
		{
			get
			{
				return this._itemListScroll;
			}
			set
			{
				if (this._itemListScroll != value)
				{
					this._itemListScroll = value;
					this._isDirty = true;
				}
			}
		}

		// Token: 0x0400085A RID: 2138
		private bool _isDirty;

		// Token: 0x0400085B RID: 2139
		private bool _isListSizeInitialized;

		// Token: 0x0400085C RID: 2140
		private string _lastSelectedItemId;

		// Token: 0x0400085D RID: 2141
		private ListPanel _itemList;

		// Token: 0x0400085E RID: 2142
		private ScrollbarWidget _itemListScroll;
	}
}
