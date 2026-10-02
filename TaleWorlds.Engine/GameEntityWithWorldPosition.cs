using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200004F RID: 79
	public class GameEntityWithWorldPosition
	{
		// Token: 0x0600085F RID: 2143 RVA: 0x00006830 File Offset: 0x00004A30
		public GameEntityWithWorldPosition(WeakGameEntity gameEntity)
		{
			this._customLocalFrame = MatrixFrame.Identity;
			this._gameEntity = gameEntity;
			Scene scene = gameEntity.Scene;
			float groundHeightAtPosition = scene.GetGroundHeightAtPosition(gameEntity.GlobalPosition, BodyFlags.CommonCollisionExcludeFlags);
			this._worldPosition = new WorldPosition(scene, UIntPtr.Zero, new Vec3(gameEntity.GlobalPosition.AsVec2, groundHeightAtPosition, -1f), false);
			this._worldPosition.GetGroundVec3();
			this._orthonormalRotation = gameEntity.GetGlobalFrame().rotation;
			this._orthonormalRotation.Orthonormalize();
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x000068C5 File Offset: 0x00004AC5
		public WeakGameEntity GameEntity
		{
			get
			{
				return this._gameEntity;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x000068CD File Offset: 0x00004ACD
		public WorldPosition WorldPosition
		{
			get
			{
				this.ValidateWorldPosition();
				return this._worldPosition;
			}
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x000068DC File Offset: 0x00004ADC
		private void ValidateWorldPosition()
		{
			Vec3 vec = (this._customLocalFrame.IsIdentity ? this.GameEntity.GetGlobalFrame().origin : this.GameEntity.GetGlobalFrame().TransformToParent(in this._customLocalFrame).origin);
			if (!this._worldPosition.AsVec2.NearlyEquals(vec.AsVec2, 1E-05f))
			{
				this._worldPosition.SetVec3(UIntPtr.Zero, vec, false);
			}
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00006960 File Offset: 0x00004B60
		public void InvalidateWorldPosition()
		{
			this._worldPosition.State = ZValidityState.Invalid;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x00006970 File Offset: 0x00004B70
		public WorldFrame WorldFrame
		{
			get
			{
				Mat3 mat = (this._customLocalFrame.rotation.IsIdentity() ? this.GameEntity.GetGlobalFrame().rotation : this.GameEntity.GetGlobalFrame().rotation.TransformToParent(in this._customLocalFrame.rotation));
				if (!mat.NearlyEquals(in this._orthonormalRotation, 1E-05f))
				{
					this._orthonormalRotation = mat;
					this._orthonormalRotation.Orthonormalize();
				}
				return new WorldFrame(this._orthonormalRotation, this.WorldPosition);
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00006A02 File Offset: 0x00004C02
		public void SetCustomLocalFrame(in MatrixFrame customLocalFrame)
		{
			this._customLocalFrame = customLocalFrame;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x00006A10 File Offset: 0x00004C10
		public Vec2 AsVec2
		{
			get
			{
				this.ValidateWorldPosition();
				return this._worldPosition.AsVec2;
			}
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00006A23 File Offset: 0x00004C23
		public UIntPtr GetNavMesh()
		{
			this.ValidateWorldPosition();
			return this._worldPosition.GetNavMesh();
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00006A36 File Offset: 0x00004C36
		public Vec3 GetNavMeshVec3()
		{
			this.ValidateWorldPosition();
			return this._worldPosition.GetNavMeshVec3();
		}

		// Token: 0x040000B2 RID: 178
		private MatrixFrame _customLocalFrame;

		// Token: 0x040000B3 RID: 179
		private readonly WeakGameEntity _gameEntity;

		// Token: 0x040000B4 RID: 180
		private WorldPosition _worldPosition;

		// Token: 0x040000B5 RID: 181
		private Mat3 _orthonormalRotation;
	}
}
