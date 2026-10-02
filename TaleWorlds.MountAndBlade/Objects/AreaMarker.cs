using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x0200039E RID: 926
	public class AreaMarker : MissionObject, ITrackableBase
	{
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060034CA RID: 13514 RVA: 0x000D9579 File Offset: 0x000D7779
		public virtual string Tag
		{
			get
			{
				return "area_marker_" + this.AreaIndex;
			}
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x000D9590 File Offset: 0x000D7790
		protected internal override void OnEditorTick(float dt)
		{
			if (this.CheckToggle)
			{
				MBEditor.HelpersEnabled();
			}
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x000D95A0 File Offset: 0x000D77A0
		protected internal override void OnEditorInit()
		{
			this.CheckToggle = false;
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x000D95AC File Offset: 0x000D77AC
		public bool IsPositionInRange(Vec3 position)
		{
			return position.DistanceSquared(base.GameEntity.GlobalPosition) <= this.AreaRadius * this.AreaRadius;
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x000D95E0 File Offset: 0x000D77E0
		public virtual List<UsableMachine> GetUsableMachinesInRange(string excludeTag = null)
		{
			float distanceSquared = this.AreaRadius * this.AreaRadius;
			return (from x in Mission.Current.ActiveMissionObjects.FindAllWithType<UsableMachine>()
				where !x.IsDeactivated && x.GameEntity.GlobalPosition.DistanceSquared(this.GameEntity.GlobalPosition) <= distanceSquared && !x.GameEntity.HasTag(excludeTag)
				select x).ToList<UsableMachine>();
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x000D963C File Offset: 0x000D783C
		public virtual List<UsableMachine> GetUsableMachinesWithTagInRange(string tag)
		{
			float distanceSquared = this.AreaRadius * this.AreaRadius;
			return (from x in Mission.Current.ActiveMissionObjects.FindAllWithType<UsableMachine>()
				where x.GameEntity.GlobalPosition.DistanceSquared(this.GameEntity.GlobalPosition) <= distanceSquared && x.GameEntity.HasTag(tag)
				select x).ToList<UsableMachine>();
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x000D9698 File Offset: 0x000D7898
		public virtual List<GameEntity> GetGameEntitiesWithTagInRange(string tag)
		{
			float distanceSquared = this.AreaRadius * this.AreaRadius;
			return (from x in Mission.Current.Scene.FindEntitiesWithTag(tag)
				where x.GlobalPosition.DistanceSquared(this.GameEntity.GlobalPosition) <= distanceSquared && x.HasTag(tag)
				select x).ToList<GameEntity>();
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x000D96F8 File Offset: 0x000D78F8
		public virtual TextObject GetName()
		{
			return new TextObject(base.GameEntity.Name, null);
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x000D971C File Offset: 0x000D791C
		public virtual Vec3 GetPosition()
		{
			return base.GameEntity.GlobalPosition;
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x000D9737 File Offset: 0x000D7937
		TextObject ITrackableBase.GetName()
		{
			return this.GetName();
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x000D973F File Offset: 0x000D793F
		Vec3 ITrackableBase.GetPosition()
		{
			return this.GetPosition();
		}

		// Token: 0x04001668 RID: 5736
		public float AreaRadius = 3f;

		// Token: 0x04001669 RID: 5737
		public int AreaIndex;

		// Token: 0x0400166A RID: 5738
		public bool CheckToggle;
	}
}
