using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Commands;
using ProductService.Application.Commands.CreateProduct;
using ProductService.Application.Commands.DeleteProduct;
using ProductService.Application.Commands.UpdateProduct;
using ProductService.Application.Queries;
using ProductService.Application.Queries.GetProductById;
using ProductService.Application.Queries.GetProductById.Dto;
using ProductService.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace ProductService.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly ICommandDispatcher cmdDispatcher;
		private readonly IQueryDispatcher qryDispatcher;

		public ProductController(ICommandDispatcher cmdDispatcher, IQueryDispatcher qryDispatcher)
		{
			this.cmdDispatcher = cmdDispatcher;
			this.qryDispatcher = qryDispatcher;
		}

		[HttpGet("[action]")]
		public IActionResult ping()
		{
			return Ok("pong...");
		}

		[HttpPost("")]
		public async Task<IActionResult> Create(CreateProductCommand cmd)
		{
			try
			{
				Guid productId = await cmdDispatcher.Send<CreateProductCommand, Guid>(cmd);
				return CreatedAtAction(nameof(Get), new { id = productId.ToString() }, null);
			}
			catch(EmptyPropertyException e)
			{
				return BadRequest(new { error = e.Message });
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(GetProductByIdQuery qry)
		{
			IProductDto productDto = await qryDispatcher.Send<GetProductByIdQuery, IProductDto>(qry);
			if (productDto == null)
				return NotFound();
			return Ok(productDto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(UpdateProductCommand cmd)
		{
			try
			{
				bool isUpdatedSuccessful = await cmdDispatcher.Send<UpdateProductCommand, bool>(cmd);
				if (!isUpdatedSuccessful)
					return NotFound();
				return Ok();
			}
			catch(EmptyPropertyException e)
			{
				return BadRequest(new { error = e.Message });
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(DeleteProductCommand cmd)
		{
			bool isDeleteSuccessful = await cmdDispatcher.Send<DeleteProductCommand, bool>(cmd);
			if (!isDeleteSuccessful)
				return NotFound();
			return Ok();
		}

	}
}
