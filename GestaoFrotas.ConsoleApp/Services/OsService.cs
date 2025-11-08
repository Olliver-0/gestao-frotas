using System;
using System.Linq;
using System.Collections.Generic;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Views;

namespace GestaoFrotas.ConsoleApp.Services
{
    public class OsService
    {
        private static OsView _osView = new OsView();
        private static int _proximoId = 1;
        private List<OrdemDeServico> _ordemDeServico = new List<OrdemDeServico>();
        public string CadastrarOS(OrdemDeServico OsService)
        {
            // int pecaJaCadastrada;
            // pecaJaCadastrada = buscaPecaNome(peca.nome);

            /*if (pecaJaCadastrada != 0)
                return $"ERRO- PEÇA JA CADASTRADA - ID {pecaJaCadastrada}";
            if (!int.TryParse(peca.quantidadeEstoque, out int QuantEst) || QuantEst < 0)
                return "ERRO- A QUANTIDADE DEVE SER UM NÚMERO MAIOR OU IGUAL A 0!!";
            if (!int.TryParse(peca.pontoReposicao, out int PontRep) || PontRep < 0)
                return "ERRO- A QUANTIDADE MINIMA DEVE SER UM NÚMERO MAIOR OU IGUAL A 0!!";*/
            if (_ordemDeServico.Count == 0)
            {

                if (OsService.tipo == "1")
                    OsService.tipo = "Corretiva";
                else if (OsService.tipo == "2")
                    OsService.tipo = "Preventiva";
                    
                OsService.id = 1;
                _ordemDeServico.Add(OsService);
            }
            else
            {
                if (OsService.tipo == "1")
                    OsService.tipo = "Corretiva";
                else if (OsService.tipo == "2")
                    OsService.tipo = "Preventiva";

                _proximoId++;
                OsService.id = _proximoId;
                _ordemDeServico.Add(OsService);
            }
            return "ORDEM DE SERVIÇO CADASTRADA COM SUCESSO!!";
        }

        public OrdemDeServico buscarOsID(string id, out int entradaInvalida)
        {
            entradaInvalida = 0;
            if (string.IsNullOrWhiteSpace(id))
            {
                entradaInvalida = 1;
                return null;
            }
            if (!int.TryParse(id, out int ID))
            {
                entradaInvalida = 2;
                return null;
            }
            foreach (var OS in _ordemDeServico)
            {
                if (OS.id == ID)
                    return OS;
            }
            return null;
        }

        public string EditarOs(OrdemDeServico ordemDeServico,
                               string novaDataAbert,
                               string novoTipoManut,
                               string novoVeic,
                               string novoIdMec,
                               string novaDataFecham,
                               string novaPeca,
                               string op)
        {
            /*if (string.IsNullOrWhiteSpace(novoNome))
                return "ERRO- O NOME NÃO DEVE ESTAR VAZIO!!";
            int resp = buscaPecaNome(novoNome);
            int idPeca = peca.id;

            if (resp != 0 && resp != idPeca)
                return "ERRO — NOME JÁ CADASTRADO";
            if (!int.TryParse(novaQuant, out int novaQuantInt) || novaQuantInt < 0)
                return "ERRO- A NOVA QUANTIDADE DEVE SER UM NÚMERO MAIOR OU IGUAL A 0!!";
            if (!int.TryParse(novoEstMin, out int novoEstMinInt) || novoEstMinInt < 0)
                return "ERRO- O NOVO ESTOQUE MINIMO DEVE SER UM NÚMERO MAIOR OU IGUAL A 0!!";*/
            if (op == "S" || op == "s")
            {
                ordemDeServico.dataAbertura = novaDataAbert;
                ordemDeServico.tipo = novoTipoManut;
                ordemDeServico.veiculoId = novoVeic;
                ordemDeServico.mecanicoId = novoIdMec;
                ordemDeServico.dataFechamento = novaDataFecham;
                ordemDeServico.pecas = novaPeca;
                return "ORDEM DE SERVIÇO ATUALIZADA COM SUCESSO!!";
            }
            else if (op == "N" || op == "n")
                return "\nALTERAÇÕES CANCELADAS!!";
            else
                return "\nOPÇÃO INVÁLIDA";
        }
        
        public string ExcluirOs(OrdemDeServico OsExcluir, string op)
        {
            if (op.ToUpper() == "S")
            {
                bool pecaApagada;
                pecaApagada = _ordemDeServico.Remove(OsExcluir);
                if (pecaApagada == true) return "ORDEM DE SERVIÇO EXCLUIDA COM SUCESSO!!";
                else
                    return "ERRO- ORDEM DE SERVIÇO NÃO EXCLUIDA!!";
            }
            else if (op.ToUpper() == "N")
                return "OPERAÇÃO CANCELADA!!";
            else
                return "ERRO- OPÇÃO INVÁLIDA!!";
        }

        public List<OrdemDeServico> ListarOS()
        {
            if (_ordemDeServico.Count == 0)
                return null;
            else
                return _ordemDeServico.ToList();

        }
    }
}