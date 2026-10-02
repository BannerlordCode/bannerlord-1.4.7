using System;
using System.Linq;
using SandBox.AI;
using SandBox.Objects.AnimationPoints;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects.Usables
{
	// Token: 0x02000049 RID: 73
	public class Chair : UsableMachine
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x0000F9FC File Offset: 0x0000DBFC
		protected override void OnInit()
		{
			base.OnInit();
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				standingPoint.AutoSheathWeapons = true;
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000FA54 File Offset: 0x0000DC54
		public bool IsAgentFullySitting(Agent usingAgent)
		{
			return base.StandingPoints.Count > 0 && base.StandingPoints.Contains(usingAgent.CurrentlyUsedGameObject) && usingAgent.IsSitting();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000FA7F File Offset: 0x0000DC7F
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new UsablePlaceAI(this);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000FA88 File Offset: 0x0000DC88
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = new TextObject(this.IsAgentFullySitting(Agent.Main) ? "{=QGdaakYW}{KEY} Get Up" : "{=bl2aRW8f}{KEY} Sit", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000FAD8 File Offset: 0x0000DCD8
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			switch (this.ChairType)
			{
			case Chair.SittableType.Log:
				return new TextObject("{=9pgOGq7X}Log", null);
			case Chair.SittableType.Sofa:
				return new TextObject("{=GvLZKQ1U}Sofa", null);
			case Chair.SittableType.Ground:
				return new TextObject("{=L7ZQtIuM}Ground", null);
			default:
				return new TextObject("{=OgTUrRlR}Chair", null);
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000FB34 File Offset: 0x0000DD34
		public override StandingPoint GetBestPointAlternativeTo(StandingPoint standingPoint, Agent agent)
		{
			AnimationPoint animationPoint = standingPoint as AnimationPoint;
			if (animationPoint == null || animationPoint.GroupId < 0)
			{
				return animationPoint;
			}
			WorldFrame worldFrame = standingPoint.GetUserFrameForAgent(agent);
			float num = worldFrame.Origin.GetGroundVec3().DistanceSquared(agent.Position);
			foreach (StandingPoint standingPoint2 in base.StandingPoints)
			{
				AnimationPoint animationPoint2;
				if ((animationPoint2 = standingPoint2 as AnimationPoint) != null && standingPoint != standingPoint2 && animationPoint.GroupId == animationPoint2.GroupId && !animationPoint2.IsDisabledForAgent(agent))
				{
					worldFrame = animationPoint2.GetUserFrameForAgent(agent);
					float num2 = worldFrame.Origin.GetGroundVec3().DistanceSquared(agent.Position);
					if (num2 < num)
					{
						num = num2;
						animationPoint = animationPoint2;
					}
				}
			}
			return animationPoint;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000FC18 File Offset: 0x0000DE18
		public override OrderType GetOrder(BattleSideEnum side)
		{
			return OrderType.None;
		}

		// Token: 0x0400012D RID: 301
		public Chair.SittableType ChairType;

		// Token: 0x0200014F RID: 335
		public enum SittableType
		{
			// Token: 0x040006A6 RID: 1702
			Chair,
			// Token: 0x040006A7 RID: 1703
			Log,
			// Token: 0x040006A8 RID: 1704
			Sofa,
			// Token: 0x040006A9 RID: 1705
			Ground
		}
	}
}
