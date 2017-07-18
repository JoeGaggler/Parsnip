using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal enum IntrinsicType
	{
		EndOfStream,
		EndOfLine,
		AnyCharacter,
		AnyLetter,
		CString,
	}

	internal class Intrinsic : IParseFunction
	{
		public Intrinsic(IntrinsicType type)
		{
			this.Type = type;
		}

		public IntrinsicType Type { get; }

		public Boolean IsMemoized => false;

		public INodeType ReturnType
		{
			get
			{
				switch (Type)
				{
					case IntrinsicType.AnyLetter:
					case IntrinsicType.AnyCharacter:
					case IntrinsicType.CString:
					case IntrinsicType.EndOfLine:
					return new SingleNodeType("String");

					case IntrinsicType.EndOfStream: return EmptyNodeType.Instance;
					default: throw new NotImplementedException();
				}

			}
		}


		public void ApplyVisitor(IParseFunctionActionVisitor visitor) => visitor.Visit(this);
	}
}
