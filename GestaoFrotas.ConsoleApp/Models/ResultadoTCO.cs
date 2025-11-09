namespace GestaoFrotas.ConsoleApp.Models
{
    public class ResultadoTCO
    {
        public int IdFrotaOuVeiculo { get; set; }
        public string Identificador { get; set; }
        public double CustoCombustivel { get; set; }
        public double CustoManutencao { get; set; }
        public double CustoAquisicaoDepreciacao { get; set; }
        public double CustoSegurosLicenciamento { get; set; }
        public double TCOTotal { get; set; }
    }
}
