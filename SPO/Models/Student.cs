namespace SPO.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Student : DatabaseEntity
    {
        [ForeignKey("User")]
        public string UserFk { get; set; }
        public virtual IdentityUser User { get; set; }

        [Required]
        public string Index { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string School { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }
    }
}