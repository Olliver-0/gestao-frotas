using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;

namespace GestaoFrotas.ConsoleApp.Views
{
        public class AddPecaOsView
        {
            private readonly OsService _osService;
            private readonly VeiculoService _veiculoService;
            private readonly MotoristaService _motoristaService;
            private readonly AddPecaOsService _addPecaOsService;
            private readonly PecaService _pecaService;
           
            
            public AddPecaOsView(OsService osService,
                                 VeiculoService veiculoService,
                                 MotoristaService motoristaService,
                                 PecaService pecaService,
                                 AddPecaOsService addPecaOsService)
            {
                _osService = osService;
                _veiculoService = veiculoService;
                _motoristaService = motoristaService;
                _pecaService = pecaService;
                _addPecaOsService = addPecaOsService;
            }
            public void ViewExibirMenuAddPecaOS()
            {
                LimparTela();

                while (true)
                {
                    LimparTela();
                    MostrarMsg("====================================================");
                    MostrarMsg(" GESTÃO DE ORDENS DE SERVIÇO - REGISTRO DE PEÇAS ");
                    MostrarMsg("====================================================");
                    MostrarMsg("");
                    MostrarMsg("Selecione uma opção:");
                    MostrarMsg("1 - Registrar peça na OS");
                    MostrarMsg("0 - Voltar ao Menu Principal");
                    MostrarMsg("");
                    MostrarMsg("Digite sua opção: ");

                    string opcao = Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            ViewCadastrarPecaOS();
                            break;
                        case "0":
                            return;
                        default:
                            MostrarMsg("=========================================");
                            MostrarMsg("OPÇÃO INVÁLIDA, TENTE NOVAMENTE");
                            MostrarMsg("=========================================");
                            break;
                    }
                }
            }

            private void ViewCadastrarPecaOS()
        {
            List<OrdemDeServico> OS = _osService.ListarTodas();
            List<Peca> pecas = _pecaService.ListarPecas();

            int osId;
            string pecaId;
            int quantidade;
            string msg;
            string op;

            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(" REGISTRAR PEÇA NA OS");
            MostrarMsg("=========================================");

           
            if (OS == null || OS.Count == 0)
            {
                LimparTela();
                MostrarMsg("NENHUMA ORDEM DE SERVIÇO CADASTRADA!!");
                Pausar();
                return;
            }
            else
            {
                MostrarMsg(" ORDENS DE SERVIÇOS ");
                MostrarMsg("=========================================\n");
                MostrarMsg($"{"ID",-10} {"DATA ABERT.",-23} {"TIPO",-12} {"DESCRIÇÃO",-30} {"OFICINA",-20}");
                MostrarMsg(new string('-', 80));
                foreach (var Os in OS)
                    MostrarMsg($"{Os.id,-10} {Os.dataAbertura,-23} {Os.tipo,-12} {Os.descricao,-30} {Os.oficina,-20}");

                osId = _addPecaOsService.LerInteiro("\nDigite o ID da OS: ");
                if (osId == 0) return;
            }


            if (pecas == null || pecas.Count == 0)
            {
                LimparTela();
                MostrarMsg("NENHUMA PEÇA CADASTRADA!!");
                Pausar();
                return;
            }
            else
            {
                MostrarMsg("\nPEÇAS DISPONÍVEIS NO ESTOQUE ");
                MostrarMsg("=========================================\n");
                MostrarMsg($"{"ID PEÇA",-10} {"NOME",-20} {"QTD ESTOQUE",-15}");
                MostrarMsg(new string('-', 50));
                foreach (var peca in pecas)
                    MostrarMsg($"{peca.id,-10} {peca.nome,-20} {peca.quantidadeEstoque,-15}");

                MostrarMsg("\nDigite o ID da Peça: ");
                pecaId = Console.ReadLine();

                Peca pecabuscada = _pecaService.buscarPecaID(pecaId, out int erro);
                if (erro == 1)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg("ERRO- NENHUM VALOR DIGITADO!!");
                    MostrarMsg("=========================================");
                    Pausar();
                    return;
                }
                else if (erro == 2)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg("ERRO- O ID TEM QUE SER UM NÚMERO!!");
                    MostrarMsg("=========================================");
                    Pausar();
                    return;
                }

                if (pecabuscada == null)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg("ERRO - PEÇA NÃO ENCONTRADA!!");
                    MostrarMsg("=========================================");
                    Pausar();
                    return;
                }

                quantidade = _addPecaOsService.LerInteiro("\nDigite a Quantidade utilizada: ");
                if (quantidade <= 0)
                {
                    MostrarMsg("A quantidade precisa ser maior que zero!!");
                    Pausar();
                    return;
                }

                int newQuant = int.Parse(pecabuscada.quantidadeEstoque);
                newQuant -= quantidade;




                if (quantidade > newQuant)
                {
                    LimparTela();
                    MostrarMsg("=========================================");
                    MostrarMsg($"ERRO - Estoque insuficiente. Disponível: {pecabuscada.quantidadeEstoque}.");
                    MostrarMsg("=========================================");
                    Pausar();
                    return;
                }

                string resumo = _addPecaOsService.ResumoPecaOs(osId, pecaId, quantidade);
                MostrarMsg($"Deseja cadastrar {resumo} na OS {osId} (S/N) ?");
                op = Console.ReadLine();

                if (op == "" || op == "N" || op == "n")
                {
                    MostrarMsg("OPERAÇÃO CANCELADA!!");
                    Pausar();
                    return;
                }

                if (op != "S" && op != "s")
                {
                    MostrarMsg("ERRO - OPÇÃO INVÁLIDA!!");
                    Pausar();
                    return;
                }

                msg = _addPecaOsService.CadastrarPecaOs(osId, pecaId, quantidade, op);
                newQuant.ToString();
                
            }
            

            LimparTela();
            MostrarMsg("=========================================");
            MostrarMsg(msg);
            MostrarMsg("=========================================");
            Pausar();
        }

            
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
