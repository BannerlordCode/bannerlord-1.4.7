using System;
using System.Collections;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000CC RID: 204
	public class ShipPhysicsReference : MBObjectBase
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00023972 File Offset: 0x00021B72
		// (set) Token: 0x06000B02 RID: 2818 RVA: 0x0002397A File Offset: 0x00021B7A
		public LinearFrictionTerm LinearDragTerm { get; private set; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00023983 File Offset: 0x00021B83
		// (set) Token: 0x06000B04 RID: 2820 RVA: 0x0002398B File Offset: 0x00021B8B
		public LinearFrictionTerm LinearDampingTerm { get; private set; }

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00023994 File Offset: 0x00021B94
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x0002399C File Offset: 0x00021B9C
		public LinearFrictionTerm ConstantLinearDampingTerm { get; private set; }

		// Token: 0x06000B07 RID: 2823 RVA: 0x000239A8 File Offset: 0x00021BA8
		static ShipPhysicsReference()
		{
			ShipPhysicsReference.Default.PostProcessPhysicsReference();
			ShipPhysicsReference.DefaultDebris.PostProcessPhysicsReference();
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00023AD3 File Offset: 0x00021CD3
		public ShipPhysicsReference()
		{
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00023ADB File Offset: 0x00021CDB
		public ShipPhysicsReference(string stringId)
			: base(stringId)
		{
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00023AE4 File Offset: 0x00021CE4
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			bool flag;
			float num = this.DeserializeScalarAttribute<float>(node, "reference_mass", true, out flag);
			bool flag2;
			this.LinearDragTerm = this.DeserializeDragTermElement(node, "linear_drag_term", out flag2);
			bool flag3;
			this.LinearDampingTerm = this.DeserializeDragTermElement(node, "linear_damping_term", out flag3);
			bool flag4;
			this.ConstantLinearDampingTerm = this.DeserializeDragTermElement(node, "constant_linear_damping_term", out flag4);
			this.LinearDragTerm /= num;
			this.LinearDampingTerm /= num;
			this.ConstantLinearDampingTerm /= num;
			this.PostProcessPhysicsReference();
			this.AssertFieldValidity(flag2, "linear_drag_term");
			this.AssertFieldValidity(flag3, "linear_damping_term");
			this.AssertFieldValidity(flag4, "constant_linear_damping_term");
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00023BA8 File Offset: 0x00021DA8
		private void PostProcessPhysicsReference()
		{
			float defaultWaterDensity = ShipPhysicsReference.GetDefaultWaterDensity();
			this.LinearDragTerm /= defaultWaterDensity;
			this.LinearDampingTerm /= defaultWaterDensity;
			this.ConstantLinearDampingTerm /= defaultWaterDensity;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00023BF1 File Offset: 0x00021DF1
		public static float GetDefaultWaterDensity()
		{
			return 1020f;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00023BF8 File Offset: 0x00021DF8
		private T DeserializeScalarAttribute<T>(XmlNode node, string attributeName, bool isRequiredAttribute, out bool isAttributeValid)
		{
			XmlAttribute xmlAttribute = node.Attributes[attributeName];
			isAttributeValid = xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.InnerText);
			if (isAttributeValid)
			{
				return (T)((object)Convert.ChangeType(xmlAttribute.Value, typeof(T)));
			}
			this.AssertFieldValidity(!isRequiredAttribute, attributeName);
			return default(T);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00023C60 File Offset: 0x00021E60
		private Vec3 DeserializeVectorElement(XmlNode node, string elementName, bool isRequiredElement, out bool isElementValid)
		{
			Vec3 invalid = Vec3.Invalid;
			XmlNode xmlNode = node.SelectSingleNode(elementName);
			isElementValid = xmlNode != null;
			if (isElementValid)
			{
				using (IEnumerator enumerator = xmlNode.Attributes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						string text = xmlAttribute.Name.ToLower();
						float num = float.Parse(xmlAttribute.Value);
						if (text == "x")
						{
							invalid.x = num;
						}
						else if (text == "y")
						{
							invalid.y = num;
						}
						else if (text == "z")
						{
							invalid.z = num;
						}
					}
					return invalid;
				}
			}
			this.AssertFieldValidity(!isRequiredElement, elementName);
			return invalid;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00023D3C File Offset: 0x00021F3C
		private LinearFrictionTerm DeserializeDragTermElement(XmlNode node, string elementName, out bool isElementValid)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			XmlNode xmlNode = node.SelectSingleNode(elementName);
			isElementValid = xmlNode != null;
			if (isElementValid)
			{
				foreach (object obj in xmlNode.Attributes)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)obj;
					string text = xmlAttribute.Name.ToLower();
					string value = xmlAttribute.Value;
					if (text == "right")
					{
						num = float.Parse(value);
					}
					else if (text == "left")
					{
						num2 = float.Parse(value);
					}
					else if (text == "forward")
					{
						num3 = float.Parse(value);
					}
					else if (text == "backward")
					{
						num4 = float.Parse(value);
					}
					else if (text == "up")
					{
						num5 = float.Parse(value);
					}
					else if (text == "down")
					{
						num6 = float.Parse(value);
					}
				}
			}
			return new LinearFrictionTerm(num, num2, num3, num4, num5, num6);
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00023E8C File Offset: 0x0002208C
		private void AssertFieldValidity(bool assert, string fieldName)
		{
		}

		// Token: 0x04000614 RID: 1556
		public static readonly ShipPhysicsReference Default = new ShipPhysicsReference
		{
			LinearDragTerm = new LinearFrictionTerm(0.89126307f, 0.89126307f, 0.0009766732f, 0.0033027069f, 0.08070293f, 0.80702925f),
			LinearDampingTerm = new LinearFrictionTerm(0.28781262f, 0.28781262f, 0.0026044627f, 0.008807218f, 0.21520779f, 2.152078f),
			ConstantLinearDampingTerm = new LinearFrictionTerm(0.045454543f, 0.045454543f, 0.013636364f, 0.027272727f, 0.045454543f, 0.045454543f)
		};

		// Token: 0x04000615 RID: 1557
		public static readonly ShipPhysicsReference DefaultDebris = new ShipPhysicsReference
		{
			LinearDragTerm = new LinearFrictionTerm(0.89126307f, 0.89126307f, 0.89126307f, 0.89126307f, 0.80702925f, 0.80702925f),
			LinearDampingTerm = new LinearFrictionTerm(0.28781262f, 0.28781262f, 0.28781262f, 0.28781262f, 2.152078f, 2.152078f),
			ConstantLinearDampingTerm = new LinearFrictionTerm(0.045454543f, 0.045454543f, 0.045454543f, 0.045454543f, 0.045454543f, 0.045454543f)
		};
	}
}
