using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.TextureProviders.ImageIdentifiers
{
	// Token: 0x02000028 RID: 40
	public class ItemImageTextureProvider : ImageIdentifierTextureProvider
	{
		// Token: 0x06000199 RID: 409 RVA: 0x00009554 File Offset: 0x00007754
		protected override void OnCreateImageWithId(string id, string additionalArgs)
		{
			if (string.IsNullOrEmpty(id))
			{
				base.OnTextureCreated(null);
				return;
			}
			ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(id);
			Debug.Print("Render Requested: " + id, 0, Debug.DebugColor.White, 17592186044416UL);
			if (@object == null)
			{
				Debug.FailedAssert("WRONG Item IMAGE IDENTIFIER ID", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\TextureProviders\\ImageIdentifiers\\ItemImageTextureProvider.cs", "OnCreateImageWithId", 27);
				base.OnTextureCreated(null);
				return;
			}
			base.ThumbnailCreationData = new ItemThumbnailCreationData(@object, additionalArgs, new Action<Texture>(base.OnTextureCreated), new Action(base.OnTextureCreationCancelled));
			ThumbnailCacheManager.Current.CreateTexture(base.ThumbnailCreationData);
		}
	}
}
