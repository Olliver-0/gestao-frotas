using System; 

namespace GestaoFrotas.ConsoleApp.Models
{
  public class OrdemDeServico
  {
    public int id;
    public DateTime dataAbertura;
    public double hodometroEntrada;
    public string tipo; // Ex: "Correção"
    public int veiculoId;
    public int motoristaId; // Necessário para RN-002
    public int? mecanicoId; 

    // --- Campos Adicionados (RF06 / RF07) ---
    public string descricao; 
    public string oficina; 
    public string status; // "Aberta", "Finalizada", "Excluída"
    public double? custoFinal; 
    public DateTime? dataFechamento; 
    public double? hodometroSaida; // RN-005
    public string observacoesFechamento; 
    public bool documentosValidados; // RF07


    /**
    * Finaliza a Ordem de Serviço (RN-005)
    */
    public void fecharOS(double custo, DateTime dataConclusao, string observacoes, double hodometroSaida)
    {
      if (this.status == "Aberta")
      {
        this.status = "Finalizada";
        this.dataFechamento = dataConclusao;
        this.custoFinal = custo;
        this.observacoesFechamento = observacoes;
        this.hodometroSaida = hodometroSaida; 
      }
    }

    /**
    * Marca a OS como validada (RF07)
    */
    public void validarDocumentos()
    {
      this.documentosValidados = true;
    }
  }
}