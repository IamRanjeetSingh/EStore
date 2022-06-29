using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands
{
	public sealed class CommandDispatcher : ICommandDispatcher
	{
		private readonly IServiceProvider serviceProvider;

		public CommandDispatcher(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public Task<TResult> Send<TCommand, TResult>(TCommand cmd) where TCommand : ICommand
		{
			ICommandHandler<TCommand, TResult> cmdHandler = GetCommandHandler<TCommand,TResult>();
			if (cmdHandler == null)
				throw new InvalidOperationException($"No {typeof(ICommandHandler<,>).FullName} found for command {typeof(TCommand).FullName} and result {typeof(TResult).FullName}.");

			return cmdHandler.Handle(cmd);
		}

		private ICommandHandler<TCommand,TResult> GetCommandHandler<TCommand,TResult>() where TCommand : ICommand
		{
			Type cmdHandlerType = typeof(ICommandHandler<,>).MakeGenericType(typeof(TCommand), typeof(TResult));
			object cmdHandler = serviceProvider.GetService(cmdHandlerType);
			return (ICommandHandler<TCommand, TResult>)cmdHandler;
			
		}
	}
}
