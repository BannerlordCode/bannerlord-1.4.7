using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200004C RID: 76
	public struct TextureCreationInfo
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0001133C File Offset: 0x0000F53C
		public bool IsSuccess
		{
			get
			{
				return this.IsValid && (this.CreatedNewTexture || this.UsingExistingTexture);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00011358 File Offset: 0x0000F558
		public bool IsFail
		{
			get
			{
				return this.IsValid && !this.CreatedNewTexture && !this.UsingExistingTexture;
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00011378 File Offset: 0x0000F578
		public static TextureCreationInfo WithNewTexture(Texture texture = null)
		{
			return new TextureCreationInfo
			{
				IsValid = true,
				CreatedNewTexture = true,
				Texture = texture
			};
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000113A8 File Offset: 0x0000F5A8
		public static TextureCreationInfo WithExistingTexture(Texture texture)
		{
			return new TextureCreationInfo
			{
				IsValid = true,
				UsingExistingTexture = true,
				Texture = texture
			};
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000113D8 File Offset: 0x0000F5D8
		public static TextureCreationInfo Fail()
		{
			return new TextureCreationInfo
			{
				IsValid = true
			};
		}

		// Token: 0x0400014E RID: 334
		public bool IsValid;

		// Token: 0x0400014F RID: 335
		public bool CreatedNewTexture;

		// Token: 0x04000150 RID: 336
		public bool UsingExistingTexture;

		// Token: 0x04000151 RID: 337
		public Texture Texture;
	}
}
