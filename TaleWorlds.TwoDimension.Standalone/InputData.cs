using System;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000008 RID: 8
	public class InputData
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003FA7 File Offset: 0x000021A7
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00003FAF File Offset: 0x000021AF
		public bool[] KeyData { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003FB8 File Offset: 0x000021B8
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00003FC0 File Offset: 0x000021C0
		public bool LeftMouse { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003FC9 File Offset: 0x000021C9
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00003FD1 File Offset: 0x000021D1
		public bool RightMouse { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003FDA File Offset: 0x000021DA
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00003FE2 File Offset: 0x000021E2
		public int CursorX { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00003FEB File Offset: 0x000021EB
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00003FF3 File Offset: 0x000021F3
		public int CursorY { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00003FFC File Offset: 0x000021FC
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00004004 File Offset: 0x00002204
		public bool MouseMove { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000400D File Offset: 0x0000220D
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00004015 File Offset: 0x00002215
		public float MouseScrollDelta { get; set; }

		// Token: 0x06000061 RID: 97 RVA: 0x00004020 File Offset: 0x00002220
		public InputData()
		{
			this.KeyData = new bool[256];
			this.CursorX = 0;
			this.CursorY = 0;
			this.LeftMouse = false;
			this.RightMouse = false;
			this.MouseMove = false;
			this.MouseScrollDelta = 0f;
			for (int i = 0; i < 256; i++)
			{
				this.KeyData[i] = false;
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000408A File Offset: 0x0000228A
		public void Reset()
		{
			this.MouseScrollDelta = 0f;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004098 File Offset: 0x00002298
		public void FillFrom(InputData inputData)
		{
			this.CursorX = inputData.CursorX;
			this.CursorY = inputData.CursorY;
			this.LeftMouse = inputData.LeftMouse;
			this.RightMouse = inputData.RightMouse;
			this.MouseMove = inputData.MouseMove;
			this.MouseScrollDelta = inputData.MouseScrollDelta;
			for (int i = 0; i < 256; i++)
			{
				this.KeyData[i] = inputData.KeyData[i];
			}
		}
	}
}
