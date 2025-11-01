using System;
using System.Collections.Generic;
using System.Linq;
using GestaoFrotas.ConsoleApp.Models;

namespace GestaoFrotas.ConsoleApp.Services
{
  public class AbastecimentoService
  {
    private static readonly List<Abastecimento> _abastecimentos = new List<Abastecimento>();

    public string Adicionar(Abastecimento novoAbastecimento, Veiculo veiculo)
    {
      if (novoAbastecimento.litrosAbastecidos <= 0 || novoAbastecimento.valorTotal <= 0)
      {
        return "Erro: Litros e Valor Total devem ser positivos.";
      }

      if (novoAbastecimento.hodometro < veiculo.hodometroAtual)
      {
        return $"Erro: Hodômetro inválido. O valor ({novoAbastecimento.hodometro}) não pode ser menor que o atual ({veiculo.hodometroAtual}).";
      }

      veiculo.atualizarHodometro(novoAbastecimento.hodometro);

      Abastecimento ultimo = GetUltimoAbastecimento(veiculo);

      double? consumoCalculado = CalcularConsumo(novoAbastecimento, ultimo);

      novoAbastecimento.consumoMedioCalculado = consumoCalculado;

      string alertaDesvio = null;
      if (consumoCalculado.HasValue)
      {
        double? mediaHistorica = GetMediaHistorica(veiculo);
        alertaDesvio = VerificarDesvioConsumo(consumoCalculado.Value, mediaHistorica, veiculo);
      }

      _abastecimentos.Add(novoAbastecimento);

      string mensagemSucesso = "Abastecimento registrado com sucesso.";
      if (consumoCalculado.HasValue)
      {
        mensagemSucesso += $"\nConsumo (RF12) desde o último registro: {consumoCalculado.Value:F2} Km/L.";
      }

      if (!string.IsNullOrEmpty(alertaDesvio))
      {
        mensagemSucesso += $"\n{alertaDesvio}";
      }

      return mensagemSucesso;
    }

    private Abastecimento GetUltimoAbastecimento(Veiculo veiculo)
    {
      return _abastecimentos
          .Where(a => a.veiculoId == veiculo.idFrota)
          .OrderByDescending(a => a.data)
          .ThenByDescending(a => a.hodometro)
          .FirstOrDefault();
    }

    private double? CalcularConsumo(Abastecimento novo, Abastecimento ultimo)
    {
      if (ultimo == null)
      {
        return null;
      }

      double kmRodados = novo.hodometro - ultimo.hodometro;

      if (novo.litrosAbastecidos == 0)
      {
        return null;
      }

      if (kmRodados == 0)
      {
        return 0;
      }

      return kmRodados / novo.litrosAbastecidos;
    }

    private double? GetMediaHistorica(Veiculo veiculo)
    {
      var consumosPassados = _abastecimentos
          .Where(a => a.veiculoId == veiculo.idFrota && a.consumoMedioCalculado.HasValue)
          .Select(a => a.consumoMedioCalculado.Value);

      if (consumosPassados.Any())
      {
        return consumosPassados.Average();
      }

      return null;
    }

    private string VerificarDesvioConsumo(double consumoCalculado, double? mediaHistorica, Veiculo veiculo)
    {
      if (!mediaHistorica.HasValue)
      {
        return null;
      }

      double media = mediaHistorica.Value;

      double limitePiora = media * 0.85;

      if (consumoCalculado < limitePiora)
      {
        double percentualPiora = (1 - (consumoCalculado / media)) * 100;

        return $"[ALERTA - RF13] Desvio de consumo detectado!\nConsumo {percentualPiora:F1}% pior que a média histórica do veículo ({media:F2} Km/L).";
      }

      return null;
    }
  }
}
