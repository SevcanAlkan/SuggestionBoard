﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Core.ViewModel
{
    public interface ITableEntityVM : IBaseEntityVM
    {
        DateTime CreateDT { get; set; }
        DateTime? UpdateDT { get; set; }
        Guid CreateBy { get; set; }
        Guid UpdateBy { get; set; }
    }
    public class TableEntityVM : BaseEntityVM, ITableEntityVM
    {
        public DateTime CreateDT { get; set; }
        public DateTime? UpdateDT { get; set; }
        public Guid CreateBy { get; set; }
        public Guid UpdateBy { get; set; }
    }
}
