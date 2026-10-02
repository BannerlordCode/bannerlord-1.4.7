using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.OfficialGame
{
	// Token: 0x02000042 RID: 66
	public class MPMatchmakingItemVM : ViewModel
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000647 RID: 1607 RVA: 0x000147EC File Offset: 0x000129EC
		// (remove) Token: 0x06000648 RID: 1608 RVA: 0x00014824 File Offset: 0x00012A24
		public event Action<MPMatchmakingItemVM, bool> OnSelectionChanged;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000649 RID: 1609 RVA: 0x0001485C File Offset: 0x00012A5C
		// (remove) Token: 0x0600064A RID: 1610 RVA: 0x00014894 File Offset: 0x00012A94
		public event Action<MPMatchmakingItemVM> OnSetFocusItem;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600064B RID: 1611 RVA: 0x000148CC File Offset: 0x00012ACC
		// (remove) Token: 0x0600064C RID: 1612 RVA: 0x00014904 File Offset: 0x00012B04
		public event Action OnRemoveFocus;

		// Token: 0x0600064D RID: 1613 RVA: 0x00014939 File Offset: 0x00012B39
		public MPMatchmakingItemVM(MultiplayerGameType type)
		{
			this.Type = type.ToString();
			this.IsAvailable = true;
			this.IsSelected = this.IsAvailable;
			this.RefreshValues();
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001496D File Offset: 0x00012B6D
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = GameTexts.FindText("str_multiplayer_official_game_type_name", this.Type).ToString();
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00014990 File Offset: 0x00012B90
		private void ExecuteSetFocusItem()
		{
			Action<MPMatchmakingItemVM> onSetFocusItem = this.OnSetFocusItem;
			if (onSetFocusItem == null)
			{
				return;
			}
			onSetFocusItem(this);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000149A3 File Offset: 0x00012BA3
		private void ExecuteRemoveFocus()
		{
			Action onRemoveFocus = this.OnRemoveFocus;
			if (onRemoveFocus == null)
			{
				return;
			}
			onRemoveFocus();
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x000149B5 File Offset: 0x00012BB5
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x000149BD File Offset: 0x00012BBD
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

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x000149E0 File Offset: 0x00012BE0
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x000149E8 File Offset: 0x00012BE8
		[DataSourceProperty]
		public string Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					base.OnPropertyChangedWithValue<string>(value, "Type");
				}
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00014A0B File Offset: 0x00012C0B
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x00014A13 File Offset: 0x00012C13
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
					Action<MPMatchmakingItemVM, bool> onSelectionChanged = this.OnSelectionChanged;
					if (onSelectionChanged == null)
					{
						return;
					}
					onSelectionChanged(this, this._isSelected);
				}
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00014A48 File Offset: 0x00012C48
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x00014A50 File Offset: 0x00012C50
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

		// Token: 0x040002F8 RID: 760
		private string _name;

		// Token: 0x040002F9 RID: 761
		private string _type;

		// Token: 0x040002FA RID: 762
		private bool _isSelected;

		// Token: 0x040002FB RID: 763
		private bool _isAvailable;
	}
}
