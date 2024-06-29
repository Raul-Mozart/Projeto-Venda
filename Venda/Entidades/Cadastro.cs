using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venda.Entidades
{
    public class Cadastro
    {
        public int Id { get; set; }
        public Divida Divida { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;

    }
}
