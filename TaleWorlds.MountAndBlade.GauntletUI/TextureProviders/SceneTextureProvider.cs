using System;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.TextureProviders
{
	// Token: 0x02000023 RID: 35
	public class SceneTextureProvider : TextureProvider
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00008EDA File Offset: 0x000070DA
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00008EE2 File Offset: 0x000070E2
		public Scene WantedScene { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00008EEC File Offset: 0x000070EC
		public bool? IsReady
		{
			get
			{
				SceneTableau sceneTableau = this._sceneTableau;
				if (sceneTableau == null)
				{
					return null;
				}
				return sceneTableau.IsReady;
			}
		}

		// Token: 0x17000057 RID: 87
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00008F12 File Offset: 0x00007112
		public object Scene
		{
			set
			{
				if (value != null)
				{
					this._sceneTableau = new SceneTableau();
					this._sceneTableau.SetScene(value);
					return;
				}
				this._sceneTableau.OnFinalize();
				this._sceneTableau = null;
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008F41 File Offset: 0x00007141
		public SceneTextureProvider()
		{
			this._sceneTableau = new SceneTableau();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008F54 File Offset: 0x00007154
		private void CheckTexture()
		{
			if (this._sceneTableau != null)
			{
				if (this._texture != this._sceneTableau._texture)
				{
					this._texture = this._sceneTableau._texture;
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

		// Token: 0x06000176 RID: 374 RVA: 0x00008FD2 File Offset: 0x000071D2
		public override void Tick(float dt)
		{
			base.Tick(dt);
			this.CheckTexture();
			SceneTableau sceneTableau = this._sceneTableau;
			if (sceneTableau == null)
			{
				return;
			}
			sceneTableau.OnTick(dt);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00008FF2 File Offset: 0x000071F2
		public override void SetTargetSize(int width, int height)
		{
			base.SetTargetSize(width, height);
			this._sceneTableau.SetTargetSize(width, height);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00009009 File Offset: 0x00007209
		protected override TaleWorlds.TwoDimension.Texture OnGetTextureForRender(TwoDimensionContext twoDimensionContext, string name)
		{
			this.CheckTexture();
			return this._providedTexture;
		}

		// Token: 0x040000C9 RID: 201
		private SceneTableau _sceneTableau;

		// Token: 0x040000CA RID: 202
		private TaleWorlds.Engine.Texture _texture;

		// Token: 0x040000CB RID: 203
		private TaleWorlds.TwoDimension.Texture _providedTexture;

		// Token: 0x040000CC RID: 204
		private EngineTexture wrappedTexture;
	}
}
