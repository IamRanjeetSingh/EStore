using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands
{
	public interface ICommandHandler<TCommand,TResult> where TCommand : ICommand
	{
		public Task<TResult> Handle(TCommand cmd);
	}
}
