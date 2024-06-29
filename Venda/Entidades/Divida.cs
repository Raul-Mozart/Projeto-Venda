using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venda.Entidades
{
    public class Divida
    {

        public int Id { get; set; }
        public int Valor { get; set; }

        [Required]
        public string Situacoes { get; set; }
        // 0 - pago, 1 - devendo

        [Required]
        public SituacaoDivida Situacao { get; set; }

        [Required]
        public DateTime? DataCriacao { get; set; }

        [Required]
        public DateTime? DataPagamento { get; set; }

        [Required]
        public string Descricao { get; set; }

        void Metodo()
        {
            if (Situacao == SituacaoDivida.pago) { }// faz alguma coisa
            else if (Situacao == SituacaoDivida.devendo) { } // faz outra coisa 
        }
    }

    public class DividaTranferirDados
    {
        public DividaTranferirDados(Divida x)
        {

            Id = x.Id;
            Valor = x.Valor;
            Situacoes = x.Situacoes.ToString();
            DataCriacao = (DateTime)x.DataCriacao;
            DataPagamento = (DateTime)x.DataPagamento;
            Descricao = x.Descricao;
        }
        public int Id { get; set; }
        public int Valor { get; set; }
        public string Situacoes { get; set; }
        public string Situacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataPagamento { get; set; }
        public string Descricao { get; set; }
    }

    public enum SituacaoDivida
    {
        pago = 0,
        devendo = 1,
    }
}
