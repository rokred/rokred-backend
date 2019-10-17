using System;
using System.ComponentModel.DataAnnotations;

namespace RokredBackend.Models
{
    public class Opinion
    {
        [Key]
        public string Guid { get; set; }

        public string MyOpinion { get; set; }

        public string OpinionThreadKey { get; set; }

        public string SubjectKey { get; set; }

        public int Position { get; set; }
    }
}