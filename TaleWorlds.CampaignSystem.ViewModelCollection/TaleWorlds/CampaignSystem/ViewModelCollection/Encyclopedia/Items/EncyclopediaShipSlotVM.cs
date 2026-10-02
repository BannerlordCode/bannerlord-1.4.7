using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000E9 RID: 233
	public class EncyclopediaShipSlotVM : ViewModel
	{
		// Token: 0x060015B0 RID: 5552 RVA: 0x000555F2 File Offset: 0x000537F2
		public EncyclopediaShipSlotVM(string slotId, bool isAvailable)
		{
			this.SlotTypeId = slotId;
			this.IsAvailable = isAvailable;
			this.RefreshValues();
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0005560E File Offset: 0x0005380E
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = GameTexts.FindText("str_ship_slot_type", this.SlotTypeId).ToString();
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x060015B2 RID: 5554 RVA: 0x00055631 File Offset: 0x00053831
		// (set) Token: 0x060015B3 RID: 5555 RVA: 0x00055639 File Offset: 0x00053839
		[DataSourceProperty]
		public string SlotTypeId
		{
			get
			{
				return this._slotTypeId;
			}
			set
			{
				if (value != this._slotTypeId)
				{
					this._slotTypeId = value;
					base.OnPropertyChangedWithValue<string>(value, "SlotTypeId");
				}
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x060015B4 RID: 5556 RVA: 0x0005565C File Offset: 0x0005385C
		// (set) Token: 0x060015B5 RID: 5557 RVA: 0x00055664 File Offset: 0x00053864
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x00055687 File Offset: 0x00053887
		// (set) Token: 0x060015B7 RID: 5559 RVA: 0x0005568F File Offset: 0x0005388F
		[DataSourceProperty]
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
			set
			{
				if (value != this._isAvailable)
				{
					this._isAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsAvailable");
				}
			}
		}

		// Token: 0x040009DC RID: 2524
		private string _slotTypeId;

		// Token: 0x040009DD RID: 2525
		private string _name;

		// Token: 0x040009DE RID: 2526
		private bool _isAvailable;
	}
}
