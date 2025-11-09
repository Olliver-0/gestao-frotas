using System;
using System.Globalization;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;

namespace GestaoFrotas.ConsoleApp.Views
{
    public class RelatorioView
    {
        private readonly RelatorioService _relatorioService;
        private readonly VeiculoService _veiculoService;
        public RelatorioView(RelatorioService relatorioService, VeiculoService veiculoService)
        {
            _relatorioService = relatorioService;
            _veiculoService = veiculoService;
        }

        public void MenuRelatorios()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine(" GESTÃO DE RELATÓRIOS ");
                Console.WriteLine("=========================================");
                Console.WriteLine("1. Gerar Relatório de Custo Total de Propriedade (TCO)");
                Console.WriteLine("0. Voltar ao Menu Principal");
                Console.WriteLine("=========================================");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1":
                        GerarRelatorioTCO();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nOpção inválida. Tente novamente.");
                        PausarEVoltar();
                        break;
                }
            }
        }

        public void GerarRelatorioTCO()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine(" RELATÓRIO TCO (Total Cost of Ownership)");
            Console.WriteLine("=========================================");

            try
            {
                Console.WriteLine("\nOpções de Filtro:");
                string placaOuId = LerStringOpcional("Placa ou ID da Frota do Veículo (Deixe em branco para toda a frota): ").ToUpper();
                
                int? veiculoId = null;
                string identificador = "TODA A FROTA";

                if (!string.IsNullOrWhiteSpace(placaOuId))
                {
                    // Usa o Service para buscar o veículo
                    Veiculo veiculo = _veiculoService.BuscarPorPlacaOuId(placaOuId);
                    if (veiculo == null)
                    {
                        Console.WriteLine("\nErro: Veículo não encontrado. O relatório não será gerado.");
                        PausarEVoltar();
                        return;
                    }
                    veiculoId = veiculo.idFrota;
                    identificador = veiculo.placa;
                }

                DateTime dataInicial = LerData("Data Inicial do Período (dd/MM/yyyy): ");
                DateTime dataFinal = LerData("Data Final do Período (dd/MM/yyyy): ");

                if (dataInicial > dataFinal)
                {
                    Console.WriteLine("\nErro: A data inicial não pode ser posterior à data final.");
                    PausarEVoltar();
                    return;
                }

                Console.WriteLine("\nGerando relatório...");
                
                // Chamada ao Service de Relatório
                GestaoFrotas.ConsoleApp.Models.ResultadoTCO resultado = _relatorioService.CalcularTCO(veiculoId, dataInicial, dataFinal);

                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine($" RESULTADO TCO - {identificador}");
                Console.WriteLine("=========================================");
                Console.WriteLine($" Período: {dataInicial:dd/MM/yyyy} a {dataFinal:dd/MM/yyyy}");
                Console.WriteLine("-----------------------------------------");
                
                if (resultado == null)
                {
                    Console.WriteLine("\nNão foi possível gerar o relatório. Verifique os filtros ou se o Veículo/Frota existe e possui dados.");
                }
                else
                {
                    // Exibição formatada dos resultados
                    Console.WriteLine($" Custo Manutenção:   R$ {resultado.CustoManutencao:N2}");
                    Console.WriteLine($" Custo Combustível:  R$ {resultado.CustoCombustivel:N2}");
                    Console.WriteLine($" Custo Depreciação:  R$ {resultado.CustoAquisicaoDepreciacao:N2}");
                    Console.WriteLine($" Custo Seguros/Taxas:R$ {resultado.CustoSegurosLicenciamento:N2}");
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine($" CUSTO TOTAL (TCO):  R$ {resultado.TCOTotal:N2}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nERRO INESPERADO: {ex.Message}");
            }
            PausarEVoltar();
        }

        private void PausarEVoltar(string mensagem = "Pressione qualquer tecla para voltar...")
        {
            Console.WriteLine();
            Console.WriteLine(mensagem);
            Console.ReadKey();
        }
        
        private string LerStringOpcional(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine().Trim();
        }

        private DateTime LerData(string prompt)
        {
            DateTime data;
            while (true)
            {
                Console.Write(prompt);
                string entrada = Console.ReadLine();

                if (DateTime.TryParseExact(entrada, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out data))
                {
                    return data;
                }
                else
                {
                    Console.WriteLine("Erro: Data inválida. Digite no formato DD/MM/AAAA.");
                }
            }
        }
    }
}
