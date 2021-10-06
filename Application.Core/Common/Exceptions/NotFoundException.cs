using System;

namespace Application.Core.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name) :
            base($"({name}) Not found.")

        {
        }
    }
}