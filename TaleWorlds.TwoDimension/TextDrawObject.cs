using System;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200001D RID: 29
	public struct TextDrawObject : IDrawObject
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00006F07 File Offset: 0x00005107
		public static TextDrawObject Invalid
		{
			get
			{
				return TextDrawObject.CreateInvalid();
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00006F0E File Offset: 0x0000510E
		bool IDrawObject.IsValid
		{
			get
			{
				return this.IsValid;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00006F16 File Offset: 0x00005116
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00006F1E File Offset: 0x0000511E
		Rectangle2D IDrawObject.Rectangle
		{
			get
			{
				return this.Rectangle;
			}
			set
			{
				this.Rectangle = value;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006F28 File Offset: 0x00005128
		private static TextDrawObject CreateInvalid()
		{
			return new TextDrawObject
			{
				IsValid = false
			};
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006F48 File Offset: 0x00005148
		public static TextDrawObject Create(float[] vertices, float[] uvs, uint[] indices, float text_MeshWidth, float text_MeshHeight, in Rectangle2D rectangle)
		{
			return new TextDrawObject
			{
				IsValid = true,
				Text_Vertices = vertices,
				Text_TextureCoordinates = uvs,
				Text_Indices = indices,
				Text_MeshWidth = text_MeshWidth,
				Text_MeshHeight = text_MeshHeight,
				Rectangle = rectangle
			};
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006FA0 File Offset: 0x000051A0
		public void ConvertToHashInPlace()
		{
			ulong num = 5381UL;
			ulong num2 = 5381UL;
			for (int i = 0; i < this.Text_Vertices.Length; i++)
			{
				num = (num << 5) + num + (ulong)(this.Text_Vertices[i] * 10000f);
			}
			for (int j = 0; j < this.Text_Indices.Length; j++)
			{
				num2 = (num2 << 5) + num2 + (ulong)this.Text_Indices[j];
			}
			for (int k = 0; k < this.Text_TextureCoordinates.Length; k++)
			{
				num = (num << 5) + num + (ulong)(this.Text_TextureCoordinates[k] * 1000f);
			}
			num2 = (num2 << 5) + num2 + (ulong)this.Text_MeshWidth;
			num = (num << 5) + num + (ulong)this.Text_MeshHeight;
			this.HashCode1 = num;
			this.HashCode2 = num2;
		}

		// Token: 0x040000A9 RID: 169
		public bool IsValid;

		// Token: 0x040000AA RID: 170
		public float[] Text_Vertices;

		// Token: 0x040000AB RID: 171
		public float[] Text_TextureCoordinates;

		// Token: 0x040000AC RID: 172
		public uint[] Text_Indices;

		// Token: 0x040000AD RID: 173
		public float Text_MeshWidth;

		// Token: 0x040000AE RID: 174
		public float Text_MeshHeight;

		// Token: 0x040000AF RID: 175
		public ulong HashCode1;

		// Token: 0x040000B0 RID: 176
		public ulong HashCode2;

		// Token: 0x040000B1 RID: 177
		public Rectangle2D Rectangle;
	}
}
