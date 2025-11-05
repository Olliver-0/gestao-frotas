using System;
using System.Linq;                 // se for usar FirstOrDefault no futuro
using System.Collections.Generic;
using GestaoFrotas.ConsoleApp.Models;

namespace GestaoFrotas.ConsoleApp.Services
{
    public class PecaService 
    {
        private List<Peca> _pecas = new List<Peca>();
       
        public string cadastrarPeca (Peca peca)
        {
           
            int posicao = peca.id - 1;

            if (string.IsNullOrEmpty(peca.nome))
                {return "O campo nome não pode ser vazio" ;}
            
            if (peca.quantidadeEstoque < 0){
                return "Quantidade invalida";}

            if (peca.pontoReposicao < 0){
                return "Estoque minimo inválido";}

            if (_pecas.Count == 0) peca.id = 1;
            else { peca.id = _pecas.Count + 1; }

            _pecas.Add(peca);

            return "Peca cadastrada com sucesso!!";
            
        }
            
        
        public Peca buscarPeca(string cod)
        {
            Console.WriteLine("Simulação: busca ainda não implementada.");
            return new Peca(); 
        }


        public void editarPecaConsult(Peca peca, string novoNome, string novaQuant, string novaDesc, string novoEstMin)
        {
            
            if (novoNome != "") peca.nome = novoNome;
            if (novaQuant != "") peca.quantidadeEstoque = int.Parse(novaQuant);
            if (novaDesc != "") peca.descricao = novaDesc;
            if (novoEstMin != "") peca.pontoReposicao = int.Parse(novoEstMin);
        }

       public void editarPecaMenu(Peca peca, string nome, string quant, string desc, string estMin)
        {
            
        }


        public void excluirPeca()
        {
            Console.WriteLine("Simulação: exclusão de peça ainda não implementada.");
        }
    }
}


