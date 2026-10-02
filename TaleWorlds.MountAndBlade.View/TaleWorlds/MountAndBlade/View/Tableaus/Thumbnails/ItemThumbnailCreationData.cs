using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000049 RID: 73
	public class ItemThumbnailCreationData : ThumbnailCreationData
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600026B RID: 619 RVA: 0x000112B4 File Offset: 0x0000F4B4
		// (set) Token: 0x0600026C RID: 620 RVA: 0x000112BC File Offset: 0x0000F4BC
		public ItemObject ItemObject { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600026D RID: 621 RVA: 0x000112C5 File Offset: 0x0000F4C5
		// (set) Token: 0x0600026E RID: 622 RVA: 0x000112CD File Offset: 0x0000F4CD
		public string AdditionalArgs { get; private set; }

		// Token: 0x0600026F RID: 623 RVA: 0x000112D6 File Offset: 0x0000F4D6
		public ItemThumbnailCreationData(ItemObject itemObject, string additionalArgs, Action<Texture> setAction, Action cancelAction)
			: base(itemObject.StringId, setAction, cancelAction)
		{
			this.ItemObject = itemObject;
			this.AdditionalArgs = additionalArgs;
		}
	}
}
