using Newtonsoft.Json;
using System.Net;

namespace IdentityApi.Boundary;

public class BaseResponseModel
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string? Message { get; set; }

    public BaseResponseModel() { }

    public BaseResponseModel(string message, HttpStatusCode code)
    {
        Message = message;
        StatusCode = code;
    }

    public override string ToString() => JsonConvert.SerializeObject(this);
}
