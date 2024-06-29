using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venda.Entidades;
using Venda.TranferirDados;

namespace Venda.Service
{
    public class DividaService
    {
        private readonly ISessionFactory session;

        public DividaService(ISessionFactory session)
        {
            this.session = session;
        }

        public static bool Validacao(Divida cliente,
            out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();
            var valido = Validator.TryValidateObject(cliente,
                new ValidationContext(cliente),
                erros,
                true
            );

            return valido;
        }

        public bool Criar(Divida divida, out List<ValidationResult> erros)
        {
            if (Validacao(divida, out erros))
            {
                using var sessao = session.OpenSession();
                using var transaction = sessao.BeginTransaction();
                sessao.Save(divida);
                transaction.Commit();
                return true;
            }
            return false;
        }

        public bool Editar(Divida divida, out List<ValidationResult> erros)
        {
            if (Validacao(divida, out erros))
            {
                using var sessao = session.OpenSession();
                using var transaction = sessao.BeginTransaction();
                sessao.Merge(divida);
                transaction.Commit();
                return true;
            }
            return false;
        }

        public bool Excluir(int id, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();
            using var sessao = session.OpenSession();
            using var transaction = sessao.BeginTransaction();
            var divida = sessao.Query<Divida>()
                .Where(c => c.Id == id)
                .FirstOrDefault();
            if (divida == null)
            {
                erros.Add(new ValidationResult("Dívida não encontrada",new[] { "id" }));
                return false;
            }

            sessao.Delete(divida);
            transaction.Commit();
            return true;
        }

        public virtual List<Divida> Listar()
        {
            using var sessao = session.OpenSession();
            var Dividas = sessao.Query<Divida>().OrderByDescending(c => c.Id).ToList();
            return Dividas;
        }


        public virtual Divida Retorna(int codigo)
        {
            using var sessao = session.OpenSession();
            var divida = sessao.Get<Divida>(codigo);
            return divida;
        }

        public virtual List<Divida> Listar(string busca)
        {
            using var sessao = session.OpenSession();
            var dividas = sessao.Query<Divida>()
                .Where(c => c.Id.ToString().Contains(busca) ||
                            c.Descricao.Contains(busca))
                .OrderBy(c => c.Id)
                .Take(4)
                .ToList();
            return dividas;
        }

        public List<RelatorioClienteTranfDados> ConsultaClientePorSituacao()
        {
            using var sessao = session.OpenSession();
            var dividas = sessao.Query<Cadastro>()
                .GroupBy(x => x.Divida.Situacao)
                .Select(x => new RelatorioClienteTranfDados
                {
                    Situacao = x.Key,
                    Total = x.Count()
                })
                .ToList();
            return dividas;
        }
    }

    public class DividaService2 : DividaService
    {
        public DividaService2(ISessionFactory session) : base(session)
        {
        }

        public override List<Divida> Listar()
        {
            return base.Listar();
        }
    }
}
