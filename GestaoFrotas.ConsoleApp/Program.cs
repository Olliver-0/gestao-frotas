using System;
using GestaoFrotas.ConsoleApp.Services;
using GestaoFrotas.ConsoleApp.Views;
using GestaoFrotas.ConsoleApp.Models;

namespace GestaoFrotas.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var usuarioService = new UsuarioService();
            var motoristaService = new MotoristaService();
            var veiculoService = new VeiculoService();
            var abastecimentoService = new AbastecimentoService();
            var osService = new OsService(veiculoService, motoristaService);
            var checklistService = new ChecklistService();
            var manutencaoAutomaticaService = new ManutencaoAutomaticaService(veiculoService);
            var relatorioService = new RelatorioService(
                veiculoService,
                abastecimentoService,
                osService,
                motoristaService,
                checklistService);

            var loginView = new LoginView(usuarioService);
            var veiculoView = new VeiculoView(veiculoService, motoristaService);
            var abastecimentoView = new AbastecimentoView(abastecimentoService, veiculoService);
            var osView = new OsView(osService, veiculoService, motoristaService);
            var relatorioView = new RelatorioView(relatorioService, veiculoService);
            var usuarioView = new UsuarioView(usuarioService);

            Usuario usuarioLogado = usuarioService.ConsultarPorLogin("admin@frota.com");


            if (usuarioLogado != null && usuarioLogado.autenticar("123"))
            {
                Console.WriteLine($"\nUsuário logado: {usuarioLogado.nome} ({usuarioLogado.perfil})");
                ExibirMenuPrincipal(usuarioLogado.perfil, veiculoView, abastecimentoView, osView, relatorioView, usuarioView);
            }
            else
            {
                Console.WriteLine("\nErro: Não foi possível logar com o usuário padrão. Verifique o UsuarioService e a senha.");
            }
        }
        static void ExibirMenuPrincipal(string perfil, VeiculoView veiculoView, AbastecimentoView abastecimentoView, OsView osView, RelatorioView relatorioView, UsuarioView usuarioView)
        {
            bool continuar = true;
            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine(" MENU PRINCIPAL - GESTÃO DE FROTAS");
                Console.WriteLine("=========================================");
                Console.WriteLine("1. Veículos ");
                Console.WriteLine("2. Ordens de Serviço (OS)");
                Console.WriteLine("3. Abastecimentos");
                Console.WriteLine("4. Relatórios");
                Console.WriteLine("5. Gerenciar Usuários");
                Console.WriteLine("0. Sair");
                Console.Write("Opção: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        veiculoView.ExibirMenuVeiculos();
                        break;
                    case "2":
                        osView.ExibirMenuOS();
                        break;
                    case "3":
                        abastecimentoView.ExibirMenuAbastecimentos();
                        break;
                    case "4":
                        relatorioView.MenuRelatorios();
                        break;
                    case "5":
                        usuarioView.GerenciarUsuarios();
                        break;
                    case "9":
                        Console.WriteLine("\nLogout realizado.");
                        return;
                    case "0":
                        continuar = false;
                        Console.WriteLine("\nSistema encerrado.");
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Pressione qualquer tecla para tentar novamente.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
