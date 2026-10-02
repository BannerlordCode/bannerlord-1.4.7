using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate
{
	// Token: 0x02000082 RID: 130
	public class SettlementNameplateManagerWidget : Widget
	{
		// Token: 0x0600073B RID: 1851 RVA: 0x00015011 File Offset: 0x00013211
		public SettlementNameplateManagerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00015030 File Offset: 0x00013230
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			this._visibleNameplates.Clear();
			for (int i = 0; i < this._allChildrenNameplates.Count; i++)
			{
				SettlementNameplateWidget settlementNameplateWidget = this._allChildrenNameplates[i];
				if (settlementNameplateWidget != null && settlementNameplateWidget.IsVisibleOnMap)
				{
					this._visibleNameplates.Add(settlementNameplateWidget);
				}
			}
			this._visibleNameplates.Sort();
			for (int j = 0; j < this._visibleNameplates.Count; j++)
			{
				SettlementNameplateWidget settlementNameplateWidget2 = this._visibleNameplates[j];
				settlementNameplateWidget2.DisableRender = false;
				settlementNameplateWidget2.Render(twoDimensionContext, drawContext);
				settlementNameplateWidget2.DisableRender = true;
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x000150C4 File Offset: 0x000132C4
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.DisableRender = true;
			this._allChildrenNameplates.Add(child as SettlementNameplateWidget);
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000150E5 File Offset: 0x000132E5
		protected override void OnBeforeChildRemoved(Widget child)
		{
			base.OnBeforeChildRemoved(child);
			this._allChildrenNameplates.Remove(child as SettlementNameplateWidget);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00015100 File Offset: 0x00013300
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			this._allChildrenNameplates.Clear();
			this._allChildrenNameplates = null;
		}

		// Token: 0x04000325 RID: 805
		private readonly List<SettlementNameplateWidget> _visibleNameplates = new List<SettlementNameplateWidget>();

		// Token: 0x04000326 RID: 806
		private List<SettlementNameplateWidget> _allChildrenNameplates = new List<SettlementNameplateWidget>();
	}
}
