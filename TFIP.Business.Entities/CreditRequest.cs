﻿using System.Collections.Generic;

namespace TFIP.Business.Entities
{
    public class CreditRequest : Entity, IEntityWithAttachments 
    {
        public AttachmentHeader AttachmentHeader { get; set; }

        public long? AttachmentHeaderId { get; set; }

        public virtual JuridicalClient JuridicalClient { get; set; }

        public long? JuridicalClientId { get; set; }

        public virtual IndividualClient IndividualClient { get; set; }

        public long? IndividualClientId { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual CreditType CreditType { get; set; }

        public virtual ICollection<Guarantor> Guarantors { get; set; }
    }
}