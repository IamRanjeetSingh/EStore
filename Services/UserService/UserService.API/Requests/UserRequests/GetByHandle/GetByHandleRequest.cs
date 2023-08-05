using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Requests.UserRequests.GetByHandle
{
    internal sealed class GetByHandleRequest : IRequest<IActionResult>
    {
        public string Handle { get; }

        public GetByHandleRequest(string handle)
        {
            Handle = handle;
        }
    }
}
