using System;

namespace GestaoFrotas.ConsoleApp.Models
{
  public class ChecklistPreViagem
  {
    public int id;
    public DateTime dataHora;
    public double hodometro;
    public bool pneusOk;
    public bool oleoOk;
    public int veiculoId;
    public int motoristaId;

    public void preencherRepostas(double hodometroAtual, bool respostaPneus, bool respostaOleo)
    {
      // Registra o momento exato do preenchimento
      this.dataHora = DateTime.Now;

      // Registra o hodômetro informado pelo motorista
      this.hodometro = hodometroAtual;

      // Salva as respostas do check-list
      this.pneusOk = respostaPneus;
      this.oleoOk = respostaOleo;
    }
  }
}


