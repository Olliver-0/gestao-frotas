using System;
using System.Collections.Generic;
using System.Globalization;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;


namespace GestaoFrotas.ConsoleApp.Views
{
    public class PecaView
    {
        public void ExibirMenuPeca()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine(" GESTÃO DE PEÇAS ");
                Console.WriteLine("=========================================");
                Console.WriteLine();
                Console.WriteLine("Selecione uma opção:");
                Console.WriteLine("1 - Cadastrar Nova Peça");
                Console.WriteLine("2 - Consultar Peça");
                Console.WriteLine("3 - Editar Peça");
                Console.WriteLine("4 - Listar Todas as Peças");
                Console.WriteLine();
                Console.WriteLine("0 - Voltar ao Menu Principal");
                Console.WriteLine();
                Console.Write("Digite sua opção: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        CadastrarPeca();
                        break;
                    case "2":
                        // ConsultarPeca();
                        break;
                    case "3":
                        // EditarPeca();
                        break;
                    case "4":
                        // ExcluirPeca();
                        break;
                    case "5":
                        // ListarPeca();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida! Tente novamente.");
                        break;
                }
            }
        }
         Peca novoPeca = new Peca();

        private void CadastrarPeca()
        {
            Console.Clear();

            Console.WriteLine(" ===== CADASTRAR PEÇA =====");
            Console.WriteLine("Para cancelar, digite 0 a qualquer momento");

            Console.WriteLine("Digite o nome: ");
            novoPeca.nome = Console.ReadLine();
            Console.WriteLine("Digite a quantidade: ");
            novoPeca.quantidadeEstoque = int.Parse(Console.ReadLine());
            Console.WriteLine("Digite a Descrição: ");
            novoPeca.descricao = Console.ReadLine();
            Console.WriteLine("Estoque minimo: ");
            novoPeca.pontoReposicao = int.Parse(Console.ReadLine());
           
           

        

            Console.WriteLine("=========================================");
            Console.WriteLine(" CADASTRAR NOVA PEÇA ");
            Console.WriteLine("=========================================");



            Console.WriteLine("");
        }
    }
}
