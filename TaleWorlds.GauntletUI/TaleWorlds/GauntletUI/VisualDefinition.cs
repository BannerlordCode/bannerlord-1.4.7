using System;
using System.Collections.Generic;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000036 RID: 54
	public class VisualDefinition
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060003AD RID: 941 RVA: 0x0000FA1E File Offset: 0x0000DC1E
		// (set) Token: 0x060003AE RID: 942 RVA: 0x0000FA26 File Offset: 0x0000DC26
		public string Name { get; private set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000FA2F File Offset: 0x0000DC2F
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x0000FA37 File Offset: 0x0000DC37
		public float TransitionDuration { get; private set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0000FA40 File Offset: 0x0000DC40
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x0000FA48 File Offset: 0x0000DC48
		public float DelayOnBegin { get; private set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0000FA51 File Offset: 0x0000DC51
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x0000FA59 File Offset: 0x0000DC59
		public AnimationInterpolation.Type EaseType { get; private set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000FA62 File Offset: 0x0000DC62
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x0000FA6A File Offset: 0x0000DC6A
		public AnimationInterpolation.Function EaseFunction { get; private set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000FA73 File Offset: 0x0000DC73
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x0000FA7B File Offset: 0x0000DC7B
		public Dictionary<string, VisualState> VisualStates { get; private set; }

		// Token: 0x060003B9 RID: 953 RVA: 0x0000FA84 File Offset: 0x0000DC84
		public VisualDefinition(string name, float transitionDuration, float delayOnBegin, AnimationInterpolation.Type easeType, AnimationInterpolation.Function easeFunction)
		{
			this.Name = name;
			this.TransitionDuration = transitionDuration;
			this.DelayOnBegin = delayOnBegin;
			this.EaseType = easeType;
			this.EaseFunction = easeFunction;
			this.VisualStates = new Dictionary<string, VisualState>();
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000FABC File Offset: 0x0000DCBC
		public void AddVisualState(VisualState visualState)
		{
			this.VisualStates.Add(visualState.State, visualState);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000FAD0 File Offset: 0x0000DCD0
		public VisualState GetVisualState(string state)
		{
			if (this.VisualStates.ContainsKey(state))
			{
				return this.VisualStates[state];
			}
			return null;
		}
	}
}
