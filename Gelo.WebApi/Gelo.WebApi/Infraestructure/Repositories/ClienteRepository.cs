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

        public ClienteRepository(GeloContext context)
        {
            _context = context;
        }

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
                //TODO: Fazer uma busca de clientes pelo NomeRazaoSocial, verificando se este cliente já existe[...]
                //[...] Caso existir, o if pula este processo do switch e soma segundaLinhaIndex +1
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
            int CapacidadeFreezerEmPacotes = 0;
            string Endereco = "";

            Cliente clienteAddDb = null;

            for (int rowNum = segundaLinhaIndex; rowNum <= ultimaLinhaIndex; rowNum++)
            {
                //Recupera a linha atual
                IRow linha = tabelaClientes.GetRow(rowNum);

                foreach (ICell cell in linha.Cells)
                {
                    //Propriedade ID não usada
                    //if (cell.ColumnIndex == TituloId.ColumnIndex)
                    //    IdCliente = Convert.ToInt32(cell.NumericCellValue);

                    if (cell.ColumnIndex == TituloNomeRazaoSocial.ColumnIndex)
                        NomeRazaoSocial = cell.StringCellValue;

                    if (cell.ColumnIndex == TituloCapacidadeFreezerEmPacotes.ColumnIndex)
                        CapacidadeFreezerEmPacotes = Convert.ToInt32(cell.NumericCellValue);

                    if (cell.ColumnIndex == TituloEndereco.ColumnIndex)
                        Endereco = cell.StringCellValue;
                }

                // Tentando instanciar um cliente
                Cliente cliente = null;
                try
                {
                    cliente = new Cliente(NomeRazaoSocial, Convert.ToUInt32(CapacidadeFreezerEmPacotes));
                }
                catch (Exception)
                {
                    throw new Exception("Houve um erro ao instanciar classe Cliente");
                }

                if (cliente != null)
                    clientes.Add(cliente);

                // Adicionando cliente na DB
                clienteAddDb = Create(cliente);

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
            _context.Enderecos.AddRange(enderecos);

            //TODO Contatos, NF, Boletos e Preço por Produto (FAZER TODO O PROCEDIMENTO IGUAL A TabelaClientes)

            _context.SaveChanges();

            // Nota Fiscal Condição
            ISheet tabelaNF = excel.GetSheet("NF");

            if (tabelaNF == null)
            {
                throw new Exception("A planilha Nota Fiscal(NF) não foi encontrada");
            }

            // Pega o index da primeira linha (Titulos) dessa tabela
            primeiraLinhaIndexTitulos = tabelaNF.FirstRowNum;

            // Pega o index da seguna linha (1° Registro) dessa tabela
            segundaLinhaIndex = (tabelaNF.FirstRowNum + 1);

            // Pega o index da ultima linha (Ultimo Registro) dessa tabela
            ultimaLinhaIndex = tabelaNF.LastRowNum;

            ICell TituloIdNF = null;
            ICell TituloDescricaoNF = null;
            ICell TituloEmitirNF = null;
            ICell TituloClienteId = null;

            List<NotaFiscalCondicao> notasFiscais = new List<NotaFiscalCondicao>();

            linhasTitulos = tabelaNF.GetRow(primeiraLinhaIndexTitulos);

            foreach (ICell cell in linhasTitulos.Cells)
            {
                switch (cell.StringCellValue)
                {
                    case "Id":
                        TituloIdNF = cell;
                        break;
                    case "Descricao":
                        TituloDescricaoNF = cell;
                        break;
                    case "EmitirNF":
                        TituloEmitirNF = cell;
                        break;
                    case "ClienteId":
                        TituloClienteId = cell;
                        break;
                    default:
                        break;
                }
            }

            string descricaoNF = "";
            bool emitirNF = true;
            long clienteIdNF = 0;

            for (int rowNum = segundaLinhaIndex; rowNum <= ultimaLinhaIndex; rowNum++)
            {
                //Recupera a linha atual
                IRow linha = tabelaNF.GetRow(rowNum);

                foreach (ICell cell in linha.Cells)
                {
                    if (cell.ColumnIndex == TituloDescricaoNF.ColumnIndex)
                        descricaoNF = cell.StringCellValue;

                    if (cell.ColumnIndex == TituloEmitirNF.ColumnIndex)
                    {
                        if (cell.StringCellValue.ToUpper() == "SIM")
                            emitirNF = true;
                        if (cell.StringCellValue.ToUpper() == "NAO" || cell.StringCellValue.ToUpper() == "NÃO")
                            emitirNF = false;
                    }

                    if (cell.ColumnIndex == TituloClienteId.ColumnIndex)
                        clienteIdNF = Convert.ToInt32(cell.NumericCellValue);
                }

                NotaFiscalCondicao nfc = null;

                if (!string.IsNullOrEmpty(descricaoNF))
                    nfc = new NotaFiscalCondicao(descricaoNF, emitirNF, clienteIdNF);
                else
                    nfc = new NotaFiscalCondicao(emitirNF, clienteIdNF);

                notasFiscais.Add(nfc);
            }
            // Adiciona todas as notasFiscais no banco
            _context.NotasFiscaisCondicoes.AddRange(notasFiscais);

            // Boletos
            ISheet tabelaBoletos = excel.GetSheet("Boletos");

            if (tabelaBoletos == null)
            {
                throw new Exception("A planilha Boletos não foi encontrada");
            }

            // Pega o index da primeira linha (Titulos) dessa tabela
            primeiraLinhaIndexTitulos = tabelaBoletos.FirstRowNum;

            // Pega o index da seguna linha (1° Registro) dessa tabela
            segundaLinhaIndex = (tabelaBoletos.FirstRowNum + 1);

            // Pega o index da ultima linha (Ultimo Registro) dessa tabela
            ultimaLinhaIndex = tabelaBoletos.LastRowNum;

            ICell TituloIdBO = null;
            ICell TituloDescricaoBO = null;
            TituloClienteId = null;

            List<BoletoCondicao> boletos = new List<BoletoCondicao>();

            linhasTitulos = tabelaBoletos.GetRow(primeiraLinhaIndexTitulos);

            foreach (ICell cell in linhasTitulos.Cells)
            {
                switch (cell.StringCellValue)
                {
                    case "Id":
                        TituloIdBO = cell;
                        break;
                    case "Descricao":
                        TituloDescricaoBO = cell;
                        break;
                    case "ClienteId":
                        TituloClienteId = cell;
                        break;
                    default:
                        break;
                }
            }

            string descricaoBO = "";
            long clienteIdBO = 0;

            for (int rowNum = segundaLinhaIndex; rowNum <= ultimaLinhaIndex; rowNum++)
            {
                //Recupera a linha atual
                IRow linha = tabelaBoletos.GetRow(rowNum);

                foreach (ICell cell in linha.Cells)
                {
                    if (cell.ColumnIndex == TituloDescricaoBO.ColumnIndex)
                        descricaoBO = cell.StringCellValue;

                    if (cell.ColumnIndex == TituloClienteId.ColumnIndex)
                        clienteIdBO = Convert.ToInt32(cell.NumericCellValue);
                }

                BoletoCondicao bc = null;
                try
                {
                    bc = new BoletoCondicao(descricaoBO, clienteIdBO);
                }
                catch (Exception)
                {
                    throw new Exception("Houve um erro ao instanciar a classe BoletoCondicao");
                }

                boletos.Add(bc);
            }
            // Adiciona todos os BoletosCondicoes no banco
            _context.BoletosCondicoes.AddRange(boletos);

            // Contatos
            ISheet tabelaContatos = excel.GetSheet("Contatos");

            if (tabelaContatos == null)
            {
                throw new Exception("A planilha Contatos não foi encontrada");
            }

            // Pega o index da primeira linha (Titulos) dessa tabela
            primeiraLinhaIndexTitulos = tabelaContatos.FirstRowNum;

            // Pega o index da seguna linha (1° Registro) dessa tabela
            segundaLinhaIndex = (tabelaContatos.FirstRowNum + 1);

            // Pega o index da ultima linha (Ultimo Registro) dessa tabela
            ultimaLinhaIndex = tabelaContatos.LastRowNum;

            ICell TituloIdCO = null;
            ICell TituloTelefoneCO = null;
            ICell TituloNomesDonosCO = null;
            TituloClienteId = null;

            List<Contato> contatos = new List<Contato>();

            linhasTitulos = tabelaContatos.GetRow(primeiraLinhaIndexTitulos);

            foreach (ICell cell in linhasTitulos.Cells)
            {
                switch (cell.StringCellValue)
                {
                    case "Id":
                        TituloIdCO = cell;
                        break;
                    case "Telefone":
                        TituloTelefoneCO = cell;
                        break;
                    case "NomesDonos":
                        TituloNomesDonosCO = cell;
                        break;
                    case "ClienteId":
                        TituloClienteId = cell;
                        break;
                    default:
                        break;
                }
            }

            string telefoneCO = "";
            string nomesDonosCO = "";
            long clienteIdCO = 0;

            for (int rowNum = segundaLinhaIndex; rowNum <= ultimaLinhaIndex; rowNum++)
            {
                //Recupera a linha atual
                IRow linha = tabelaContatos.GetRow(rowNum);

                foreach (ICell cell in linha.Cells)
                {
                    if (cell.ColumnIndex == TituloTelefoneCO.ColumnIndex)
                        telefoneCO = cell.NumericCellValue.ToString();

                    if (cell.ColumnIndex == TituloNomesDonosCO.ColumnIndex)
                    {
                        if (!string.IsNullOrEmpty(cell.StringCellValue))
                            nomesDonosCO = cell.StringCellValue;
                    }

                    if (cell.ColumnIndex == TituloClienteId.ColumnIndex)
                        clienteIdCO = Convert.ToInt32(cell.NumericCellValue);
                }

                Contato con = null;
                try
                {
                    con = new Contato(telefoneCO, nomesDonosCO, clienteIdCO);
                }
                catch (Exception)
                {
                    throw new Exception("Houve um erro ao instanciar a classe Contato");
                }

                contatos.Add(con);
            }
            // Adiciona todos os contatos no banco
            _context.Contatos.AddRange(contatos);

            return clientes;
        }
    }
}
