using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000033 RID: 51
	public class SpritePart
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00009396 File Offset: 0x00007596
		// (set) Token: 0x06000243 RID: 579 RVA: 0x0000939E File Offset: 0x0000759E
		public string Name { get; private set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000244 RID: 580 RVA: 0x000093A7 File Offset: 0x000075A7
		// (set) Token: 0x06000245 RID: 581 RVA: 0x000093AF File Offset: 0x000075AF
		public int Width { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000246 RID: 582 RVA: 0x000093B8 File Offset: 0x000075B8
		// (set) Token: 0x06000247 RID: 583 RVA: 0x000093C0 File Offset: 0x000075C0
		public int Height { get; private set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000248 RID: 584 RVA: 0x000093C9 File Offset: 0x000075C9
		// (set) Token: 0x06000249 RID: 585 RVA: 0x000093D1 File Offset: 0x000075D1
		public int SheetID { get; set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600024A RID: 586 RVA: 0x000093DA File Offset: 0x000075DA
		// (set) Token: 0x0600024B RID: 587 RVA: 0x000093E2 File Offset: 0x000075E2
		public int SheetX { get; set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600024C RID: 588 RVA: 0x000093EB File Offset: 0x000075EB
		// (set) Token: 0x0600024D RID: 589 RVA: 0x000093F3 File Offset: 0x000075F3
		public int SheetY { get; set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600024E RID: 590 RVA: 0x000093FC File Offset: 0x000075FC
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00009404 File Offset: 0x00007604
		public float MinU { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000940D File Offset: 0x0000760D
		// (set) Token: 0x06000251 RID: 593 RVA: 0x00009415 File Offset: 0x00007615
		public float MinV { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000941E File Offset: 0x0000761E
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00009426 File Offset: 0x00007626
		public float MaxU { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000942F File Offset: 0x0000762F
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00009437 File Offset: 0x00007637
		public float MaxV { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00009440 File Offset: 0x00007640
		// (set) Token: 0x06000257 RID: 599 RVA: 0x00009448 File Offset: 0x00007648
		public int SheetWidth { get; private set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00009451 File Offset: 0x00007651
		// (set) Token: 0x06000259 RID: 601 RVA: 0x00009459 File Offset: 0x00007659
		public int SheetHeight { get; private set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00009464 File Offset: 0x00007664
		public Texture Texture
		{
			get
			{
				SpriteCategory category = this._category;
				if (category != null && category.IsLoaded)
				{
					List<Texture> spriteSheets = this._category.SpriteSheets;
					int? num = ((spriteSheets != null) ? new int?(spriteSheets.Count) : null);
					int sheetID = this.SheetID;
					if ((num.GetValueOrDefault() >= sheetID) & (num != null))
					{
						return this._category.SpriteSheets[this.SheetID - 1];
					}
				}
				return null;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600025B RID: 603 RVA: 0x000094E2 File Offset: 0x000076E2
		// (set) Token: 0x0600025C RID: 604 RVA: 0x000094EA File Offset: 0x000076EA
		public SpriteCategory Category
		{
			get
			{
				return this._category;
			}
			internal set
			{
				this._category = value;
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000094F3 File Offset: 0x000076F3
		public SpritePart(string name, SpriteCategory category, int width, int height)
		{
			this.Name = name;
			this.Width = width;
			this.Height = height;
			this._category = category;
			this._category.SpriteParts.Add(this);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000952C File Offset: 0x0000772C
		public void UpdateInitValues()
		{
			Vec2i vec2i = this._category.SheetSizes[this.SheetID - 1];
			this.SheetWidth = vec2i.X;
			this.SheetHeight = vec2i.Y;
			double num = 1.0 / (double)this.SheetWidth;
			double num2 = 1.0 / (double)this.SheetHeight;
			double num3 = (double)this.SheetX * num;
			double num4 = (double)(this.SheetX + this.Width) * num;
			double num5 = (double)this.SheetY * num2;
			double num6 = (double)(this.SheetY + this.Height) * num2;
			this.MinU = (float)num3;
			this.MaxU = (float)num4;
			this.MinV = (float)num5;
			this.MaxV = (float)num6;
		}

		// Token: 0x0400012B RID: 299
		private SpriteCategory _category;
	}
}
