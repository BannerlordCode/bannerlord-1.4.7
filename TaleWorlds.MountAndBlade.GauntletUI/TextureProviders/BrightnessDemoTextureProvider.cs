using System;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.TextureProviders
{
	// Token: 0x0200001E RID: 30
	public class BrightnessDemoTextureProvider : TextureProvider
	{
		// Token: 0x17000029 RID: 41
		// (set) Token: 0x06000121 RID: 289 RVA: 0x0000877C File Offset: 0x0000697C
		public int DemoType
		{
			set
			{
				this._sceneTableau.SetDemoType(value);
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000878A File Offset: 0x0000698A
		public BrightnessDemoTextureProvider()
		{
			this._sceneTableau = new BrightnessDemoTableau();
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000087A0 File Offset: 0x000069A0
		private void CheckTexture()
		{
			if (this._sceneTableau != null)
			{
				if (this._texture != this._sceneTableau.Texture)
				{
					this._texture = this._sceneTableau.Texture;
					if (this._texture != null)
					{
						this.wrappedTexture = new EngineTexture(this._texture);
						this._providedTexture = new TaleWorlds.TwoDimension.Texture(this.wrappedTexture);
						return;
					}
					this._providedTexture = null;
					return;
				}
			}
			else
			{
				this._providedTexture = null;
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000881E File Offset: 0x00006A1E
		public override void Tick(float dt)
		{
			base.Tick(dt);
			this.CheckTexture();
			BrightnessDemoTableau sceneTableau = this._sceneTableau;
			if (sceneTableau == null)
			{
				return;
			}
			sceneTableau.OnTick(dt);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000883E File Offset: 0x00006A3E
		public override void Clear(bool clearNextFrame)
		{
			BrightnessDemoTableau sceneTableau = this._sceneTableau;
			if (sceneTableau != null)
			{
				sceneTableau.OnFinalize();
			}
			base.Clear(clearNextFrame);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00008858 File Offset: 0x00006A58
		public override void SetTargetSize(int width, int height)
		{
			base.SetTargetSize(width, height);
			this._sceneTableau.SetTargetSize(width, height);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000886F File Offset: 0x00006A6F
		protected override TaleWorlds.TwoDimension.Texture OnGetTextureForRender(TwoDimensionContext twoDimensionContext, string name)
		{
			this.CheckTexture();
			return this._providedTexture;
		}

		// Token: 0x040000B2 RID: 178
		private BrightnessDemoTableau _sceneTableau;

		// Token: 0x040000B3 RID: 179
		private TaleWorlds.Engine.Texture _texture;

		// Token: 0x040000B4 RID: 180
		private TaleWorlds.TwoDimension.Texture _providedTexture;

		// Token: 0x040000B5 RID: 181
		private EngineTexture wrappedTexture;
	}
}
