using System;
using System.Numerics;
using System.Text;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native.OpenGL;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x0200000C RID: 12
	public class Shader
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00004B27 File Offset: 0x00002D27
		private Shader(GraphicsContext graphicsContext, int program)
		{
			this._graphicsContext = graphicsContext;
			this._program = program;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004B40 File Offset: 0x00002D40
		public static Shader CreateShader(GraphicsContext graphicsContext, string vertexShaderCode, string fragmentShaderCode)
		{
			int num = Shader.CompileShaders(vertexShaderCode, fragmentShaderCode);
			if (num < 0)
			{
				return null;
			}
			return new Shader(graphicsContext, num);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004B64 File Offset: 0x00002D64
		public static int CompileShaders(string vertexShaderCode, string fragmentShaderCode)
		{
			bool flag = false;
			int num = Opengl32ARB.CreateShaderObject(ShaderType.VertexShader);
			Opengl32ARB.ShaderSource(num, vertexShaderCode);
			Opengl32ARB.CompileShader(num);
			int num2 = -1;
			Opengl32ARB.GetShaderiv(num, 35713, out num2);
			if (num2 != 1)
			{
				int num3 = -1;
				Opengl32ARB.GetShaderiv(num, 35716, out num3);
				int num4 = -1;
				byte[] array = new byte[4096];
				Opengl32ARB.GetShaderInfoLog(num, 4096, out num4, array);
				Encoding.ASCII.GetString(array);
				flag = true;
			}
			int num5 = Opengl32ARB.CreateShaderObject(ShaderType.FragmentShader);
			Opengl32ARB.ShaderSource(num5, fragmentShaderCode);
			Opengl32ARB.CompileShader(num5);
			Opengl32ARB.GetShaderiv(num5, 35713, out num2);
			if (num2 != 1)
			{
				int num6 = -1;
				Opengl32ARB.GetShaderiv(num5, 35716, out num6);
				int num7 = -1;
				byte[] array2 = new byte[4096];
				Opengl32ARB.GetShaderInfoLog(num5, 4096, out num7, array2);
				Encoding.ASCII.GetString(array2);
				flag = true;
			}
			int num8 = Opengl32ARB.CreateProgramObject();
			Opengl32ARB.AttachShader(num8, num);
			Opengl32ARB.AttachShader(num8, num5);
			Opengl32ARB.LinkProgram(num8);
			Opengl32ARB.GetProgramiv(num8, 35714, out num2);
			if (num2 != 1)
			{
				int num9 = -1;
				Opengl32ARB.GetProgramiv(num8, 35716, out num9);
				int num10 = -1;
				byte[] array3 = new byte[4096];
				Opengl32ARB.GetProgramInfoLog(num8, 4096, out num10, array3);
				Encoding.ASCII.GetString(array3);
				flag = true;
			}
			Opengl32ARB.DetachShader(num8, num);
			Opengl32ARB.DetachShader(num8, num5);
			Opengl32ARB.DeleteShader(num);
			Opengl32ARB.DeleteShader(num5);
			if (flag)
			{
				Opengl32ARB.DeleteProgram(num8);
				return -1;
			}
			return num8;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004D4C File Offset: 0x00002F4C
		public void SetTexture(string name, OpenGLTexture texture)
		{
			if (this._currentTextureUnit == 0)
			{
				Opengl32ARB.ActiveTexture(TextureUnit.Texture0);
			}
			else if (this._currentTextureUnit == 1)
			{
				Opengl32ARB.ActiveTexture(TextureUnit.Texture1);
			}
			Opengl32.BindTexture(Target.Texture2D, (texture != null) ? texture.Id : (-1));
			int uniformLocation = Opengl32ARB.GetUniformLocation(this._program, name);
			Opengl32ARB.Uniform1i(uniformLocation, this._currentTextureUnit);
			this._currentTextureUnit++;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004DCC File Offset: 0x00002FCC
		public void SetColor(string name, Color color)
		{
			int uniformLocation = Opengl32ARB.GetUniformLocation(this._program, name);
			Opengl32ARB.Uniform4f(uniformLocation, color.Red, color.Green, color.Blue, color.Alpha);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004E09 File Offset: 0x00003009
		public void Use()
		{
			Opengl32ARB.UseProgram(this._program);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004E1B File Offset: 0x0000301B
		public void StopUsing()
		{
			this._currentTextureUnit = 0;
			Opengl32ARB.UseProgram(0);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004E2F File Offset: 0x0000302F
		public void SetMatrix(string name, in Matrix4x4 matrix)
		{
			Opengl32ARB.UniformMatrix4fv(Opengl32ARB.GetUniformLocation(this._program, name), 1, false, in matrix);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004E48 File Offset: 0x00003048
		public void SetBoolean(string name, bool value)
		{
			int uniformLocation = Opengl32ARB.GetUniformLocation(this._program, name);
			Opengl32ARB.Uniform1i(uniformLocation, value ? 1 : 0);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004E74 File Offset: 0x00003074
		public void SetFloat(string name, float value)
		{
			int uniformLocation = Opengl32ARB.GetUniformLocation(this._program, name);
			Opengl32ARB.Uniform1f(uniformLocation, value);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004E9C File Offset: 0x0000309C
		public void SetVector2(string name, Vector2 value)
		{
			int uniformLocation = Opengl32ARB.GetUniformLocation(this._program, name);
			Opengl32ARB.Uniform2f(uniformLocation, value.X, value.Y);
		}

		// Token: 0x04000040 RID: 64
		private GraphicsContext _graphicsContext;

		// Token: 0x04000041 RID: 65
		private int _program;

		// Token: 0x04000042 RID: 66
		private int _currentTextureUnit;
	}
}
