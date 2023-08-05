using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Requests.UserRequests.DeleteById
{
    internal sealed class DeleteByIdRequest : IRequest<IActionResult>
    {
        public Guid Id { get; }

        public DeleteByIdRequest(Guid id)
        {
            Id = id;
        }

    }
}
