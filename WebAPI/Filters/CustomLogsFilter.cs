using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Interfaces;
using WebAPI.Logs;
using WebAPI.Models;

namespace WebAPI.Filters
{
    public class CustomLogsFilter : IResultFilter, IActionFilter
    {
        private readonly List<int> _sucessStatusCodes;
        private readonly IBaseRepository<Books> _repository;
        private readonly Dictionary<int, Books> _contextDict;

        public CustomLogsFilter(IBaseRepository<Books> repository)
        {
            _repository = repository;
            _contextDict = new Dictionary<int, Books>();
            _sucessStatusCodes = new List<int>() { StatusCodes.Status200OK, StatusCodes.Status201Created };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.Equals(context.ActionDescriptor.RouteValues["controller"], "books", StringComparison.InvariantCultureIgnoreCase))
            {
                int id = 0;
                if (context.ActionArguments.ContainsKey("id") && int.TryParse(context.ActionArguments["id"].ToString(), out id))
                {
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("patch", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var book = _repository.GetByKey(id).Result;
                        if (book != null)
                        {
                            var bookClone = book.clone();
                            _contextDict.Add(id, bookClone);
                        }
                    }
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (string.Equals(context.ActionDescriptor.RouteValues["controller"], "books", StringComparison.InvariantCultureIgnoreCase))
            {
                if (_sucessStatusCodes.Contains(context.HttpContext.Response.StatusCode))
                {
                    var idToParse = context.HttpContext.Request.Path.ToString().Split("/").Last();
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("patch", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var id = int.Parse(idToParse);
                        var afterUpdate = _repository.GetByKey(id).Result;
                        if (afterUpdate != null)
                        {
                            Books beforeUpdate;
                            if (_contextDict.TryGetValue(id, out beforeUpdate))
                            {
                                CustomLogs.SaveLog(afterUpdate.Id, "Books", afterUpdate.BookName, context.HttpContext.Request.Method, beforeUpdate, afterUpdate);
                                _contextDict.Remove(id);
                            }
                        }
                    }
                    else if (context.HttpContext.Request.Method.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var id = int.Parse(idToParse);
                        Books beforeUpdate;
                        if (_contextDict.TryGetValue(id, out beforeUpdate))
                        {
                            CustomLogs.SaveLog(beforeUpdate.Id, "Books", beforeUpdate.BookName, context.HttpContext.Request.Method);
                            _contextDict.Remove(id);
                        }
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
        public void OnResultExecuting(ResultExecutingContext context) { }
    }
}

