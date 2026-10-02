using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000336 RID: 822
	public class Mover : ScriptComponentBehavior
	{
		// Token: 0x06002E23 RID: 11811 RVA: 0x000B2394 File Offset: 0x000B0594
		protected internal override void OnEditorTick(float dt)
		{
			if (!base.GameEntity.EntityFlags.HasAnyFlag(EntityFlags.IsHelper))
			{
				if (this._moverGhost == null && this._pathname != "")
				{
					this.CreateOrUpdateMoverGhost();
				}
				if (this._tracker != null && this._tracker.IsValid)
				{
					if (this._moveGhost)
					{
						this._tracker.Advance(this._speed * dt);
						if (this._tracker.TotalDistanceTraveled >= this._tracker.GetPathLength())
						{
							this._tracker.Reset();
						}
					}
					else
					{
						this._tracker.Advance(0f);
					}
					MatrixFrame currentFrame = this._tracker.CurrentFrame;
					this._moverGhost.SetFrame(ref currentFrame, true);
				}
			}
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x000B2464 File Offset: 0x000B0664
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			if (variableName == "_pathname")
			{
				this.CreateOrUpdateMoverGhost();
				return;
			}
			if (variableName == "_moveGhost")
			{
				if (!this._moveGhost)
				{
					this._moverGhost.SetVisibilityExcludeParents(false);
					return;
				}
				this._moverGhost.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x000B24B4 File Offset: 0x000B06B4
		private void CreateOrUpdateMoverGhost()
		{
			Path pathWithName = base.GameEntity.Scene.GetPathWithName(this._pathname);
			if (pathWithName != null)
			{
				this._tracker = new PathTracker(pathWithName, Vec3.One);
				this._tracker.Reset();
				base.GameEntity.SetLocalPosition(this._tracker.CurrentFrame.origin);
				if (this._moverGhost == null)
				{
					this._moverGhost = TaleWorlds.Engine.GameEntity.CopyFrom(base.GameEntity.Scene, base.GameEntity, true, true);
					this._moverGhost.EntityFlags |= EntityFlags.IsHelper | EntityFlags.DontSaveToScene | EntityFlags.DoNotTick;
					this._moverGhost.SetAlpha(0.2f);
					return;
				}
				this._moverGhost.SetLocalPosition(this._tracker.CurrentFrame.origin);
			}
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000B2594 File Offset: 0x000B0794
		protected internal override void OnInit()
		{
			base.OnInit();
			Path pathWithName = base.GameEntity.Scene.GetPathWithName(this._pathname);
			if (pathWithName != null)
			{
				this._tracker = new PathTracker(pathWithName, Vec3.One);
				this._tracker.Reset();
				base.GameEntity.SetLocalPosition(this._tracker.CurrentFrame.origin);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000B2610 File Offset: 0x000B0810
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000B263C File Offset: 0x000B083C
		protected internal override void OnTick(float dt)
		{
			if (Mission.Current.Mode == MissionMode.Battle && this._tracker != null && this._tracker.IsValid && this._tracker.TotalDistanceTraveled < this._tracker.GetPathLength())
			{
				this._tracker.Advance(this._speed * dt);
				MatrixFrame currentFrame = this._tracker.CurrentFrame;
				base.GameEntity.SetFrame(ref currentFrame, true);
			}
		}

		// Token: 0x04001251 RID: 4689
		[EditorVisibleScriptComponentVariable(true)]
		private string _pathname = "";

		// Token: 0x04001252 RID: 4690
		[EditorVisibleScriptComponentVariable(true)]
		private float _speed;

		// Token: 0x04001253 RID: 4691
		[EditorVisibleScriptComponentVariable(true)]
		private bool _moveGhost;

		// Token: 0x04001254 RID: 4692
		private GameEntity _moverGhost;

		// Token: 0x04001255 RID: 4693
		private PathTracker _tracker;
	}
}
