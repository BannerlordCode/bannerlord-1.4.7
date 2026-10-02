using System;
using TaleWorlds.PlayerServices.Avatar;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200003D RID: 61
	public class AvatarThumbnailCreationData : ThumbnailCreationData
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000EE35 File Offset: 0x0000D035
		// (set) Token: 0x06000225 RID: 549 RVA: 0x0000EE3D File Offset: 0x0000D03D
		public string AvatarID { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000EE46 File Offset: 0x0000D046
		// (set) Token: 0x06000227 RID: 551 RVA: 0x0000EE4E File Offset: 0x0000D04E
		public byte[] AvatarBytes { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000EE57 File Offset: 0x0000D057
		// (set) Token: 0x06000229 RID: 553 RVA: 0x0000EE5F File Offset: 0x0000D05F
		public uint Width { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000EE68 File Offset: 0x0000D068
		// (set) Token: 0x0600022B RID: 555 RVA: 0x0000EE70 File Offset: 0x0000D070
		public uint Height { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000EE79 File Offset: 0x0000D079
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000EE81 File Offset: 0x0000D081
		public AvatarData.ImageType ImageType { get; private set; }

		// Token: 0x0600022E RID: 558 RVA: 0x0000EE8A File Offset: 0x0000D08A
		public AvatarThumbnailCreationData(string avatarID, byte[] avatarBytes, uint width, uint height, AvatarData.ImageType imageType)
			: base(avatarID, null, null)
		{
			this.AvatarID = avatarID;
			this.AvatarBytes = avatarBytes;
			this.Width = width;
			this.Height = height;
			this.ImageType = imageType;
		}
	}
}
