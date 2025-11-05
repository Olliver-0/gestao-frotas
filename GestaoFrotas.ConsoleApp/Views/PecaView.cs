using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;
using GestaoFrotas.ConsoleApp.Views;


namespace GestaoFrotas.ConsoleApp.Views
{
    public class PecaView
    {
        private readonly PecaService _pecaService = new PecaService();
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
                         ConsultarPeca();
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
            Peca novaPeca = new Peca();
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine(" CADASTRAR NOVA PEÇA ");
            Console.WriteLine("=========================================");
            Console.WriteLine("Para cancelar deixa o nome em branco e pressione enter");


            Console.WriteLine("Digite o nome: ");
            novaPeca.nome = Console.ReadLine();
            if (string.IsNullOrEmpty(novaPeca.nome)) return;
            Console.WriteLine("Digite a quantidade: ");
            novaPeca.quantidadeEstoque = int.Parse(Console.ReadLine());
            Console.WriteLine("Digite a Descrição: ");
            novaPeca.descricao = Console.ReadLine();
            Console.WriteLine("Estoque minimo: ");
            novaPeca.pontoReposicao = int.Parse(Console.ReadLine());
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
            Console.WriteLine(" PEÇA CADASTRADA COM SUCESSO ");

        }

        private Peca? ConsultarPeca()
        {
            string cod;
            int opBuscar, opAcao;
            Peca buscarPeca = null;

            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine(" CONSULTAR PEÇA ");
            Console.WriteLine("=========================================");

            Console.WriteLine("1- Buscar por ID");
            Console.WriteLine("2- Buscar por nome");
            // troque só esta linha:
            if (!int.TryParse(Console.ReadLine(), out opBuscar))
            { 
                Console.WriteLine("Opção inválida"); return null; 
            }

            if (opBuscar == 1)
            {
                Console.WriteLine("Digite o ID da peça: ");
                cod = Console.ReadLine();
                buscarPeca = _pecaService.buscarPeca(cod);
            }
            else if (opBuscar == 2)
            {
                Console.WriteLine("Digite nome da peça: ");
                cod = Console.ReadLine();
                buscarPeca = _pecaService.buscarPeca(cod);
            }
            else
            {
                Console.WriteLine("Opção inválida");
                return null;
            }

            if (buscarPeca == null)
            {
                Console.WriteLine("\n=========================================");
                Console.WriteLine(" ERRO - PEÇA NÃO ENCONTRADA");
                Console.WriteLine("=========================================");
                return null;
            }
            else
            {
                Console.WriteLine($"ID: {buscarPeca.id}");
                Console.WriteLine($"Nome: {buscarPeca.nome}");
                Console.WriteLine($"Quantidade em Estoque: {buscarPeca.quantidadeEstoque}");
                Console.WriteLine($"Ponto de Reposição: {buscarPeca.pontoReposicao}");

                Console.WriteLine("1- Editar peça");
                Console.WriteLine("2- Excluir peça");

                if (!int.TryParse(Console.ReadLine(), out opAcao))
                {
                    Console.WriteLine("Opção inválida");
                    return buscarPeca;
                }

                if (opAcao == 1)
                {
                    _pecaService.editarPeca(buscarPeca.id);
                }
                if (opAcao == 2)
                {
                    _pecaService.excluirPeca(buscarPeca.id);
                }
                return buscarPeca;
            }
        }
    }
}
    


