using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.ClassFilter
{
	// Token: 0x02000063 RID: 99
	public class MPLobbyClassFilterClassItemVM : ViewModel
	{
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x0001E06B File Offset: 0x0001C26B
		// (set) Token: 0x06000993 RID: 2451 RVA: 0x0001E073 File Offset: 0x0001C273
		public MultiplayerClassDivisions.MPHeroClass HeroClass { get; private set; }

		// Token: 0x06000994 RID: 2452 RVA: 0x0001E07C File Offset: 0x0001C27C
		public MPLobbyClassFilterClassItemVM(BasicCultureObject culture, MultiplayerClassDivisions.MPHeroClass heroClass, Action<MPLobbyClassFilterClassItemVM> onSelect)
		{
			this.HeroClass = heroClass;
			this._onSelect = onSelect;
			this.CultureColor = Color.FromUint(culture.Color);
			this.IconType = this.HeroClass.IconType.ToString();
			this.RefreshValues();
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0001E0D3 File Offset: 0x0001C2D3
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.HeroClass.HeroName.ToString();
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0001E0F1 File Offset: 0x0001C2F1
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.HeroClass = null;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0001E100 File Offset: 0x0001C300
		private void ExecuteSelect()
		{
			if (this._onSelect != null)
			{
				this._onSelect(this);
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x0001E116 File Offset: 0x0001C316
		// (set) Token: 0x06000999 RID: 2457 RVA: 0x0001E11E File Offset: 0x0001C31E
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x0001E13C File Offset: 0x0001C33C
		// (set) Token: 0x0600099B RID: 2459 RVA: 0x0001E144 File Offset: 0x0001C344
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
				}
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x0001E162 File Offset: 0x0001C362
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x0001E16A File Offset: 0x0001C36A
		[DataSourceProperty]
		public Color CultureColor
		{
			get
			{
				return this._cultureColor;
			}
			set
			{
				if (value != this._cultureColor)
				{
					this._cultureColor = value;
					base.OnPropertyChangedWithValue(value, "CultureColor");
				}
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x0001E18D File Offset: 0x0001C38D
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x0001E195 File Offset: 0x0001C395
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

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x0001E1B8 File Offset: 0x0001C3B8
		// (set) Token: 0x060009A1 RID: 2465 RVA: 0x0001E1C0 File Offset: 0x0001C3C0
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconType");
				}
			}
		}

		// Token: 0x04000469 RID: 1129
		private Action<MPLobbyClassFilterClassItemVM> _onSelect;

		// Token: 0x0400046B RID: 1131
		private bool _isEnabled;

		// Token: 0x0400046C RID: 1132
		private bool _isSelected;

		// Token: 0x0400046D RID: 1133
		private Color _cultureColor;

		// Token: 0x0400046E RID: 1134
		private string _name;

		// Token: 0x0400046F RID: 1135
		private string _iconType;
	}
}
