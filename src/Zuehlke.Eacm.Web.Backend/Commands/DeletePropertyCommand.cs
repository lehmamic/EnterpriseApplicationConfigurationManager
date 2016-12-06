﻿using System;
using CQRSlite.Commands;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class DeletePropertyCommand : ICommand
    {
        public Guid Id { get; set; }

        public Guid ParentEntityId { get; set; }

        public Guid PropertyId { get; set; }

        public int ExpectedVersion { get; set; }
    }
}