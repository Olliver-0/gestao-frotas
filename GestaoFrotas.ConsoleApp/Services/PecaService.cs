using System;
using System.Linq;                
using System.Collections.Generic;
using GestaoFrotas.ConsoleApp.Models;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;
using System.Data;

namespace GestaoFrotas.ConsoleApp.Services
{
    public class PecaService 
    {
        private List<Peca> _pecas = new List<Peca>();
       
       public string CadastrarPeca(Peca peca)
        {
            if (string.IsNullOrEmpty(peca.nome)){
                return "O campo nome não pode ser vazio";}

            if (peca.quantidadeEstoque < 0){
                return "Quantidade inválida";}

            if (peca.pontoReposicao < 0){
                return "Ponto de reposição inválido";}

            if (_pecas.Count == 0)
                peca.id = 1;
            else
                peca.id = _pecas.Count + 1;

            _pecas.Add(peca);
            return "Peça cadastrada com sucesso!";
        }  
        
        public Peca buscarPeca(int id)
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

        public List<Peca> ListarPecas()
        {
            return _pecas.ToList(); 
        }
    }
}


