using System.Collections.Generic;
using System.Linq;
using GestaoFrotas.ConsoleApp.Models;

namespace GestaoFrotas.ConsoleApp.Services
{
  public class VeiculoService
  {
    private static List<Veiculo> _veiculos = new List<Veiculo>();
    private static int _proximoIdFrota = 101;

    public string Adicionar(Veiculo veiculo)
    {
      if (_veiculos.Any(v => v.placa == veiculo.placa))
      {
        return "Erro: Placa já cadastrada.";
      }

      veiculo.idFrota = _proximoIdFrota;
      _proximoIdFrota++;

      veiculo.status = "Disponível";

      _veiculos.Add(veiculo);
      return "Veículo cadastrado com sucesso.";
    }

    public Veiculo BuscarPorPlacaOuId(string busca)
    {
      int idFrotaBuscado = 0;
      bool isId = int.TryParse(busca, out idFrotaBuscado);

      Veiculo veiculoEncontrado = _veiculos.FirstOrDefault(v => v.placa == busca || (isId && v.idFrota == idFrotaBuscado));
      return veiculoEncontrado;
    }

    public string Atualizar(Veiculo veiculoAtualizado)
    {
      Veiculo veiculoOriginal = _veiculos.FirstOrDefault(v => v.idFrota == veiculoAtualizado.idFrota);

      if (veiculoOriginal == null)
      {
        return "Erro: Veículo não encontrado. Não foi possível atualizar.";
      }

      if (veiculoAtualizado.hodometroAtual < veiculoOriginal.hodometroAtual)
      {
        return $"Erro: Hodômetro inválido. O novo valor ({veiculoAtualizado.hodometroAtual}) não pode ser menor que o atual ({veiculoOriginal.hodometroAtual}).";
      }

      veiculoOriginal.status = veiculoAtualizado.status;
      veiculoOriginal.hodometroAtual = veiculoAtualizado.hodometroAtual;
      veiculoOriginal.vencimentoLicenciamento = veiculoAtualizado.vencimentoLicenciamento;
      veiculoOriginal.capacidadeCarga = veiculoAtualizado.capacidadeCarga;

      return "Veículo atualizado com sucesso.";
    }

    public string Inativar(Veiculo veiculoParaInativar)
    {
      Veiculo veiculoOriginal = _veiculos.FirstOrDefault(v => v.idFrota == veiculoParaInativar.idFrota);

      if (veiculoOriginal == null)
      {
        return "Erro: Veículo não encontrado. Não foi possível inativar.";
      }

      if (veiculoOriginal.status == "Inativo")
      {
        return "Aviso: Este veículo já está inativo.";
      }
      veiculoOriginal.status = "Inativo";

      return "Veículo inativado com sucesso.";
    }

    public List<Veiculo> ListarTodos()
    {
      return _veiculos.ToList();
    }
  }
}
