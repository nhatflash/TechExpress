using System;
using System.Collections.Generic;
using System.Text;

namespace TechExpress.Service.Commands
{
    public class CreateProductSpecValueCommand
    {
        public required Guid SpecDefinitionId { get; set; }
        public required string Value { get; set; }
    }
}
