using System;
using SandBox.BoardGames.Objects;
using SandBox.BoardGames.Pawns;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.BoardGames.Tiles
{
	// Token: 0x020000F5 RID: 245
	public abstract class TileBase
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000C61 RID: 3169 RVA: 0x0005D215 File Offset: 0x0005B415
		public GameEntity Entity { get; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x0005D21D File Offset: 0x0005B41D
		public BoardGameDecal ValidMoveDecal { get; }

		// Token: 0x06000C63 RID: 3171 RVA: 0x0005D225 File Offset: 0x0005B425
		protected TileBase(GameEntity entity, BoardGameDecal decal)
		{
			this.Entity = entity;
			this.ValidMoveDecal = decal;
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0005D23B File Offset: 0x0005B43B
		public virtual void Reset()
		{
			this.PawnOnTile = null;
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0005D244 File Offset: 0x0005B444
		public void Tick(float dt)
		{
			int num = (this._showTile ? 1 : (-1));
			this._tileFadeTimer += (float)num * dt * 5f;
			this._tileFadeTimer = MBMath.ClampFloat(this._tileFadeTimer, 0f, 1f);
			this.ValidMoveDecal.SetAlpha(this._tileFadeTimer);
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x0005D2A1 File Offset: 0x0005B4A1
		public void SetVisibility(bool isVisible)
		{
			this._showTile = isVisible;
		}

		// Token: 0x04000558 RID: 1368
		public PawnBase PawnOnTile;

		// Token: 0x04000559 RID: 1369
		private bool _showTile;

		// Token: 0x0400055A RID: 1370
		private float _tileFadeTimer;

		// Token: 0x0400055B RID: 1371
		private const float TileFadeDuration = 0.2f;
	}
}
