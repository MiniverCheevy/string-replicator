using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StringReplicator.Core.Operations
{
    public class FormatRequest
    {
        [Required(ErrorMessage = "required")]
        public string DataString { get; set; }

        [Required(ErrorMessage = "required")]
        public string FormatString { get; set; }

        public DatabaseRequest Database { get; set; }

        public FormatRequest()
        {
            Database = new DatabaseRequest();
        }
    }
}