using System;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000034 RID: 52
	public class Texture
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600025F RID: 607 RVA: 0x000095EA File Offset: 0x000077EA
		// (set) Token: 0x06000260 RID: 608 RVA: 0x000095F2 File Offset: 0x000077F2
		public ITexture PlatformTexture { get; private set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000261 RID: 609 RVA: 0x000095FB File Offset: 0x000077FB
		public bool IsValid
		{
			get
			{
				return this.PlatformTexture.IsValid;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000262 RID: 610 RVA: 0x00009608 File Offset: 0x00007808
		public int Width
		{
			get
			{
				return this.PlatformTexture.Width;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00009615 File Offset: 0x00007815
		public int Height
		{
			get
			{
				return this.PlatformTexture.Height;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009622 File Offset: 0x00007822
		public Texture(ITexture platformTexture)
		{
			this.PlatformTexture = platformTexture;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009631 File Offset: 0x00007831
		public bool IsLoaded()
		{
			return this.PlatformTexture.IsLoaded();
		}
	}
}
