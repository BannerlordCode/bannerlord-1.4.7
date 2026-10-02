using System;
using System.Collections.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Library.Information;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar
{
	// Token: 0x0200005B RID: 91
	public class MapInfoItemVM : ViewModel
	{
		// Token: 0x06000691 RID: 1681 RVA: 0x00021111 File Offset: 0x0001F311
		public MapInfoItemVM(string itemId, Func<List<TooltipProperty>> getTooltip)
		{
			this.ItemId = itemId;
			this.VisualId = itemId;
			this._tooltip = new BasicTooltipViewModel(getTooltip);
			this.FloatValue = -1f;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0002113E File Offset: 0x0001F33E
		public MapInfoItemVM(string itemId, TooltipTriggerVM tooltipTrigger)
		{
			this.ItemId = itemId;
			this.VisualId = itemId;
			this._tooltipTrigger = tooltipTrigger;
			this.FloatValue = -1f;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00021166 File Offset: 0x0001F366
		public void ExecuteBeginHint()
		{
			if (this._tooltip != null)
			{
				this._tooltip.ExecuteBeginHint();
				return;
			}
			if (this._tooltipTrigger != null)
			{
				this._tooltipTrigger.ExecuteBeginHint();
			}
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002118F File Offset: 0x0001F38F
		public void ExecuteEndHint()
		{
			if (this._tooltip != null)
			{
				this._tooltip.ExecuteEndHint();
				return;
			}
			if (this._tooltipTrigger != null)
			{
				this._tooltipTrigger.ExecuteEndHint();
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x000211B8 File Offset: 0x0001F3B8
		public void SetOverriddenVisualId(string visualId)
		{
			this.VisualId = (string.IsNullOrEmpty(visualId) ? this.ItemId : visualId);
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x000211D1 File Offset: 0x0001F3D1
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x000211D9 File Offset: 0x0001F3D9
		[DataSourceProperty]
		public bool HasWarning
		{
			get
			{
				return this._hasWarning;
			}
			set
			{
				if (value != this._hasWarning)
				{
					this._hasWarning = value;
					base.OnPropertyChangedWithValue(value, "HasWarning");
				}
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x000211F7 File Offset: 0x0001F3F7
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x000211FF File Offset: 0x0001F3FF
		[DataSourceProperty]
		public int IntValue
		{
			get
			{
				return this._intValue;
			}
			set
			{
				if (value != this._intValue)
				{
					this._intValue = value;
					base.OnPropertyChangedWithValue(value, "IntValue");
					this.FloatValue = (float)value;
				}
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x00021225 File Offset: 0x0001F425
		// (set) Token: 0x0600069B RID: 1691 RVA: 0x0002122D File Offset: 0x0001F42D
		[DataSourceProperty]
		public float FloatValue
		{
			get
			{
				return this._floatValue;
			}
			set
			{
				if (value != this._floatValue)
				{
					this._floatValue = value;
					base.OnPropertyChangedWithValue(value, "FloatValue");
					this.IntValue = (int)value;
				}
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x00021253 File Offset: 0x0001F453
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x0002125B File Offset: 0x0001F45B
		[DataSourceProperty]
		public string VisualId
		{
			get
			{
				return this._visualId;
			}
			set
			{
				if (value != this._visualId)
				{
					this._visualId = value;
					base.OnPropertyChangedWithValue<string>(value, "VisualId");
				}
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x0002127E File Offset: 0x0001F47E
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x00021286 File Offset: 0x0001F486
		[DataSourceProperty]
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue<string>(value, "Value");
				}
			}
		}

		// Token: 0x040002D3 RID: 723
		public readonly string ItemId;

		// Token: 0x040002D4 RID: 724
		private readonly BasicTooltipViewModel _tooltip;

		// Token: 0x040002D5 RID: 725
		private readonly TooltipTriggerVM _tooltipTrigger;

		// Token: 0x040002D6 RID: 726
		private bool _hasWarning;

		// Token: 0x040002D7 RID: 727
		private int _intValue;

		// Token: 0x040002D8 RID: 728
		private float _floatValue;

		// Token: 0x040002D9 RID: 729
		private string _visualId;

		// Token: 0x040002DA RID: 730
		private string _value;
	}
}
