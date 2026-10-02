using System;
using System.Collections;
using System.Xml;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200009B RID: 155
	public class MBBodyProperty : MBObjectBase
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x0001D3FF File Offset: 0x0001B5FF
		// (set) Token: 0x060008EB RID: 2283 RVA: 0x0001D407 File Offset: 0x0001B607
		public string HairTags { get; set; } = "";

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0001D410 File Offset: 0x0001B610
		// (set) Token: 0x060008ED RID: 2285 RVA: 0x0001D418 File Offset: 0x0001B618
		public string BeardTags { get; set; } = "";

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x0001D421 File Offset: 0x0001B621
		// (set) Token: 0x060008EF RID: 2287 RVA: 0x0001D429 File Offset: 0x0001B629
		public string TattooTags { get; set; } = "";

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0001D432 File Offset: 0x0001B632
		public BodyProperties BodyPropertyMin
		{
			get
			{
				return this._bodyPropertyMin;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x0001D43A File Offset: 0x0001B63A
		public BodyProperties BodyPropertyMax
		{
			get
			{
				return this._bodyPropertyMax;
			}
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0001D442 File Offset: 0x0001B642
		public MBBodyProperty(string stringId)
			: base(stringId)
		{
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0001D46C File Offset: 0x0001B66C
		public MBBodyProperty()
		{
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0001D498 File Offset: 0x0001B698
		public static MBBodyProperty CreateFrom(MBBodyProperty bodyProperty)
		{
			MBBodyProperty mbbodyProperty = MBObjectManager.Instance.CreateObject<MBBodyProperty>();
			mbbodyProperty.HairTags = bodyProperty.HairTags;
			mbbodyProperty.BeardTags = bodyProperty.BeardTags;
			mbbodyProperty.TattooTags = bodyProperty.TattooTags;
			mbbodyProperty._bodyPropertyMin = bodyProperty._bodyPropertyMin;
			mbbodyProperty._bodyPropertyMax = bodyProperty._bodyPropertyMax;
			return mbbodyProperty;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0001D4EC File Offset: 0x0001B6EC
		public void Init(BodyProperties bodyPropertyMin, BodyProperties bodyPropertyMax)
		{
			base.Initialize();
			this._bodyPropertyMin = bodyPropertyMin;
			this._bodyPropertyMax = bodyPropertyMax;
			if (this._bodyPropertyMax.Age <= 0f)
			{
				this._bodyPropertyMax = this._bodyPropertyMin;
			}
			if (this._bodyPropertyMin.Age <= 0f)
			{
				this._bodyPropertyMin = this._bodyPropertyMax;
			}
			base.AfterInitialized();
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001D550 File Offset: 0x0001B750
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "BodyPropertiesMin")
				{
					BodyProperties.FromXmlNode(xmlNode, out this._bodyPropertyMin);
				}
				else if (xmlNode.Name == "BodyPropertiesMax")
				{
					BodyProperties.FromXmlNode(xmlNode, out this._bodyPropertyMax);
				}
				else
				{
					if (xmlNode.Name == "hair_tags")
					{
						using (IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								object obj2 = enumerator2.Current;
								XmlNode xmlNode2 = (XmlNode)obj2;
								string hairTags = this.HairTags;
								XmlAttributeCollection attributes = xmlNode2.Attributes;
								this.HairTags = hairTags + ((attributes != null) ? attributes["name"].Value : null) + ",";
							}
							continue;
						}
					}
					if (xmlNode.Name == "beard_tags")
					{
						using (IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								object obj3 = enumerator2.Current;
								XmlNode xmlNode3 = (XmlNode)obj3;
								string beardTags = this.BeardTags;
								XmlAttributeCollection attributes2 = xmlNode3.Attributes;
								this.BeardTags = beardTags + ((attributes2 != null) ? attributes2["name"].Value : null) + ",";
							}
							continue;
						}
					}
					if (xmlNode.Name == "tattoo_tags")
					{
						foreach (object obj4 in xmlNode.ChildNodes)
						{
							XmlNode xmlNode4 = (XmlNode)obj4;
							string tattooTags = this.TattooTags;
							XmlAttributeCollection attributes3 = xmlNode4.Attributes;
							this.TattooTags = tattooTags + ((attributes3 != null) ? attributes3["name"].Value : null) + ",";
						}
					}
				}
			}
			if (this._bodyPropertyMax.Age <= 0f)
			{
				this._bodyPropertyMax = this._bodyPropertyMin;
			}
			if (this._bodyPropertyMin.Age <= 0f)
			{
				this._bodyPropertyMin = this._bodyPropertyMax;
			}
		}

		// Token: 0x04000501 RID: 1281
		private BodyProperties _bodyPropertyMin;

		// Token: 0x04000502 RID: 1282
		private BodyProperties _bodyPropertyMax;
	}
}
