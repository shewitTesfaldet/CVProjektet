using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DeltagarListaViewModel
    {
        public int UID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool Privat { get; set; }
        public string ProjectDescription { get; set; }
    }
}
