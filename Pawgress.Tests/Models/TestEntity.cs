using System;

namespace Pawgress.Tests.Models
{
    public class TestEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
} 