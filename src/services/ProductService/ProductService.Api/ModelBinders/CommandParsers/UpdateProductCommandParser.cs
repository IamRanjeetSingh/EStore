using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using ProductService.Api.Exceptions;
using ProductService.Application.Commands;
using ProductService.Application.Commands.UpdateProduct;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace ProductService.Api.ModelBinders.CommandParsers
{
	internal sealed class UpdateProductCommandParser : CommandParser<UpdateProductCommand>
	{
		protected override async Task<UpdateProductCommand> ParseCommand(ModelBindingContext bindingContext)
		{
			HttpRequest httpRequest = bindingContext.HttpContext.Request;
			dynamic reqBodyJson = await GetRequestBodyJson(httpRequest);
			UpdateProductCommand cmd = new();
			cmd.Id = ParseId(httpRequest);
			cmd.Title = (string)reqBodyJson.title;
			cmd.Description = (string)reqBodyJson.description;
			cmd.Price = (double)reqBodyJson.price;
			cmd.Properties = ParseProperties(reqBodyJson.properties);
			return cmd;
		}

		private async Task<dynamic> GetRequestBodyJson(HttpRequest httpRequest)
		{
			StreamReader streamReader = new(httpRequest.Body);
			string reqBodyJsonString = await streamReader.ReadToEndAsync();
			ValidateRequestBody(reqBodyJsonString);
			dynamic reqBodyJson = JsonConvert.DeserializeObject(reqBodyJsonString);
			return reqBodyJson;
		}
		
		private void ValidateRequestBody(string reqBodyJsonString)
		{
			JObject reqBody = JObject.Parse(reqBodyJsonString);
			if (!reqBody.IsValid(JSchema.Parse(RequestBodyJsonSchema), out IList<string> errorMessages))
				throw new InvalidCommandException(errorMessages);
		}

		private Guid ParseId(HttpRequest httpRequest)
		{
			string id = (string)httpRequest.RouteValues["id"];
			if (id == null)
				throw new InvalidCommandException("'id' property is missing from request route.");
			if (!Guid.TryParse(id, out Guid guid))
				throw new InvalidCommandException($"'id' is not a valid {typeof(Guid).FullName}.");
			return guid;
		}

		private List<PropertyUpdateDetails> ParseProperties(dynamic propertiesJson)
		{
			List<PropertyUpdateDetails> properties = new();
			foreach (dynamic propertyDetails in propertiesJson)
			{
				PropertyUpdateDetails property = new();
				property.Name = propertyDetails.name;

				List<string> values = new();
				foreach (dynamic value in propertyDetails.values)
					values.Add((string)value);
				property.Values = values;

				properties.Add(property);
			}
			return properties;
		}

		private const string RequestBodyJsonSchema = @"
			{
				'type': 'object',
				'properties': {
					'title': {
						'type': 'string'
					},
					'description': {
						'type': 'string'
					},
					'price': {
						'type': 'number'
					},
					'properties': {
						'type': 'array',
						'items' : {
							'type': 'object',
							'properties': {
								'name': {
									'type': 'string'
								},
								'values': {
									'type': 'array',
									'items': {
										'type': ['string', 'number', 'boolean']
									}
								}
							},
							'required': ['name', 'values']
						}
					}
				},
				'required': ['title', 'description', 'price', 'properties']
			}";
	}
}
