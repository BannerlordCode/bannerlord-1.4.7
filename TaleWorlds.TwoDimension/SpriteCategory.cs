using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000030 RID: 48
	public class SpriteCategory
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00008547 File Offset: 0x00006747
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000854F File Offset: 0x0000674F
		public string Name { get; private set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00008558 File Offset: 0x00006758
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00008560 File Offset: 0x00006760
		public List<SpritePart> SpriteParts { get; private set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00008569 File Offset: 0x00006769
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00008571 File Offset: 0x00006771
		public List<SpritePart> SortedSpritePartList { get; private set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000857A File Offset: 0x0000677A
		// (set) Token: 0x0600021A RID: 538 RVA: 0x00008582 File Offset: 0x00006782
		public List<Texture> SpriteSheets { get; private set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000858B File Offset: 0x0000678B
		// (set) Token: 0x0600021C RID: 540 RVA: 0x00008593 File Offset: 0x00006793
		public int SpriteSheetCount { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000859C File Offset: 0x0000679C
		// (set) Token: 0x0600021E RID: 542 RVA: 0x000085A4 File Offset: 0x000067A4
		public bool IsLoaded { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600021F RID: 543 RVA: 0x000085AD File Offset: 0x000067AD
		// (set) Token: 0x06000220 RID: 544 RVA: 0x000085B5 File Offset: 0x000067B5
		public bool IsPartiallyLoaded { get; private set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000221 RID: 545 RVA: 0x000085BE File Offset: 0x000067BE
		// (set) Token: 0x06000222 RID: 546 RVA: 0x000085C6 File Offset: 0x000067C6
		public Vec2i[] SheetSizes { get; set; }

		// Token: 0x06000223 RID: 547 RVA: 0x000085D0 File Offset: 0x000067D0
		public SpriteCategory(string name, int spriteSheetCount, bool alwaysLoad = false)
		{
			this.Name = name;
			this.SpriteSheetCount = spriteSheetCount;
			this.AlwaysLoad = alwaysLoad;
			this.SpriteSheets = new List<Texture>();
			this.SpriteParts = new List<SpritePart>();
			this.SortedSpritePartList = new List<SpritePart>();
			this.SheetSizes = new Vec2i[spriteSheetCount];
			this._spritePartComparer = new SpriteCategory.SpriteSizeComparer();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00008630 File Offset: 0x00006830
		public void Load(ITwoDimensionResourceContext resourceContext, ResourceDepot resourceDepot)
		{
			if (!this.IsLoaded)
			{
				this.IsLoaded = true;
				this.IsPartiallyLoaded = false;
				for (int i = 1; i <= this.SpriteSheetCount; i++)
				{
					Texture texture = resourceContext.LoadTexture(resourceDepot, string.Concat(new object[] { "SpriteSheets\\", this.Name, "\\", this.Name, "_", i }));
					this.SpriteSheets.Add(texture);
				}
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x000086B8 File Offset: 0x000068B8
		public void Unload()
		{
			if (this.IsLoaded)
			{
				this.SpriteSheets.ForEach(delegate(Texture s)
				{
					s.PlatformTexture.Release();
				});
				this.SpriteSheets.Clear();
				this.IsLoaded = false;
				this.IsPartiallyLoaded = false;
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00008710 File Offset: 0x00006910
		public void Reload(ITwoDimensionResourceContext resourceContext, ResourceDepot resourceDepot, SpriteCategory newCategoryInfo)
		{
			if (this.IsLoaded)
			{
				this.SpriteParts = newCategoryInfo.SpriteParts;
				this.SheetSizes = newCategoryInfo.SheetSizes;
				this.SortList();
				if (this.IsPartiallyLoaded)
				{
					List<int> list = new List<int>();
					for (int i = 0; i < this.SpriteSheetCount; i++)
					{
						if (this.SpriteSheets[i] != null)
						{
							list.Add(i + 1);
							this.PartialUnloadAtIndex(i + 1);
						}
					}
					for (int j = 0; j < list.Count; j++)
					{
						this.PartialLoadAtIndex(resourceContext, resourceDepot, list[j]);
					}
					return;
				}
				this.Unload();
				this.Load(resourceContext, resourceDepot);
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x000087B4 File Offset: 0x000069B4
		public void InitializePartialLoad()
		{
			if (!this.IsLoaded)
			{
				this.IsLoaded = true;
				this.IsPartiallyLoaded = true;
				for (int i = 1; i <= this.SpriteSheetCount; i++)
				{
					this.SpriteSheets.Add(null);
				}
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000087F4 File Offset: 0x000069F4
		public void ReleasePartialLoad()
		{
			if (this.IsLoaded)
			{
				for (int i = 1; i <= this.SpriteSheetCount; i++)
				{
					this.PartialUnloadAtIndex(i);
				}
				this.SpriteSheets.Clear();
				this.IsLoaded = false;
				this.IsPartiallyLoaded = false;
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000883C File Offset: 0x00006A3C
		public void PartialLoadAtIndex(ITwoDimensionResourceContext resourceContext, ResourceDepot resourceDepot, int sheetIndex)
		{
			if (sheetIndex >= 1 && sheetIndex <= this.SpriteSheetCount && this.IsLoaded && this.SpriteSheets[sheetIndex - 1] == null)
			{
				Texture texture = resourceContext.LoadTexture(resourceDepot, string.Concat(new object[] { "SpriteSheets\\", this.Name, "\\", this.Name, "_", sheetIndex }));
				this.SpriteSheets[sheetIndex - 1] = texture;
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000088C4 File Offset: 0x00006AC4
		public void PartialUnloadAtIndex(int sheetIndex)
		{
			if (sheetIndex >= 1 && sheetIndex <= this.SpriteSheetCount && this.IsLoaded && this.SpriteSheets[sheetIndex - 1] != null)
			{
				this.SpriteSheets[sheetIndex - 1].PlatformTexture.Release();
				this.SpriteSheets[sheetIndex - 1] = null;
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000891D File Offset: 0x00006B1D
		public void SortList()
		{
			this.SortedSpritePartList.Clear();
			this.SortedSpritePartList.AddRange(this.SpriteParts);
			this.SortedSpritePartList.Sort(this._spritePartComparer);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000894C File Offset: 0x00006B4C
		public bool IsCategoryFullyLoaded()
		{
			for (int i = 0; i < this.SpriteSheets.Count; i++)
			{
				Texture texture = this.SpriteSheets[i];
				if (texture == null || !texture.IsLoaded())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400010F RID: 271
		public const int SpriteSheetSize = 4096;

		// Token: 0x04000117 RID: 279
		public readonly bool AlwaysLoad;

		// Token: 0x04000119 RID: 281
		private SpriteCategory.SpriteSizeComparer _spritePartComparer;

		// Token: 0x02000044 RID: 68
		protected class SpriteSizeComparer : IComparer<SpritePart>
		{
			// Token: 0x060002C7 RID: 711 RVA: 0x0000A92F File Offset: 0x00008B2F
			public int Compare(SpritePart x, SpritePart y)
			{
				return y.Width * y.Height - x.Width * x.Height;
			}
		}
	}
}
