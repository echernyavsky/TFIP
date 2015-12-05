﻿using TFIP.Business.Models;

namespace TFIP.Business.Contracts
{
    public interface IIndividualClientsService
    {
        long IsClientExist(string identificationNo);

        void CreateClient(IndividualClientViewModel client);
    }
}
