using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class CustomerProfileImageUploadViewModel
    {
        [Required]
        public int CustomerId;
        [Required]
        public IFormFile ImageFile;
    }
}
