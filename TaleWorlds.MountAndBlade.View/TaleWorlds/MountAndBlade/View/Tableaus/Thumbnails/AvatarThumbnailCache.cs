using System;
using TaleWorlds.Engine;
using TaleWorlds.PlayerServices.Avatar;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200003C RID: 60
	public class AvatarThumbnailCache : ThumbnailCache<AvatarThumbnailCreationData>
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000ED1B File Offset: 0x0000CF1B
		// (set) Token: 0x0600021E RID: 542 RVA: 0x0000ED22 File Offset: 0x0000CF22
		public static AvatarThumbnailCache Current { get; private set; }

		// Token: 0x0600021F RID: 543 RVA: 0x0000ED2A File Offset: 0x0000CF2A
		public AvatarThumbnailCache(int capacity)
			: base(capacity)
		{
			AvatarThumbnailCache.Current = this;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000ED39 File Offset: 0x0000CF39
		protected override void OnFinalize()
		{
			base.OnFinalize();
			AvatarThumbnailCache.Current = null;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000ED48 File Offset: 0x0000CF48
		protected override TextureCreationInfo OnCreateTexture(AvatarThumbnailCreationData thumbnailCreationData)
		{
			Texture texture;
			((IThumbnailCache)this).GetValue(thumbnailCreationData.AvatarID, out texture);
			if (!(texture == null))
			{
				((IThumbnailCache)this).AddReference(thumbnailCreationData.AvatarID);
				return TextureCreationInfo.WithExistingTexture(texture);
			}
			if (thumbnailCreationData.AvatarBytes == null || thumbnailCreationData.AvatarBytes.Length == 0)
			{
				return TextureCreationInfo.WithNewTexture(null);
			}
			if (thumbnailCreationData.ImageType == AvatarData.ImageType.Image)
			{
				texture = Texture.CreateFromMemory(thumbnailCreationData.AvatarBytes);
				texture.Name = ThumbnailDebugUtility.CreateDebugIdFrom(thumbnailCreationData.AvatarID, "avatar", "byte_array");
			}
			else if (thumbnailCreationData.ImageType == AvatarData.ImageType.Raw)
			{
				texture = Texture.CreateFromByteArray(thumbnailCreationData.AvatarBytes, (int)thumbnailCreationData.Width, (int)thumbnailCreationData.Height);
				texture.Name = ThumbnailDebugUtility.CreateDebugIdFrom(thumbnailCreationData.AvatarID, "avatar", "raw_data");
			}
			((IThumbnailCache)this).Add(thumbnailCreationData.AvatarID, texture);
			((IThumbnailCache)this).AddReference(thumbnailCreationData.AvatarID);
			return TextureCreationInfo.WithNewTexture(texture);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000EE29 File Offset: 0x0000D029
		protected override bool OnReleaseTexture(AvatarThumbnailCreationData thumbnailCreationData)
		{
			return true;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000EE2C File Offset: 0x0000D02C
		public void FlushCache()
		{
			((IThumbnailCache)this).Clear(true);
		}
	}
}
