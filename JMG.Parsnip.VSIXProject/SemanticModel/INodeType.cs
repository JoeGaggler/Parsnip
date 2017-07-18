using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal interface INodeType
	{
		TOutput ApplyVisitor<TOutput>(INodeTypeFuncVisitor<TOutput> visitor);
	}

	internal interface INodeTypeFuncVisitor<TOutput> :
		IFuncVisitor<EmptyNodeType, TOutput>,
		IFuncVisitor<SingleNodeType, TOutput>
	{

	}

	internal class EmptyNodeType : INodeType
	{
		private EmptyNodeType() { }

		public static EmptyNodeType Instance = new EmptyNodeType();

		public TOutput ApplyVisitor<TOutput>(INodeTypeFuncVisitor<TOutput> visitor) => visitor.Visit(this);
	}

	internal class SingleNodeType : INodeType
	{
		public SingleNodeType(String name)
		{
			this.Name = name;
		}

		public String Name { get; }

		public TOutput ApplyVisitor<TOutput>(INodeTypeFuncVisitor<TOutput> visitor) => visitor.Visit(this);
	}
}
