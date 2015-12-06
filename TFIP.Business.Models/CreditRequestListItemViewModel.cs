﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIP.Business.Models
{
    public class CreditRequestListItemViewModel
    {
        public string CreditName { get; set; }

        public string CreditKind { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ApprovalDate { get; set; }

        public string Status { get; set; }
    }
}