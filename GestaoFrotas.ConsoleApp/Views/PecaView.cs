using System;
using System.Collections.Generic;
using System.Globalization;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;

    namespace GestaoFrotas.ConsoleApp.Views
    {
        public class PecaView
        {
            private readonly PecaService _pecaService;

            public PecaView(PecaService pecaService)
            {
                _pecaService = pecaService;
            }

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
                            excluirPeca();
                            break;
                        case "5":
                            ListarPeca();
                            break;
                        case "0":
                            return;
                        default:
                            MostrarMsg("=========================================");
                            MostrarMsg("OPÇÃO INVALIDA, TENTE NOVAMENTE");
                            MostrarMsg("=========================================");
                            break;
                    }
                }
            }
            //---------------------------------------------------------------------------------------------------
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
                if (string.IsNullOrEmpty(novaPeca.nome))
                    return;
                MostrarMsg("Digite a quantidade: ");
                novaPeca.quantidadeEstoque = Console.ReadLine();
                MostrarMsg("Digite a Descrição: ");
                novaPeca.descricao = Console.ReadLine();
                MostrarMsg("Estoque minimo: ");
                novaPeca.pontoReposicao = Console.ReadLine();

                msg = _pecaService.CadastrarPeca(novaPeca);
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg(msg);
                MostrarMsg("=========================================");
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

                int erroEntrada;
                buscarPeca = _pecaService.buscarPecaID(cod, out erroEntrada);
                if (erroEntrada == 1)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg("ERRO- NENHUM VALOR DIGITADO!!");
                    MostrarMsg("=========================================");
                    Pausar();
                    return null;
                }
                else if (erroEntrada == 2)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg("ERRO- O ID TEM QUE SER UM NÚMERO!!");
                    MostrarMsg("=========================================");
                    Pausar();
                    return null;
                }

                if (buscarPeca == null)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg(" ERRO - PEÇA NÃO ENCONTRADA ");
                    MostrarMsg("=========================================");
                    Pausar();
                    return null;
                }
                else
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg($"PEÇA ID: {cod} ");
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
                        LimparTela();
                        MostrarMsg("=========================================");
                        MostrarMsg("OPÇÃO INVALIDA");
                        MostrarMsg("=========================================");
                        Pausar();
                        return null;
                    }
                    if (opAcao == 1)
                        ViewEditarPecaConsult(buscarPeca);
                    else if (opAcao == 2)
                        ViewExcluirPecaConsult(buscarPeca);
                    else if (opAcao == 3)
                        ViewAdicionarEstoque(buscarPeca);
                    else if (opAcao == 0)
                        return null;
                    else
                    {
                        LimparTela();
                        MostrarMsg("=========================================");
                        MostrarMsg("OPÇÃO INVALIDA");
                        MostrarMsg("=========================================");
                        Pausar();
                        return null;
                    }
                    return buscarPeca;
                }
            }
            //---------------------------------------------------------------------------------------------------
            private void ViewEditarPeca()
            {
                string cod;
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
                cod = Console.ReadLine();

                int erroEntrada;
                buscarPeca = _pecaService.buscarPecaID(cod, out erroEntrada);
                if (erroEntrada == 1)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg("ERRO- NENHUM VALOR DIGITADO!!");
                    MostrarMsg("=========================================");
                    Pausar();
                    return;
                }
                else if (erroEntrada == 2)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg("ERRO- O ID TEM QUE SER UM NÚMERO!!");
                    MostrarMsg("=========================================");
                    Pausar();
                    return;
                }
                if (buscarPeca == null)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg(" ERRO - PEÇA NÃO ENCONTRADA ");
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
                string msg = _pecaService.EditarPeca(buscarPeca, novoNome, novaQuant, novaDesc, novoEstMin, op);
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg(msg);
                MostrarMsg("=========================================");
                Pausar();
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

                MostrarMsg("\nNovo nome: ");
                string nomeNovo = Console.ReadLine();
                MostrarMsg("Nova quantidade: ");
                string novaQuantStr = Console.ReadLine();
                MostrarMsg("Nova descrição: ");
                string novaDesc = Console.ReadLine();
                MostrarMsg("Novo estoque mínimo: ");
                string novoEstMinStr = Console.ReadLine();
                MostrarMsg("\nConfirmar alterações? (S/N): ");
                string op = Console.ReadLine();

                string msg = _pecaService.EditarPeca(peca, nomeNovo, novaQuantStr, novaDesc, novoEstMinStr, op);
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg(msg);
                MostrarMsg("=========================================");
                Pausar();
            }
            //---------------------------------------------------------------------------------------------------
            public void ListarPeca()
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg(" LISTAR PEÇAS ");
                MostrarMsg("=========================================");
                List<Peca> pecas = _pecaService.ListarPecas();

                if (pecas == null)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg(" NENHUMA PEÇA CADASTRADA! ");
                    MostrarMsg("=========================================");
                    Pausar();
                    return;
                }
                else
                {
                    MostrarMsg($"{"ID",-10} {"Nome",-10} {"Quantidade",-15} {"Descrição",-12} {"Estoque minimo",-12}");
                    MostrarMsg(new string('-', 70));
                    foreach (var dadosPecas in pecas)
                    {
                    Console.WriteLine($"{dadosPecas.id,-10} {dadosPecas.nome,-10} {dadosPecas.quantidadeEstoque,-15} {dadosPecas.descricao,-12} {dadosPecas.pontoReposicao,-12}");
                    }
                    Pausar();
                }
            }
            //---------------------------------------------------------------------------------------------------       
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
                if (msg == "\nQUANTIDADE ACRESCENTADA COM SUCESSO!!")
                {
                    LimparTela();
                    MostrarMsg("\n===== ESTOQUE ATUALIZADO =====");
                    MostrarMsg($"ID: {peca.id}");
                    MostrarMsg($"Nome: {peca.nome}");
                    MostrarMsg($"Quantidade nova: {peca.quantidadeEstoque}");
                    Pausar();
                }
            }
            //---------------------------------------------------------------------------------------------------
            public void excluirPeca()
            {
                string cod;
                string op;
                string msg;
                Peca buscarPeca = null;
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg(" EXCLUIR PEÇA ");
                MostrarMsg("=========================================");
                MostrarMsg("Para sair deixe o campo em branco e pressione ENTER.");

                MostrarMsg("\nDigite o ID da peça: ");
                cod = Console.ReadLine();
                if (string.IsNullOrEmpty(cod))
                    return;
                int erroEntrada;
                buscarPeca = _pecaService.buscarPecaID(cod, out erroEntrada);
                if (erroEntrada == 1)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg("ERRO- NENHUM VALOR DIGITADO!!");
                    MostrarMsg("=========================================");
                    return;
                }
                else if (erroEntrada == 2)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg("ERRO- O ID TEM QUE SER UM NÚMERO!!");
                    MostrarMsg("=========================================");
                    Pausar();
                    return;
                }
                else if (buscarPeca == null)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg(" ERRO - PEÇA NÃO ENCONTRADA ");
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

                    MostrarMsg("\nDeseja realmente excluir? (S/N)");
                    op = Console.ReadLine();
                    msg = _pecaService.ExcluirPeca(buscarPeca, op);
                    LimparTela();
                    MostrarMsg(msg);
                    Pausar();
                    return;
                }
            }
            //---------------------------------------------------------------------------------------------------
            public void ViewExcluirPecaConsult(Peca peca)
            {
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg(" EXCLUIR PEÇA ");
                MostrarMsg("=========================================");

                MostrarMsg($"ID: {peca.id}");
                MostrarMsg($"Nome atual: {peca.nome}");
                MostrarMsg($"Quantidade atual: {peca.quantidadeEstoque}");
                MostrarMsg($"Descrição atual: {peca.descricao}");
                MostrarMsg($"Estoque mínimo atual: {peca.pontoReposicao}");

                MostrarMsg("\nConfirmar a exclusão? (S/N): ");
                string op = Console.ReadLine();
                string msg = _pecaService.ExcluirPeca(peca, op);
                LimparTela();
                MostrarMsg("=========================================");
                MostrarMsg(msg);
                MostrarMsg("=========================================");
                Pausar();
            }
            //---------------------------------------------------------------------------------------------------        
            public void MostrarMsg(string mensagem)
            {
                Console.WriteLine(mensagem);
            }
            //---------------------------------------------------------------------------------------------------
            public void Pausar(string msg = "Pressione qualquer tecla para continuar...")
            {
                Console.WriteLine();
                Console.Write(msg);
                Console.ReadKey(true);
            }
            //---------------------------------------------------------------------------------------------------
            public void LimparTela()
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.Write("\x1b[3J");
            }
        }
    }

