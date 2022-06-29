using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using ProductService.Api.Exceptions;
using ProductService.Application.Commands;
using ProductService.Application.Commands.CreateProduct;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace ProductService.Api.ModelBinders.CommandParsers
{
	internal sealed class CreateProductCommandParser : CommandParser<CreateProductCommand>
	{
		protected override async Task<CreateProductCommand> ParseCommand(ModelBindingContext bindingContext)
		{
			dynamic reqBodyJson = await GetRequestBodyJson(bindingContext.HttpContext.Request);			
			CreateProductCommand cmd = new();
			cmd.Title = (string)reqBodyJson.title;
			cmd.Description = (string)reqBodyJson.description;
			cmd.Price = (double)reqBodyJson.price;
			cmd.Properties = ParsePropertiesFromJson(reqBodyJson.properties);
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

		private List<PropertyCreateDetails> ParsePropertiesFromJson(dynamic propertiesJson)
		{
			List<PropertyCreateDetails> properties = new();
			foreach (dynamic propertyDetails in propertiesJson)
			{
				List<string> values = new();
				foreach (dynamic value in propertyDetails.values)
					values.Add((string)value);
				properties.Add(new() { Name = propertyDetails.name, Values = values });
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
