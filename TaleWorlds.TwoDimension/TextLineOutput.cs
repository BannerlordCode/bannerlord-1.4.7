using System;
using System.Collections.Generic;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000014 RID: 20
	internal class TextLineOutput
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000CB RID: 203 RVA: 0x000060BD File Offset: 0x000042BD
		// (set) Token: 0x060000CC RID: 204 RVA: 0x000060C5 File Offset: 0x000042C5
		public float Width { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000060CE File Offset: 0x000042CE
		// (set) Token: 0x060000CE RID: 206 RVA: 0x000060D6 File Offset: 0x000042D6
		public float TextWidth { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000060DF File Offset: 0x000042DF
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x000060E7 File Offset: 0x000042E7
		public bool LineEnded { get; internal set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x000060F0 File Offset: 0x000042F0
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x000060F8 File Offset: 0x000042F8
		public int EmptyCharacterCount { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00006101 File Offset: 0x00004301
		public int TokenCount
		{
			get
			{
				return this._tokens.Count;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x0000610E File Offset: 0x0000430E
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00006116 File Offset: 0x00004316
		public float Height { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x0000611F File Offset: 0x0000431F
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00006127 File Offset: 0x00004327
		public float MaxScale { get; private set; }

		// Token: 0x060000D8 RID: 216 RVA: 0x00006130 File Offset: 0x00004330
		public TextLineOutput(float lineHeight)
		{
			this._tokens = new List<TextTokenOutput>();
			this.Height = lineHeight;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000614C File Offset: 0x0000434C
		public void AddToken(TextToken textToken, float tokenWidth, float tokenHeight, string style, float scaleValue)
		{
			if (textToken.Type == TextToken.TokenType.EmptyCharacter)
			{
				int emptyCharacterCount = this.EmptyCharacterCount;
				this.EmptyCharacterCount = emptyCharacterCount + 1;
			}
			else
			{
				this.TextWidth += tokenWidth;
			}
			TextTokenOutput textTokenOutput;
			if (tokenHeight > 0f)
			{
				textTokenOutput = new TextTokenOutput(textToken, tokenWidth, tokenHeight, style, scaleValue);
			}
			else
			{
				textTokenOutput = new TextTokenOutput(textToken, tokenWidth, this.Height, style, scaleValue);
			}
			this._tokens.Add(textTokenOutput);
			this.Width += tokenWidth;
			if (tokenHeight > this.Height)
			{
				this.Height = tokenHeight;
			}
			if (scaleValue > this.MaxScale)
			{
				this.MaxScale = scaleValue;
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000061E6 File Offset: 0x000043E6
		public TextToken GetToken(int i)
		{
			return this._tokens[i].Token;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000061F9 File Offset: 0x000043F9
		public TextTokenOutput GetTokenOutput(int i)
		{
			return this._tokens[i];
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006208 File Offset: 0x00004408
		public TextTokenOutput RemoveTokenFromEnd()
		{
			TextTokenOutput textTokenOutput = this._tokens[this._tokens.Count - 1];
			this._tokens.Remove(textTokenOutput);
			this.Width -= textTokenOutput.Width;
			return textTokenOutput;
		}

		// Token: 0x04000083 RID: 131
		private List<TextTokenOutput> _tokens;
	}
}
