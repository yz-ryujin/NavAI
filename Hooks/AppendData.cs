using System.IO;
namespace NavAI.Hooks
{
    public class AppendData
    {
        private readonly string _caminhoCsv;
        public AppendData(string caminhoCsvCompleto)
        {
            _caminhoCsv = caminhoCsvCompleto;
        }

        public void AdicionarDadosAoCsv(bool label, string texto)
        {
            try
            {
                string linha = $"{label},{texto}";

                using (StreamWriter sw = new StreamWriter(_caminhoCsv, true))
                {
                    sw.WriteLine(linha);
                }


                Console.WriteLine($"Dado adicionado: {linha}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar dados ao CSV: {ex.Message}");
            }
        }

        public void DadosDeTeste()
        {
            AdicionarDadosAoCsv(true, "Esta cidade tem uma ótima infraestrutura e serviços.");
            AdicionarDadosAoCsv(false, "A qualidade do ar nesta área é muito ruim.");
            AdicionarDadosAoCsv(true, "Os parques e áreas verdes são bem cuidados.");
            AdicionarDadosAoCsv(false, "O transporte público é ineficiente e lotado.");
            AdicionarDadosAoCsv(true, "As escolas locais são de alta qualidade.");
            AdicionarDadosAoCsv(false, "A taxa de criminalidade nesta região é alta.");
        }
    }
}
