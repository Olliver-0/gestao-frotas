using System;

namespace GestaoFrotas.ConsoleApp.Models
{
  public class ManutencaoAutomatica
  {
    public int id;
    public int veiculoId; // Ligação com o Veiculo
    public string servico; // Ex: "Troca de Óleo", "Revisão de Freios"
    public DateTime data; // Data agendada
    public string oficina; // Ex: "Oficina do Zé"
    public string status; // "Pendente", "Agendada", "Confirmada", "Cancelada"

    // Construtor para facilitar a criação de dados de simulação
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