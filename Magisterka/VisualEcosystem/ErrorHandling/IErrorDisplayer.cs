﻿using Magisterka.Domain.ExceptionContracts;

namespace Magisterka.VisualEcosystem.ErrorHandling
{
    public interface IErrorDisplayer
    {
        void DisplayError(eErrorTypes errorType, string message);
    }
}
