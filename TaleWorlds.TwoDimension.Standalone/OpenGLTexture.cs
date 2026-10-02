using System;
using System.IO;
using StbSharp;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native.OpenGL;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x0200000B RID: 11
	public class OpenGLTexture : ITexture
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000047E4 File Offset: 0x000029E4
		public bool IsValid
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000047E7 File Offset: 0x000029E7
		public int Width
		{
			get
			{
				return this._width;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000047EF File Offset: 0x000029EF
		public int Height
		{
			get
			{
				return this._height;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000047F7 File Offset: 0x000029F7
		// (set) Token: 0x06000072 RID: 114 RVA: 0x000047FF File Offset: 0x000029FF
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00004808 File Offset: 0x00002A08
		// (set) Token: 0x06000074 RID: 116 RVA: 0x0000480F File Offset: 0x00002A0F
		internal static OpenGLTexture ActiveTexture { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00004817 File Offset: 0x00002A17
		internal int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000076 RID: 118 RVA: 0x0000481F File Offset: 0x00002A1F
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00004827 File Offset: 0x00002A27
		public bool ClampToEdge
		{
			get
			{
				return this._clampToEdge;
			}
			set
			{
				this._clampToEdge = value;
				if (OpenGLTexture.ActiveTexture != this)
				{
					this.MakeActive();
					return;
				}
				this.SetTextureParameters();
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000484D File Offset: 0x00002A4D
		public void Initialize(string name, int width, int height)
		{
			this._context = GraphicsContext.Active;
			this.Name = name;
			this._id = 0;
			Opengl32.GenTextures(1, ref this._id);
			this._width = width;
			this._height = height;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004882 File Offset: 0x00002A82
		public void CopyFrom(OpenGLTexture texture)
		{
			this._width = texture._width;
			this._height = texture._height;
			this.Name = texture.Name;
			this._id = texture._id;
			this._context = texture._context;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000048C0 File Offset: 0x00002AC0
		public void Delete()
		{
			Opengl32.DeleteTextures(1, new int[] { this._id });
			Debug.Print("texture deleted! : " + this.Name, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000048F8 File Offset: 0x00002AF8
		internal void MakeActive()
		{
			if (OpenGLTexture.ActiveTexture != this)
			{
				Opengl32.BindTexture(Target.Texture2D, this._id);
				OpenGLTexture.ActiveTexture = this;
				this.SetTextureParameters();
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004920 File Offset: 0x00002B20
		private void SetTextureParameters()
		{
			Opengl32.TexParameteri(Target.Texture2D, TextureParameterName.TextureMinFilter, 9729);
			Opengl32.TexParameteri(Target.Texture2D, TextureParameterName.TextureMagFilter, 9729);
			if (this.ClampToEdge)
			{
				Opengl32.TexParameteri(Target.Texture2D, TextureParameterName.TextureWrapS, 33071);
				Opengl32.TexParameteri(Target.Texture2D, TextureParameterName.TextureWrapT, 33071);
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004985 File Offset: 0x00002B85
		public static OpenGLTexture FromFile(ResourceDepot resourceDepot, string name)
		{
			OpenGLTexture openGLTexture = new OpenGLTexture();
			openGLTexture.LoadFromFile(resourceDepot, name);
			return openGLTexture;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004994 File Offset: 0x00002B94
		public static OpenGLTexture FromFile(string fullFilePath)
		{
			OpenGLTexture openGLTexture = new OpenGLTexture();
			openGLTexture.LoadFromFile(fullFilePath);
			return openGLTexture;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000049A2 File Offset: 0x00002BA2
		public void Release()
		{
			this.Delete();
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000049AC File Offset: 0x00002BAC
		public void LoadFromFile(ResourceDepot resourceDepot, string name)
		{
			string filePath = resourceDepot.GetFilePath(name + ".png");
			this.LoadFromFile(filePath);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000049D4 File Offset: 0x00002BD4
		public void LoadFromFile(string fullPathName)
		{
			if (!File.Exists(fullPathName))
			{
				Debug.Print("File not found: " + fullPathName, 0, Debug.DebugColor.White, 17592186044416UL);
				return;
			}
			Image image = null;
			using (Stream stream = new MemoryStream(File.ReadAllBytes(fullPathName)))
			{
				image = new ImageReader().Read(stream, 0);
			}
			if (image == null)
			{
				Debug.Print("Error while reading file: " + fullPathName, 0, Debug.DebugColor.White, 17592186044416UL);
				return;
			}
			int width = image.Width;
			int height = image.Height;
			this.Initialize(Path.GetFileName(fullPathName), width, height);
			this.MakeActive();
			PixelFormat pixelFormat = PixelFormat.Red;
			uint num = 0U;
			bool flag = true;
			switch (image.Comp)
			{
			case 1:
				pixelFormat = PixelFormat.Red;
				num = 33321U;
				goto IL_0112;
			case 3:
				pixelFormat = PixelFormat.RGB;
				num = 32849U;
				goto IL_0112;
			case 4:
				pixelFormat = PixelFormat.RGBA;
				num = 32856U;
				goto IL_0112;
			}
			flag = false;
			Debug.Print("Unknown image format at file: " + fullPathName + ". Supported formats are: Single-Channel, RGB and RGBA.", 0, Debug.DebugColor.White, 17592186044416UL);
			IL_0112:
			if (flag)
			{
				Opengl32.TexImage2D(Target.Texture2D, 0, num, width, height, 0, pixelFormat, DataType.UnsignedByte, image.Data);
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004B24 File Offset: 0x00002D24
		public bool IsLoaded()
		{
			return true;
		}

		// Token: 0x04000039 RID: 57
		private int _width;

		// Token: 0x0400003A RID: 58
		private int _height;

		// Token: 0x0400003B RID: 59
		private string _name;

		// Token: 0x0400003D RID: 61
		private GraphicsContext _context;

		// Token: 0x0400003E RID: 62
		private int _id;

		// Token: 0x0400003F RID: 63
		private bool _clampToEdge;
	}
}
