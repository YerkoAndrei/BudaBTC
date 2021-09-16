using System;
using System.Net.Http;
using Newtonsoft.Json;
using Figgle;
using System.Globalization;
using System.Timers;

// API BUDA
//https://api.buda.com/#ticker

public class Programa
{
    private string api = "https://www.buda.com/api/v2/markets/btc-clp/ticker";
    private HttpClient cliente;
    private HttpResponseMessage respuesta;
    private string resultado;

    private static Timer timer;

    private static void Main(string[] args)
    {
        Console.Title = "                                                  --- Cargando ---";
        Console.BackgroundColor = ConsoleColor.Black;
        Console.SetWindowSize(58, 12);
        Console.SetBufferSize(58, 12);

        Programa programa = new Programa();
        programa.ObtenerRespuesta();

        timer = new Timer();
        timer.Interval = 10000;
        timer.Elapsed += programa.ObtenerRespuesta;
        timer.AutoReset = true;
        timer.Enabled = true;
        Console.ReadKey();
    }

    private void ObtenerRespuesta(Object source = null, System.Timers.ElapsedEventArgs e = null)
    {        
        cliente = new HttpClient();
        respuesta = cliente.GetAsync(api).Result;
        resultado = respuesta.Content.ReadAsStringAsync().Result;

        if (respuesta.IsSuccessStatusCode)
            Imprimir(JsonConvert.DeserializeObject(resultado));
        else
            ImprimirError(JsonConvert.DeserializeObject(resultado));
    }

    private void Imprimir(dynamic json)
    {
        Console.Clear();
        var textoPrecio = json.ticker.last_price[0].ToString();
        var valorPrecio = Double.Parse(textoPrecio, CultureInfo.InvariantCulture);
        var valorFormateado = string.Format(new CultureInfo("es-CL"), "{0:C0}", valorPrecio);

        Console.Title = "                                                  --- Bitcoin ---";

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("*******************************************************");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("                 ");
        Console.WriteLine("Bitcoin - Peso Chileno");
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("*******************************************************");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(FiggleFonts.Standard.Render(valorFormateado));

        Console.Beep();
    }

    private void ImprimirError(dynamic json)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Title = "                                                 --- Error ---";

        Console.WriteLine("*******************************************************");
        Console.Write("                    ");
        Console.WriteLine("Error " + json.code);
        Console.WriteLine();
        Console.WriteLine("*******************************************************");

        Console.WriteLine(json.message_code);
        Console.WriteLine(json.message);
    }
}