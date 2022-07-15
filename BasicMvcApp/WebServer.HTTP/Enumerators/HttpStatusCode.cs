namespace WebServer.HTTP.Enumerators
{
    public enum HttpStatusCode
    {
        Ok = 200,
        MovedPermanently = 301,
        Found = 302,
        TemporaryRedirect = 307,
        NotFound = 404,
        Gone = 410,
        InternalServerError = 500,
        ServiceUnavailable = 503
    }
}
