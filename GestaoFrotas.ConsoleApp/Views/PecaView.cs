using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;
using GestaoFrotas.ConsoleApp.Views;


namespace GestaoFrotas.ConsoleApp.Views
{
    public class PecaView
    {
        Peca novaPeca = new Peca();
        private static int proximoId = 1;
        
        //private readonly VeiculoView _veiculoView = new VeiculoView(new VeiculoService());
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
        private void CadastrarPeca()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine(" CADASTRAR NOVA PEÇA ");
            Console.WriteLine("=========================================");

            Console.WriteLine("Digite o nome: ");
            novaPeca.nome = Console.ReadLine();

            Console.WriteLine("Digite a quantidade: ");
            novaPeca.quantidadeEstoque = int.Parse(Console.ReadLine());

            Console.WriteLine("Digite a Descrição: ");
            novaPeca.descricao = Console.ReadLine();
            Console.WriteLine("Estoque minimo: ");
            novaPeca.pontoReposicao = int.Parse(Console.ReadLine());

            if (string.IsNullOrEmpty(novaPeca.nome)) return;

            if (novaPeca.quantidadeEstoque < 0)
            {
                Console.WriteLine("O valor deve ser maior ou igual a zero");
                return;
            }

            if (novaPeca.pontoReposicao < 0)
            {
                Console.WriteLine("O valor deve ser maior ou igual a zero");
                return;
            }
        }
        private void ConsultarPeca()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine(" CONSULTAR PEÇA ");
            Console.WriteLine("=========================================");

            Console.Write

        }
    }

}
