﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands.UpdateProduct
{
	public sealed class UpdateProductCommand : ICommand
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public List<PropertyUpdateDetails> Properties { get; set; }
	}
}
