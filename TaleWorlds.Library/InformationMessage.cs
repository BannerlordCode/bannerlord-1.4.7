using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200003C RID: 60
	public class InformationMessage
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001EF RID: 495 RVA: 0x000076A2 File Offset: 0x000058A2
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x000076AA File Offset: 0x000058AA
		public string Information { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x000076B3 File Offset: 0x000058B3
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x000076BB File Offset: 0x000058BB
		public string Detail { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x000076C4 File Offset: 0x000058C4
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x000076CC File Offset: 0x000058CC
		public Color Color { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x000076D5 File Offset: 0x000058D5
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x000076DD File Offset: 0x000058DD
		public string SoundEventPath { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x000076E6 File Offset: 0x000058E6
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x000076EE File Offset: 0x000058EE
		public string Category { get; set; }

		// Token: 0x060001F9 RID: 505 RVA: 0x000076F7 File Offset: 0x000058F7
		public InformationMessage(string information)
		{
			this.Information = information;
			this.Color = Color.White;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00007711 File Offset: 0x00005911
		public InformationMessage(string information, Color color)
		{
			this.Information = information;
			this.Color = color;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00007727 File Offset: 0x00005927
		public InformationMessage(string information, Color color, string category)
		{
			this.Information = information;
			this.Color = color;
			this.Category = category;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00007744 File Offset: 0x00005944
		public InformationMessage(string information, string soundEventPath)
		{
			this.Information = information;
			this.SoundEventPath = soundEventPath;
			this.Color = Color.White;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00007765 File Offset: 0x00005965
		public InformationMessage()
		{
			this.Information = "";
		}
	}
}
