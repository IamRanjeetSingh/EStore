using System;

namespace ProductService.Api.ModelBinders.CommandParsers
{
	public sealed class CommandParserFactory : ICommandParserFactory
	{
		private readonly IServiceProvider serviceProvider;

		public CommandParserFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public ICommandParser GetParserForType(Type cmdType)
		{
			Type cmdParserType = typeof(CommandParser<>).MakeGenericType(cmdType);
			ICommandParser cmdParser = (ICommandParser)serviceProvider.GetService(cmdParserType);
			return cmdParser;
		}
	}
}
