using System;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.OfficialGame
{
	// Token: 0x02000043 RID: 67
	public class MPMatchmakingRegionSelectorItemVM : SelectorItemVM
	{
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00014A6E File Offset: 0x00012C6E
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x00014A76 File Offset: 0x00012C76
		public string RegionCode { get; private set; }

		// Token: 0x0600065B RID: 1627 RVA: 0x00014A7F File Offset: 0x00012C7F
		public MPMatchmakingRegionSelectorItemVM(string regionCode, TextObject regionName)
			: base(regionName)
		{
			this.RegionCode = regionCode;
			this.IsRegionNone = regionCode == "None";
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00014AA0 File Offset: 0x00012CA0
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x00014AA8 File Offset: 0x00012CA8
		[DataSourceProperty]
		public bool IsRegionNone
		{
			get
			{
				return this._isRegionNone;
			}
			set
			{
				if (value != this._isRegionNone)
				{
					this._isRegionNone = value;
					base.OnPropertyChangedWithValue(value, "IsRegionNone");
				}
			}
		}

		// Token: 0x040002FD RID: 765
		private bool _isRegionNone;
	}
}
