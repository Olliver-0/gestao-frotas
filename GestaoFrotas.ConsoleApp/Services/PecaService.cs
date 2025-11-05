using System;
using System.Linq;                 // se for usar FirstOrDefault no futuro
using System.Collections.Generic;
using GestaoFrotas.ConsoleApp.Models;

namespace GestaoFrotas.ConsoleApp.Services
{
    public class PecaService    // <- sem parênteses
    {
        private readonly List<Peca> _pecas = new();

        // ⬇️ SEU MÉTODO FICA AQUI DENTRO, entre as chaves da classe
        public Peca? buscarPeca(string cod)
        {
            Console.WriteLine("Simulação: busca ainda não implementada.");
            return new Peca(); 
        }


        public void editarPeca(int id)
        {
            Console.WriteLine("Simulação: edição de peça ainda não implementada.");
        }

        public void excluirPeca(int id)
        {
            Console.WriteLine("Simulação: exclusão de peça ainda não implementada.");
        }
    }
}


