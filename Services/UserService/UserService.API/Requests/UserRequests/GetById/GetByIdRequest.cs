using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Requests.UserRequests.GetById
{
    internal sealed class GetByIdRequest : IRequest<IActionResult>
    {
        public Guid Id { get; }

        public GetByIdRequest(Guid id)
        {
            Id = id;
        }
    }
}
