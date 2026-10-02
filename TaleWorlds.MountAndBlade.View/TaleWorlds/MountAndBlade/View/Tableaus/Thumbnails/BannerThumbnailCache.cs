using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000042 RID: 66
	public class BannerThumbnailCache : ThumbnailCache<BannerThumbnailCreationData>
	{
		// Token: 0x0600023F RID: 575 RVA: 0x0000F180 File Offset: 0x0000D380
		public BannerThumbnailCache(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000F189 File Offset: 0x0000D389
		protected override void OnInitialize()
		{
			base.OnInitialize();
			BannerTextureCreator.Initialize(this._thumbnailCreatorView);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000F19C File Offset: 0x0000D39C
		protected override void OnFinalize()
		{
			base.OnFinalize();
			BannerTextureCreator.OnFinalize();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000F1AC File Offset: 0x0000D3AC
		protected override TextureCreationInfo OnCreateTexture(BannerThumbnailCreationData thumbnailCreationData)
		{
			Action<Texture> setAction = thumbnailCreationData.SetAction;
			Action cancelAction = thumbnailCreationData.CancelAction;
			string renderId = thumbnailCreationData.RenderId;
			Texture texture;
			if (((IThumbnailCache)this).GetValue(renderId, out texture))
			{
				if (this._renderCallbacks.ContainsKey(renderId))
				{
					this._renderCallbacks[renderId].SetActions.Add(setAction);
					this._renderCallbacks[renderId].CancelActions.Add(cancelAction);
				}
				else if (setAction != null)
				{
					setAction(texture);
				}
				((IThumbnailCache)this).AddReference(renderId);
				return TextureCreationInfo.WithExistingTexture(texture);
			}
			Texture texture2 = BannerTextureCreator.CreateTexture(thumbnailCreationData);
			((IThumbnailCache)this).Add(renderId, texture2);
			((IThumbnailCache)this).AddReference(renderId);
			if (!this._renderCallbacks.ContainsKey(renderId))
			{
				this._renderCallbacks.Add(renderId, RenderCallbackCollection.CreateEmpty());
			}
			this._renderCallbacks[renderId].SetActions.Add(setAction);
			this._renderCallbacks[renderId].CancelActions.Add(cancelAction);
			return TextureCreationInfo.WithNewTexture(texture2);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000F2B0 File Offset: 0x0000D4B0
		protected override bool OnReleaseTexture(BannerThumbnailCreationData thumbnailCreationData)
		{
			string renderId = thumbnailCreationData.RenderId;
			return ((IThumbnailCache)this).RemoveReference(renderId);
		}
	}
}
