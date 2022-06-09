using System;
using static Bluebird.Core.Starter.Domain.Models.Enums;

namespace Bluebird.Core.Starter.Domain.Models
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Genre Genre { get; set; }
        public string Director { get; set; }
        public string ReleaseYear { get; set; }
    }
}
