﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFIP.Business.Entities;
using System.ComponentModel.DataAnnotations;
using TFIP.Common.Constants;

namespace TFIP.Business.Models
{
    public class IndividualClientViewModel
    {

        public long Id { get; set; }

        [Required]
        [RegularExpression("[A-Z0-9]{2,14}")]
        public string IdentificationNo { get; set; }

        [Required]
        [RegularExpression("[A-Z0-9]{2,9}")]
        public string PassportNo { get; set; }

        [Required]
        [RegularExpression(RegexConstants.BigLetters)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("[A-ZА-Я'-]+")]
        public string LastName { get; set; }

        [RegularExpression(RegexConstants.BigLetters)]
        public string Patronymic { get; set; }

        public Gender Gender { get; set; }

        [Required]
        [RegularExpression(RegexConstants.BigLetters)]
        public string Nationality { get; set; }

        [Required]
        [RegularExpression("[A-ZА-Я\\s]+")]
        public string PlaceOfBirth { get; set; }

        [Required]
        public string Authority { get; set; }

        [Required]
        public DateTime DateOfIssue { get; set; }

        [Required]
        public DateTime DateOfExpiry { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int CoutryId { get; set; }

        [Required]
        [RegularExpression("[A-ZА-Я0-9'-\\s]+")]
        public string RegistrationCity { get; set; }

        [Required]
        [RegularExpression("[A-ZА-Я0-9'-\\s]+")]
        public string RegistrationRegion { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        [RegularExpression("[A-ZА-Я0-9\\.]+")]
        public string HouseNo { get; set; }

        [Required]
        [RegularExpression("[A-ZА-Я0-9\\.]+")]
        public string FlatNo { get; set; }

        public DateTime RegistrationDate { get; set; }

        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }

        [Required]
        [Phone]
        public string ContactPhone { get; set; }
    }
}