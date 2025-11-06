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
        private static PecaService _pecaService = new PecaService();
        public void ViewExibirMenuPeca()
        {
            while (true)
            {
                Console.Clear();
                MostrarMsg("=========================================");
                MostrarMsg(" GESTÃO DE PEÇAS ");
                MostrarMsg("=========================================");
                MostrarMsg("");
                MostrarMsg("Selecione uma opção:");
                MostrarMsg("1 - Cadastrar Nova Peça");
                MostrarMsg("2 - Consultar Peça");
                MostrarMsg("3 - Editar Peça");
                MostrarMsg("4 - Excluir Peça");
                MostrarMsg("5 - Listar Todas as Peças");
                MostrarMsg("");
                MostrarMsg("0 - Voltar ao Menu Principal");
                MostrarMsg("");
                MostrarMsg("Digite sua opção: ");

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
                         
                        ListarPeca();
                        break;
                    case "0":
                        return;
                    default:
                        MostrarMsg("Opção inválida! Tente novamente.");
                        break;
                }
            }
        }
        private void ViewCadastrarPeca()
        {
            string msg;

            Peca novaPeca = new Peca();
            Console.Clear();
            MostrarMsg("=========================================");
            MostrarMsg(" CADASTRAR NOVA PEÇA ");
            MostrarMsg("=========================================");
            MostrarMsg("Para cancelar deixa o nome em branco e pressione enter");

            MostrarMsg("Digite o nome: ");
            novaPeca.nome = Console.ReadLine();
            if (string.IsNullOrEmpty(novaPeca.nome)) return;
            MostrarMsg("Digite a quantidade: ");
            novaPeca.quantidadeEstoque = int.Parse(Console.ReadLine());
            MostrarMsg("Digite a Descrição: ");
            novaPeca.descricao = Console.ReadLine();
            MostrarMsg("Estoque minimo: ");
            novaPeca.pontoReposicao = int.Parse(Console.ReadLine());

            msg = _pecaService.CadastrarPeca(novaPeca);

            MostrarMsg(msg);
            Pausar();
        }

        //---------------------------------------------------------------------------------------------------


        private Peca ViewConsultarPeca()
        {
            int cod;
            int opAcao;
            Peca buscarPeca = null;

            Console.Clear();
            MostrarMsg("=========================================");
            MostrarMsg(" CONSULTAR PEÇA ");
            MostrarMsg("=========================================");

            MostrarMsg("Digite o ID da peça: ");
            cod = int.Parse(Console.ReadLine());
            buscarPeca = _pecaService.buscarPeca(cod);

            if (buscarPeca == null)
            {
                MostrarMsg("\n=======================================");
                MostrarMsg(" ERRO - PEÇA NÃO ENCONTRADA");
                MostrarMsg("=========================================");
                Pausar();
                return null;
            }
            else
            {
                MostrarMsg($"ID: {buscarPeca.id}");
                MostrarMsg($"Nome: {buscarPeca.nome}");
                MostrarMsg($"Descrição: {buscarPeca.descricao}");
                MostrarMsg($"Quantidade em Estoque: {buscarPeca.quantidadeEstoque}");
                MostrarMsg($"Ponto de Reposição: {buscarPeca.pontoReposicao}");

                MostrarMsg("1- Editar peça");
                MostrarMsg("2- Excluir peça");

                if (!int.TryParse(Console.ReadLine(), out opAcao))
                {
                    MostrarMsg("Opção inválida");
                    Pausar();
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
            int cod;
            string novoNome;
            string novaQuant;  
            string novaDesc;
            string novoEstMin;  
            string op;

            Peca buscarPeca = null;

            Console.Clear();
            MostrarMsg("=========================================");
            MostrarMsg(" EDITAR PEÇA ");
            MostrarMsg("=========================================");

            MostrarMsg("Digite o ID da peça: \n");
            cod = int.Parse(Console.ReadLine());

            /*if (!int.TryParse(cod, out int id))
            {
                MostrarMsg("\n=======================================");
                MostrarMsg(" ERRO - ID INVALIDO");
                MostrarMsg("=========================================");
                Pausar();
                return null;
            }*/

            buscarPeca = _pecaService.buscarPeca(cod);

            if (buscarPeca == null)
            {
                MostrarMsg("\n=======================================");
                MostrarMsg(" ERRO - PEÇA NÃO ENCONTRADA");
                MostrarMsg("=========================================");
                Pausar();
                return null;
            }
            else
            {
                MostrarMsg($"ID: {buscarPeca.id}");
                MostrarMsg($"Nome: {buscarPeca.nome}");
                MostrarMsg($"Quantidade em Estoque: {buscarPeca.quantidadeEstoque}");
                MostrarMsg($"Ponto de Reposição: {buscarPeca.pontoReposicao}");
            }

            MostrarMsg("\n=======================================");
            MostrarMsg(" NOVOS DADOS ");
            MostrarMsg("=========================================");


            MostrarMsg("Digite o novo nome: ");
            novoNome = Console.ReadLine();
            MostrarMsg("Digite a nova quantidade: ");
            novaQuant = Console.ReadLine();   
            MostrarMsg("Digite a nova Descrição: ");
            novaDesc = Console.ReadLine();
            MostrarMsg("Estoque minimo novo: ");
            novoEstMin = Console.ReadLine();

            MostrarMsg("Confirmar alterações? (S/N)");
            op = Console.ReadLine();

            if (op == "S" || op == "s")
            {
                _pecaService.editarPecaMenu(buscarPeca, novoNome, novaQuant, novaDesc, novoEstMin);
                MostrarMsg("Dados salvos com sucesso!!");
                Pausar();
                return buscarPeca;    
            }
            else if (op == "N" || op == "n")
            {
                MostrarMsg("Operação cancelada");
                Pausar();
                return null;
            }
            else
            {
                MostrarMsg("Opção invalida");
                Pausar();
                return null;
            }
        }

        //---------------------------------------------------------------------------------------------------

        private void ViewEditarPecaConsult(Peca peca)
        {
            Console.Clear();
            MostrarMsg("=========================================");
            MostrarMsg(" EDITAR PEÇA ");
            MostrarMsg("=========================================");

            MostrarMsg($"ID: {peca.id}");
            MostrarMsg($"Nome atual: {peca.nome}");
            MostrarMsg($"Quantidade atual: {peca.quantidadeEstoque}");
            MostrarMsg($"Descrição atual: {peca.descricao}");
            MostrarMsg($"Estoque mínimo atual: {peca.pontoReposicao}");

            MostrarMsg("Novo nome: ");
            var nomeNovo = Console.ReadLine();
            MostrarMsg("Nova quantidade: ");
            var novaQuant = Console.ReadLine();
            MostrarMsg("Nova descrição: ");
            var novaDesc = Console.ReadLine();
            MostrarMsg("Novo estoque mínimo: ");
            var novoEstMin = Console.ReadLine();
            MostrarMsg("\nConfirmar alterações? (S/N): ");
            var op = Console.ReadLine();

            if (op == "S" || op == "s")
            {
                _pecaService.editarPecaConsult(peca, nomeNovo, novaQuant, novaDesc, novoEstMin);
                MostrarMsg("\nDados salvos com sucesso!");
                Pausar();
                return;
            }
            else
            {
                MostrarMsg("\nOperação cancelada.");
                Pausar();
                return;
            }
        }

        public void MostrarMsg(string mensagem)
        {
            Console.WriteLine(mensagem);
        }

        public void Pausar(string msg = "Pressione qualquer tecla para continuar...")
        {
            Console.WriteLine();
            Console.Write(msg);
            Console.ReadKey(true);
        }


        public void ListarPeca()
        {
            Console.Clear();
            MostrarMsg("=========================================");
            MostrarMsg(" LISTAR PEÇAS ");
            MostrarMsg("=========================================");

            List<Peca> pecas = _pecaService.ListarPecas();

            if (pecas.Count == 0)
            {
                MostrarMsg("Nenhuma peça cadastrada");
                Pausar();
                return;
            }
            else
            {
                Console.WriteLine($"{"ID Peça",-10} {"Nome",-10} {"Quantidade",-15} {"Descrição",-12} {"Estoque minimo",-12}");
                Console.WriteLine(new string('-', 70));

                foreach (var dadosPecas in pecas)
                {
                    Console.WriteLine($"{dadosPecas.id,-10} {dadosPecas.nome,-10} {dadosPecas.quantidadeEstoque,-15} {dadosPecas.descricao,-12} {dadosPecas.pontoReposicao,-12}");
                }
                Pausar();
            } 
        }
    }
}

    


