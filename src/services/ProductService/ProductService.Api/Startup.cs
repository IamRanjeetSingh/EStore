using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProductService.Api.ModelBinders;
using ProductService.Api.ModelBinders.CommandParsers;
using ProductService.Api.ModelBinders.QueryParsers;
using ProductService.Application.Commands;
using ProductService.Application.Commands.CreateProduct;
using ProductService.Application.Commands.DeleteProduct;
using ProductService.Application.Commands.UpdateProduct;
using ProductService.Application.Queries;
using ProductService.Application.Queries.GetProductById;
using ProductService.Application.Queries.GetProductById.Dto;
using ProductService.Domain.Aggregates.Common;
using ProductService.Domain.Aggregates.ProductAggregate;
using ProductService.Infrastructure.Queries.GetProductById;
using ProductService.Infrastructure.Repositories;
using ProductService.Infrastructure.Repositories.ProductRepositoryNS;
using ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			AddProductService(services);
			services.AddControllers(options => 
			{
				options.ModelBinderProviders.Insert(0, new CommandModelBinderProvider());
				options.ModelBinderProviders.Insert(1, new QueryModelBinderProvider());
			});
			//services.AddSwaggerGen(c =>
			//{
			//	c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductService.Api", Version = "v1" });
			//});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				//app.UseSwagger();
				//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductService.Api v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private void AddProductService(IServiceCollection services)
		{
			string connectionString = Configuration["ConnectionString"];

			#region Domain Layer
			services.AddScoped<DataContext>();
			services.AddScoped<IProductRepository, ProductRepository>(sp => sp.GetService<ProductRepository>());
			#endregion

			#region Application Layer
			services.AddScoped<CommandParser<CreateProductCommand>, CreateProductCommandParser>();
			services.AddScoped<CommandParser<UpdateProductCommand>, UpdateProductCommandParser>();
			services.AddScoped<CommandParser<DeleteProductCommand>, DeleteProductCommandParser>();
			services.AddScoped<QueryParser<GetProductByIdQuery>, GetProductByIdQueryParser>();
			services.AddScoped<ICommandParserFactory, CommandParserFactory>();
			services.AddScoped<IQueryParserFactory, QueryParserFactory>();
			services.AddScoped<ICommandDispatcher, CommandDispatcher>();
			services.AddScoped<IQueryDispatcher, QueryDispatcher>();
			services.AddScoped<ICommandHandler<CreateProductCommand, Guid>, CreateProductCommandHandler>();
			services.AddScoped<ICommandHandler<UpdateProductCommand, bool>, UpdateProductCommandHandler>();
			services.AddScoped<ICommandHandler<DeleteProductCommand, bool>, DeleteProductCommandHandler>();
			services.AddScoped<IQueryHandler<GetProductByIdQuery, IProductDto>, IGetProductByIdQueryHandler>(sp => sp.GetService<IGetProductByIdQueryHandler>());
			#endregion

			#region Infrastructure Layer
			services.AddScoped<IGetProductByIdQueryHandler, GetProductByIdQueryHandler>(_ => new(connectionString));
			services.AddScoped<ProductRepository, ProductRepositoryImpl>();
			services.AddScoped<IPersistentProductDao, PersistentProductDao>();
			services.AddScoped<IPersistentPropertyDao, PersistentPropertyDao>();
			services.AddScoped<IPersistentPropertyValueDao, PersistentPropertyValueDao>();
			services.AddScoped<IProductIntegrator, ProductIntegrator>();
			services.AddScoped<IProductDecomposer, ProductDecomposer>();
			services.AddScoped<IUnitOfWork, ISqlUnitOfWork>(sp => sp.GetService<ISqlUnitOfWork>());
			services.AddScoped<ISqlUnitOfWork, SqlUnitOfWork>(_ => new(connectionString));
			#endregion
		}
	}
}
