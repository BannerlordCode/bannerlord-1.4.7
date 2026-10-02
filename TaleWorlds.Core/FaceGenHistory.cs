using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x0200005C RID: 92
	public class FaceGenHistory
	{
		// Token: 0x06000730 RID: 1840 RVA: 0x00018DF4 File Offset: 0x00016FF4
		public FaceGenHistory(List<UndoRedoKey> undoCommands, List<UndoRedoKey> redoCommands, Dictionary<string, float> initialValues)
		{
			this.UndoCommands = undoCommands;
			this.RedoCommands = redoCommands;
			this.InitialValues = initialValues;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00018E11 File Offset: 0x00017011
		public void ClearHistory()
		{
			this.UndoCommands.Clear();
			this.RedoCommands.Clear();
			this.InitialValues.Clear();
		}

		// Token: 0x04000399 RID: 921
		public readonly List<UndoRedoKey> UndoCommands;

		// Token: 0x0400039A RID: 922
		public readonly List<UndoRedoKey> RedoCommands;

		// Token: 0x0400039B RID: 923
		public readonly Dictionary<string, float> InitialValues;
	}
}
