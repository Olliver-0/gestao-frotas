using System;

namespace GestaoFrotas.ConsoleApp.Models
{
  public class Veiculo
  {
    public int id;
    public string placa;
    public int idFrota;
    public double hodometroInicial;
    public double hodometroAtual;
    public DateTime dataAquisicao;
    public double capacidadeCarga;
    public string status;
    public DateTime vencimentoLicenciamento;

    public void atualizarHodometro(double novoKm)
    {
      if (novoKm > this.hodometroAtual)
      {
        this.hodometroAtual = novoKm;
      }
    }
    public bool isLicenciamentoVencido()
    {
      if (this.vencimentoLicenciamento < DateTime.Today)
      {
        return true;
      }
      else
      {
        return false;
      }
    }
    public void definirStatus(string novoStatus)
    {
      this.status = novoStatus;
    }
  }
}
