using System;
using System.Linq;                
using System.Collections.Generic;
using GestaoFrotas.ConsoleApp.Models;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace GestaoFrotas.ConsoleApp.Services
{
    public class PecaService 
    {
        private List<Peca> _pecas = new List<Peca>();

        public string CadastrarPeca(Peca peca)
        {
            int pecaJaCadastrada;
            pecaJaCadastrada = buscaPecaNome(peca.nome);
            
            if(pecaJaCadastrada != 0) return $"Peça ja cadastrada - ID {pecaJaCadastrada}";
            
            if (!int.TryParse(peca.quantidadeEstoque, out int quantidade))
                return "Erro: quantidade deve ser um número!";
            if (quantidade < 0)
                return "Erro: quantidade não pode ser negativa!";

            if (!int.TryParse(peca.pontoReposicao, out int Rep))
                return "Erro: A quantidade minima deve ser um número!";
            if (quantidade < 0)
                return "Erro: A quantidade minima não pode ser negativa!";
    
            if (_pecas.Count == 0)
            {
                peca.id = 1;
                _pecas.Add(peca);
            }
            else
            {
                peca.id = _pecas.Count + 1;
                _pecas.Add(peca);
            }
            return "Peça cadastrada com sucesso!";
        }

        public Peca buscarPecaID(int id)
        {
            foreach (var peca in _pecas)
            {
                if (peca.id == id)
                {
                    return peca;
                }
            }
            return null;
        }
        
        public int buscaPecaNome(string nome)
        {
            foreach (var peca in _pecas)
            {
                if (peca.nome == nome)
                {
                    return peca.id;
                }
            }
            return 0;
        }


        public string editarPeca(Peca peca, string novoNome, string novaQuant, string novaDesc, string novoEstMin, string op)
        {
            if (string.IsNullOrWhiteSpace(novoNome)) return "O nome não deve estar vazio";
            if (!int.TryParse(novaQuant, out int novaQuantInt) || novaQuantInt < 0) return "A nova quantidade precisa ser um número maior ou igual a 0";
            if (!int.TryParse(novoEstMin, out int novoEstMinInt) || novoEstMinInt < 0) return"O novo estoque mínimo precisa ser um número maior ou igual a 0";
            if (op == "S" || op == "s")
            {
                peca.nome = novoNome;
                peca.quantidadeEstoque = novaQuantInt.ToString();
                peca.descricao = novaDesc;
                peca.pontoReposicao = novoEstMinInt.ToString();

                return "Peça atualizada com sucesso!";
            }
            else if(op == "N" || op == "n" ) return "\nAlterações canceladas.";
            else return "\nOpção invalida"; 
        }
        public void excluirPeca()
        {
            Console.WriteLine("Simulação: exclusão de peça ainda não implementada.");
        }

        public List<Peca> ListarPecas()
        {
            return _pecas.ToList(); 
        }
    }
}


