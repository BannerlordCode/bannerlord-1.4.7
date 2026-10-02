using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000240 RID: 576
	public class InitialStateOption
	{
		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002137 RID: 8503 RVA: 0x00074BA2 File Offset: 0x00072DA2
		// (set) Token: 0x06002138 RID: 8504 RVA: 0x00074BAA File Offset: 0x00072DAA
		public int OrderIndex { get; private set; }

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002139 RID: 8505 RVA: 0x00074BB3 File Offset: 0x00072DB3
		// (set) Token: 0x0600213A RID: 8506 RVA: 0x00074BBB File Offset: 0x00072DBB
		public TextObject Name { get; private set; }

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x0600213B RID: 8507 RVA: 0x00074BC4 File Offset: 0x00072DC4
		// (set) Token: 0x0600213C RID: 8508 RVA: 0x00074BCC File Offset: 0x00072DCC
		public string Id { get; private set; }

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600213D RID: 8509 RVA: 0x00074BD5 File Offset: 0x00072DD5
		// (set) Token: 0x0600213E RID: 8510 RVA: 0x00074BDD File Offset: 0x00072DDD
		public Func<bool> IsHidden { get; private set; }

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x0600213F RID: 8511 RVA: 0x00074BE6 File Offset: 0x00072DE6
		// (set) Token: 0x06002140 RID: 8512 RVA: 0x00074BEE File Offset: 0x00072DEE
		public Func<ValueTuple<bool, TextObject>> IsDisabledAndReason { get; private set; }

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002141 RID: 8513 RVA: 0x00074BF7 File Offset: 0x00072DF7
		// (set) Token: 0x06002142 RID: 8514 RVA: 0x00074BFF File Offset: 0x00072DFF
		public TextObject EnabledHint { get; private set; }

		// Token: 0x06002143 RID: 8515 RVA: 0x00074C08 File Offset: 0x00072E08
		public InitialStateOption(string id, TextObject name, int orderIndex, Action action, Func<ValueTuple<bool, TextObject>> isDisabledAndReason, TextObject enabledHint = null, Func<bool> isHidden = null)
		{
			this.Name = name;
			this.Id = id;
			this.OrderIndex = orderIndex;
			this._action = action;
			this.IsHidden = isHidden;
			this.IsDisabledAndReason = isDisabledAndReason;
			this.EnabledHint = enabledHint;
			TextObject item = this.IsDisabledAndReason().Item2;
			string.IsNullOrEmpty((item != null) ? item.ToString() : null);
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x00074C72 File Offset: 0x00072E72
		public void DoAction()
		{
			Action action = this._action;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04000CC1 RID: 3265
		private Action _action;
	}
}
