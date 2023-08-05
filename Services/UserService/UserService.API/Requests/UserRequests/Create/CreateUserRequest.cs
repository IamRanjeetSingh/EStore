using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Schemas;

namespace UserService.API.Requests.UserRequests.Create
{
    internal sealed class CreateUserRequest : IRequest<IActionResult>
    {
        public CreateUserArgs Args { get; }

        public CreateUserRequest(CreateUserArgs args)
        {
            Args = args;
        }
    }
}
