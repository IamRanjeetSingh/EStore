using System;

namespace ProductService.Api.ModelBinders.CommandParsers
{
	public interface ICommandParserFactory
	{
		public ICommandParser GetParserForType(Type cmdType);
	}
}
