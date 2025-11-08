using System;
using System.Linq;
using System.Collections.Generic;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Views;

namespace GestaoFrotas.ConsoleApp.Services
{
    public class PecaService
    {
        private static PecaView _pecaView = new PecaView();
        private static int _proximoId = 1;
        private List<Peca> _pecas = new List<Peca>();
//=====================================================================================
        public string CadastrarPeca(Peca peca)
        {
            int pecaJaCadastrada;
            pecaJaCadastrada = buscaPecaNome(peca.nome);

            if (pecaJaCadastrada != 0)
                return $"ERRO- PEÇA JA CADASTRADA - ID {pecaJaCadastrada}";
            if (!int.TryParse(peca.quantidadeEstoque, out int QuantEst) || QuantEst < 0)
                return "ERRO- A QUANTIDADE DEVE SER UM NÚMERO MAIOR OU IGUAL A 0!!";
            if (!int.TryParse(peca.pontoReposicao, out int PontRep) || PontRep < 0)
                return "ERRO- A QUANTIDADE MINIMA DEVE SER UM NÚMERO MAIOR OU IGUAL A 0!!";
            if (_pecas.Count == 0)
            {
                peca.id = 1;
                _pecas.Add(peca);
            }
            else
            {
                _proximoId++;
                peca.id = _proximoId;
                _pecas.Add(peca);
            }
            return "PEÇA CADASTRADA COM SUCESSO!!";
        }
//=====================================================================================
        public Peca buscarPecaID(string id, out int entradaInvalida)
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
            foreach (var peca in _pecas)
            {
                if (peca.id == ID)
                    return peca;
            }
            return null;
        }
//=====================================================================================          
        public int buscaPecaNome(string nome)
        {
            foreach (var peca in _pecas)
            {
                if (peca.nome == nome)
                    return peca.id;
            }
            return 0;
        }
//=====================================================================================
        public string EditarPeca(Peca peca, string novoNome, string novaQuant, string novaDesc, string novoEstMin, string op)
        {
            if (string.IsNullOrWhiteSpace(novoNome))
                return "ERRO- O NOME NÃO DEVE ESTAR VAZIO!!";
            int resp = buscaPecaNome(novoNome);
            int idPeca = peca.id;

            if (resp != 0 && resp != idPeca)
                return "ERRO — NOME JÁ CADASTRADO";
            if (!int.TryParse(novaQuant, out int novaQuantInt) || novaQuantInt < 0)
                return "ERRO- A NOVA QUANTIDADE DEVE SER UM NÚMERO MAIOR OU IGUAL A 0!!";
            if (!int.TryParse(novoEstMin, out int novoEstMinInt) || novoEstMinInt < 0)
                return "ERRO- O NOVO ESTOQUE MINIMO DEVE SER UM NÚMERO MAIOR OU IGUAL A 0!!";
            if (op == "S" || op == "s")
            {
                peca.nome = novoNome;
                peca.quantidadeEstoque = novaQuantInt.ToString();
                peca.descricao = novaDesc;
                peca.pontoReposicao = novoEstMinInt.ToString();
                return "PEÇA ATUALIZADA COM SUCESSO!!";
            }
            else if (op == "N" || op == "n")
                return "\nALTERAÇÕES CANCELADAS!!";
            else
                return "\nOPÇÃO INVÁLIDA";
        }

//=====================================================================================       
        public string ExcluirPeca(Peca pecaExcluir, string op)
        {
            if (op.ToUpper() == "S")
            {
                bool pecaApagada;
                pecaApagada = _pecas.Remove(pecaExcluir);
                if (pecaApagada == true) return "PEÇA EXCLUIDA COM SUCESSO!!";
                else
                    return "ERRO- PEÇA NÃO EXCLUIDA!!";
            }
            else if (op.ToUpper() == "N")
                return "OPERAÇÃO CANCELADA!!";
            else
                return "ERRO- OPÇÃO INVÁLIDA!!";

        }

//=====================================================================================       

        public List<Peca> ListarPecas()
        {
            if (_pecas.Count == 0)
                return null;
            else
                return _pecas.ToList();

        }

//=====================================================================================

        public string AdicionarEstoque(Peca peca, string quant, string op)
        {
            if (op == "S" || op == "s")
            {
                if (!int.TryParse(quant, out int Quant))
                    return "ERRO- A QUANTIDADE ACRESCENTADA DEVE SER UM NÚMERO!!";
                if (Quant < 0)
                    return "ERRO- A QUANTIDADE ACRESCENTADA NÃO PODE SER NEGATIVA!!";
                int EstAnt = int.Parse(peca.quantidadeEstoque);
                EstAnt += Quant;
                peca.quantidadeEstoque = EstAnt.ToString();
                return "\nQUANTIDADE ACRESCENTADA COM SUCESSO!!";
            }
            if (op == "N" || op == "n")
                return "\nINCLUSÃO CANCELADA!!";

            return null;
        }
    }
}
