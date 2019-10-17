using System;
using System.ComponentModel.DataAnnotations;

namespace RokredBackend.Models
{
    public class Subject
    {
        [Key]
        public string Guid { get; set; }

        public string MySubject { get; set; }
    }
}
