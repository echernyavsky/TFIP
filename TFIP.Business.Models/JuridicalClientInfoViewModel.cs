﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIP.Business.Models
{
    public class JuridicalClientInfoViewModel: CreateJuridicalClientViewModel
    {
        public ICollection<CreditRequestListItemViewModel> Credits;
    }
}
