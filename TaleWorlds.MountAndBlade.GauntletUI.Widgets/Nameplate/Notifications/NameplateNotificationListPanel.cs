using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate.Notifications
{
	// Token: 0x02000084 RID: 132
	public class NameplateNotificationListPanel : ListPanel
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x00015C8F File Offset: 0x00013E8F
		public NameplateNotificationListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00015CC0 File Offset: 0x00013EC0
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._isFirstFrame)
			{
				switch (this.RelationType)
				{
				case -1:
					this.RelationVisualWidget.Color = NameplateNotificationListPanel.NegativeRelationColor;
					break;
				case 0:
					this.RelationVisualWidget.Color = NameplateNotificationListPanel.NeutralRelationColor;
					break;
				case 1:
					this.RelationVisualWidget.Color = NameplateNotificationListPanel.PositiveRelationColor;
					break;
				}
				this._isFirstFrame = false;
			}
			this._totalDt += dt;
			if (base.AlphaFactor <= 0f || this._totalDt > this._stayAmount + this._fadeTime)
			{
				base.EventFired("OnRemove", Array.Empty<object>());
				return;
			}
			if (this._totalDt > this._stayAmount)
			{
				float num = 1f - (this._totalDt - this._stayAmount) / this._fadeTime;
				this.SetGlobalAlphaRecursively(num);
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x00015DA5 File Offset: 0x00013FA5
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x00015DAD File Offset: 0x00013FAD
		public Widget RelationVisualWidget
		{
			get
			{
				return this._relationVisualWidget;
			}
			set
			{
				if (this._relationVisualWidget != value)
				{
					this._relationVisualWidget = value;
					base.OnPropertyChanged<Widget>(value, "RelationVisualWidget");
				}
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x00015DCB File Offset: 0x00013FCB
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x00015DD3 File Offset: 0x00013FD3
		public int RelationType
		{
			get
			{
				return this._relationType;
			}
			set
			{
				if (this._relationType != value)
				{
					this._relationType = value;
					base.OnPropertyChanged(value, "RelationType");
				}
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x00015DF1 File Offset: 0x00013FF1
		// (set) Token: 0x06000779 RID: 1913 RVA: 0x00015DF9 File Offset: 0x00013FF9
		public float StayAmount
		{
			get
			{
				return this._stayAmount;
			}
			set
			{
				if (this._stayAmount != value)
				{
					this._stayAmount = value;
					base.OnPropertyChanged(value, "StayAmount");
				}
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00015E17 File Offset: 0x00014017
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x00015E1F File Offset: 0x0001401F
		public float FadeTime
		{
			get
			{
				return this._fadeTime;
			}
			set
			{
				if (this._fadeTime != value)
				{
					this._fadeTime = value;
					base.OnPropertyChanged(value, "FadeTime");
				}
			}
		}

		// Token: 0x0400033D RID: 829
		private static readonly Color NegativeRelationColor = Color.ConvertStringToColor("#D6543BFF");

		// Token: 0x0400033E RID: 830
		private static readonly Color NeutralRelationColor = Color.ConvertStringToColor("#ECB05BFF");

		// Token: 0x0400033F RID: 831
		private static readonly Color PositiveRelationColor = Color.ConvertStringToColor("#98CA3AFF");

		// Token: 0x04000340 RID: 832
		private float _totalDt;

		// Token: 0x04000341 RID: 833
		private bool _isFirstFrame = true;

		// Token: 0x04000342 RID: 834
		private Widget _relationVisualWidget;

		// Token: 0x04000343 RID: 835
		private float _stayAmount = 2f;

		// Token: 0x04000344 RID: 836
		private float _fadeTime = 1f;

		// Token: 0x04000345 RID: 837
		private int _relationType = -2;
	}
}
