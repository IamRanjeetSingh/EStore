
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands
{
	public interface ICommandDispatcher
	{
		public Task<TResult> Send<TCommand, TResult>(TCommand cmd) where TCommand : ICommand;
	}
}
