using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200003E RID: 62
	public class BannerEditorTextureCache : ThumbnailCache<BannerEditorTextureCreationData>
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000EEBA File Offset: 0x0000D0BA
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000EEC1 File Offset: 0x0000D0C1
		public static BannerEditorTextureCache Current { get; private set; }

		// Token: 0x06000231 RID: 561 RVA: 0x0000EEC9 File Offset: 0x0000D0C9
		public BannerEditorTextureCache(int capacity)
			: base(capacity)
		{
			BannerEditorTextureCache.Current = this;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000EED8 File Offset: 0x0000D0D8
		protected override void OnFinalize()
		{
			base.OnFinalize();
			BannerEditorTextureCache.Current = null;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000EEE8 File Offset: 0x0000D0E8
		protected override TextureCreationInfo OnCreateTexture(BannerEditorTextureCreationData textureCreationData)
		{
			Action<Texture> setAction = textureCreationData.SetAction;
			Action cancelAction = textureCreationData.CancelAction;
			string renderId = textureCreationData.RenderId;
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
			Texture texture2 = BannerTextureCreator.CreateTexture(textureCreationData);
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

		// Token: 0x06000234 RID: 564 RVA: 0x0000EFEC File Offset: 0x0000D1EC
		protected override bool OnReleaseTexture(BannerEditorTextureCreationData thumbnailCreationData)
		{
			string renderId = thumbnailCreationData.RenderId;
			return ((IThumbnailCache)this).RemoveReference(renderId);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000F007 File Offset: 0x0000D207
		public void FlushCache()
		{
			((IThumbnailCache)this).Clear(true);
		}
	}
}
