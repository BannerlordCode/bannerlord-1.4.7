using System;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000022 RID: 34
	public class MapItemVM : SelectorItemVM
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000A775 File Offset: 0x00008975
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x0000A77D File Offset: 0x0000897D
		public string MapName { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000A786 File Offset: 0x00008986
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x0000A78E File Offset: 0x0000898E
		public string MapId { get; private set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000A797 File Offset: 0x00008997
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000A79F File Offset: 0x0000899F
		public string ForcedSceneLevel { get; private set; }

		// Token: 0x060001CA RID: 458 RVA: 0x0000A7A8 File Offset: 0x000089A8
		public MapItemVM(string mapName, string mapId, string forcedSceneLevel)
			: base(mapName)
		{
			this.MapName = mapName;
			this.MapId = mapId;
			this.NameText = mapName;
			this.ForcedSceneLevel = forcedSceneLevel;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000A7D0 File Offset: 0x000089D0
		public void UpdateSearchedText(string searchedText)
		{
			this._searchedText = searchedText;
			string text = null;
			if (this.MapName.IndexOf(this._searchedText, StringComparison.OrdinalIgnoreCase) != -1)
			{
				text = this.MapName.Substring(this.MapName.IndexOf(this._searchedText, StringComparison.OrdinalIgnoreCase), this._searchedText.Length);
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.NameText = this.MapName.Replace(text, "<a>" + text + "</a>");
				return;
			}
			this.NameText = this.MapName;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000A85B File Offset: 0x00008A5B
		// (set) Token: 0x060001CD RID: 461 RVA: 0x0000A863 File Offset: 0x00008A63
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (this._nameText != value)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x0400010F RID: 271
		private string _searchedText;

		// Token: 0x04000113 RID: 275
		public string _nameText;
	}
}
