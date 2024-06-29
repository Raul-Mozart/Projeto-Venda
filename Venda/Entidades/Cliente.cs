using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venda.Entidades
{
    public class Cliente
    {
        public Cliente()
        {
            Nome = "Inicializado";
            CPF = "123-456-789.55";
            DataNascimento = new DateTime(2000, 10, 10);
            Email = "teste@teste.com";
        }
        public int Codigo { get; set; }

        [Required(ErrorMessage = "Nome obrigatório, no mínimo 12 caracteres")]
        [StringLength(52, MinimumLength = 12)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "CPF obrigatório")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Data de nascimento obrigatório")]
        public DateTime? DataNascimento { get; set; }

        private string? email;

        [Required(ErrorMessage = "Email obrigatório")]
        public string Email
        {
            get => email;
            set => email = value?.ToLower();
        }

        // public string PhotoUrl { get; set; }

        public virtual void PrintDados()
        {
            Console.WriteLine(
                "{0} {1} {2:dd/MM/yyyy} {3}",
                Nome, CPF, DataNascimento, Email
             );
        }
    }
}
