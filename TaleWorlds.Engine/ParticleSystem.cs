using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000072 RID: 114
	[EngineClass("rglParticle_system_instanced")]
	public sealed class ParticleSystem : GameEntityComponent
	{
		// Token: 0x06000A6E RID: 2670 RVA: 0x0000AA67 File Offset: 0x00008C67
		internal ParticleSystem(UIntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0000AA70 File Offset: 0x00008C70
		public static ParticleSystem CreateParticleSystemAttachedToBone(string systemName, Skeleton skeleton, sbyte boneIndex, ref MatrixFrame boneLocalFrame)
		{
			return ParticleSystem.CreateParticleSystemAttachedToBone(ParticleSystemManager.GetRuntimeIdByName(systemName), skeleton, boneIndex, ref boneLocalFrame);
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0000AA80 File Offset: 0x00008C80
		public static ParticleSystem CreateParticleSystemAttachedToBone(int systemRuntimeId, Skeleton skeleton, sbyte boneIndex, ref MatrixFrame boneLocalFrame)
		{
			return EngineApplicationInterface.IParticleSystem.CreateParticleSystemAttachedToBone(systemRuntimeId, skeleton.Pointer, boneIndex, ref boneLocalFrame);
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0000AA95 File Offset: 0x00008C95
		public static ParticleSystem CreateParticleSystemAttachedToEntity(string systemName, GameEntity parentEntity, ref MatrixFrame boneLocalFrame)
		{
			return ParticleSystem.CreateParticleSystemAttachedToEntity(ParticleSystemManager.GetRuntimeIdByName(systemName), parentEntity, ref boneLocalFrame);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0000AAA4 File Offset: 0x00008CA4
		public static ParticleSystem CreateParticleSystemAttachedToEntity(string systemName, WeakGameEntity parentEntity, ref MatrixFrame boneLocalFrame)
		{
			return ParticleSystem.CreateParticleSystemAttachedToEntity(ParticleSystemManager.GetRuntimeIdByName(systemName), parentEntity, ref boneLocalFrame);
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0000AAB3 File Offset: 0x00008CB3
		public static ParticleSystem CreateParticleSystemAttachedToEntity(int systemRuntimeId, GameEntity parentEntity, ref MatrixFrame boneLocalFrame)
		{
			return EngineApplicationInterface.IParticleSystem.CreateParticleSystemAttachedToEntity(systemRuntimeId, parentEntity.Pointer, ref boneLocalFrame);
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0000AAC7 File Offset: 0x00008CC7
		public static ParticleSystem CreateParticleSystemAttachedToEntity(int systemRuntimeId, WeakGameEntity parentEntity, ref MatrixFrame boneLocalFrame)
		{
			return EngineApplicationInterface.IParticleSystem.CreateParticleSystemAttachedToEntity(systemRuntimeId, parentEntity.Pointer, ref boneLocalFrame);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0000AADC File Offset: 0x00008CDC
		public void AddMesh(Mesh mesh)
		{
			EngineApplicationInterface.IMetaMesh.AddMesh(base.Pointer, mesh.Pointer, 0U);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0000AAF5 File Offset: 0x00008CF5
		public void SetEnable(bool enable)
		{
			EngineApplicationInterface.IParticleSystem.SetEnable(base.Pointer, enable);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0000AB08 File Offset: 0x00008D08
		public void SetRuntimeEmissionRateMultiplier(float multiplier)
		{
			EngineApplicationInterface.IParticleSystem.SetRuntimeEmissionRateMultiplier(base.Pointer, multiplier);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0000AB1B File Offset: 0x00008D1B
		public void Restart()
		{
			EngineApplicationInterface.IParticleSystem.Restart(base.Pointer);
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0000AB2D File Offset: 0x00008D2D
		public void SetLocalFrame(in MatrixFrame newLocalFrame)
		{
			EngineApplicationInterface.IParticleSystem.SetLocalFrame(base.Pointer, in newLocalFrame);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0000AB40 File Offset: 0x00008D40
		public void SetPreviousGlobalFrame(in MatrixFrame globalFrame)
		{
			EngineApplicationInterface.IParticleSystem.SetPreviousGlobalFrame(base.Pointer, in globalFrame);
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0000AB54 File Offset: 0x00008D54
		public MatrixFrame GetLocalFrame()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			EngineApplicationInterface.IParticleSystem.GetLocalFrame(base.Pointer, ref identity);
			return identity;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0000AB7A File Offset: 0x00008D7A
		public bool HasAliveParticles()
		{
			return EngineApplicationInterface.IParticleSystem.HasAliveParticles(base.Pointer);
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0000AB8C File Offset: 0x00008D8C
		public void SetDontRemoveFromEntity(bool value)
		{
			EngineApplicationInterface.IParticleSystem.SetDontRemoveFromEntity(base.Pointer, value);
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0000AB9F File Offset: 0x00008D9F
		public void SetParticleEffectByName(string effectName)
		{
			EngineApplicationInterface.IParticleSystem.SetParticleEffectByName(base.Pointer, effectName);
		}
	}
}
