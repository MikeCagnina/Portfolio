using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace AzurePortfolio.Components.Pages;

/// <summary>
/// Code-behind for the Error page component.
/// </summary>
public partial class Error : ComponentBase
{
    /// <summary>
    /// Gets or sets the HTTP context from the cascading parameter.
    /// </summary>
    [CascadingParameter]
    private HttpContext? HttpContext { get; set; }

    /// <summary>
    /// Gets or sets the request ID for error tracking.
    /// </summary>
    private string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether to show the request ID.
    /// </summary>
    private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    /// <summary>
    /// Initializes the component and sets the request ID.
    /// </summary>
    protected override void OnInitialized()
    {
        RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
    }
}

