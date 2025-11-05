using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;
using GestaoFrotas.ConsoleApp.Views;
using Microsoft.VisualBasic;


namespace GestaoFrotas.ConsoleApp.Views
{
    public class PecaView
    {
        private readonly PecaService _pecaService = new PecaService();
        //private readonly VeiculoView _veiculoView = new VeiculoView(new VeiculoService());
        public void viewExibirMenuPeca()
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
                        ViewCadastrarPeca();
                        break;
                    case "2":
                        ViewConsultarPeca();
                        break;
                    case "3":
                        ViewEditarPeca();
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
        private void ViewCadastrarPeca()
        {
            string msg;

            Peca novaPeca = new Peca();
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine(" CADASTRAR NOVA PEÇA ");
            Console.WriteLine("=========================================");
            Console.WriteLine("Para cancelar deixa o nome em branco e pressione enter");

            MostrarMsg("Digite o nome: ");
            novaPeca.nome = Console.ReadLine();
            if (string.IsNullOrEmpty(novaPeca.nome)) return;
            Console.WriteLine("Digite a quantidade: ");
            novaPeca.quantidadeEstoque = int.Parse(Console.ReadLine());
            Console.WriteLine("Digite a Descrição: ");
            novaPeca.descricao = Console.ReadLine();
            Console.WriteLine("Estoque minimo: ");
            novaPeca.pontoReposicao = int.Parse(Console.ReadLine());

            msg = _pecaService.cadastrarPeca(novaPeca);

            MostrarMsg(msg);
        }

        //---------------------------------------------------------------------------------------------------


        private Peca ViewConsultarPeca()
        {
            string cod;
            int opAcao;
            Peca buscarPeca = null;

            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine(" CONSULTAR PEÇA ");
            Console.WriteLine("=========================================");

            Console.WriteLine("Digite o ID da peça: ");
            cod = Console.ReadLine();
            buscarPeca = _pecaService.buscarPeca(cod);

            if (buscarPeca == null)
            {
                Console.WriteLine("\n=======================================");
                Console.WriteLine(" ERRO - PEÇA NÃO ENCONTRADA");
                Console.WriteLine("=========================================");
                return null;
            }
            else
            {
                Console.WriteLine($"ID: {buscarPeca.id}");
                Console.WriteLine($"Nome: {buscarPeca.nome}");
                Console.WriteLine($"Descrição: {buscarPeca.descricao}");
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
                    ViewEditarPecaConsult(buscarPeca);
                }
                if (opAcao == 2)
                {
                    //xcluirPeca();
                }
                return buscarPeca;
            }
        }
        

//---------------------------------------------------------------------------------------------------


        private Peca ViewEditarPeca()
        {
            string cod;
            string novoNome;
            string novaQuant;  
            string novaDesc;
            string novoEstMin;  
            string op;

            Peca buscarPeca = null;

            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine(" EDITAR PEÇA ");
            Console.WriteLine("=========================================");

            Console.WriteLine("Digite o ID da peça: ");
            cod = Console.ReadLine();

            if (!int.TryParse(cod, out int id))
            {
                Console.WriteLine("\n=======================================");
                Console.WriteLine(" ERRO - ID INVALIDO");
                Console.WriteLine("=========================================");
                return null;
            }

            buscarPeca = _pecaService.buscarPeca(cod);

            if (buscarPeca == null)
            {
                Console.WriteLine("\n=======================================");
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
            }

            Console.WriteLine("\n=======================================");
            Console.WriteLine(" NOVOS DADOS ");
            Console.WriteLine("=========================================");


            Console.WriteLine("Digite o novo nome: ");
            novoNome = Console.ReadLine();
            Console.WriteLine("Digite a nova quantidade: ");
            Console.Write("Digite a nova quantidade: ");
            novaQuant = Console.ReadLine();   
            Console.WriteLine("Digite a nova Descrição: ");
            novaDesc = Console.ReadLine();
            Console.WriteLine("Estoque minimo novo: ");
            novoEstMin = Console.ReadLine();

            Console.WriteLine("Confirmar alterações? (S/N)");
            op = Console.ReadLine();

            if (op == "S" || op == "s")
            {
                _pecaService.editarPecaMenu(buscarPeca, novoNome, novaQuant, novaDesc, novoEstMin);
                Console.WriteLine("Dados salvos com sucesso!!");
                return buscarPeca;    
            }
            else if (op == "N" || op == "n")
            {
                Console.WriteLine("Operação cancelada");
                return null;
            }
            else
            {
                Console.WriteLine("Opção invalida");
                return null;
            }
        }

        //---------------------------------------------------------------------------------------------------

        private void ViewEditarPecaConsult(Peca peca)
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine(" EDITAR PEÇA ");
            Console.WriteLine("=========================================");

            Console.WriteLine($"ID: {peca.id}");
            Console.WriteLine($"Nome atual: {peca.nome}");
            Console.WriteLine($"Quantidade atual: {peca.quantidadeEstoque}");
            Console.WriteLine($"Descrição atual: {peca.descricao}");
            Console.WriteLine($"Estoque mínimo atual: {peca.pontoReposicao}");

            Console.Write("Novo nome: ");
            var nomeNovo = Console.ReadLine();
            Console.Write("Nova quantidade: ");
            var novaQuant = Console.ReadLine();
            Console.Write("Nova descrição: ");
            var novaDesc = Console.ReadLine();
            Console.Write("Novo estoque mínimo: ");
            var novoEstMin = Console.ReadLine();
            Console.Write("\nConfirmar alterações? (S/N): ");
            var op = Console.ReadLine();

            if (op == "S" || op == "s")
            {
                _pecaService.editarPecaConsult(peca, nomeNovo, novaQuant, novaDesc, novoEstMin);
                Console.WriteLine("\nDados salvos com sucesso!");
            }
            else
            {
                Console.WriteLine("\nOperação cancelada.");
            }
        }
        
        public void MostrarMsg(string mensagem)
        {
            Console.WriteLine(mensagem);
        }
    }
}
    


