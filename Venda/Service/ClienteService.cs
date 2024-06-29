using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venda.Entidades;

namespace Venda.Service
{
    public class ClienteService
    {
        private readonly ISessionFactory session;

        public ClienteService(ISessionFactory session)
        {
            this.session = session;
        }

        public static bool Validacao(Cliente cliente, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();
            var valido = Validator.TryValidateObject(cliente,new ValidationContext(cliente),erros,true);

            var diaMinimo = DateTime.Today.AddYears(-18);
            if (cliente.DataNascimento > diaMinimo)
            {
                erros.Add(new ValidationResult("O cliente deve ser maior de 18 anos",new[] { "DataNascimento" }));
                valido = false;
            }

            return valido;
        }

        public bool Cadastrar(Cliente cliente, out List<ValidationResult> erros)
        {
            if (Validacao(cliente, out erros))
            {
                using var sessao = session.OpenSession();
                using var transaction = sessao.BeginTransaction();
                sessao.Save(cliente);
                transaction.Commit();
                return true;
            }
            return false;
        }

        public bool Atualizar(Cliente cliente, out List<ValidationResult> erros)
        {
            if (Validacao(cliente, out erros))
            {
                using var sessao = session.OpenSession();
                using var transaction = sessao.BeginTransaction();

                sessao.Merge(cliente);
                transaction.Commit();
                return true;
            }
            return false;
        }

        public Cliente Excluir(int id, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();
            using var sessao = session.OpenSession();
            using var transaction = sessao.BeginTransaction();
            var Cliente = sessao.Query<Cliente>()
                .Where(c => c.Codigo == id)
                .FirstOrDefault();
            if (Cliente == null)
            {
                erros.Add(new ValidationResult("Registro não encontrado",new[] { "id" }));
                return null;
            }

            sessao.Delete(Cliente);
            transaction.Commit();
            return Cliente;
        }

        public virtual Cliente Retorna(int codigo)
        {
            using var sessao = session.OpenSession();
            var cliente = sessao.Get<Cliente>(codigo);
            return cliente;
        }

        public virtual List<Cliente> Listar(int page, int pageSize)
        {
            using var sessao = session.OpenSession();
            var clientes = page == 0 ? sessao.Query<Cliente>()
                .OrderByDescending(c => c.Codigo)
                .ToList() :
                sessao.Query<Cliente>()
                .OrderByDescending(c => c.Codigo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return clientes;
        }

        public virtual List<Cliente> Listar(string busca, int page, int pageSize)
        {
            using var sessao = session.OpenSession();
            var Cliente = sessao.Query<Cliente>()
                .Where(c => c.Nome.Contains(busca) ||
                            c.Email.Contains(busca))
                .OrderBy(c => c.Codigo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return Cliente;
        }

        public List<Cadastro> ListarCadastro()
        {
            using var sessao = session.OpenSession();
            var cadastro = sessao.Query<Cadastro>()
                .OrderBy(c => c.Id)
                .ToList();
            return cadastro;
        }
    }
}
