using System;

namespace GestaoFrotas.ConsoleApp.Models
{
  public class ManutencaoAutomatica
  {
    public int id;
    public int veiculoId; 
    public string servico; 
    public DateTime data; 
    public string oficina; 
    public string status; 
    public ManutencaoAutomatica(int id, int veiculoId, string servico, string status, DateTime? data = null, string oficina = null)
    {
      this.id = id;
      this.veiculoId = veiculoId;
      this.servico = servico;
      this.status = status;
      this.data = data ?? DateTime.MinValue;
      this.oficina = oficina;
    }
  }
}