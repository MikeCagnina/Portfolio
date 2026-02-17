using System;
using Microsoft.TeamFoundation.Client;

namespace TFSData.Validations
{
    public static class ValidateTFSOperations
    {
        public static bool ValidUri(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }

        public static bool ValidConnection(TfsTeamProjectCollection server)
        {
            return server != null && !server.ConnectivityFailureOnLastWebServiceCall && !server.ConnectivityFailureOnLastWebServiceCall && !server.Disposed;
        }
    }
}