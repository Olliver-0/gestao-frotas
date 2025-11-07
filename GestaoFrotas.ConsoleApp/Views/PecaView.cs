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
            LimparTela();
            while (true)
            {
                LimparTela();
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
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" CADASTRAR NOVA PEÇA ");
            MostrarMsg("=========================================");
            MostrarMsg("Para cancelar deixa o nome em branco e pressione enter");

            MostrarMsg("Digite o nome: ");
            novaPeca.nome = Console.ReadLine();
            if (string.IsNullOrEmpty(novaPeca.nome)) return;
            MostrarMsg("Digite a quantidade: ");
            novaPeca.quantidadeEstoque = Console.ReadLine();
            MostrarMsg("Digite a Descrição: ");
            novaPeca.descricao = Console.ReadLine();
            MostrarMsg("Estoque minimo: ");
            novaPeca.pontoReposicao = Console.ReadLine();

            msg = _pecaService.CadastrarPeca(novaPeca);

            MostrarMsg(msg);
            Pausar();
        }

        //---------------------------------------------------------------------------------------------------


        private Peca ViewConsultarPeca()
        {
            string cod;
            int opAcao;
            Peca buscarPeca = null;
            

            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" CONSULTAR PEÇA ");
            MostrarMsg("=========================================");

            MostrarMsg("Digite o ID da peça: ");
            cod = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(cod))
            {
                MostrarMsg("Nenhum valor digitado");
                Pausar();
                return null;
            }
            if (!int.TryParse(cod, out int id))
            {
                MostrarMsg("ID inválido. Digite apenas números.");
                Pausar();
                return null;
            }

            buscarPeca = _pecaService.buscarPecaID(id);

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
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg($"PEÇA ID: {id} ");
                MostrarMsg("=========================================");

                MostrarMsg($"Nome: {buscarPeca.nome}");
                MostrarMsg($"Quantidade estoque: {buscarPeca.quantidadeEstoque}");
                MostrarMsg($"Descrição: {buscarPeca.descricao}");
                MostrarMsg($"Estoque mínimo: {buscarPeca.pontoReposicao}");

                MostrarMsg("\n1- Editar peça");
                MostrarMsg("2- Excluir peça");
                MostrarMsg("3- Adicionar estoque");
                MostrarMsg("0- Voltar ao menu\n");
                MostrarMsg("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opAcao))
                {
                    MostrarMsg("Opção inválida!");
                    Pausar();
                    return null;
                }

                if (opAcao == 1)
                {
                    ViewEditarPecaConsult(buscarPeca);
                }
                else if (opAcao == 2)
                {
                    //excluirPeca();
                }
                else if(opAcao == 3)
                {
                    ViewAdicionarEstoque(buscarPeca);
                }
                else if (opAcao == 0)
                {
                    return null;
                }
                else
                {
                    MostrarMsg("Opção inválida.");
                    Pausar();
                    return null;
                }

                return buscarPeca;
            }
        }

        

//---------------------------------------------------------------------------------------------------


        private void ViewEditarPeca()
        {
            int cod;
            string novoNome;
            string novaQuant;  
            string novaDesc;
            string novoEstMin;  
            string op;

            Peca buscarPeca = null;

            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" EDITAR PEÇA ");
            MostrarMsg("=========================================");


            MostrarMsg("Digite o ID da peça: \n");
            cod = int.Parse(Console.ReadLine());

            buscarPeca = _pecaService.buscarPecaID(cod);

            if (buscarPeca == null)
            {
                MostrarMsg("\n=======================================");
                MostrarMsg(" ERRO - PEÇA NÃO ENCONTRADA");
                MostrarMsg("=========================================");
                Pausar();
                return;
            }
            else
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg($"PEÇA ID: {cod} ");
                MostrarMsg("=========================================");
                MostrarMsg($"Nome: {buscarPeca.nome}");
                MostrarMsg($"Quantidade em Estoque: {buscarPeca.quantidadeEstoque}");
                MostrarMsg($"Descrição: {buscarPeca.descricao}");
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

            //_pecaService.editarPeca(buscarPeca, novoNome, novaQuant, novaDesc, novoEstMin, op);
            string msg = _pecaService.editarPeca(buscarPeca, novoNome, novaQuant, novaDesc, novoEstMin, op);

            MostrarMsg(msg);
            Pausar();

            /*if (op == "S" || op == "s")
            {
                
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
            }*/
            return;
        }

        //---------------------------------------------------------------------------------------------------

        public void ViewEditarPecaConsult(Peca peca)
        {
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" EDITAR PEÇA ");
            MostrarMsg("=========================================");

            MostrarMsg($"ID: {peca.id}");
            MostrarMsg($"Nome atual: {peca.nome}");
            MostrarMsg($"Quantidade atual: {peca.quantidadeEstoque}");
            MostrarMsg($"Descrição atual: {peca.descricao}");
            MostrarMsg($"Estoque mínimo atual: {peca.pontoReposicao}");

            MostrarMsg("=========================================");
            MostrarMsg(" NOVOS DADOS: ");
            MostrarMsg("=========================================\n");

            MostrarMsg("Novo nome: ");
            string nomeNovo = Console.ReadLine();

            MostrarMsg("Nova quantidade: ");
            string novaQuantStr = Console.ReadLine();

            MostrarMsg("Nova descrição: ");
            string novaDesc = Console.ReadLine();

            MostrarMsg("Novo estoque mínimo: ");
            string novoEstMinStr = Console.ReadLine();

            MostrarMsg("\nConfirmar alterações? (S/N): ");
            string op = Console.ReadLine();

            _pecaService.editarPeca(peca, nomeNovo, novaQuantStr, novaDesc, novoEstMinStr, op);

            string msg = _pecaService.editarPeca(peca, nomeNovo, novaQuantStr, novaDesc, novoEstMinStr, op);
            MostrarMsg(msg);
            Pausar();
        }

        public void ListarPeca()
        {
            LimparTela();
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
                Console.WriteLine($"{"ID",-10} {"Nome",-10} {"Quantidade",-15} {"Descrição",-12} {"Estoque minimo",-12}");
                Console.WriteLine(new string('-', 70));

                foreach (var dadosPecas in pecas)
                {
                    Console.WriteLine($"{dadosPecas.id,-10} {dadosPecas.nome,-10} {dadosPecas.quantidadeEstoque,-15} {dadosPecas.descricao,-12} {dadosPecas.pontoReposicao,-12}");
                }
                Pausar();
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
        public void LimparTela()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write("\x1b[3J");
        }

        public void ViewAdicionarEstoque(Peca peca)
        {
            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" ADICIONAR QUANTIDADE ESTOQUE ");
            MostrarMsg("=========================================");

            MostrarMsg($"ID: {peca.id}");
            MostrarMsg($"Nome atual: {peca.nome}");
            MostrarMsg($"Quantidade atual: {peca.quantidadeEstoque}");

            MostrarMsg("=========================================\n");

            MostrarMsg("Adicionar quantidade: ");
            string quant = Console.ReadLine();
            MostrarMsg("\nDeseja confirmar a inclusão ? (S/N)");
            string resp = Console.ReadLine();

            string msg = _pecaService.AdicionarEstoque(peca, quant, resp);
            MostrarMsg(msg);
            Pausar();
            
            if (msg == "\nQuantidade acrescentada com sucesso!!")
            {
                LimparTela();
                MostrarMsg("\n===== Estoque atualizado =====");
                MostrarMsg($"ID: {peca.id}");
                MostrarMsg($"Nome: {peca.nome}");
                MostrarMsg($"Quantidade nova: {peca.quantidadeEstoque}");
                Pausar();
            }
        }
    }
}

    


