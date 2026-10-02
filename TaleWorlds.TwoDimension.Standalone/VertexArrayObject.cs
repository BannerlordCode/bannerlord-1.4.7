using System;
using TaleWorlds.TwoDimension.Standalone.Native;
using TaleWorlds.TwoDimension.Standalone.Native.OpenGL;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000010 RID: 16
	public class VertexArrayObject
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x00005203 File Offset: 0x00003403
		private VertexArrayObject(uint vertexArrayObject)
		{
			this._vertexArrayObject = vertexArrayObject;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005212 File Offset: 0x00003412
		public void LoadVertexData(float[] vertices)
		{
			this.LoadDataToBuffer(this._vertexBuffer, vertices);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005221 File Offset: 0x00003421
		public void LoadUVData(float[] uvs)
		{
			this.LoadDataToBuffer(this._uvBuffer, uvs);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005230 File Offset: 0x00003430
		public void LoadIndexData(uint[] indices)
		{
			this.LoadDataToIndexBuffer(this._indexBuffer, indices);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005240 File Offset: 0x00003440
		private void LoadDataToBuffer(uint buffer, float[] data)
		{
			this.Bind();
			using (AutoPinner autoPinner = new AutoPinner(data))
			{
				IntPtr intPtr = autoPinner;
				Opengl32ARB.BindBuffer(BufferBindingTarget.ArrayBuffer, buffer);
				Opengl32ARB.BufferSubData(BufferBindingTarget.ArrayBuffer, 0, data.Length * 4, intPtr);
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000052A4 File Offset: 0x000034A4
		private void LoadDataToIndexBuffer(uint buffer, uint[] data)
		{
			using (AutoPinner autoPinner = new AutoPinner(data))
			{
				IntPtr intPtr = autoPinner;
				Opengl32ARB.BindBuffer(BufferBindingTarget.ElementArrayBuffer, buffer);
				Opengl32ARB.BufferSubData(BufferBindingTarget.ElementArrayBuffer, 0, data.Length * 4, intPtr);
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005304 File Offset: 0x00003504
		public void Bind()
		{
			Opengl32ARB.BindVertexArray(this._vertexArrayObject);
			Opengl32ARB.BindBuffer(BufferBindingTarget.ElementArrayBuffer, this._indexBuffer);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000532B File Offset: 0x0000352B
		public static void UnBind()
		{
			Opengl32ARB.BindVertexArray(0U);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005338 File Offset: 0x00003538
		private static uint CreateArrayBuffer()
		{
			uint[] array = new uint[1];
			Opengl32ARB.GenBuffers(1, array);
			uint num = array[0];
			Opengl32ARB.BindBuffer(BufferBindingTarget.ArrayBuffer, num);
			Opengl32ARB.BufferData(BufferBindingTarget.ArrayBuffer, 524288, IntPtr.Zero, 35048);
			return num;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000538C File Offset: 0x0000358C
		private static uint CreateElementArrayBuffer()
		{
			uint[] array = new uint[1];
			Opengl32ARB.GenBuffers(1, array);
			uint num = array[0];
			Opengl32ARB.BindBuffer(BufferBindingTarget.ElementArrayBuffer, num);
			Opengl32ARB.BufferData(BufferBindingTarget.ElementArrayBuffer, 524288, IntPtr.Zero, 35048);
			return num;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000053E0 File Offset: 0x000035E0
		public static VertexArrayObject Create()
		{
			VertexArrayObject vertexArrayObject = new VertexArrayObject(VertexArrayObject.CreateVertexArray());
			uint num = VertexArrayObject.CreateArrayBuffer();
			VertexArrayObject.BindBuffer(0U, num);
			uint num2 = VertexArrayObject.CreateElementArrayBuffer();
			VertexArrayObject.BindIndexBuffer(num2);
			vertexArrayObject._vertexBuffer = num;
			vertexArrayObject._indexBuffer = num2;
			return vertexArrayObject;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005420 File Offset: 0x00003620
		public static VertexArrayObject CreateWithUVBuffer()
		{
			VertexArrayObject vertexArrayObject = new VertexArrayObject(VertexArrayObject.CreateVertexArray());
			uint num = VertexArrayObject.CreateArrayBuffer();
			uint num2 = VertexArrayObject.CreateArrayBuffer();
			VertexArrayObject.BindBuffer(0U, num);
			VertexArrayObject.BindBuffer(1U, num2);
			uint num3 = VertexArrayObject.CreateElementArrayBuffer();
			VertexArrayObject.BindIndexBuffer(num3);
			vertexArrayObject._vertexBuffer = num;
			vertexArrayObject._uvBuffer = num2;
			vertexArrayObject._indexBuffer = num3;
			return vertexArrayObject;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005472 File Offset: 0x00003672
		private static void BindBuffer(uint index, uint buffer)
		{
			Opengl32ARB.EnableVertexAttribArray(index);
			Opengl32ARB.BindBuffer(BufferBindingTarget.ArrayBuffer, buffer);
			Opengl32ARB.VertexAttribPointer(index, 2, DataType.Float, 0, 0, IntPtr.Zero);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000054A7 File Offset: 0x000036A7
		private static void BindIndexBuffer(uint buffer)
		{
			Opengl32ARB.BindBuffer(BufferBindingTarget.ElementArrayBuffer, buffer);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000054BC File Offset: 0x000036BC
		private static uint CreateVertexArray()
		{
			uint[] array = new uint[1];
			Opengl32ARB.GenVertexArrays(1, array);
			uint num = array[0];
			Opengl32ARB.BindVertexArray(num);
			return num;
		}

		// Token: 0x04000047 RID: 71
		private uint _vertexArrayObject;

		// Token: 0x04000048 RID: 72
		private uint _vertexBuffer;

		// Token: 0x04000049 RID: 73
		private uint _uvBuffer;

		// Token: 0x0400004A RID: 74
		private uint _indexBuffer;
	}
}
