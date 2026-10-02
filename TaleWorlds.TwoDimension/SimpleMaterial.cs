using System;
using System.Numerics;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000026 RID: 38
	public class SimpleMaterial : Material
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000714D File Offset: 0x0000534D
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00007155 File Offset: 0x00005355
		public Texture Texture { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000715E File Offset: 0x0000535E
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00007166 File Offset: 0x00005366
		public Color Color { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000716F File Offset: 0x0000536F
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00007177 File Offset: 0x00005377
		public float ColorFactor { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00007180 File Offset: 0x00005380
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00007188 File Offset: 0x00005388
		public float AlphaFactor { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00007191 File Offset: 0x00005391
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00007199 File Offset: 0x00005399
		public float HueFactor { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000071A2 File Offset: 0x000053A2
		// (set) Token: 0x06000174 RID: 372 RVA: 0x000071AA File Offset: 0x000053AA
		public float SaturationFactor { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000071B3 File Offset: 0x000053B3
		// (set) Token: 0x06000176 RID: 374 RVA: 0x000071BB File Offset: 0x000053BB
		public float ValueFactor { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000177 RID: 375 RVA: 0x000071C4 File Offset: 0x000053C4
		// (set) Token: 0x06000178 RID: 376 RVA: 0x000071CC File Offset: 0x000053CC
		public bool CircularMaskingEnabled { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000071D5 File Offset: 0x000053D5
		// (set) Token: 0x0600017A RID: 378 RVA: 0x000071DD File Offset: 0x000053DD
		public Vector2 CircularMaskingCenter { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600017B RID: 379 RVA: 0x000071E6 File Offset: 0x000053E6
		// (set) Token: 0x0600017C RID: 380 RVA: 0x000071EE File Offset: 0x000053EE
		public float CircularMaskingRadius { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600017D RID: 381 RVA: 0x000071F7 File Offset: 0x000053F7
		// (set) Token: 0x0600017E RID: 382 RVA: 0x000071FF File Offset: 0x000053FF
		public float CircularMaskingSmoothingRadius { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00007208 File Offset: 0x00005408
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00007210 File Offset: 0x00005410
		public SpriteNinePatchParameters NinePatchParameters { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00007219 File Offset: 0x00005419
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00007221 File Offset: 0x00005421
		public bool OverlayEnabled { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000722A File Offset: 0x0000542A
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00007232 File Offset: 0x00005432
		public Vector2 StartCoordinate { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000723B File Offset: 0x0000543B
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00007243 File Offset: 0x00005443
		public Vector2 Size { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000724C File Offset: 0x0000544C
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00007254 File Offset: 0x00005454
		public Texture OverlayTexture { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000725D File Offset: 0x0000545D
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00007265 File Offset: 0x00005465
		public bool UseOverlayAlphaAsMask { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000726E File Offset: 0x0000546E
		// (set) Token: 0x0600018C RID: 396 RVA: 0x00007276 File Offset: 0x00005476
		public float Scale { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000727F File Offset: 0x0000547F
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00007287 File Offset: 0x00005487
		public float OverlayTextureWidth { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00007290 File Offset: 0x00005490
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00007298 File Offset: 0x00005498
		public float OverlayTextureHeight { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000072A1 File Offset: 0x000054A1
		// (set) Token: 0x06000192 RID: 402 RVA: 0x000072A9 File Offset: 0x000054A9
		public float OverlayXOffset { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000193 RID: 403 RVA: 0x000072B2 File Offset: 0x000054B2
		// (set) Token: 0x06000194 RID: 404 RVA: 0x000072BA File Offset: 0x000054BA
		public float OverlayYOffset { get; set; }

		// Token: 0x06000195 RID: 405 RVA: 0x000072C3 File Offset: 0x000054C3
		public SimpleMaterial()
			: this(null, 0)
		{
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000072CD File Offset: 0x000054CD
		public SimpleMaterial(Texture texture)
			: this(texture, 0)
		{
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000072D7 File Offset: 0x000054D7
		public SimpleMaterial(Texture texture, int renderOrder)
			: this(texture, renderOrder, true)
		{
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000072E2 File Offset: 0x000054E2
		public SimpleMaterial(Texture texture, int renderOrder, bool blending)
			: base(blending, renderOrder)
		{
			this.Reset(texture);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000072F4 File Offset: 0x000054F4
		public void Reset(Texture texture = null)
		{
			this.Texture = texture;
			this.NinePatchParameters = SpriteNinePatchParameters.Empty;
			this.ColorFactor = 1f;
			this.AlphaFactor = 1f;
			this.HueFactor = 0f;
			this.SaturationFactor = 0f;
			this.ValueFactor = 0f;
			this.Color = new Color(1f, 1f, 1f, 1f);
			this.CircularMaskingEnabled = false;
			this.OverlayEnabled = false;
			this.OverlayTextureWidth = 512f;
			this.OverlayTextureHeight = 512f;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000738D File Offset: 0x0000558D
		public Vec2 GetCircularMaskingCenter()
		{
			return this.CircularMaskingCenter;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000739A File Offset: 0x0000559A
		public Vec2 GetOverlayStartCoordinate()
		{
			return this.StartCoordinate;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000073A7 File Offset: 0x000055A7
		public Vec2 GetOverlaySize()
		{
			return this.Size;
		}
	}
}
