﻿using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class CreateEntityCommand : IDomainCommand
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ExpectedVersion { get; set; }
    }
}