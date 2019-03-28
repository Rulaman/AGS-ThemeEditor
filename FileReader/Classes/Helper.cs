namespace System
{
	public delegate void Action();
}

namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class |
		AttributeTargets.Method)]
	public sealed class ExtensionAttribute : Attribute { }
}
