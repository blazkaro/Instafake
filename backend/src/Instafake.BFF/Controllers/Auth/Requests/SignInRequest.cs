using Instafake.BFF.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Instafake.BFF.Controllers.Auth.Requests;

public class SignInRequest
{
    [FromQuery]
    [Required(AllowEmptyStrings = false)]
    [AllowedValues(SupportedIdp.GitHub, ErrorMessage = "Unsupported provider")]
    public string Provider { get; set; }

    public string? RedirectPath { get; set; }
}