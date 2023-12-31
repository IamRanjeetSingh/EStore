﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Queries
{
	public interface IQueryHandler<TQuery,TResult> where TQuery : IQuery
	{
		public Task<TResult> Handle(TQuery qry);
	}
}
