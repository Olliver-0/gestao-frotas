using System; 

namespace GestaoFrotas.ConsoleApp.Models
{
  public class OrdemDeServico
  {
    public int id;
    public DateTime dataAbertura;
    public double hodometroEntrada;
    public string tipo;
    public int veiculoId;
    public int motoristaId;
    public int? mecanicoId; 
    public string descricao; 
    public string oficina; 
    public string status;
    public double? custoFinal; 
    public DateTime? dataFechamento; 
    public double? hodometroSaida;
    
    public string observacoesFechamento; 
    public bool documentosValidados;
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
    public void validarDocumentos()
    {
      this.documentosValidados = true;
    }
  }
}