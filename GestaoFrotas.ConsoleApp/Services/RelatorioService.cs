using GestaoFrotas.ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFrotas.ConsoleApp.Services
{
    public class RelatorioService
    {
        private readonly VeiculoService _veiculoService;
        private readonly AbastecimentoService _abastecimentoService;
        private readonly OsService _osService;
        private readonly MotoristaService _motoristaService;
        private readonly ChecklistService _checklistService;

        public RelatorioService(
            VeiculoService veiculoService,
            AbastecimentoService abastecimentoService,
            OsService osService,
            MotoristaService motoristaService,
            ChecklistService checklistService)
        {
            _veiculoService = veiculoService;
            _abastecimentoService = abastecimentoService;
            _osService = osService;
            _motoristaService = motoristaService;
            _checklistService = checklistService;
        }
        
        public GestaoFrotas.ConsoleApp.Models.ResultadoTCO CalcularTCO(int? veiculoId, DateTime dataInicial, DateTime dataFinal)
        {

            var todasOs = _osService.ListarPo();
            var todosAbastecimentos = _abastecimentoService.ListarTodos();
            var todosVeiculos = _veiculoService.ListarTodos();

            Veiculo veiculoAlvo = null;
            if (veiculoId.HasValue)
            {
                veiculoAlvo = todosVeiculos.FirstOrDefault(v => v.idFrota == veiculoId.Value);
                if (veiculoAlvo == null)
                    return null;
            }

            var osFiltradas = todasOs
                .Where(os => (!veiculoId.HasValue || os.veiculoId == veiculoId.Value)
                             && os.dataFechamento.HasValue // Só considera OS fechadas
                             && os.dataFechamento.Value >= dataInicial
                             && os.dataFechamento.Value <= dataFinal
                             && os.custoFinal.HasValue)
                .ToList();

            var abastecimentosFiltrados = todosAbastecimentos
                .Where(a => (!veiculoId.HasValue || a.veiculoId == veiculoId.Value)
                            && a.data >= dataInicial
                            && a.data <= dataFinal)
                .ToList();

            double custoManutencao = osFiltradas.Sum(os => os.custoFinal.GetValueOrDefault());
            double custoCombustivel = abastecimentosFiltrados.Sum(a => a.valorTotal);
            double diasNoPeriodo = dataFinal.Subtract(dataInicial).TotalDays;
            
            double custoDepreciacaoPorDia = (veiculoAlvo != null) ? (500.0 / 30) : (2500.0 / 30);
            double custoSegurosPorDia = (veiculoAlvo != null) ? (100.0 / 30) : (500.0 / 30);

            double custoDepreciacao = custoDepreciacaoPorDia * diasNoPeriodo;
            double custoSeguros = custoSegurosPorDia * diasNoPeriodo;

            double tcoTotal = custoManutencao + custoCombustivel + custoDepreciacao + custoSeguros;

            return new GestaoFrotas.ConsoleApp.Models.ResultadoTCO
            {
                IdFrotaOuVeiculo = veiculoId.HasValue ? veiculoId.Value : 0,
            };
        }
    }
}
