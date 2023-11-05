using System;
using System.ComponentModel.DataAnnotations;

namespace AutoNSubstitute.Shortcuts.Test.Example
{
    public class User
    {
        public string Name { get; set; }

        [Range(1, 99)]
        public int Number { get; set; }
    }
}
