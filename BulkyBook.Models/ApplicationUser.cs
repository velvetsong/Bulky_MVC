﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BulkyBook.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
        public int Id { get; set; }
        [Required]
		public string? Name { get; set; }
		public string? StreetAddress { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }

		public int? CompanyId { get; set; }

		[ForeignKey("CompanyId")]
		[ValidateNever]                        //  We do not want validation on the Navigation Property
		public Company Company { get; set; }
    }
}
