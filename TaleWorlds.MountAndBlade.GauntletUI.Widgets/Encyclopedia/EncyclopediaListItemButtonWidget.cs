using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x0200015A RID: 346
	public class EncyclopediaListItemButtonWidget : ButtonWidget
	{
		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06001255 RID: 4693 RVA: 0x00032896 File Offset: 0x00030A96
		// (set) Token: 0x06001256 RID: 4694 RVA: 0x0003289E File Offset: 0x00030A9E
		public TextWidget ListItemNameTextWidget { get; set; }

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001257 RID: 4695 RVA: 0x000328A7 File Offset: 0x00030AA7
		// (set) Token: 0x06001258 RID: 4696 RVA: 0x000328AF File Offset: 0x00030AAF
		public TextWidget ListComparedValueTextWidget { get; set; }

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001259 RID: 4697 RVA: 0x000328B8 File Offset: 0x00030AB8
		// (set) Token: 0x0600125A RID: 4698 RVA: 0x000328C0 File Offset: 0x00030AC0
		public Brush InfoAvailableItemNameBrush { get; set; }

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x0600125B RID: 4699 RVA: 0x000328C9 File Offset: 0x00030AC9
		// (set) Token: 0x0600125C RID: 4700 RVA: 0x000328D1 File Offset: 0x00030AD1
		public Brush InfoUnvailableItemNameBrush { get; set; }

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x0600125D RID: 4701 RVA: 0x000328DA File Offset: 0x00030ADA
		// (set) Token: 0x0600125E RID: 4702 RVA: 0x000328E2 File Offset: 0x00030AE2
		public bool IsInfoAvailable { get; set; }

		// Token: 0x0600125F RID: 4703 RVA: 0x000328EB File Offset: 0x00030AEB
		public EncyclopediaListItemButtonWidget(UIContext context)
			: base(context)
		{
			base.EventManager.AddLateUpdateAction(this, new Action<float>(this.OnThisLateUpdate), 1);
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00032910 File Offset: 0x00030B10
		public void OnThisLateUpdate(float dt)
		{
			this.ListItemNameTextWidget.Brush = (this.IsInfoAvailable ? this.InfoAvailableItemNameBrush : this.InfoUnvailableItemNameBrush);
			this.ListComparedValueTextWidget.Brush = (this.IsInfoAvailable ? this.InfoAvailableItemNameBrush : this.InfoUnvailableItemNameBrush);
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x0003295F File Offset: 0x00030B5F
		// (set) Token: 0x06001262 RID: 4706 RVA: 0x00032967 File Offset: 0x00030B67
		[Editor(false)]
		public string ListItemId
		{
			get
			{
				return this._listItemId;
			}
			set
			{
				if (this._listItemId != value)
				{
					this._listItemId = value;
					base.OnPropertyChanged<string>(value, "ListItemId");
				}
			}
		}

		// Token: 0x04000859 RID: 2137
		private string _listItemId;
	}
}
