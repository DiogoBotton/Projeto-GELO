using Gelo.WebApi.Domains;
using Gelo.WebApi.Infraestructure.Contexts;
using Gelo.WebApi.Interfaces;
using Gelo.WebApi.SeedWork;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.Infraestructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        public GeloContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public Cliente Create(Cliente objeto)
        {
            return _context.Clientes.Add(objeto).Entity;
        }

        public List<Cliente> ImportarClientesExcel(IFormFile file)
        {
            IWorkbook excel = WorkbookFactory.Create(file.OpenReadStream());

            ISheet tabelaClientes = excel.GetSheet("Clientes");

            if (tabelaClientes == null)
            {
                throw new Exception("A planilha Clientes não foi encontrada");
            }

            // Pega o index da primeira linha (Titulos) dessa tabela
            int primeiraLinhaIndexTitulos = tabelaClientes.FirstRowNum;

            // Pega o index da seguna linha (1° Registro) dessa tabela
            int segundaLinhaIndex = (tabelaClientes.FirstRowNum + 1);

            // Pega o index da ultima linha (Ultimo Registro) dessa tabela
            int ultimaLinhaIndex = tabelaClientes.LastRowNum;

            ICell TituloId = null;
            ICell TituloNomeRazaoSocial = null;
            ICell TituloCapacidadeFreezerEmPacotes = null;
            ICell TituloEndereco = null;

            List<Cliente> clientes = new List<Cliente>();
            List<Endereco> enderecos = new List<Endereco>();

            IRow linhasTitulos = tabelaClientes.GetRow(primeiraLinhaIndexTitulos);

            foreach (ICell cell in linhasTitulos.Cells)
            {
                switch (cell.StringCellValue)
                {
                    case "Id":
                        TituloId = cell;
                        break;
                    case "NomeRazaoSocial":
                        TituloNomeRazaoSocial = cell;
                        break;
                    case "CapacidadeFreezerEmPacotes":
                        TituloCapacidadeFreezerEmPacotes = cell;
                        break;
                    case "Endereco":
                        TituloEndereco = cell;
                        break;
                    default:
                        break;
                }
            }

            long IdCliente = 0;
            string NomeRazaoSocial = "";
            string CapacidadeFreezerEmPacotes = "";
            string Endereco = "";

            for (int rowNum = segundaLinhaIndex; rowNum < ultimaLinhaIndex; rowNum++)
            {
                //Recupera a linha atual
                IRow linha = tabelaClientes.GetRow(rowNum);

                foreach (ICell cell in linha.Cells)
                {
                    //Propriedade ID não usada
                    if (cell.ColumnIndex == TituloId.ColumnIndex)
                        IdCliente = Convert.ToInt32(cell.StringCellValue);

                    if (cell.ColumnIndex == TituloNomeRazaoSocial.ColumnIndex)
                        NomeRazaoSocial = cell.StringCellValue;

                    if (cell.ColumnIndex == TituloCapacidadeFreezerEmPacotes.ColumnIndex)
                        CapacidadeFreezerEmPacotes = cell.StringCellValue;

                    if (cell.ColumnIndex == TituloEndereco.ColumnIndex)
                        Endereco = cell.StringCellValue;
                }

                // Tentando instanciar um usuario
                Cliente cliente = null;
                try
                {
                    cliente = new Cliente(NomeRazaoSocial, Convert.ToUInt32(CapacidadeFreezerEmPacotes));
                }
                catch (Exception)
                {
                    throw new Exception("Houve um erro ao instanciar classe Usuario");
                }

                if (cliente != null)
                    clientes.Add(cliente);
                
                // Adicionando cliente na DB
                Cliente clienteAddDb = Create(cliente);

                Endereco endereco = null;
                try
                {
                    endereco = new Endereco(Endereco, clienteAddDb.Id);
                }
                catch (Exception)
                {

                    throw new Exception("Houve um erro ao instanciar classe Endereco");
                }

                if (endereco != null)
                    enderecos.Add(endereco);
            }

            //Adicionando Enderecos vinculados à clientes na DB
            _context.AddRange(enderecos);

            //TODO Contatos, NF, Boletos e Preço por Produto (FAZER TODO O PROCEDIMENTO IGUAL A TabelaClientes)

            // Nota Fiscal Condição
            ISheet tabelaNF = excel.GetSheet("NF");

            // Boletos
            ISheet tabelaBoletos = excel.GetSheet("Boletos");
            
            // Contatos
            ISheet tabelaContatos = excel.GetSheet("Contatos");

            return clientes;
        }
    }
}
