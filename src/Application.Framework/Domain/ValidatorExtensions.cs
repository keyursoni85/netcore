/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Application.Framework.Commands;

namespace Application.Framework.Domain
{
    public static class ValidatorExtensions
    {
        public static void ValidateCommand<TCommand>(this IValidator<TCommand> validator, TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
                throw new Exception(BuildErrorMesage(validationResult.Errors));
        }

        private static string BuildErrorMesage(IEnumerable<ValidationFailure> errors)
        {
            var errorsText = errors.Select(x => "\r\n - " + x.ErrorMessage).ToArray();
            return "Validation failed: " + string.Join("", errorsText);
        }
    }
}
