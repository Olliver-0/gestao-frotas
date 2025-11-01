using System.Collections.Generic; // Para usar a List<>
using System.Linq; // Para usar o .Any()
using GestaoFrotas.ConsoleApp.Models;

namespace GestaoFrotas.ConsoleApp.Services
{
  public class VeiculoService
  {
    // Esta lista vai "simular" seu banco de dados
    private static List<Veiculo> _veiculos = new List<Veiculo>();
    private static int _proximoIdFrota = 101; // ID de Frota automático

    // Método para Adicionar (implementa RN-001)
    public string Adicionar(Veiculo veiculo)
    {
      // RN-001: Placa (única) - (Definido em requisitos.docx e RF01.docx)
      if (_veiculos.Any(v => v.placa == veiculo.placa))
      {
        return "Erro: Placa já cadastrada.";
      }

      // RN-001: Gerar ID de Frota automático - (Definido em requisitos.docx e RF01.docx)
      veiculo.idFrota = _proximoIdFrota;
      _proximoIdFrota++; // Incrementa para o próximo

      // Define o status inicial como "Disponível" - (Definido em RF01.docx)
      veiculo.status = "Disponível";

      _veiculos.Add(veiculo);
      return "Veículo cadastrado com sucesso.";
    }

    public Veiculo BuscarPorPlacaOuId(string busca)
    {
      // 1. Primeiro, tentamos converter a 'busca' para um número (ID de Frota).
      int idFrotaBuscado = 0;
      // 'int.TryParse' não quebra o programa. Ele retorna 'true' ou 'false'.
      bool isId = int.TryParse(busca, out idFrotaBuscado);

      // 2. Agora, usamos .FirstOrDefault() para encontrar o veículo.
      // .FirstOrDefault() retorna o objeto se achar, ou 'null' se não achar.
      Veiculo veiculoEncontrado = _veiculos.FirstOrDefault(v => v.placa == busca || (isId && v.idFrota == idFrotaBuscado));
      // 3. Retorna o resultado. (Será o objeto Veiculo ou null)
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
      // 1. Localiza o veículo original na lista (você fez certo)
      Veiculo veiculoOriginal = _veiculos.FirstOrDefault(v => v.idFrota == veiculoParaInativar.idFrota);

      // 2. [CORREÇÃO 1] Adiciona a verificação de 'null' (para evitar quebras)
      if (veiculoOriginal == null)
      {
        return "Erro: Veículo não encontrado. Não foi possível inativar.";
      }

      // (Opcional, mas boa prática) Verifica se ele já não está inativo
      if (veiculoOriginal.status == "Inativo")
      {
        return "Aviso: Este veículo já está inativo.";
      }

      // 3. [CORREÇÃO 2] Define o status diretamente para "Inativo",
      //    conforme manda a sua regra de negócio (RF01.docx).
      veiculoOriginal.status = "Inativo";

      return "Veículo inativado com sucesso.";
    }

    public List<Veiculo> ListarTodos()
    {
      return _veiculos.ToList();
    }
  }
}
