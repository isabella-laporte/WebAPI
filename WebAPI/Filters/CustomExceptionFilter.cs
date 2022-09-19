﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NullReferenceException nullReference)
            {
                context.Result = new ObjectResult(new
                {
                    message = "Ops! Id not found."
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };


            }
            else if (context.Exception is KeyNotFoundException keyNotfount)
            {
                context.Result = new ObjectResult(new
                {
                    message = "Ops! Id not found."
                })
                {
                    StatusCode = StatusCodes.Status204NoContent
                };
            }
            else
            {
                context.Result = new ObjectResult(new
                {
                    message = "Ops! An unexpected error occurred"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            context.ExceptionHandled = true;
        }
    }
}