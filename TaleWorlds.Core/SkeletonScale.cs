using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000CE RID: 206
	public sealed class SkeletonScale : MBObjectBase
	{
		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x00023F37 File Offset: 0x00022137
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x00023F3F File Offset: 0x0002213F
		public string SkeletonModel { get; private set; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00023F48 File Offset: 0x00022148
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x00023F50 File Offset: 0x00022150
		public Vec3 MountSitBoneScale { get; private set; }

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00023F59 File Offset: 0x00022159
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x00023F61 File Offset: 0x00022161
		public float MountRadiusAdder { get; private set; }

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x00023F6A File Offset: 0x0002216A
		// (set) Token: 0x06000B1A RID: 2842 RVA: 0x00023F72 File Offset: 0x00022172
		public Vec3[] Scales { get; private set; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x00023F7B File Offset: 0x0002217B
		// (set) Token: 0x06000B1C RID: 2844 RVA: 0x00023F83 File Offset: 0x00022183
		public List<string> BoneNames { get; private set; }

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00023F8C File Offset: 0x0002218C
		// (set) Token: 0x06000B1E RID: 2846 RVA: 0x00023F94 File Offset: 0x00022194
		public sbyte[] BoneIndices { get; private set; }

		// Token: 0x06000B1F RID: 2847 RVA: 0x00023F9D File Offset: 0x0002219D
		public SkeletonScale()
		{
			this.BoneNames = null;
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00023FAC File Offset: 0x000221AC
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.SkeletonModel = node.Attributes["skeleton"].InnerText;
			XmlAttribute xmlAttribute = node.Attributes["mount_sit_bone_scale"];
			Vec3 vec = new Vec3(1f, 1f, 1f, -1f);
			if (xmlAttribute != null)
			{
				string[] array = xmlAttribute.Value.Split(new char[] { ',' });
				if (array.Length == 3)
				{
					float.TryParse(array[0], out vec.x);
					float.TryParse(array[1], out vec.y);
					float.TryParse(array[2], out vec.z);
				}
			}
			this.MountSitBoneScale = vec;
			XmlAttribute xmlAttribute2 = node.Attributes["mount_radius_adder"];
			if (xmlAttribute2 != null)
			{
				this.MountRadiusAdder = float.Parse(xmlAttribute2.Value);
			}
			this.BoneNames = new List<string>();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				string text = xmlNode.Name;
				if (text == "BoneScales")
				{
					List<Vec3> list = new List<Vec3>();
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Attributes != null)
						{
							text = xmlNode2.Name;
							if (text == "BoneScale")
							{
								XmlAttribute xmlAttribute3 = xmlNode2.Attributes["scale"];
								Vec3 vec2 = default(Vec3);
								if (xmlAttribute3 != null)
								{
									string[] array2 = xmlAttribute3.Value.Split(new char[] { ',' });
									if (array2.Length == 3)
									{
										float.TryParse(array2[0], out vec2.x);
										float.TryParse(array2[1], out vec2.y);
										float.TryParse(array2[2], out vec2.z);
									}
								}
								this.BoneNames.Add(xmlNode2.Attributes["bone_name"].InnerText);
								list.Add(vec2);
							}
						}
					}
					this.Scales = list.ToArray();
				}
			}
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0002423C File Offset: 0x0002243C
		public void SetBoneIndices(sbyte[] boneIndices)
		{
			this.BoneIndices = boneIndices;
			this.BoneNames = null;
		}
	}
}
