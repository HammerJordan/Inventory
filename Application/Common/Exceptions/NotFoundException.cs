using System;

namespace Inventory.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name) :
            base($"({name}) Not found.")

        {
        }
    }
}