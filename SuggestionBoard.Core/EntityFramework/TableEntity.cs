﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Core.EntityFramework
{
    public interface ITableEntity : IBaseEntity
    {
        DateTime CreateDT { get; set; }
        DateTime? UpdateDT { get; set; }
        Guid CreateBy { get; set; }
        Guid? UpdateBy { get; set; }
    }
    public class TableEntity : BaseEntity, ITableEntity
    {
        public DateTime CreateDT { get; set; }
        public DateTime? UpdateDT { get; set; }
        public Guid CreateBy { get; set; }
        public Guid? UpdateBy { get; set; }
    }
}
