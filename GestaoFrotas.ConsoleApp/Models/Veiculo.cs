using System; // Necessário para usar o DateTime

namespace GestaoFrotas.ConsoleApp.Models
{
  public class Veiculo
  {
    public int id;
    public string placa;
    public int idFrota; // Você definiu como int, o que está ótimo.
    public double hodometroInicial;
    public double hodometroAtual;
    public DateTime dataAquisicao;
    public double capacidadeCarga;
    public string status;
    public DateTime vencimentoLicenciamento;

    /**
    * Atualiza a quilometragem atual do veículo (RN-005, RN-007).
    */
    public void atualizarHodometro(double novoKm)
    {
      // Adicionamos uma verificação simples para garantir
      // que o novo hodômetro seja maior que o atual.
      if (novoKm > this.hodometroAtual)
      {
        this.hodometroAtual = novoKm;
      }
    }

    /**
    * Verifica se o licenciamento do veículo está vencido (RN-002).
    */
    public bool isLicenciamentoVencido()
    {
      // A lógica é a mesma da CNH:
      // Se a data de vencimento for MENOR (anterior) a hoje, está vencido.
      if (this.vencimentoLicenciamento < DateTime.Today)
      {
        return true;
      }
      else
      {
        return false;
      }

      // Versão curta:
      // return this.vencimentoLicenciamento < DateTime.Today;
    }

    /**
    * Define o status operacional do veículo (RN-008).
    * Ex: "Disponivel", "Quebra/Corretiva", "Em Manutencao"
    */
    public void definirStatus(string novoStatus)
    {
      this.status = novoStatus;
    }
  }
}
