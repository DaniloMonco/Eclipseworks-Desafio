namespace EclipseWorks.Api.Middleware
{
    internal record ExceptionResponse(string Title, int Status, string TraceId, Dictionary<string, List<string>> Errors);
}
