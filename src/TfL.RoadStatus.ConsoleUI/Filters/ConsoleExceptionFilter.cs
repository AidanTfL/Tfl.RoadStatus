using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using TfL.RoadStatus.Application.Exceptions;
using static System.Console;

namespace TfL.RoadStatus.ConsoleUI.Filters
{
    public static class ConsoleExceptionFilter
    {
        private static readonly IDictionary<Type, Action<UnhandledExceptionEventArgs>> exceptionHandlers =
            new Dictionary<Type, Action<UnhandledExceptionEventArgs>>
            {
                // Register known exception types and handlers.

                {typeof(AggregateException), HandleAggregateException},
                {typeof(ValidationException), HandleValidationException},
                {typeof(NotFoundException), HandleNotFoundException},
                {typeof(ApiClientException), HandleApiClientException}
            };


        public static void HandleException(object sender, UnhandledExceptionEventArgs context)
        {
            var type = context.ExceptionObject.GetType();
            if (exceptionHandlers.ContainsKey(type))
            {
                exceptionHandlers[type].Invoke(context); // invokes the relevant handler below
                return;
            }

            HandleUnknownException(context);
        }

        private static void HandleAggregateException(UnhandledExceptionEventArgs context)
        {
            var aggregateException = context.ExceptionObject as AggregateException;

            foreach (var exception in aggregateException.InnerExceptions)
            {
                var type = exception.GetType();

                if (exceptionHandlers.ContainsKey(type))
                    exceptionHandlers[type]
                        .Invoke(new UnhandledExceptionEventArgs(exception,
                            context.IsTerminating)); // invokes the relevant handler below
            }
        }

        private static void HandleValidationException(UnhandledExceptionEventArgs context)
        {
            var exception = context.ExceptionObject as ValidationException;

            foreach (var validationError in exception.Errors.Select(e => e.ErrorMessage)) 
                WriteLine(validationError);

            Environment.Exit(0); // Don't set error code to non-zero value - requirements don't specify to do that
        }

        private static void HandleNotFoundException(UnhandledExceptionEventArgs context)
        {
            var exception = context.ExceptionObject as NotFoundException;

            WriteLine(exception?.Message);

            Environment.Exit(1); // Set error code to non-zero value
        }

        internal static void HandleArgsParserFailure(string helpText)
        {
            if (helpText.Contains("ERROR(S):\r\n  A required value not bound to option name is missing.")) //roadIds missing
                Environment.Exit(2); // Set error code to non-zero value
        }

        private static void HandleApiClientException(UnhandledExceptionEventArgs context)
        {
            var exception = context.ExceptionObject as ApiClientException;

            WriteLine(exception?.Message);

            Environment.Exit(0);
        }

        private static void HandleUnknownException(UnhandledExceptionEventArgs context)
        {
            WriteLine($"An unknown error occurred. {context.ExceptionObject}");

            Environment.Exit(0);
        }
    }
}