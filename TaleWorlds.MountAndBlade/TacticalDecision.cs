using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000182 RID: 386
	public struct TacticalDecision
	{
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x0004C9EB File Offset: 0x0004ABEB
		// (set) Token: 0x060014A8 RID: 5288 RVA: 0x0004C9F3 File Offset: 0x0004ABF3
		public TacticComponent DecidingComponent { get; private set; }

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060014A9 RID: 5289 RVA: 0x0004C9FC File Offset: 0x0004ABFC
		// (set) Token: 0x060014AA RID: 5290 RVA: 0x0004CA04 File Offset: 0x0004AC04
		public byte DecisionCode { get; private set; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060014AB RID: 5291 RVA: 0x0004CA0D File Offset: 0x0004AC0D
		// (set) Token: 0x060014AC RID: 5292 RVA: 0x0004CA15 File Offset: 0x0004AC15
		public Formation SubjectFormation { get; private set; }

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060014AD RID: 5293 RVA: 0x0004CA1E File Offset: 0x0004AC1E
		// (set) Token: 0x060014AE RID: 5294 RVA: 0x0004CA26 File Offset: 0x0004AC26
		public Formation TargetFormation { get; private set; }

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x0004CA2F File Offset: 0x0004AC2F
		// (set) Token: 0x060014B0 RID: 5296 RVA: 0x0004CA37 File Offset: 0x0004AC37
		public WorldPosition? TargetPosition { get; private set; }

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0004CA40 File Offset: 0x0004AC40
		// (set) Token: 0x060014B2 RID: 5298 RVA: 0x0004CA48 File Offset: 0x0004AC48
		public MissionObject TargetObject { get; private set; }

		// Token: 0x060014B3 RID: 5299 RVA: 0x0004CA51 File Offset: 0x0004AC51
		public TacticalDecision(TacticComponent decidingComponent, byte decisionCode, Formation subjectFormation = null, Formation targetFormation = null, WorldPosition? targetPosition = null, MissionObject targetObject = null)
		{
			this.DecidingComponent = decidingComponent;
			this.DecisionCode = decisionCode;
			this.SubjectFormation = subjectFormation;
			this.TargetFormation = targetFormation;
			this.TargetPosition = targetPosition;
			this.TargetObject = targetObject;
		}
	}
}
