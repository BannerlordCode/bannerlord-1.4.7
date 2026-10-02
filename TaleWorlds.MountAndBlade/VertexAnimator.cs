using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200037E RID: 894
	public class VertexAnimator : SynchedMissionObject
	{
		// Token: 0x06003389 RID: 13193 RVA: 0x000D39EC File Offset: 0x000D1BEC
		public VertexAnimator()
		{
			this.Speed = 20f;
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x000D3A0A File Offset: 0x000D1C0A
		private void SetIsPlaying(bool value)
		{
			if (this._isPlaying != value)
			{
				this._isPlaying = value;
				base.SetScriptComponentToTick(this.GetTickRequirement());
			}
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x000D3A28 File Offset: 0x000D1C28
		protected internal override void OnInit()
		{
			base.OnInit();
			this.RefreshEditDataUsers();
			this.SetIsPlaying(true);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x000D3A49 File Offset: 0x000D1C49
		protected internal override void OnEditorInit()
		{
			this.OnInit();
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x000D3A51 File Offset: 0x000D1C51
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (this._isPlaying)
			{
				return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
			}
			return base.GetTickRequirement();
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x000D3A6C File Offset: 0x000D1C6C
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._isPlaying)
			{
				if (this._curAnimTime < (float)this.BeginKey)
				{
					this._curAnimTime = (float)this.BeginKey;
				}
				base.GameEntity.SetMorphFrameOfComponents(this._curAnimTime);
				this._curAnimTime += dt * this.Speed;
				if (this._curAnimTime > (float)this.EndKey)
				{
					if (this._curAnimTime > (float)this.EndKey && this._playOnce)
					{
						this.SetIsPlaying(false);
						this._curAnimTime = (float)this.EndKey;
						base.GameEntity.SetMorphFrameOfComponents(this._curAnimTime);
						return;
					}
					int num = 0;
					while (this._curAnimTime > (float)this.EndKey && ++num < 100)
					{
						this._curAnimTime = (float)this.BeginKey + (this._curAnimTime - (float)this.EndKey);
					}
				}
			}
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x000D3B57 File Offset: 0x000D1D57
		public void PlayOnce()
		{
			this.Play();
			this._playOnce = true;
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x000D3B66 File Offset: 0x000D1D66
		public void Pause()
		{
			this.SetIsPlaying(false);
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x000D3B6F File Offset: 0x000D1D6F
		public void Play()
		{
			this.Stop();
			this.Resume();
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x000D3B7D File Offset: 0x000D1D7D
		public void Resume()
		{
			this.SetIsPlaying(true);
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x000D3B88 File Offset: 0x000D1D88
		public void Stop()
		{
			this.SetIsPlaying(false);
			this._curAnimTime = (float)this.BeginKey;
			Mesh firstMesh = base.GameEntity.GetFirstMesh();
			if (firstMesh != null)
			{
				firstMesh.MorphTime = this._curAnimTime;
			}
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x000D3BD0 File Offset: 0x000D1DD0
		public void StopAndGoToEnd()
		{
			this.SetIsPlaying(false);
			this._curAnimTime = (float)this.EndKey;
			Mesh firstMesh = base.GameEntity.GetFirstMesh();
			if (firstMesh != null)
			{
				firstMesh.MorphTime = this._curAnimTime;
			}
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x000D3C15 File Offset: 0x000D1E15
		public void SetAnimation(int beginKey, int endKey, float speed)
		{
			this.BeginKey = beginKey;
			this.EndKey = endKey;
			this.Speed = speed;
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x000D3C2C File Offset: 0x000D1E2C
		public void SetAnimationSynched(int beginKey, int endKey, float speed)
		{
			if (beginKey != this.BeginKey || endKey != this.EndKey || speed != this.Speed)
			{
				this.BeginKey = beginKey;
				this.EndKey = endKey;
				this.Speed = speed;
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectVertexAnimation(base.Id, beginKey, endKey, speed));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x000D3C90 File Offset: 0x000D1E90
		public void SetProgressSynched(float value)
		{
			if (MathF.Abs(this.Progress - value) > 0.0001f)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectVertexAnimationProgress(base.Id, value));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this.Progress = value;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06003398 RID: 13208 RVA: 0x000D3CDD File Offset: 0x000D1EDD
		// (set) Token: 0x06003399 RID: 13209 RVA: 0x000D3CFC File Offset: 0x000D1EFC
		private float Progress
		{
			get
			{
				return (this._curAnimTime - (float)this.BeginKey) / (float)(this.EndKey - this.BeginKey);
			}
			set
			{
				this._curAnimTime = (float)this.BeginKey + value * (float)(this.EndKey - this.BeginKey);
				Mesh firstMesh = base.GameEntity.GetFirstMesh();
				if (firstMesh != null)
				{
					firstMesh.MorphTime = this._curAnimTime;
				}
			}
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x000D3D4C File Offset: 0x000D1F4C
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			int count = this._animatedMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				this._animatedMeshes[i].ReleaseEditDataUser();
			}
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x000D3D8C File Offset: 0x000D1F8C
		protected internal override void OnEditorTick(float dt)
		{
			int componentCount = base.GameEntity.GetComponentCount(TaleWorlds.Engine.GameEntity.ComponentType.MetaMesh);
			bool flag = false;
			for (int i = 0; i < componentCount; i++)
			{
				MetaMesh metaMesh = base.GameEntity.GetComponentAtIndex(i, TaleWorlds.Engine.GameEntity.ComponentType.MetaMesh) as MetaMesh;
				for (int j = 0; j < metaMesh.MeshCount; j++)
				{
					int count = this._animatedMeshes.Count;
					bool flag2 = false;
					for (int k = 0; k < count; k++)
					{
						if (metaMesh.GetMeshAtIndex(j) == this._animatedMeshes[k])
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.RefreshEditDataUsers();
			}
			this.OnTick(dt);
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x000D3E40 File Offset: 0x000D2040
		private void RefreshEditDataUsers()
		{
			foreach (Mesh mesh in this._animatedMeshes)
			{
				mesh.ReleaseEditDataUser();
			}
			this._animatedMeshes.Clear();
			int componentCount = base.GameEntity.GetComponentCount(TaleWorlds.Engine.GameEntity.ComponentType.MetaMesh);
			for (int i = 0; i < componentCount; i++)
			{
				MetaMesh metaMesh = base.GameEntity.GetComponentAtIndex(i, TaleWorlds.Engine.GameEntity.ComponentType.MetaMesh) as MetaMesh;
				for (int j = 0; j < metaMesh.MeshCount; j++)
				{
					Mesh meshAtIndex = metaMesh.GetMeshAtIndex(j);
					meshAtIndex.AddEditDataUser();
					meshAtIndex.HintVerticesDynamic();
					meshAtIndex.HintIndicesDynamic();
					this._animatedMeshes.Add(meshAtIndex);
					Mesh baseMesh = meshAtIndex.GetBaseMesh();
					if (baseMesh != null)
					{
						baseMesh.AddEditDataUser();
						this._animatedMeshes.Add(baseMesh);
					}
				}
			}
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x000D3F40 File Offset: 0x000D2140
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteIntToPacket(this.BeginKey, CompressionBasic.AnimationKeyCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.EndKey, CompressionBasic.AnimationKeyCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.Speed, CompressionBasic.VertexAnimationSpeedCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.Progress, CompressionBasic.AnimationProgressCompressionInfo);
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x000D3F94 File Offset: 0x000D2194
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			VertexAnimator.VertexAnimatorRecord vertexAnimatorRecord = (VertexAnimator.VertexAnimatorRecord)synchedMissionObjectReadableRecord.Item2;
			this.BeginKey = vertexAnimatorRecord.BeginKey;
			this.EndKey = vertexAnimatorRecord.EndKey;
			this.Speed = vertexAnimatorRecord.Speed;
			this.Progress = vertexAnimatorRecord.Progress;
		}

		// Token: 0x040015B1 RID: 5553
		public float Speed;

		// Token: 0x040015B2 RID: 5554
		public int BeginKey;

		// Token: 0x040015B3 RID: 5555
		public int EndKey;

		// Token: 0x040015B4 RID: 5556
		private bool _playOnce;

		// Token: 0x040015B5 RID: 5557
		private float _curAnimTime;

		// Token: 0x040015B6 RID: 5558
		private bool _isPlaying;

		// Token: 0x040015B7 RID: 5559
		private readonly List<Mesh> _animatedMeshes = new List<Mesh>();

		// Token: 0x02000657 RID: 1623
		[DefineSynchedMissionObjectType(typeof(VertexAnimator))]
		public struct VertexAnimatorRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AC5 RID: 2757
			// (get) Token: 0x06004054 RID: 16468 RVA: 0x000F8815 File Offset: 0x000F6A15
			// (set) Token: 0x06004055 RID: 16469 RVA: 0x000F881D File Offset: 0x000F6A1D
			public int BeginKey { get; private set; }

			// Token: 0x17000AC6 RID: 2758
			// (get) Token: 0x06004056 RID: 16470 RVA: 0x000F8826 File Offset: 0x000F6A26
			// (set) Token: 0x06004057 RID: 16471 RVA: 0x000F882E File Offset: 0x000F6A2E
			public int EndKey { get; private set; }

			// Token: 0x17000AC7 RID: 2759
			// (get) Token: 0x06004058 RID: 16472 RVA: 0x000F8837 File Offset: 0x000F6A37
			// (set) Token: 0x06004059 RID: 16473 RVA: 0x000F883F File Offset: 0x000F6A3F
			public float Speed { get; private set; }

			// Token: 0x17000AC8 RID: 2760
			// (get) Token: 0x0600405A RID: 16474 RVA: 0x000F8848 File Offset: 0x000F6A48
			// (set) Token: 0x0600405B RID: 16475 RVA: 0x000F8850 File Offset: 0x000F6A50
			public float Progress { get; private set; }

			// Token: 0x0600405C RID: 16476 RVA: 0x000F885C File Offset: 0x000F6A5C
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.BeginKey = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationKeyCompressionInfo, ref bufferReadValid);
				this.EndKey = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationKeyCompressionInfo, ref bufferReadValid);
				this.Speed = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.VertexAnimationSpeedCompressionInfo, ref bufferReadValid);
				this.Progress = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationProgressCompressionInfo, ref bufferReadValid);
				return bufferReadValid;
			}
		}
	}
}
